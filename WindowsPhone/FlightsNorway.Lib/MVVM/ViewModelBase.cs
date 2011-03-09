// ****************************************************************************
// <copyright file="ViewModelBase.cs" company="GalaSoft Laurent Bugnion">
// Copyright © GalaSoft Laurent Bugnion 2009-2011
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>22.4.2009</date>
// <project>GalaSoft.MvvmLight</project>
// <web>http://www.galasoft.ch</web>
// <license>
// See license.txt in this project or http://www.galasoft.ch/license_MIT.txt
// </license>
// <LastBaseLevel>BL0009</LastBaseLevel>
// ****************************************************************************

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Linq.Expressions;

////using GalaSoft.Utilities.Attributes;

namespace FlightsNorway.Lib.MVVM
{
    /// <summary>
    /// A base class for the ViewModel classes in the MVVM pattern.
    /// <para>The IDisposable implementation of this class is obsolete, and
    /// should not be used anymore. It will be removed in a future version.
    /// </para>
    /// </summary>
    //// [ClassInfo(typeof(ViewModelBase),
    ////  VersionString = "4.0.0.0/BL0009",
    ////  DateString = "201102062240",
    ////  Description = "A base class for the ViewModel classes in the MVVM pattern.",
    ////  UrlContacts = "http://www.galasoft.ch/contact_en.html",
    ////  Email = "laurent@galasoft.ch")]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private static bool? _isInDesignMode;

        #if SILVERLIGHT
        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        [SuppressMessage(
            "Microsoft.Security",
            "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
            Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = DesignerProperties.IsInDesignTool;
                }
                return _isInDesignMode.Value;
            }
        }
        #endif

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Verifies that a property name exists in this ViewModel. This method
        /// can be called before the property is used, for instance before
        /// calling RaisePropertyChanged. It avoids errors when a property name
        /// is changed but some places are missed.
        /// <para>This method is only active in DEBUG mode.</para>
        /// </summary>
        /// <param name="propertyName"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            var myType = GetType();
            if (myType.GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="oldValue">The property's value before the change
        /// occurred.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue, T newValue, bool broadcast)
        {
            RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                return;
            }

            var handler = PropertyChanged;

            if (handler != null)
            {
                var body = propertyExpression.Body as MemberExpression;
                var expression = body.Expression as ConstantExpression;
                handler(expression.Value, new PropertyChangedEventArgs(body.Member.Name));
            }
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, T oldValue, T newValue, bool broadcast)
        {
            if (propertyExpression == null)
            {
                return;
            }

            var handler = PropertyChanged;

            if (handler != null
                || broadcast)
            {
                var body = propertyExpression.Body as MemberExpression;
                var expression = body.Expression as ConstantExpression;

                if (handler != null)
                {
                    handler(expression.Value, new PropertyChangedEventArgs(body.Member.Name));
                }
            }
        }

        protected virtual void RaisePropertyChanged()
        {
            var frames = new StackTrace();

            for (var i = 0; i < frames.FrameCount; i++)
            {
                var frame = frames.GetFrame(i).GetMethod() as MethodInfo;
                if (frame != null)
                    if (frame.IsSpecialName && frame.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase))
                    {
                        RaisePropertyChanged(frame.Name.Substring(4));
                        return;
                    }
            }

            throw new InvalidOperationException("RaisePropertyChanged() can only by invoked within a property setter.");
        }
    }
}