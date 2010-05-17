using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using System.ComponentModel;

namespace SmartyPantsPivotLibrary
{
    public partial class PivotControl : UserControl
    {
        protected const double MOVE_VS_TAP_COMMIT_DISTANCE = 15.0D; // we dont move the page until they've moved beyond some amount - at which point they've committed to moving
        protected const double MOVEMENT_NECESSARY_FOR_PAGE_CHANGE = 220.00D; // how far the user must drag to cause a page change in either direction
        protected const double MovementPadding = 20.0D; // the extra distance we put an item off the page before animating it in
        protected const double MenuSpacingFromLeft = 2.0D; // how much space should be between the pivot menu name and the left edge of the screen

        // the timespan and easings used for animations
        protected static TimeSpan DefaultTransitionTimespan = new TimeSpan(0, 0, 0, 0, 320);
        protected static IEasingFunction DefaultTransitionEasing = new CircleEase() { EasingMode = EasingMode.EaseOut };

        protected static BitmapCache StaticBitmapCache = new BitmapCache();

        // the styles applied to menu titles
        protected static Style TextBlockStyle; // = Application.Current.Resources["PhoneTextTitle1Style"] as Style;
        protected static SolidColorBrush ActiveMenuColor; // = Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
        protected static SolidColorBrush InactiveMenuColor; // = Application.Current.Resources["PhoneInactiveBrush"] as SolidColorBrush;

        protected int MaxPivotIndex { get { return PivotPages.Count() - 1; } }
        protected int CurrentMenuIndex { get; set; }
        protected int AnimationsPendingCount = 0;
        protected bool AlreadyLoaded = false; // its possible for loaded event to fire multiple times (especially in the designer), so make sure we dont do our loading logic twice
        protected bool CommittedToMove = false;
        protected bool PageMovingEventFired = false;
        protected bool IgnoreRemainingManipulations = false; // once the user moves far enough we change pages and ignore the rest of the manipulation        
        protected double StartedMenuPosition = 0.0D; // the XOffset of the menu when the user starts to manipulate the screen
        protected List<MenuTitleEntry> MenuTitles { get; set; }
        protected TextBlock StartingRealMenu { get; set; }

        protected int CurrentPivotContentIndex 
        {
            get { return _currentPivotContentIndex; }
            set
            {
                // if the value they are setting is beyond the max index then go back to zero ..
                if (value > MaxPivotIndex)
                    _currentPivotContentIndex = 0;
                else if (value < 0) // .. if they go below 0 then go to the last index
                    _currentPivotContentIndex = MaxPivotIndex;
                else
                    _currentPivotContentIndex = value;
            }
        }
        protected int _currentPivotContentIndex;

        /// <summary>
        /// Wrapping quick-reference variable to Pivot Menu's CompositeTransform
        /// </summary>
        protected CompositeTransform PivotMenuCompositeTransform
        {
            get { return (PivotTitleStackPanel.RenderTransform as CompositeTransform); }
        }

        #region Dependency Properties

        /// <summary>
        /// The <see cref="PivotPages" /> dependency property's name.
        /// </summary>
        public const string PivotPagesPropertyName = "PivotPages";

        /// <summary>
        /// Gets or sets the value of the <see cref="PivotPages" />
        /// property. This is a dependency property.
        /// </summary>
        public List<PivotPage> PivotPages
        {
            get
            {
                return (List<PivotPage>)GetValue(PivotPagesProperty);
            }
            set
            {
                SetValue(PivotPagesProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="PivotPages" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty PivotPagesProperty = DependencyProperty.Register(
            PivotPagesPropertyName,
            typeof(List<PivotPage>),
            typeof(PivotControl),
            new PropertyMetadata(new List<PivotPage>()));

        #endregion

        #region Public Events

        /// <summary>
        /// This event tells you that pivot pages should disable their interactions to prevent accidental interactions when the user
        /// lets up their finger after doing some gestures. 
        /// </summary>
        public event EventHandler PivotPageMoving;

        /// <summary>
        /// This event tells you that pivot pages are done being moved, so you should re-enable the pivot pages interactions
        /// </summary>
        public event EventHandler PivotPageDoneMoving;

        private void FirePivotPageMovingEvent()
        {
            if (PivotPageMoving != null)
                PivotPageMoving(this, EventArgs.Empty);
        }

        private void FirePivotPageDoneMovingEvent()
        {
            if (PivotPageDoneMoving != null)
                PivotPageDoneMoving(this, EventArgs.Empty);
        }

        #endregion

        public PivotControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PivotControl_Loaded);
        }

        void PivotControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (AlreadyLoaded)
                return;

            AlreadyLoaded = true;

            // when in design mode the lookup for these styles fails, so we are bringing them in from our own local app.xaml for design mode
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                App a = new App();
                TextBlockStyle = a.Resources["PhoneTextTitle1Style"] as Style;
                ActiveMenuColor = a.Resources["PhoneForegroundBrush"] as SolidColorBrush;
                InactiveMenuColor = a.Resources["PhoneInactiveBrush"] as SolidColorBrush;
            }
            else
            {
                TextBlockStyle = Application.Current.Resources["PhoneTextTitle1Style"] as Style;
                ActiveMenuColor = Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
                InactiveMenuColor = Application.Current.Resources["PhoneInactiveBrush"] as SolidColorBrush;
            }

            // make sure we found the resources we needed
            if (TextBlockStyle == null || ActiveMenuColor == null || InactiveMenuColor == null)
            {
                Debug.Assert(false, "Unable to find one ore more of the required styles: PhoneTextTitle1Style, PhoneForegroundBrush, PhoneInactiveBrush");
                return;
            }

            SetupPivotTitles();
            SetupMainArea();

            // if the user only has 1 page then we dont want to listen to any gestures because there are no pivot pages to switch between
            if (PivotPages.Count > 0)
            {
                // hookup events for manipulations (gestures) - start, change, and complete
                LayoutRoot.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(LayoutRoot_ManipulationStarted);
                LayoutRoot.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(LayoutRoot_ManipulationDelta);
                LayoutRoot.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(LayoutRoot_ManipulationCompleted);
            }

            // HACK:: we shouldnt have to do this, but the measurements arent updating until i let things run for a moment
            // we want to let the layout render once, and then we want to animate to the starting menu position
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, a) =>
                {
                    UpdateStartingMenuPosition();

                    timer.Stop();
                    timer = null;
                };
            timer.Start();
        }

        void LayoutRoot_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            IgnoreRemainingManipulations = false;
            CommittedToMove = false;
            PageMovingEventFired = false;

            StartedMenuPosition = (PivotTitleStackPanel.RenderTransform as CompositeTransform).TranslateX;

            // if not enough time has elapsed since the last gesture then go ahead and ignore this manipulation
            if (AnimationsPendingCount > 0)
            {
                IgnoreRemainingManipulations = true;
                CommittedToMove = true;
            }
        }

        /// <summary>
        /// When the manipulation/gesture changes we want to change the location of the content and menu areas relative to the 
        /// total amount in which the user has gestured left/right (Translation.X)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LayoutRoot_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (IgnoreRemainingManipulations)
                return;

            // if the manipulation delta is above the required amount, then they have comitted to a move
            if (Math.Abs(e.CumulativeManipulation.Translation.X) > MOVE_VS_TAP_COMMIT_DISTANCE)
            {
                CommittedToMove = true;

                // we want to notify the page that we have started moving, but we only want to fire it once - not everytime this delta event occurs
                if (!PageMovingEventFired)
                {
                    PageMovingEventFired = true;
                    FirePivotPageMovingEvent();
                    // TODO:: make nothing in the current view clickable
                }                
            }

            // we only want to move the page if they have moved enough to commit to moving, otherwise while they are scrolling up/down on a
            // page we would be barely moving left/right as well
            if (CommittedToMove)
            {
                var currentDisplayedSectionTransform = PivotSectionDisplayGrid.Children[0].RenderTransform as CompositeTransform;
                currentDisplayedSectionTransform.TranslateX = e.CumulativeManipulation.Translation.X;
                PivotMenuCompositeTransform.TranslateX = StartedMenuPosition + (e.CumulativeManipulation.Translation.X / 2.0D);
            }

            // if the user swiped far enough right or left then we are going to change pages
            if (Math.Abs(e.CumulativeManipulation.Translation.X) > MOVEMENT_NECESSARY_FOR_PAGE_CHANGE)
            {
                // if the user swiped far enough right, then move them to the left (go to the previous menu state), otherwise go left
                IgnoreRemainingManipulations = true;
                FrameworkElement currentPage = GetPivotMenuContentForPivotIndex(CurrentPivotContentIndex);
                bool toLeft = e.CumulativeManipulation.Translation.X > MOVEMENT_NECESSARY_FOR_PAGE_CHANGE;

                CurrentPivotContentIndex = CurrentPivotContentIndex + (toLeft ? -1 : 1);
                CurrentMenuIndex = CurrentMenuIndex + (toLeft ? -1 : 1);

                FrameworkElement nextPage = GetPivotMenuContentForPivotIndex(CurrentPivotContentIndex);
                GoFromPageToPage(currentPage, nextPage, toLeft);
                GoToMenu(CurrentMenuIndex); // animate the menu
            }
        }

        /// <summary>
        /// When the manipulation/gesture ends we want to take action - see if the user dragged their finger long enough on
        /// the X axis to make them move to a previous or next page. If they didn't move enough, then reset their position to the
        /// visual state they are already on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LayoutRoot_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            FirePivotPageDoneMovingEvent();

            if (IgnoreRemainingManipulations)
                return;

            GoToCurrentState();
        }

        void PivotMenuItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // see if we've already handled this action
            if (IgnoreRemainingManipulations || CommittedToMove)
                return;

            // the sender is our textblock, its tag is the corresponding menu view
            FrameworkElement nextPage = ((sender as TextBlock).Tag as MenuTitleEntry).PageContentReference;
            FrameworkElement currentPage = GetCurrentPivotPage();

            // see if the user clicked on the same page as what is currently selected, if so do nothing
            if (nextPage == currentPage)
                return;

            // figure out which what position in the stack panel the clicked textblock is
            int tbIndex = -1;
            for (int i = 0; i < PivotTitleStackPanel.Children.Count; i++)
            {
                var child = PivotTitleStackPanel.Children[i];
                if (child == sender)
                {
                    tbIndex = i;
                    break;
                }
            }
            // assert that we found the position
            Debug.Assert(tbIndex != -1, "Could not find the position of the TextBlock clicked on");

            // figure out if we are going forward or back
            bool backward = CurrentMenuIndex > tbIndex;

            CurrentMenuIndex = tbIndex;

            GoFromPageToPage(currentPage, nextPage, backward);
            GoToMenu(CurrentMenuIndex); // animate the menu
        }

        private void GoToCurrentState()
        {
            // if we are already doing an animation then do nothing
            if (AnimationsPendingCount > 0) return;            

            FrameworkElement currentPage = GetPivotMenuContentForPivotIndex(CurrentPivotContentIndex);
            GoToMenu(CurrentMenuIndex);

            Storyboard sbPage = TransitionStoryboardHelper.BuildTranslateTransformTransition(currentPage, new Point(0, 0),
                new TimeSpan(0, 0, 0, 0, 200), null);
            sbPage.Completed += (s, e) => { AnimationsPendingCount--; };
            AnimationsPendingCount++;
            sbPage.Begin();         
        }

        private void GoFromPageToPage(FrameworkElement currentPage, FrameworkElement nextPage, bool backward)
        {
            // if we are already doing an animation then do nothing
            if (AnimationsPendingCount > 0) return;

            // do a sanity check and make sure our current display isnt already on the nextPage (could happen with lots of fast motions)
            if (PivotSectionDisplayGrid.Children[0] == nextPage)
                return;

            if (currentPage == nextPage)
                return;

            // figure out how far to the right we need to go.. get the width of the current page, we want to go that far to the right plus 20px padding            
            double totalOffStageOffset = 0 - LayoutRoot.ActualWidth - MovementPadding;
            if (backward)
                totalOffStageOffset = LayoutRoot.ActualWidth + MovementPadding;

            Storyboard sbLeavingPage = TransitionStoryboardHelper.BuildTranslateTransformTransition(currentPage, new Point(totalOffStageOffset, 0),
                DefaultTransitionTimespan, DefaultTransitionEasing);

            Storyboard sbLeavingFade = TransitionStoryboardHelper.BuildTransparencyTransition(currentPage, 0.0D, DefaultTransitionTimespan, false);

            sbLeavingPage.Completed += (s, args) =>
            {
                PivotSectionDisplayGrid.Children.Remove(currentPage);
                AnimationsPendingCount--;
            };
            AnimationsPendingCount++;
            sbLeavingPage.Begin();
            sbLeavingFade.Begin();

            nextPage.RenderTransform = new CompositeTransform();
            nextPage.Opacity = 0.00D; // we are adding it back on the stage to measure, so make it invisible

            PivotSectionDisplayGrid.Children.Add(nextPage);
            nextPage.InvalidateMeasure(); // force the size to update

            (nextPage.RenderTransform as CompositeTransform).TranslateX = totalOffStageOffset * -1.0D; // move it off to the right

            // animate the new page onto the screen
            Storyboard sbEnteringPage = TransitionStoryboardHelper.BuildTranslateTransformTransition(nextPage, new Point(0, 0),
                DefaultTransitionTimespan, DefaultTransitionEasing);

            Storyboard sbEnteringFade = TransitionStoryboardHelper.BuildTransparencyTransition(nextPage, 1.0D, DefaultTransitionTimespan, false);

            sbEnteringPage.Completed += (s, e) =>
            {
                AnimationsPendingCount--;
            };
            AnimationsPendingCount++;
            sbEnteringPage.Begin();
            sbEnteringFade.Begin();
        }

        private TextBlock GetMenuTextBlockForMenuItem(MenuTitleEntry mte)
        {
            int index = GetMenuIndex(mte);
            return PivotTitleStackPanel.Children[index] as TextBlock;
        }

        public int GetMenuIndex(MenuTitleEntry mte)
        {
            int index = 0;
            foreach (TextBlock tb in PivotTitleStackPanel.Children)
            {
                MenuTitleEntry tempMTE = tb.Tag as MenuTitleEntry;
                if (tempMTE == mte)
                    return index;

                index++;
            }

            throw new InvalidProgramException("GetMenuIndex() - Unable to find the menu provided");
        }

        public void GoToMenu(int index)
        {
            // figure out how far from the left we are, and offset that amount (thus moving it to the leftmost position)
            TextBlock selectedMenu = PivotTitleStackPanel.Children[index] as TextBlock;
            MenuTitleEntry mte = selectedMenu.Tag as MenuTitleEntry;
            MatrixTransform mt = PivotTitleStackPanel.TransformToVisual(selectedMenu) as MatrixTransform;
            double distanceFromLeft = mt.Matrix.OffsetX + MenuSpacingFromLeft;

            // animate to the menu
            Storyboard sbMenuSlide = TransitionStoryboardHelper.BuildTranslateTransformTransition(PivotTitleStackPanel, new Point(distanceFromLeft, 0),
                DefaultTransitionTimespan, DefaultTransitionEasing);
            sbMenuSlide.Completed += (s, args) =>
                {
                    // when the slide completes we want to immediately jump to the 'real' menu item and set it as selected                    
                    TextBlock realTB = GetMenuTextBlockForMenuItem(mte.RealMenuPointer);

                    // offset the panel to show the real menu now
                    MatrixTransform realMenuMT = PivotTitleStackPanel.TransformToVisual(realTB) as MatrixTransform;
                    double realMenuDistanceFromLeft = realMenuMT.Matrix.OffsetX + MenuSpacingFromLeft;
                    (PivotTitleStackPanel.RenderTransform as CompositeTransform).TranslateX = realMenuDistanceFromLeft;
                    
                    UpdateMenuColors();
                    AnimationsPendingCount--;
                };
            AnimationsPendingCount++;
            sbMenuSlide.Begin();

            CurrentMenuIndex = GetMenuIndex(mte.RealMenuPointer);
            UpdateMenuColors();
        }

        private void UpdateMenuColors()
        {
            // get the real menu selected
            MenuTitleEntry realMenuSelected = (PivotTitleStackPanel.Children[CurrentMenuIndex] as TextBlock).Tag as MenuTitleEntry;

            for (int i = 0; i < PivotTitleStackPanel.Children.Count; i++)
            {
                TextBlock tb = PivotTitleStackPanel.Children[i] as TextBlock;
                MenuTitleEntry mteTemp = tb.Tag as MenuTitleEntry;

                tb.Foreground = (mteTemp == realMenuSelected || mteTemp.RealMenuPointer == realMenuSelected) ? ActiveMenuColor : InactiveMenuColor;
            }
        }

        private void SetupPivotTitles()
        {
            MenuTitles = new List<MenuTitleEntry>();

            // clear out our titles
            PivotTitleStackPanel.Children.Clear();

            // the pattern is <XX fake items> <XX real items> <XX fake items>            

            // create our >real< menu items
            var tempRealMenuItems = new List<MenuTitleEntry>();
            foreach (PivotPage pp in PivotPages)
            {
                MenuTitleEntry mte = new MenuTitleEntry(pp);
                tempRealMenuItems.Add(mte);
            }

            // add our >fake< first set of menu items
            foreach (MenuTitleEntry menuItem in tempRealMenuItems)
            {
                MenuTitleEntry mte = new MenuTitleEntry(menuItem);
                MenuTitles.Add(mte);
            }

            // keep track of what the first 'real' menu item index will be
            CurrentMenuIndex = MenuTitles.Count;

            // add our >real< set of menu items
            // (we want to select the first one)            
            foreach (MenuTitleEntry menuItem in tempRealMenuItems)
            {
                MenuTitles.Add(menuItem);
            }

            // add our >fake< second set of menu items
            foreach (MenuTitleEntry menuItem in tempRealMenuItems)
            {
                MenuTitleEntry mte = new MenuTitleEntry(menuItem);
                MenuTitles.Add(mte);
            }

            double fullPivotTitleWidths = 0.0D;

            // iterate through our pivot pages and add a textblock for each
            for (int i = 0; i < MenuTitles.Count; i++)
            {
                MenuTitleEntry mte = MenuTitles[i];

                TextBlock tb = new TextBlock()
                {
                    Text = mte.Title,
                    Style = TextBlockStyle,
                    Margin = new Thickness(5, 0, 20, 0),
                    Tag = mte
                };

                // make the color disabled if not the selected item
                tb.Foreground = (i == CurrentMenuIndex) ? ActiveMenuColor : InactiveMenuColor;
                if (i == CurrentMenuIndex)
                {
                    StartingRealMenu = tb;
                }

                // add a click handler
                tb.MouseLeftButtonUp += new MouseButtonEventHandler(PivotMenuItem_MouseLeftButtonUp);

                PivotTitleStackPanel.Children.Add(tb);
                tb.InvalidateMeasure(); // force the width to update

                fullPivotTitleWidths += tb.ActualWidth +25.0D;
            }

            // we want to force the width of the containing grid so that the stack panel is always rendered at full width
            PivotTitleStackPanelContainer.Width = fullPivotTitleWidths + 20.0D;
        }

        private void UpdateStartingMenuPosition()
        {
            // ignore this method if in design mode
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            // offset the panel to show the real menu now
            MatrixTransform realMenuMT = PivotTitleStackPanel.TransformToVisual(StartingRealMenu) as MatrixTransform;
            double realMenuDistanceFromLeft = realMenuMT.Matrix.OffsetX + MenuSpacingFromLeft;
            (PivotTitleStackPanel.RenderTransform as CompositeTransform).TranslateX = realMenuDistanceFromLeft;
        }

        private void SetupMainArea()
        {
            PivotSectionDisplayGrid.Children.Clear();

            // if there are no pivot pages then just clear out the section and do nothing
            if (PivotPages.Count == 0)
                return;

            // update all the pages to have transforms to work with
            for (int i = 0; i < MaxPivotIndex; i++)
            {
                var page = GetPivotMenuContentForPivotIndex(i);
                page.RenderTransform = new CompositeTransform();
                page.CacheMode = StaticBitmapCache;
            }

            var currentPage = GetPivotMenuContentForPivotIndex(0);
            PivotSectionDisplayGrid.Children.Add(currentPage);
        }

        private FrameworkElement GetCurrentPivotPage()
        {
            return ((PivotTitleStackPanel.Children[CurrentMenuIndex] as TextBlock).Tag as MenuTitleEntry).PageContentReference;
        }

        private FrameworkElement GetPivotMenuContentForPivotIndex(int index)
        {
            // make sure our index is in bounds
            Debug.Assert(index >= 0, "Invalid index request for GetPivotMenuContentForPivotIndex (too low)");
            Debug.Assert(index <= MaxPivotIndex, "Invalid index request for GetPivotMenuContentForPivotIndex (too high)");

            return PivotPages[index].Content as FrameworkElement;
        }
    }
}