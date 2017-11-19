// <copyright file="Extensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using JetBrains.Annotations;

    using Newtonsoft.Json;

    /// <summary>
    ///     The extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     The XML serializers
        /// </summary>
        [NotNull]
        private static readonly Dictionary<RuntimeTypeHandle, XmlSerializer> XmlSerializers =
            new Dictionary<RuntimeTypeHandle, XmlSerializer>();

        /// <summary>
        ///     Determines whether the specified <paramref name="value" /> is between the <paramref name="from" /> and
        ///     <paramref name="to" /> values.
        /// </summary>
        /// <typeparam name="T">The type of the values to be compared.</typeparam>
        /// <param name="value">The value to be compared.</param>
        /// <param name="from">The lower bound value.</param>
        /// <param name="to">The upper bound value.</param>
        /// <returns>
        ///     Returns <c>true</c> if the specified <paramref name="value" /> is between the <paramref name="from" /> and
        ///     <paramref name="to" /> values, <c>false</c> otherwise.
        /// </returns>
        public static bool Between<T>([NotNull] this T value, [NotNull] T from, [NotNull] T to) where T : IComparable<T>
            => value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;

        /// <summary>
        ///     Converts any type to another.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="returnValueIfException">The return value if exception.</param>
        /// <returns>The output.</returns>
        [CanBeNull]
        public static T ChangeType<T>([CanBeNull] this object source, [CanBeNull] T returnValueIfException)
        {
            try
            {
                return source.ChangeType<T>();
            }
            catch (Exception)
            {
                return returnValueIfException;
            }
        }

        /// <summary>
        ///     Converts any type to another.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>The output.</returns>
        [CanBeNull]
        public static T ChangeType<T>([CanBeNull] this object source)
        {
            if (source is T u)
            {
                return u;
            }

            var destinationType = typeof(T);
            if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                destinationType = new NullableConverter(destinationType).UnderlyingType;
            }

            return (T)Convert.ChangeType(source, destinationType);
        }

        /// <summary>
        ///     Makes a copy from the object.
        ///     Doesn't copy the reference memory, only data.
        /// </summary>
        /// <typeparam name="T">Type of the return object.</typeparam>
        /// <param name="item">Object to be copied.</param>
        /// <returns>Returns the copied object.</returns>
        [CanBeNull]
        public static T Clone<T>([CanBeNull] this object item)
        {
            if (item == null)
            {
                return default;
            }

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, item);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)new BinaryFormatter().Deserialize(stream);
            }
        }

        /// <summary>
        ///     Converts the value to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="value">The input value.</param>
        /// <returns>The output value.</returns>
        [NotNull]
        public static T ConvertTo<T>([NotNull] this IConvertible value)
            => (T)Convert.ChangeType(value, typeof(T));

        /// <summary>
        ///     Converts the value to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="value">The input value.</param>
        /// <returns>The output value.</returns>
        [NotNull]
        public static T ConvertTo<T>([NotNull] this object value)
            => (T)Convert.ChangeType(value, typeof(T));

        /// <summary>
        ///     Returns a deep copy of this object.
        /// </summary>
        /// <typeparam name="T">The type of the input object.</typeparam>
        /// <param name="input">The input object.</param>
        /// <returns>The output.</returns>
        [NotNull]
        public static T DeepClone<T>([NotNull] this T input) where T : ISerializable
        {
            Contract.Ensures(!EqualityComparer<T>.Default.Equals(Contract.Result<T>(), default));
            Contract.Requires<ArgumentNullException>(!EqualityComparer<T>.Default.Equals(input, default));

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, input);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        ///     Converts a JSON string to the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object represented in the JSON string.</typeparam>
        /// <param name="jsonString">The JSON string.</param>
        /// <returns>The output.</returns>
        [CanBeNull]
        public static T FromJson<T>([CanBeNull] this object jsonString)
            => JsonConvert.DeserializeObject<T>(jsonString as string);

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <returns>The property information.</returns>
        // ReSharper disable once UnusedParameter.Global
#pragma warning disable RECS0154 // Parameter is never used
#pragma warning disable CC0057 // Unused parameters
#pragma warning disable RCS1175 // Unused this parameter.

        [CanBeNull]
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>([CanBeNull] this TSource source, [NotNull] Expression<Func<TSource, TProperty>> propertyLambda)
#pragma warning restore RCS1175 // Unused this parameter.
#pragma warning restore CC0057 // Unused parameters
#pragma warning restore RECS0154 // Parameter is never used
        {
            Contract.Requires(propertyLambda != null);

            var type = typeof(TSource);

            if (!(propertyLambda.Body is MemberExpression member))
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            if (!(member.Member is PropertyInfo propInfo))
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType) && !propInfo.ReflectedType.IsAssignableFrom(type))
            {
                throw new ArgumentException($"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");
            }

            return propInfo;
        }

        /// <summary>
        /// Gets the property value from the source object using reflection.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The property value.</returns>
        [CanBeNull]
        public static T GetPropertyValue<T>([NotNull] this object source, [NotNull] string property)
        {
            Contract.Requires(source != null);
            Contract.Requires(property != null);

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperties();

            return (T)sourceProperties.Where(s => s.Name.Equals(property, StringComparison.OrdinalIgnoreCase))
                                                .Select(s => s.GetValue(source, null))
                                                .FirstOrDefault();
        }

        /// <summary>
        ///     If the object this method is called on is not null, runs the given function and returns the value.
        ///     If the object is null, returns default(TResult)
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="getValue">The get value function.</param>
        /// <returns>The result.</returns>
        [CanBeNull]
        public static TResult IfNotNull<T, TResult>([CanBeNull] this T target, [CanBeNull] Func<T, TResult> getValue)
        {
            var handler = getValue;
            return handler == null || EqualityComparer<T>.Default.Equals(target, default) ? default : handler(target);
        }

        /// <summary>
        ///     Determines if this value is in the specified list of parameters.
        /// </summary>
        /// <typeparam name="T">The type of values being compared.</typeparam>
        /// <param name="value">This value.</param>
        /// <param name="parameters">The list of parameters.</param>
        /// <returns><c>true</c> if this value is in the specified list of parameters, <c>false</c> otherwise.</returns>
        public static bool In<T>([CanBeNull] this T value, [CanBeNull] [ItemCanBeNull] params T[] parameters)
            => parameters?.Contains(value) ?? false;

        /// <summary>
        ///     Determines whether the specified value is between the low and high values.
        /// </summary>
        /// <typeparam name="T">The types being compared.</typeparam>
        /// <param name="value">The value to compare.</param>
        /// <param name="low">The low value.</param>
        /// <param name="high">The high value.</param>
        /// <returns><c>true</c> if the specified value is between the low and high values; otherwise, <c>false</c>.</returns>
        public static bool IsBetween<T>([NotNull] this T value, [NotNull] T low, [NotNull] T high)
            where T : IComparable<T>
            => value.CompareTo(low) >= 0 && value.CompareTo(high) <= 0;

        /// <summary>
        ///     Determines whether the specified source object is not null.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns><c>true</c> if the specified source object is not null; otherwise, <c>false</c>.</returns>
        public static bool IsNotNull([CanBeNull] this object source) => source != null;

        /// <summary>
        ///     Determines whether the specified source object is null.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns><c>true</c> if the specified source object is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull([CanBeNull] this object source) => source == null;

        /// <summary>
        /// Unified advanced generic check for: DbNull.Value, INullable.IsNull, !Nullable&lt;&gt;.HasValue, null reference. Omits boxing for value types.
        /// </summary>
        /// <typeparam name="T">The type of the value we are checking.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <returns>A value indicating whether or not this value is null.</returns>
        [DebuggerStepThrough]
        public static bool IsNull<T>([CanBeNull] this T value)
        {
            if (value is INullable nullable && nullable.IsNull)
            {
                return true;
            }

            var type = typeof(T);
            if (type.IsValueType)
            {
                if (!object.ReferenceEquals(Nullable.GetUnderlyingType(type), null) && value?.GetHashCode() == 0)
                {
                    return true;
                }
            }
            else
            {
                if (EqualityComparer<T>.Default.Equals(value, default))
                {
                    return true;
                }

                if (Convert.IsDBNull(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified value is null.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value being checked.</param>
        /// <returns>A value indicating whether the specified value is null.</returns>
        [DebuggerStepThrough]
        public static bool IsNull<T>([CanBeNull] this T? value)
#pragma warning disable GCop615 // Negative logic is taxing on the brain. Use "{0} == null" instead.
            where T : struct => !value.HasValue;

#pragma warning restore GCop615 // Negative logic is taxing on the brain. Use "{0} == null" instead.

        /// <summary>
        ///     Turns any object into an exception.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The exception.</returns>
        [NotNull]
        public static Exception ToException([NotNull] this object obj) => new Exception(obj.ToString());

        /// <summary>
        ///     Convert an object to JSON.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns>The JSON representation of the object.</returns>
        [NotNull]
        public static string ToJson([NotNull] this object input) => JsonConvert.SerializeObject(input);

        /// <summary>
        ///     Converts this item to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item to be converted.</param>
        /// <param name="encoding">The string encoding.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The JSON string.</returns>
        [NotNull]
        public static string ToJson<T>(
            [NotNull] this T item,
            [CanBeNull] Encoding encoding = null,
            [CanBeNull] DataContractJsonSerializer serializer = null)
        {
            Contract.Requires(!EqualityComparer<T>.Default.Equals(item, default));
            Contract.Ensures(Contract.Result<string>() != null);

            encoding = encoding ?? Encoding.Default;
            serializer = serializer ?? new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, item);
                return encoding.GetString(stream.ToArray());
            }
        }

        /// <summary>
        ///     Serialize object to xml string by <see cref="XmlSerializer" />
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <param name="value">The input value.</param>
        /// <returns>The XML representation of the input value.</returns>
        [NotNull]
        public static string ToXml<T>([NotNull] this T value)
            where T : new()
        {
            Contract.Requires(!EqualityComparer<T>.Default.Equals(value, default));
            Contract.Ensures(Contract.Result<string>() != null);

            var serializer = Extensions.GetXmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            using (var writer = new XmlTextWriter(stream, new UTF8Encoding()))
            {
                serializer.Serialize(writer, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        ///     Serialize object to xml string by <see cref="XmlSerializer" />
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <param name="value">The input value.</param>
        /// <param name="stream">The output stream.</param>
        public static void ToXml<T>([NotNull] this T value, [NotNull] Stream stream)
            where T : new()
        {
            Contract.Requires(!EqualityComparer<T>.Default.Equals(value, default));
            Contract.Requires(stream != null);
            var serializer = Extensions.GetXmlSerializer(typeof(T));
            serializer.Serialize(stream, value);
        }

        /// <summary>
        ///     Serializes the input object to an XML string.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns>The XML string.</returns>
        [NotNull]
        public static string ToXml([NotNull] this object input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<string>() != null);

            // Serialize an object into an xml string
            var xmlSerializer = new XmlSerializer(input.GetType());

            // use new UTF8Encoding here, not Encoding.UTF8.
            using (var memoryStream = new MemoryStream())
            using (var xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding()))
            {
                xmlSerializer.Serialize(xmlTextWriter, input);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        ///     Gets the XML serializer for the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type handled by the serializer.</param>
        /// <returns>The <see cref="XmlSerializer" /> for the <paramref name="type" />.</returns>
        [CanBeNull]
        private static XmlSerializer GetXmlSerializer([NotNull] Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            if (Extensions.XmlSerializers.TryGetValue(type.TypeHandle, out var serializer))
            {
                return serializer;
            }

            lock (Extensions.XmlSerializers)
            {
                if (Extensions.XmlSerializers.TryGetValue(type.TypeHandle, out serializer))
                {
                    return serializer;
                }

                serializer = new XmlSerializer(type);
                Extensions.XmlSerializers.Add(type.TypeHandle, serializer);
            }

            return serializer;
        }
    }
}