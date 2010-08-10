using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Collections.Specialized;


namespace Phone.Controls.Samples
{
    [ContentPropertyAttribute("Items")]
    public class PivotControl : ItemsControl
    {
        // child controls
        private const string LayoutRootName = "LayoutRoot";
        internal Panel LayoutRoot { get; set; }
        internal StackPanel HeadersPanel;

        // scroll view
        private PivotView ScrollView;

        public PivotControl()
        {
            // apply default style
            this.DefaultStyleKey = typeof(PivotControl);

            // create the headers panel
            HeadersPanel = new StackPanel();
            HeadersPanel.Orientation = Orientation.Horizontal;

            // defaults selected item to none
            SelectedIndex = -1;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // get the parts
            LayoutRoot = GetTemplateChild(LayoutRootName) as Panel;

            // scroll view
            ScrollView = new PivotView(this);
            ScrollView.ScrollCompleted += new ScrollCompletedEventHandler(ScrollView_ScrollCompleted);

            // control events
            SizeChanged += new SizeChangedEventHandler(OnSizeChanged);
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // clip to control layout
            LayoutRoot.SetValue(Panel.ClipProperty, new RectangleGeometry() { Rect = new Rect(0, 0, this.Width, this.Height) });

            // reset scroll viewer
            Dispatcher.BeginInvoke(() =>
            {
                ScrollView.Invalidate(false);
            });
        }

        public void Invalidate()
        {
            ScrollView.Invalidate();
        }

        #region Navigation
        public void ScrollPrev()
        {
            if (null != ScrollView)
            {
                // complete current animation
                ScrollView.ScrollSkip();

                // move to previous item
                ScrollView.ScrollTo(this.SelectedIndex - 1);
            }
        }

        public void ScrollNext()
        {
            if (null != ScrollView)
            {
                // complete current animation
                ScrollView.ScrollSkip();

                // move to next item
                ScrollView.ScrollTo(this.SelectedIndex + 1);
            }
        }

        private void MoveTo(int index)
        {
            if (null != ScrollView)
            {
                // complete current animation
                ScrollView.ScrollSkip();

                // move to item
                ScrollView.MoveTo(index);
            }
        }

        void ScrollView_ScrollCompleted(object sender, ScrollCompletedEventArgs e)
        {
            // find out where we landed
            SelectedIndex = e.SelectedIndex;

            // special case for when we only have 1 item :
            // the above code will not trigger the SelectedIndex change
            // since we'll be staying on item(0).
            if (this.Items.Count == 1)
            {
                // reset visuals
                MoveTo(SelectedIndex);
            }
        }
        #endregion

        #region Keyboard events
        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Left:
                    ScrollPrev();
                    break;
                case Key.Down:
                case Key.Right:
                    ScrollNext();
                    break;
            }
        }
        #endregion

        #region Multitouch events
#if WINDOWS_PHONE
        ManipulationHook _hook = new ManipulationHook();
        ManipulationTracker _tracker;
        bool _trackHeader;

        private int GetHeadersIndexOf(UIElement ui)
        {
            UIElementCollection Headers = HeadersPanel.Children;

            while (null != ui)
            {
                // UI element is ContentPresenter
                if (ui.GetType() == typeof(ContentPresenter))
                {
                    // find it's index in Headers panel
                    int index = Headers.IndexOf(ui);
                    if (index != -1) return index;
                }

                // UI element in Headers panel host
                if (ui == HeadersPanel.Parent)
                {
                    // move past last item
                    return Items.Count;
                }

                ui = VisualTreeHelper.GetParent(ui) as FrameworkElement;
            }

            return -1;
        }

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            // manipulation events hook
            _hook.Hook(e.ManipulationContainer);

            // some controls just swallow the manipulation changes events
            // this is true with the ScrollViewer inside a ListBox.
            // register to ManipulationDelta of the source object
            // so we receive this event regardless of the parent implementation.
            _hook.HookDeltaHandler(OnManipulationDelta);

            // skip current animation (if any)
            ScrollView.ScrollSkip();

            // tracking Headers or Items ?
            _trackHeader = (GetHeadersIndexOf(e.ManipulationContainer) != -1);

            // start tracking and capture starting position
            if (_trackHeader)
            {
                // stop tracking Headers when 1/4 of screen size
                // track Headers and Items position
                _tracker = new ManipulationTracker(Orientation.Horizontal);
                _tracker.StartTracking(new Point(ScrollView.HeaderPosition, ScrollView.Position));
            }
            else
            {
                // stop tracking Items when 1/2 of screen size
                // track Items and Headers position
                _tracker = new ManipulationTracker(Orientation.Horizontal);
                _tracker.StartTracking(new Point(ScrollView.Position, ScrollView.HeaderPosition));
            }

            // 1 or less items, just cancel tracking
            if (Items.Count <= 1)
                _tracker.CancelTracking();
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            // manipulation canceled
            if (_tracker.Canceled) return;

            if (_tracker.TrackManipulation(e.CumulativeManipulation.Translation))
            {
                // mark as handled to stop
                // underlying control's manipulations
                e.Handled = true;

                // cancel capture from current object to disable click behavior,
                // if we've started scrolling. Not sure what the best technique is.
                // let's just hook/override OnManipulationCompleted
                // and force e.Handled on it for now...
                _hook.HookCompletedHandler(OnManipulationCompleted);

                // move to position
                double position = _tracker.Start.X - e.CumulativeManipulation.Translation.X;
                if (_trackHeader)
                {
                    // tracking Headers : Items move twice as fast
                    ScrollView.HeaderPosition = _tracker.Start.X - _tracker.Delta.X;
                    ScrollView.Position = _tracker.Start.Y - (_tracker.Delta.X * 2);
                }
                else
                {
                    // tracking Items : Headers move twice as slow
                    ScrollView.HeaderPosition = _tracker.Start.Y - (_tracker.Delta.X / 2);
                    ScrollView.Position = _tracker.Start.X - _tracker.Delta.X;
                }
            }

            // complete manipulation
            if (_tracker.Completed) e.Complete();
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            // manipulation canceled
            if (_tracker.Canceled) return;

            // end tracking
            // BUGBUG : disable due to empty e.TotalManipulation in #6326
            //_tracker.TrackManipulation(e.TotalManipulation.Translation);

            // get direction
            int direction = 0;
            if (null != e.FinalVelocities)
                direction = (int)Math.Sign(-e.FinalVelocities.LinearVelocity.X);

            // move previous/next
            switch (direction)
            {
                case -1:
                    ScrollPrev();
                    return;
                case 1:
                    ScrollNext();
                    return;
            }

            // not moving at all ?
            // let's find out if a header was clicked
            if (_tracker.Delta.X == 0)
            {
                int index = GetHeadersIndexOf(e.ManipulationContainer);
                if (index != -1)
                {
                    ScrollView.ScrollTo(index);
                    return;
                }
            }

            // find out which item is at screen center
            double position = _trackHeader ? _tracker.Start.Y - _tracker.Delta.X * 2 : _tracker.Position.X;
            double center = position + LayoutRoot.ActualWidth / 2;

            // cycle back to last item ?
            if (center < 0)
            {
                ScrollPrev();
                return;
            }

            // cycle back to first item ?
            if (center > this.Items.GetTotalWidth())
            {
                ScrollNext();
                return;
            }

            // scroll to item
            ScrollView.ScrollTo(this.Items.GetIndexOfPosition(center));
        }
#endif
        #endregion

        #region Title
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(object),
                typeof(PivotControl),
                new PropertyMetadata(OnTitleChanged));

        public object Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotControl ctrl = (PivotControl)d;
            ctrl.OnTitleChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnTitleChanged(object oldHeader, object newHeader)
        {
        }
        #endregion Title

        #region DefaultItemWidth Property
        public static readonly DependencyProperty DefaultItemWidthProperty = DependencyProperty.Register(
            "DefaultItemWidth",
            typeof(double),
            typeof(PivotControl),
            new PropertyMetadata(OnDefaultItemWidthChanged));

        public double DefaultItemWidth
        {
            get { return (double)GetValue(DefaultItemWidthProperty); }
            set { SetValue(DefaultItemWidthProperty, value); }
        }

        private static void OnDefaultItemWidthChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PivotControl ctrl = (PivotControl)sender;
            ctrl.UpdateItemWidth();
        }

        private void UpdateItemWidth()
        {
            foreach (object o in this.Items)
            {
                PivotItem item = o as PivotItem;
                if (null != item)
                {
                    // reset width for each item
                    double width = (double)item.GetValue(PivotItem.WidthProperty);
                    if (double.IsNaN(width)) width = 0;
                    item.SetValue(PivotItem.WidthProperty, Math.Max(width, DefaultItemWidth));
                }
            }
        }
        #endregion

        #region DefaultItemHeight Property
        public static readonly DependencyProperty DefaultItemHeightProperty = DependencyProperty.Register(
            "DefaultItemHeight",
            typeof(double),
            typeof(PivotControl),
            new PropertyMetadata(OnDefaultItemHeightChanged));

        public double DefaultItemHeight
        {
            get { return (double)GetValue(DefaultItemHeightProperty); }
            set { SetValue(DefaultItemHeightProperty, value); }
        }

        private static void OnDefaultItemHeightChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PivotControl ctrl = (PivotControl)sender;
            ctrl.UpdateItemHeight();
        }

        private void UpdateItemHeight()
        {
            foreach (object o in this.Items)
            {
                PivotItem item = o as PivotItem;
                if (null != item)
                {
                    // reset height for each item
                    double height = (double)item.GetValue(PivotItem.HeightProperty);
                    if (double.IsNaN(height)) height = 0;
                    item.SetValue(PivotItem.HeightProperty, Math.Max(height, DefaultItemHeight));
                }
            }
        }
        #endregion

        #region TitleTemplate
        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(
                "TitleTemplate",
                typeof(DataTemplate),
                typeof(PivotControl),
                new PropertyMetadata(OnTitleTemplateChanged));

        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }

        private static void OnTitleTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotControl ctrl = (PivotControl)d;
            ctrl.OnTitleTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnTitleTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }
        #endregion TitlTemplate

        #region UnselectedHeaderTemplate
        public static readonly DependencyProperty UnselectedHeaderTemplateProperty =
            DependencyProperty.Register(
                "UnselectedHeaderTemplate",
                typeof(DataTemplate),
                typeof(PivotControl),
                new PropertyMetadata(OnUnselectedHeaderTemplateChanged));

        public DataTemplate UnselectedHeaderTemplate
        {
            get { return (DataTemplate)GetValue(UnselectedHeaderTemplateProperty); }
            set { SetValue(UnselectedHeaderTemplateProperty, value); }
        }

        private static void OnUnselectedHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotControl ctrl = (PivotControl)d;
            ctrl.OnUnselectedHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnUnselectedHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }
        #endregion UnselectedHeaderTemplate

        #region SelectedHeaderTemplate
        public static readonly DependencyProperty SelectedHeaderTemplateProperty =
            DependencyProperty.Register(
                "SelectedHeaderTemplate",
                typeof(DataTemplate),
                typeof(PivotControl),
                new PropertyMetadata(OnSelectedHeaderTemplateChanged));

        public DataTemplate SelectedHeaderTemplate
        {
            get { return (DataTemplate)GetValue(SelectedHeaderTemplateProperty); }
            set { SetValue(SelectedHeaderTemplateProperty, value); }
        }

        private static void OnSelectedHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotControl ctrl = (PivotControl)d;
            ctrl.OnSelectedHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnSelectedHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }
        #endregion SelectedHeaderTemplate

        #region Items
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Replace:
                    // fixup all items default width
                    UpdateItemWidth();
                    UpdateItemHeight();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
            }

            // reset headers
            UIElementCollection Headers = HeadersPanel.Children;
            Headers.Clear();
            foreach (PivotItem item in Items)
            {
                ContentPresenter ctrl = new ContentPresenter();
                ctrl.Content = item.Header;
                ctrl.ContentTemplate = this.UnselectedHeaderTemplate;
                ctrl.UpdateLayout();
                Headers.Add(ctrl);
            }

            // invalidate the control
            if (null != ScrollView)
                ScrollView.Invalidate();

            // first inserted ?
            if ((SelectedIndex < 0) && (Items.Count > 0))
                SelectedIndex = 0;

            // invalid selected item ?
            if (!Items.Contains(SelectedItem) && (Items.Count > 0))
                SelectedIndex = 0;
        }
        #endregion

        #region SelectedItem
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(PivotItem),
            typeof(PivotControl),
            new PropertyMetadata(OnSelectedItemChanged));

        public PivotItem SelectedItem
        {
            get { return (PivotItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // this is a wrapper method for selection changes
            // the real work occurs in OnSelectedIndexChanges

            PivotControl ctrl = (PivotControl)d;
            PivotItem oldItem = (PivotItem)e.OldValue;
            PivotItem newItem = (PivotItem)e.NewValue;

            // find out the index for new items
            int index = ctrl.Items.IndexOf(newItem);

            // none found or null : return to old
            if ((null == newItem) || (index == -1))
                index = ctrl.Items.IndexOf(oldItem);

            // change selection
            ctrl.SelectedIndex = index;
        }
        #endregion

        #region SelectedIndex
        public event SelectionChangedEventHandler SelectionChanged;

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(PivotControl),
            new PropertyMetadata(OnSelectedIndexChanged));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotControl ctrl = (PivotControl)d;
            int oldIndex = (int)e.OldValue;
            int newIndex = (int)e.NewValue;

            // nothing to do
            if (newIndex == oldIndex)
            {
                // exit here, to avoid infinite recursion
                return;
            }

            // change selection
            ctrl.SelectedIndex = newIndex;
            ctrl.SelectedItem = (PivotItem)ctrl.Items.GetItem(newIndex);

            // change visuals
            ctrl.MoveTo(ctrl.SelectedIndex);

            // raise event
            PivotItem oldItem = (PivotItem)ctrl.Items.GetItem(oldIndex);
            PivotItem newItem = (PivotItem)ctrl.Items.GetItem(newIndex);
            SelectionChangedEventArgs args = new SelectionChangedEventArgs(
                (null == oldItem) ? new List<PivotItem> { } : new List<PivotItem> { oldItem },
                (null == newItem) ? new List<PivotItem> { } : new List<PivotItem> { newItem });

            ctrl.OnSelectionChanged(args);
        }

        protected virtual void OnSelectionChanged(SelectionChangedEventArgs args)
        {
            if (null != SelectionChanged)
                SelectionChanged(this, args);
        }
        #endregion
    }
}
