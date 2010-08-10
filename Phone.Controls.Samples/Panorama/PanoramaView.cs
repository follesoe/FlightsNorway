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
    internal enum Snapping
    {
        SnapLeft = 0,
        SnapRight
    }

    /// <summary>
    /// Helper class for PanoramaControl.
    /// Implements the UI logic.
    /// </summary>
    internal class PanoramaView
    {
        private const string LayoutRootName = "LayoutRoot";
        private const string BackgroundPanelName = "BackgroundPanel";
        private const string TitlePanelName = "TitlePanel";
        private const string ItemsPanelName = "ItemsPanel";
        private const string BackgroundPanelHostName = "BackgroundPanelHost";
        private const string TitlePanelHostName = "TitlePanelHost";
        private const string ItemsPanelHostName = "ItemsPanelHost";

        private Panel LayoutRoot;
        private PanoramaControl Parent;
        private ItemCollection Items;

        private ScrollHost BackgroundHost;
        private ScrollHost TitleHost;
        private ScrollHost ItemsHost;

        private const double AnimationDuration = 1000.0;
        private Storyboard Storyboard;
        public event ScrollCompletedEventHandler ScrollCompleted;

        private bool _ready = false;

        public PanoramaView(PanoramaControl parent)
        {
            Parent = parent;
            LayoutRoot = parent.LayoutRoot;
            Items = Parent.Items;

            BackgroundHost.Transform = new TranslateTransform();
            TitleHost.Transform = new TranslateTransform();
            ItemsHost.Transform = new TranslateTransform();
        }

        /// <summary>
        /// Initialize the panel hosting control presenters to duplicate (creating a virtual carousel)
        /// </summary>
        /// <param name="panel">Carousel panel</param>
        /// <param name="presenter">Main control presenter</param>
        /// <param name="left">Control to add to the left</param>
        /// <param name="right">Control to add to the right</param>
        /// <param name="pad">Number of empty padding pixels to add to 'left' and 'right'</param>
        private void InitializeHost(Panel panel, FrameworkElement presenter, FrameworkElement left, FrameworkElement right, double pad)
        {
            // reset/initialize layout with dummy values
            if (panel.Children.Count == 1)
            {
                panel.Children.Insert(0, new Rectangle());
                panel.Children.Add(new Rectangle());
            }
            panel.SetValue(Canvas.LeftProperty, 0.0);


            // insert items ?
            if (Items.Count > 0)
            {
                WriteableBitmap bitmap;
                Image image;
                int width;
                int height;

                // duplicate left
                width = (int)(left.ActualWidth + pad);
                height = (int)left.ActualHeight;
                bitmap = new WriteableBitmap(width, height);
                bitmap.Render(left, null);
                bitmap.Invalidate();
                image = new Image();
                image.Source = bitmap;
                image.CacheMode = new BitmapCache();
                panel.Children[0] = image;
                double offset = bitmap.PixelWidth;

                // duplicate right
                width = (int)(right.ActualWidth + pad);
                height = (int)right.ActualHeight;
                bitmap = new WriteableBitmap(width, height);
                bitmap.Render(right, new TranslateTransform() { X = pad });
                bitmap.Invalidate();
                image = new Image();
                image.Source = bitmap;
                image.CacheMode = new BitmapCache();
                panel.Children[2] = image;

                // adjust panel position
                panel.SetValue(Canvas.LeftProperty, -offset);
            }
        }

        public void Initialize()
        {
            if (!_ready)
            {
                // fetch template elements
                ContentPresenter BackgroundPanel = LayoutRoot.FindName(BackgroundPanelName) as ContentPresenter;
                ContentPresenter TitlePanel = LayoutRoot.FindName(TitlePanelName) as ContentPresenter;
                ItemsPresenter ItemsPanel = LayoutRoot.FindName(ItemsPanelName) as ItemsPresenter;
                Panel BackgroundPanelHost = LayoutRoot.FindName(BackgroundPanelHostName) as Panel;
                Panel TitlePanelHost = LayoutRoot.FindName(TitlePanelHostName) as Panel;
                Panel ItemsPanelHost = LayoutRoot.FindName(ItemsPanelHostName) as Panel;
                if (null == BackgroundPanel) throw new ArgumentException(string.Format("Cannot find {0}.", BackgroundPanelName));
                if (null == TitlePanel) throw new ArgumentException(string.Format("Cannot find {0}.", TitlePanelName));
                if (null == ItemsPanel) throw new ArgumentException(string.Format("Cannot find {0}.", ItemsPanelName));
                if (null == BackgroundPanelHost) throw new ArgumentException(string.Format("Cannot find {0}.", BackgroundPanelHostName));
                if (null == TitlePanelHost) throw new ArgumentException(string.Format("Cannot find {0}.", TitlePanelHostName));
                if (null == ItemsPanelHost) throw new ArgumentException(string.Format("Cannot find {0}.", ItemsPanelHostName));

                // reset panelhosts
                BackgroundHost.Reset();
                TitleHost.Reset();
                ItemsHost.Reset();

                // create transforms
                BackgroundPanelHost.RenderTransform = BackgroundHost.Transform;
                TitlePanelHost.RenderTransform = TitleHost.Transform;
                ItemsPanelHost.RenderTransform = ItemsHost.Transform;

                // fetch items details
                FrameworkElement item0 = null;
                FrameworkElement itemN = null;
                if (Items.Count > 0)
                {
                    int index0 = 0;
                    int indexN = Items.Count - 1;
                    item0 = Items[index0] as FrameworkElement;
                    itemN = Items[indexN] as FrameworkElement;
                }

                // reset panelhosts layout
                TitleHost.Padding = LayoutRoot.ActualWidth;
                InitializeHost(BackgroundPanelHost, BackgroundPanel, BackgroundPanel, BackgroundPanel, BackgroundHost.Padding);
                InitializeHost(TitlePanelHost, TitlePanel, TitlePanel, TitlePanel, TitleHost.Padding);
                InitializeHost(ItemsPanelHost, ItemsPanel, itemN, item0, ItemsHost.Padding);

                if (Items.Count > 0)
                {
                    double maxN = Math.Max(itemN.Width, LayoutRoot.ActualWidth);

                    // panelhosts width
                    BackgroundHost.Width = BackgroundPanel.ActualWidth;
                    TitleHost.Width = TitlePanel.ActualWidth;
                    ItemsHost.Width = Items.GetTotalWidth() - itemN.Width + maxN;

                    // panelhosts speed
                    TitleHost.Speed = BackgroundHost.Width / ItemsHost.Width;
                    TitleHost.Speed = TitleHost.Width / ItemsHost.Width;
                    if (ItemsHost.Width > maxN)
                        BackgroundHost.Speed = (BackgroundHost.Width - maxN) / (ItemsHost.Width - maxN);
                }

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

        public void MoveTo(int index)
        {
            Initialize();

            // nothing to do
            if (Items.Count == 0)
                return;

            // move to new position
            Position = Items.GetItemPosition(index);
        }

        public void ScrollTo(int index, Snapping snap = Snapping.SnapLeft)
        {
            Initialize();

            // nothing to do
            if (Items.Count == 0)
                return;

            // animate to new position
            this.ScrollStart(index, snap, AnimationDuration);
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
                BackgroundHost.Transform.X = -value * BackgroundHost.Speed;
                TitleHost.Transform.X = -value * TitleHost.Speed;
                ItemsHost.Transform.X = -value * ItemsHost.Speed;
            }
        }

        private void ScrollStart(int index, Snapping snap, double milliseconds = 0)
        {
            Initialize();

            // positions
            double offsetBackground = 0.0;
            double offsetTitle = 0.0;
            double offsetItems = 0.0;

            //
            // adjust destination item positions
            //
            int index0 = 0;
            int indexN = Items.Count - 1;

            // scroll from first to last
            if (index < index0)
            {
                if (snap == Snapping.SnapLeft)
                    offsetItems = Items.GetItemPosition(index0) - Items.GetItemWidth(indexN);
                else
                    offsetItems = Items.GetItemPosition(index0) - Parent.DefaultItemWidth;
            }
            // scroll from last to first
            else if (index > indexN)
            {
                // since we're moving left to right, and only a item at a time
                // we can only snap to left here...
                offsetItems = Items.GetItemPosition(indexN) + Items.GetItemWidth(indexN);
            }
            // normal scroll
            else
            {
                if (snap == Snapping.SnapLeft)
                    offsetItems = Items.GetItemPosition(index);
                else
                    offsetItems = Items.GetItemPosition(index) + Items.GetItemWidth(index) - Parent.DefaultItemWidth;
            }

            //
            // adjust animation speed
            //
            double offset = Math.Abs(offsetItems - Position);
            if (offset < LayoutRoot.ActualWidth)
            {
                milliseconds *= offset / LayoutRoot.ActualWidth;
            }

            //
            // adjust positions
            //
            offsetBackground = offsetItems * BackgroundHost.Speed;
            offsetTitle = offsetItems * TitleHost.Speed;
            offsetItems = offsetItems * ItemsHost.Speed;

            // back to last
            if (offsetItems < 0)
            {
                offsetBackground = Items.GetLastItemPosition() - ItemsHost.Width;
                offsetTitle = offsetBackground * TitleHost.Speed - TitleHost.Padding;
                if (Items.Count == 1)
                {
                    // only 1 item : scroll the entire background
                    offsetBackground = -BackgroundHost.Width;
                }
            }
            // back to first
            else if (offsetItems >= Items.GetTotalWidth())
            {
                offsetBackground = BackgroundHost.Width;
                offsetTitle = TitleHost.Width + TitleHost.Padding;
            }

            //
            // start storyboard
            //
            Storyboard = new Storyboard();
            Storyboard.Completed += new EventHandler(Storyboard_Completed);
            Storyboard.Children.Add(CreateAnimation(BackgroundHost.Transform, TranslateTransform.XProperty,- offsetBackground, milliseconds));
            Storyboard.Children.Add(CreateAnimation(TitleHost.Transform, TranslateTransform.XProperty, -offsetTitle, milliseconds));
            Storyboard.Children.Add(CreateAnimation(ItemsHost.Transform, TranslateTransform.XProperty, -offsetItems, milliseconds));
            Storyboard.Begin();
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

            // find selected item
            int index = Items.GetIndexOfPosition(this.Position);

            // raise event for any listener out there
            if (null != ScrollCompleted)
                ScrollCompleted(this, new ScrollCompletedEventArgs() { SelectedIndex = index });
        }
    }
}
