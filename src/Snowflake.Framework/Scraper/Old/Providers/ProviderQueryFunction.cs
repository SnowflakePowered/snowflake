using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    internal class ProviderQueryFunction<T>
    {
        /// <summary>
        /// Gets the required metadata for this provider
        /// </summary>
        public IEnumerable<string> RequiredMetadata { get; }

        /// <summary>
        /// Gets the metadata or file type that is guaranteed to return for this provider
        /// </summary>
        public IEnumerable<string> ReturnMetadata { get; }

        /// <summary>
        /// The provider function itself
        /// </summary>
        private readonly Func<IMetadataCollection, T> provider;

        internal ProviderQueryFunction(IEnumerable<string> requiredMetadata, IEnumerable<string> returnMetadata, Func<IMetadataCollection, T> provider)
        {
            this.RequiredMetadata = requiredMetadata.ToList();
            this.ReturnMetadata = returnMetadata.ToList();
            this.provider = provider;
        }

        public T Query(IMetadataCollection m) => this.provider(m);

        public static Func<IMetadataCollection, T> Make(object instance, MethodInfo methodInfo)
        {
            var metadataParam = Expression.Parameter(typeof(IMetadataCollection));
            var instanceRef = Expression.Constant(instance, instance.GetType());
            var call = Expression.Call(instanceRef, methodInfo, metadataParam);
            return Expression.Lambda<Func<IMetadataCollection, T>>(call, metadataParam).Compile();
        }
    }
}
