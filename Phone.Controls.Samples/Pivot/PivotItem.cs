using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;

namespace Phone.Controls.Samples
{
    [ContentPropertyAttribute("Content")]
    public class PivotItem : ContentControl
    {
        public PivotItem()
            : base()
        {
            this.DefaultStyleKey = typeof(PivotItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #region Title
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(object),
                typeof(PivotItem),
                new PropertyMetadata(OnTitleChanged));

        public object Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotItem ctrl = (PivotItem)d;
            ctrl.OnTitleChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnTitleChanged(object oldHeader, object newHeader)
        {
        }
        #endregion Title

        #region Header
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(object),
                typeof(PivotItem),
                new PropertyMetadata(OnHeaderChanged));

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotItem ctrl = (PivotItem)d;
            ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
        {
        }
        #endregion Header
    }
}
