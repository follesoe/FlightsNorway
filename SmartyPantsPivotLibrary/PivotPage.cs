using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SmartyPantsPivotLibrary
{
    public class PivotPage : Grid
    {
        #region PageTitle Dependency Property

        /// <summary>
        /// The <see cref="PageTitle" /> dependency property's name.
        /// </summary>
        public const string PageTitlePropertyName = "PageTitle";

        /// <summary>
        /// Gets or sets the value of the <see cref="PageTitle" />
        /// property. This is a dependency property.
        /// </summary>
        public string PageTitle
        {
            get
            {
                return (string)GetValue(PageTitleProperty);
            }
            set
            {
                SetValue(PageTitleProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="PageTitle" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty PageTitleProperty = DependencyProperty.Register(
            PageTitlePropertyName,
            typeof(string),
            typeof(PivotPage),
            new PropertyMetadata(string.Empty));

        #endregion

        #region Content Dependency Property

        /// <summary>
        /// The <see cref="Content" /> dependency property's name.
        /// </summary>
        public const string ContentPropertyName = "Content";

        /// <summary>
        /// Gets or sets the value of the <see cref="Content" />
        /// property. This is a dependency property.
        /// </summary>
        public object Content
        {
            get
            {
                return (object)GetValue(ContentProperty);
            }
            set
            {
                SetValue(ContentProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Content" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            ContentPropertyName,
            typeof(object),
            typeof(PivotPage),
            new PropertyMetadata(null));

        #endregion
    }
}
