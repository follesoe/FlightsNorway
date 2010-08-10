using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;

namespace Phone.Controls.Samples
{
    [TemplatePart(Name = "PanoramaHeader", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PanoramaContent", Type = typeof(ContentControl))]
    [ContentPropertyAttribute("Content")]
    public class PanoramaItem : ContentControl
    {
        public PanoramaItem()
            : base()
        {
            this.DefaultStyleKey = typeof(PanoramaItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #region Header
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(object),
                typeof(PanoramaItem),
                new PropertyMetadata(OnHeaderChanged));

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PanoramaItem ctrl = (PanoramaItem)d;
            ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
        {
        }

        private void UpdateHeaderVisuals()
        {
            ContentControl header = this.GetTemplateChild("PanoramaHeader") as ContentControl;
            if (header != null)
            {
                header.Content = this.Header;
                header.ContentTemplate = this.HeaderTemplate;
            }
        }
        #endregion Header

        #region HeaderTemplate
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                "HeaderTemplate",
                typeof(DataTemplate),
                typeof(PanoramaItem),
                new PropertyMetadata(OnHeaderTemplateChanged));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PanoramaItem ctrl = (PanoramaItem)d;
            ctrl.OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }
        #endregion HeaderTemplate

        #region AutoSnap
        public static readonly DependencyProperty AutoSnapProperty =
            DependencyProperty.Register(
                "AutoSnap",
                typeof(bool),
                typeof(PanoramaItem),
                new PropertyMetadata(OnAutoSnapChanged));

        public bool AutoSnap
        {
            get { return (bool)GetValue(AutoSnapProperty); }
            set { SetValue(AutoSnapProperty, value); }
        }

        private static void OnAutoSnapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PanoramaItem ctrl = (PanoramaItem)d;
            ctrl.OnAutoSnapChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnAutoSnapChanged(bool oldValue, object newValue)
        {
        }
        #endregion Header
    }
}
