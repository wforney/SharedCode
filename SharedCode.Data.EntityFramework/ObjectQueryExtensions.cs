// <copyright file="ObjectQueryExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq.Expressions;

    /// <summary>
    /// Class ObjectQueryExtensions.
    /// </summary>
    public static class ObjectQueryExtensions
    {
        /// <summary>
        /// Type-safe Include: a completely type-safe way to include nested objects in scenarios with DomainServices and RIA in, for example, Silverlight applications. Example: Include(x=>x.Parent) instead of Include("Parent"). A more detailed explanation can be found at http://www.chrismeijers.com/post/Type-safe-Include-for-RIA-DomainServices.aspx
        /// </summary>
        /// <typeparam name="T">The type of the first entity.</typeparam>
        /// <typeparam name="T2">The type of the second entity.</typeparam>
        /// <param name="data">This object query.</param>
        /// <param name="property1">The first property.</param>
        /// <param name="property2">The second property.</param>
        /// <returns>The object query.</returns>
        public static ObjectQuery<T> Include<T, T2>(this ObjectQuery<T> data, Expression<Func<T, ICollection<T2>>> property1, Expression<Func<T2, object>> property2)
            where T : class
            where T2 : class
        {
            var name1 = (property1.Body as MemberExpression)?.Member.Name;
            var name2 = (property2.Body as MemberExpression)?.Member.Name;

            return data.Include($"{name1}.{name2}");
        }

        /// <summary>
        /// Type-safe Include: a completely type-safe way to include nested objects in scenarios with DomainServices and RIA in, for example, Silverlight applications. Example: Include(x=>x.Parent) instead of Include("Parent"). A more detailed explanation can be found at http://www.chrismeijers.com/post/Type-safe-Include-for-RIA-DomainServices.aspx
        /// </summary>
        /// <typeparam name="T">The type of the first entity.</typeparam>
        /// <typeparam name="T2">The type of the second entity.</typeparam>
        /// <param name="data">This object query.</param>
        /// <param name="property1">The first property.</param>
        /// <param name="property2">The second property.</param>
        /// <returns>The object query.</returns>
        public static ObjectQuery<T> Include<T, T2>(this ObjectQuery<T> data, Expression<Func<T, T2>> property1, Expression<Func<T2, object>> property2) where T : class
        {
            var name1 = (property1.Body as MemberExpression)?.Member.Name;
            var name2 = (property2.Body as MemberExpression)?.Member.Name;

            return data.Include($"{name1}.{name2}");
        }

        /// <summary>
        /// Type-safe Include: a completely type-safe way to include nested objects in scenarios with DomainServices and RIA in, for example, Silverlight applications. Example: Include(x=>x.Parent) instead of Include("Parent"). A more detailed explanation can be found at http://www.chrismeijers.com/post/Type-safe-Include-for-RIA-DomainServices.aspx
        /// </summary>
        /// <typeparam name="T">The type of the first entity.</typeparam>
        /// <param name="data">This object query.</param>
        /// <param name="property">The property expression.</param>
        /// <returns>The object query.</returns>
        public static ObjectQuery<T> Include<T>(this ObjectQuery<T> data, Expression<Func<T, object>> property) where T : class
        {
            var name = (property.Body as MemberExpression)?.Member.Name;

            return data.Include(name);
        }
    }
}