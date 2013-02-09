using System;
using System.Linq;
using System.Reflection;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace CacheAspect.Repository
{
    /// <summary>
    /// Cache action types.
    /// </summary>
    public enum CacheAction
    {
        /// <summary>
        /// Add a new item to cache.
        /// </summary>
        Add,
        /// <summary>
        /// Remove all associated items from cache for the given domain model.
        /// </summary>
        Remove
    }

    /// <summary>
    /// Cache attribute.
    /// </summary>
    /// <remarks>
    /// A small portion of this class was based on Matthew Groves' original CacheAttribute class.
    /// https://github.com/mgroves/PostSharp5/blob/master/4-Caching/CacheAttribute.cs
    /// </remarks>
    [Serializable]
    public class CacheAttribute : MethodInterceptionAspect
    {
        [NonSerialized]
        private static readonly ICache Cache;

        [NonSerialized]
        private object _syncRoot;
        private string _methodName;
        public readonly CacheAction Action;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static CacheAttribute()
        {
            if (!PostSharpEnvironment.IsPostSharpRunning)
            {
                Cache = new AzureCache();
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="action">Cache action.</param>
        public CacheAttribute(CacheAction action)
        {
            Action = action;
        }

        /// <summary>
        /// Initialisation during compile time.
        /// </summary>
        /// <param name="methodBase">Method information.</param>
        /// <param name="aspectInfo">Aspect information.</param>
        /// <remarks>
        /// Normally you'd determine the class name at this point but because we're using generic repositories it's of no use.
        /// </remarks>
        public override void CompileTimeInitialize(MethodBase methodBase, AspectInfo aspectInfo)
        {
            _methodName = methodBase.Name;
        }

        /// <summary>
        /// Runtime initialisation.
        /// </summary>
        /// <param name="methodBase">Method information.</param>
        public override void RuntimeInitialize(MethodBase methodBase)
        {
            _syncRoot = new object();
        }

        /// <summary>
        /// Invoke on method call.
        /// </summary>
        /// <param name="methodInterceptionArgs"></param>
        public override void OnInvoke(MethodInterceptionArgs methodInterceptionArgs)
        {
            if (Action == CacheAction.Add)
            {
                var cacheKey = BuildCacheKey(methodInterceptionArgs);

                if (Cache[cacheKey] != null)
                {
                    methodInterceptionArgs.ReturnValue = Cache[cacheKey];
                }
                else
                {
                    lock (_syncRoot)
                    {
                        if (Cache[cacheKey] == null)
                        {
                            var returnVal = methodInterceptionArgs.Invoke(methodInterceptionArgs.Arguments);
                            methodInterceptionArgs.ReturnValue = returnVal;

                            Cache[cacheKey] = returnVal;
                        }
                        else
                        {
                            methodInterceptionArgs.ReturnValue = Cache[cacheKey];
                        }
                    }
                }
            }
            else
            {
                var typeName = GetTypeName(methodInterceptionArgs.Binding.GetType());

                lock (_syncRoot)
                {
                    Cache.Remove(typeName);
                }

                methodInterceptionArgs.ReturnValue = methodInterceptionArgs.Invoke(methodInterceptionArgs.Arguments);
            }
        }

        /// <summary>
        /// Build the cache key using the type name, method name and parameter argument values.
        /// </summary>
        /// <param name="methodInterceptionArgs">Aspect arguments.</param>
        /// <returns>Cache key.</returns>
        private string BuildCacheKey(MethodInterceptionArgs methodInterceptionArgs)
        {
            const string divider = "_";

            var typeName = GetTypeName(methodInterceptionArgs.Binding.GetType());

            var cacheKey = new StringBuilder();
            cacheKey.Append(typeName);
            cacheKey.Append(divider);
            cacheKey.Append(_methodName);

            foreach (var argument in methodInterceptionArgs.Arguments.ToArray())
            {
                cacheKey.Append(argument == null ? divider : argument.ToString());
            }

            return cacheKey.ToString();
        }

        /// <summary>
        /// Use reflection to get the object's type name. 
        /// </summary>
        /// <param name="type">The object's type.</param>
        /// <returns>Type name.</returns>
        /// <remarks>
        /// If we're supporting non-generic repositories we need to identify the correct type name.
        /// </remarks>
        private string GetTypeName(Type type)
        {
            return ((type.UnderlyingSystemType).GenericTypeArguments.Any())
                ? ((type.UnderlyingSystemType).GenericTypeArguments[0]).Name
                : type.DeclaringType.Name;
        }
    }
}