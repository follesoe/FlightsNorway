using System;
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
    public class PanoramaControl : ItemsControl
    {
        // child controls
        private const string LayoutRootName = "LayoutRoot";
        internal Panel LayoutRoot { get; set; }

        // scroll view
        private PanoramaView ScrollView;

        public PanoramaControl()
        {
            // apply default style
            this.DefaultStyleKey = typeof(PanoramaControl);

            // defaults selected item to none
            SelectedIndex = -1;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // get the parts
            LayoutRoot = GetTemplateChild(LayoutRootName) as Panel;

            // scroll view
            ScrollView = new PanoramaView(this);
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

            // start tracking and capture starting position
            _tracker = new ManipulationTracker(Orientation.Horizontal);
            _tracker.StartTracking(new Point(ScrollView.Position, 0.0));
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            // already handled ?
            if (e.Handled) return;

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
                ScrollView.Position = _tracker.Position.X;
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
            // _tracker.TrackManipulation(e.TotalManipulation.Translation);

            // get direction
            int direction = (int)Math.Sign(-e.FinalVelocities.LinearVelocity.X);

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

            // find out which item is at screen center
            double center = _tracker.Position.X + LayoutRoot.ActualWidth / 2;

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

            // item's start position
            int index = this.Items.GetIndexOfPosition(center);
            PanoramaItem item = (PanoramaItem)this.Items.GetItem(index);
            double start = this.Items.GetItemPosition(index);

            // close to left edge : snap left
            if (center - start < LayoutRoot.ActualWidth / 2)
            {
                ScrollView.ScrollTo(index, Snapping.SnapLeft);
            }

            // close to right edge : snap right
            else if (center - start > item.Width - LayoutRoot.ActualWidth / 2)
            {
                double end = start + item.Width;
                ScrollView.ScrollTo(index, Snapping.SnapRight);
            }

            // nowhere close to edges but
            // we're asked to snap anyways
            else if (item.AutoSnap)
            {
                ScrollView.ScrollTo(index);
            }
        }
#endif
        #endregion

        #region BackgroundImage
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register(
            "BackgroundImage",
            typeof(ImageSource),
            typeof(PanoramaControl),
            new PropertyMetadata(OnBackgroundImageChanged));

        public static readonly DependencyProperty BackgroundImageTemplateProperty =
            DependencyProperty.Register(
            "BackgroundImageTemplate",
            typeof(DataTemplate),
            typeof(PanoramaControl),
            null);

        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        private static void OnBackgroundImageChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PanoramaControl ctrl = (PanoramaControl)sender;
            ctrl.OnBackgroundImageChanged(e.OldValue, e.NewValue);
        }

        public virtual void OnBackgroundImageChanged(object oldValue, object newValue)
        {
        }

        public DataTemplate BackgroundImageTemplate
        {
            get { return (DataTemplate)GetValue(BackgroundImageTemplateProperty); }
            set { SetValue(BackgroundImageTemplateProperty, value); }
        }
        #endregion

        #region Title
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(PanoramaControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(
            "TitleTemplate",
            typeof(DataTemplate),
            typeof(PanoramaControl),
            null);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PanoramaControl ctrl = (PanoramaControl)sender;
            ctrl.OnTitleChanged(e.OldValue, e.NewValue);
        }

        public virtual void OnTitleChanged(object oldValue, object newValue)
        {
        }

        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }
        #endregion

        #region DefaultItemWidth Property
        public static readonly DependencyProperty DefaultItemWidthProperty = DependencyProperty.Register(
            "DefaultItemWidth",
            typeof(double),
            typeof(PanoramaControl),
            new PropertyMetadata(OnDefaultItemWidthChanged));

        public double DefaultItemWidth
        {
            get { return (double)GetValue(DefaultItemWidthProperty); }
            set { SetValue(DefaultItemWidthProperty, value); }
        }

        private static void OnDefaultItemWidthChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PanoramaControl ctrl = (PanoramaControl)sender;
            ctrl.UpdateItemWidthAndHeight();
        }
        #endregion

        #region DefaultItemHeight Property
        public static readonly DependencyProperty DefaultItemHeightProperty = DependencyProperty.Register(
            "DefaultItemHeight",
            typeof(double),
            typeof(PanoramaControl),
            new PropertyMetadata(OnDefaultItemHeightChanged));

        public double DefaultItemHeight
        {
            get { return (double)GetValue(DefaultItemHeightProperty); }
            set { SetValue(DefaultItemHeightProperty, value); }
        }

        private static void OnDefaultItemHeightChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PanoramaControl ctrl = (PanoramaControl)sender;
            ctrl.UpdateItemWidthAndHeight();
        }
        #endregion

        #region Items
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Replace:
                    // fixup all items default width and height
                    UpdateItemWidthAndHeight();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
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

        private void UpdateItemWidthAndHeight()
        {
            foreach (object o in this.Items)
            {
                PanoramaItem item = o as PanoramaItem;
                if (null != item)
                {
                    // reset width for each item
                    double width = (double)item.GetValue(PanoramaItem.WidthProperty);
                    if (double.IsNaN(width)) width = 0;
                    item.SetValue(PanoramaItem.WidthProperty, Math.Max(width, DefaultItemWidth));

                    // reset height for each item
                    double height = (double)item.GetValue(PanoramaItem.HeightProperty);
                    if (double.IsNaN(height)) height = 0;
                    item.SetValue(PanoramaItem.HeightProperty, Math.Max(height, DefaultItemHeight));
                }
            }
        }
        #endregion

        #region SelectedItem
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(PanoramaItem),
            typeof(PanoramaControl),
            new PropertyMetadata(OnSelectedItemChanged));

        public PanoramaItem SelectedItem
        {
            get { return (PanoramaItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // this is a wrapper method for selection changes
            // the real work occurs in OnSelectedIndexChanges

            PanoramaControl ctrl = (PanoramaControl)d;
            PanoramaItem oldItem = (PanoramaItem)e.OldValue;
            PanoramaItem newItem = (PanoramaItem)e.NewValue;

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
            typeof(PanoramaControl),
            new PropertyMetadata(OnSelectedIndexChanged));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PanoramaControl ctrl = (PanoramaControl)d;
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
            ctrl.SelectedItem = (PanoramaItem)ctrl.Items.GetItem(newIndex);

            // change visuals
            ctrl.MoveTo(ctrl.SelectedIndex);

            // raise event
            PanoramaItem oldItem = (PanoramaItem)ctrl.Items.GetItem(oldIndex);
            PanoramaItem newItem = (PanoramaItem)ctrl.Items.GetItem(newIndex);
            SelectionChangedEventArgs args = new SelectionChangedEventArgs(
                (null == oldItem) ? new List<PanoramaItem> { } : new List<PanoramaItem> { oldItem },
                (null == newItem) ? new List<PanoramaItem> { } : new List<PanoramaItem> { newItem });

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
