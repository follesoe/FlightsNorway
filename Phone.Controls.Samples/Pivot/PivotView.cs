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
using System.Windows.Media.Imaging;

namespace Phone.Controls.Samples
{
    /// <summary>
    /// Helper class for PivotControl.
    /// Implements the UI logic.
    /// </summary>
    internal class PivotView
    {
        private const string HeadersPanelHostName = "HeadersPanelHost";
        private const string ItemsPanelName = "ItemsPanelHost";

        private PivotControl Parent;
        private Panel LayoutRoot;
        private Panel HeadersPanel;
        private Panel ItemsPanel;
        private UIElementCollection Headers;
        private ItemCollection Items;

        private ScrollHost HeadersHost;
        private ScrollHost ItemsHost;

        private const double AnimationDuration = 1000.0;
        private Storyboard Storyboard;
        public event ScrollCompletedEventHandler ScrollCompleted;

        private bool _ready = false;
        private int _newindex = -1;

        public PivotView(PivotControl parent)
        {
            Parent = parent;
            LayoutRoot = Parent.LayoutRoot;
            HeadersPanel = Parent.HeadersPanel;

            Headers = HeadersPanel.Children;
            Items = Parent.Items;

            HeadersHost.Transform = new TranslateTransform();
            ItemsHost.Transform = new TranslateTransform();
        }

        private void Initialize()
        {
            if (!_ready)
            {
                // fetch template elements
                Panel HeadersPanelHost = LayoutRoot.FindName(HeadersPanelHostName) as Panel;
                ItemsPanel = LayoutRoot.FindName(ItemsPanelName) as Panel;
                if (null == HeadersPanelHost) throw new ArgumentException(string.Format("Cannot find {0}.", HeadersPanelHostName));
                if (null == ItemsPanel) throw new ArgumentException(string.Format("Cannot find {0}.", ItemsPanelName));

                // reset scrollhosts
                HeadersHost.Reset();
                ItemsHost.Reset();

                // create transforms
                HeadersPanelHost.RenderTransform = HeadersHost.Transform;
                ItemsPanel.RenderTransform = ItemsHost.Transform;

                // reset/initialize layout
                HeadersPanelHost.Children.Clear();
                HeadersPanelHost.Children.Add(HeadersPanel);
                HeadersPanelHost.SetValue(Canvas.LeftProperty, 0.0);

                if (Headers.Count > 1)
                {
                    // unselect all headers
                    UpdateHeadersVisuals(-1);
                    HeadersPanel.UpdateLayout();
                    
                    // insert left/right bitmap duplicates
                    double width = HeadersPanel.ActualWidth;
                    double height = HeadersPanel.ActualHeight;
                    WriteableBitmap bitmap = new WriteableBitmap((int)width, (int)height);
                    bitmap.Render(HeadersPanel, null);
                    bitmap.Invalidate();
                    Image image;

                    // duplicate left
                    image = new Image();
                    image.Source = bitmap;
                    image.CacheMode = new BitmapCache();
                    HeadersPanelHost.Children.Insert(0, image);
                    double offset = bitmap.PixelWidth;

                    // duplicate right
                    image = new Image();
                    image.Source = bitmap;
                    image.CacheMode = new BitmapCache();
                    HeadersPanelHost.Children.Add(image);

                    // adjust panel position
                    HeadersPanelHost.SetValue(Canvas.LeftProperty, -offset);
                }

                // select current header
                int index = Parent.SelectedIndex;
                UpdateHeadersVisuals(index);
                UpdateVisuals(index);

                // done
                _ready = true;
            }
        }

        public void Invalidate(bool lazy = true)
        {
            _ready = false;

            // reset now ?
            if (!lazy) Initialize();
        }

        public void UpdateVisuals(int index)
        {
            PivotItem item = (PivotItem)Items.GetItem(index);
            if (null != item)
            {
                // reset items panel with selected item
                UIElementCollection items = ItemsPanel.Children;
                items.Clear();
                items.Add(item);

                // update item position and opacity
                item.SetValue(Canvas.LeftProperty, Items.GetItemPosition(index));
                item.Opacity = 1.0;

                // update title
                Parent.Title = item.Title;
            }
        }

        private void UpdateVisuals(int index, double pos)
        {
            PivotItem item = (PivotItem)Items.GetItem(index);
            if (null != item)
            {
                // add item to items panel
                UIElementCollection items = ItemsPanel.Children;
                if (!items.Contains(item))
                    items.Add(item);

                // update item position
                item.SetValue(Canvas.LeftProperty, pos);

                // update title
                Parent.Title = item.Title;
            }
        }

        public void UpdateHeadersVisuals(int index)
        {
            // reset header template for each item
            foreach (ContentPresenter item in Headers)
            {
                item.ContentTemplate = Parent.UnselectedHeaderTemplate;
            }

            // highlight selected header template
            if ((index >= 0) && (index < Headers.Count))
            {
                ContentPresenter item0 = (ContentPresenter)Headers[index];
                item0.ContentTemplate = Parent.SelectedHeaderTemplate;
            }
        }

        public void MoveTo(int index)
        {
            Initialize();

            // nothing to do
            if (Items.Count == 0)
                return;

            // fix-up values
            if (index < 0) index = 0;
            if (index >= Items.Count) index = Items.Count - 1;

            // reset selected item template
            UpdateVisuals(index);
            UpdateHeadersVisuals(index);

            // move to new position
            HeaderPosition = Headers.GetItemPosition(index);
            Position = Items.GetItemPosition(index);
        }

        public void ScrollTo(int index)
        {
            Initialize();

            // nothing to do
            if (Items.Count == 0)
                return;

            // animate to new position
            this.AnimateStart(index, AnimationDuration);
        }

        public void ScrollSkip()
        {
            Initialize();

            // nothing to do
            if (Items.Count == 0)
                return;

            // storyboard not completed yet
            // speed it up
            if ((null != Storyboard) &&
                (Storyboard.GetCurrentState() == ClockState.Active))
            {
                Storyboard.SkipToFill();
                Storyboard_Completed(Storyboard, new EventArgs());
                Storyboard = null;
            }
        }

        public double HeaderPosition
        {
            get
            {
                Initialize();

                return -HeadersHost.Transform.X / HeadersHost.Speed;
            }
            set
            {
                Initialize();

                // nothing to do
                if (Items.Count == 0)
                    return;

                // complete current animation
                ScrollSkip();

                // adjust transforms
                HeadersHost.Transform.X = -value * HeadersHost.Speed;
            }
        }

        public double Position
        {
            get
            {
                Initialize();

                return -ItemsHost.Transform.X / ItemsHost.Speed;
            }
            set
            {
                Initialize();

                // nothing to do
                if (Items.Count == 0)
                    return;

                // complete current animation
                ScrollSkip();

                // adjust transforms
                ItemsHost.Transform.X = -value * ItemsHost.Speed;
            }
        }

        private void AnimateStart(int index, double milliseconds = 0)
        {
            Initialize();

            // items positions
            double offsetHeaders = 0;
            double offsetItems = 0;

            // items
            PivotItem item = (PivotItem)Items.GetItem(Parent.SelectedIndex);
            PivotItem itemX = null;

            // item limits
            int index0 = 0;
            int indexN = Items.Count - 1;

            // back to last
            if (index < index0)
            {
                HeaderPosition += HeadersPanel.ActualWidth;
                offsetHeaders = Headers.GetItemPosition(indexN);
                offsetItems = Items.GetItemPosition(index0) - Items.GetItemWidth(indexN);
                offsetItems = Position - Items.GetItemWidth(indexN);
                UpdateVisuals(indexN, offsetItems);
                UpdateHeadersVisuals(indexN);
                index = indexN;
                itemX = (PivotItem)Items.GetItem(indexN);
            }
            // back to first
            else if (index > indexN)
            {
                HeaderPosition -= HeadersPanel.ActualWidth;
                offsetHeaders = Headers.GetItemPosition(index0);
                //offsetItems = Items.GetItemPosition(indexN) + Items.GetItemWidth(indexN);
                offsetItems = Position + Items.GetItemWidth(indexN);
                UpdateVisuals(index0, offsetItems);
                UpdateHeadersVisuals(index0);
                index = index0;
                itemX = (PivotItem)Items.GetItem(index0);
            }
            // normal scroll
            else
            {
                offsetHeaders = Headers.GetItemPosition(index);
                //offsetItems = Items.GetItemPosition(index);
                offsetItems = Position - Items.GetItemPosition(Parent.SelectedIndex) + Items.GetItemPosition(index);
                UpdateVisuals(index, offsetItems);
                UpdateHeadersVisuals(index);
                itemX = (PivotItem)Items.GetItem(index);
            }

            // target index
            // after animation is completed
            _newindex = index;

            // start a new storyboard
            Storyboard = new Storyboard();
            Storyboard.Completed += new EventHandler(Storyboard_Completed);
            Storyboard.Children.Add(CreateAnimation(HeadersHost.Transform, TranslateTransform.XProperty, -offsetHeaders, milliseconds));
            Storyboard.Children.Add(CreateAnimation(ItemsHost.Transform, TranslateTransform.XProperty, -offsetItems, milliseconds));
            if (item != itemX)
            {
                item.Opacity = 1.0;
                itemX.Opacity = 0.0;
                Storyboard.Children.Add(CreateAnimation(item, UIElement.OpacityProperty, 0.0, milliseconds));
                Storyboard.Children.Add(CreateAnimation(itemX, UIElement.OpacityProperty, 1.0, milliseconds));
            }
            Storyboard.Begin();

            // update title
            Parent.Title = itemX.Title;
        }

        public DoubleAnimation CreateAnimation(DependencyObject obj, DependencyProperty prop, double value, double milliseconds, EasingMode easing = EasingMode.EaseOut)
        {
            CubicEase ease = new CubicEase() { EasingMode = easing };
            DoubleAnimation animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds)),
                From = Convert.ToDouble(obj.GetValue(prop)),
                To = Convert.ToDouble(value),
                FillBehavior = FillBehavior.HoldEnd,
                EasingFunction = ease
            };
            Storyboard.SetTarget(animation, obj);
            Storyboard.SetTargetProperty(animation, new PropertyPath(prop));

            return animation;
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            Storyboard sb = sender as Storyboard;
            if (null != sb)
                sb.Completed -= new EventHandler(Storyboard_Completed);

            // raise event for any listener out there
            if (null != ScrollCompleted)
                ScrollCompleted(this, new ScrollCompletedEventArgs() { SelectedIndex = _newindex });
        }
    }
}
