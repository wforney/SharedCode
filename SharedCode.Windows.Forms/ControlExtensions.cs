// <copyright file="ControlExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.Forms
{
    using System;
    using System.Linq.Expressions;
    using System.Windows.Forms;

    /// <summary>
    ///     The Windows Forms control extensions class
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        ///     The invoke handler delegate.
        /// </summary>
        public delegate void InvokeHandler();

        /// <summary>
        ///     Databinding with strongly typed object names
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TDataSourceItem">The type of the data source item.</typeparam>
        /// <param name="control">The control you are binding to</param>
        /// <param name="controlProperty">The property on the control you are binding to</param>
        /// <param name="dataSource">The object you are binding to</param>
        /// <param name="dataSourceProperty">The property on the object you are binding to</param>
        /// <returns>The Binding.</returns>
        public static Binding Bind<TControl, TDataSourceItem>(
            this TControl control,
            Expression<Func<TControl, object>> controlProperty,
            object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty)
            where TControl : Control
        {
            return control.DataBindings.Add(
                PropertyName.For(controlProperty),
                dataSource,
                PropertyName.For(dataSourceProperty));
        }

        /// <summary>
        ///     Binds the specified control property.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TDataSourceItem">The type of the data source item.</typeparam>
        /// <param name="control">The control you are binding to.</param>
        /// <param name="controlProperty">The control property.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dataSourceProperty">The data source property.</param>
        /// <param name="updateMode">The update mode.</param>
        /// <param name="formattingEnabled">if set to <c>true</c> formatting is enabled.</param>
        /// <returns>The Binding.</returns>
        public static Binding Bind<TControl, TDataSourceItem>(
            this TControl control,
            Expression<Func<TControl, object>> controlProperty,
            object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty,
            DataSourceUpdateMode updateMode,
            bool formattingEnabled)
            where TControl : Control
        {
            return control.DataBindings.Add(
                PropertyName.For(controlProperty),
                dataSource,
                PropertyName.For(dataSourceProperty),
                formattingEnabled,
                updateMode);
        }

        /// <summary>
        ///     Binds the specified control property.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TDataSourceItem">The type of the data source item.</typeparam>
        /// <param name="control">The control we are binding to.</param>
        /// <param name="controlProperty">The control property.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dataSourceProperty">The data source property.</param>
        /// <param name="updateMode">The update mode.</param>
        /// <param name="nullValue">The null value.</param>
        /// <param name="formattingEnabled">if set to <c>true</c> formatting is enabled.</param>
        /// <returns>The Binding.</returns>
        public static Binding Bind<TControl, TDataSourceItem>(
            this TControl control,
            Expression<Func<TControl, object>> controlProperty,
            object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty,
            DataSourceUpdateMode updateMode,
            object nullValue,
            bool formattingEnabled)
            where TControl : Control
        {
            return control.DataBindings.Add(
                PropertyName.For(controlProperty),
                dataSource,
                PropertyName.For(dataSourceProperty),
                formattingEnabled,
                updateMode,
                nullValue);
        }

        /// <summary>
        ///     Binds the specified control property.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TDataSourceItem">The type of the data source item.</typeparam>
        /// <param name="control">The control we are binding to.</param>
        /// <param name="controlProperty">The control property.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dataSourceProperty">The data source property.</param>
        /// <param name="updateMode">The update mode.</param>
        /// <param name="nullValue">The null value.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="formattingEnabled">if set to <c>true</c> formatting is enabled.</param>
        /// <returns>The Binding.</returns>
        public static Binding Bind<TControl, TDataSourceItem>(
            this TControl control,
            Expression<Func<TControl, object>> controlProperty,
            object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty,
            DataSourceUpdateMode updateMode,
            object nullValue,
            string formatString,
            bool formattingEnabled)
            where TControl : Control
        {
            return control.DataBindings.Add(
                PropertyName.For(controlProperty),
                dataSource,
                PropertyName.For(dataSourceProperty),
                formattingEnabled,
                updateMode,
                nullValue,
                formatString);
        }

        /// <summary>
        ///     Binds the specified control property.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TDataSourceItem">The type of the data source item.</typeparam>
        /// <param name="control">The control we are binding to.</param>
        /// <param name="controlProperty">The control property.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dataSourceProperty">The data source property.</param>
        /// <param name="updateMode">The update mode.</param>
        /// <param name="nullValue">The null value.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="formatInfo">The format information.</param>
        /// <param name="formattingEnabled">if set to <c>true</c> formatting is enabled.</param>
        /// <returns>The Binding.</returns>
        public static Binding Bind<TControl, TDataSourceItem>(
            this TControl control,
            Expression<Func<TControl, object>> controlProperty,
            object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty,
            DataSourceUpdateMode updateMode,
            object nullValue,
            string formatString,
            IFormatProvider formatInfo,
            bool formattingEnabled)
            where TControl : Control
        {
            return control.DataBindings.Add(
                PropertyName.For(controlProperty),
                dataSource,
                PropertyName.For(dataSourceProperty),
                formattingEnabled,
                updateMode,
                nullValue,
                formatString,
                formatInfo);
        }

        /// <summary>
        ///     Safely invokes the specified handler.
        /// </summary>
        /// <param name="control">The Windows forms control.</param>
        /// <param name="handler">The handler to invoke.</param>
        public static void SafeInvoke(this Control control, InvokeHandler handler)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(handler);
            }
            else
            {
                handler?.Invoke();
            }
        }

        /// <summary>
        ///     The property name class
        /// </summary>
        public static class PropertyName
        {
            /// <summary>
            ///     Gets the name for the specified property.
            /// </summary>
            /// <typeparam name="T">The type of the object the property is on.</typeparam>
            /// <param name="property">The property expression.</param>
            /// <returns>The name of the property.</returns>
            public static string For<T>(Expression<Func<T, object>> property)
            {
                var member = property.Body as MemberExpression;
                if (member == null && property.Body is UnaryExpression unary)
                {
                    member = unary.Operand as MemberExpression;
                }

                return member?.Member.Name ?? string.Empty;
            }
        }
    }
}