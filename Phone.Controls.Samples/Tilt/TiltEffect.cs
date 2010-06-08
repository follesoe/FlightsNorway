using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace Phone.Controls.Samples.Tilt
{
  /// <summary>
  /// Provides attached properties for adding a 'tilt' effect to all controls within a container
  /// </summary>
  public class TiltEffect : DependencyObject
  {
    /// <summary>
    /// The default amount of tilt, if none is specified
    /// </summary>
    public const double DefaultTiltStrength = 0.333;

    /// <summary>
    /// The default amount of depression, if none is specified
    /// </summary>
    public const double DefaultPressStrength = 0.333;

    /// <summary>
    /// Constant factor used to get the right press strength
    /// </summary>
    const double PressStrengthFactor = -200;

    /// <summary>
    /// Conversion of radians to degrees
    /// </summary>
    const double RadiansToDegrees = 180 / Math.PI;

    /// <summary>
    /// Strength of the tilt effect. 0 is no tilt, 1 is a lot of tilt. Values less than 0 or greater than 1 produce "interesting" results
    /// </summary>
    public static readonly DependencyProperty TiltStrengthProperty = DependencyProperty.RegisterAttached("TiltStrength", typeof(double), typeof(TiltEffect), new PropertyMetadata(double.NaN));
    
    public static double GetTiltStrength(DependencyObject source)
    {
      return (double)source.GetValue(TiltStrengthProperty);
    }

    public static void SetTiltStrength(DependencyObject source, double value)
    {
      source.SetValue(TiltStrengthProperty, value);
    }

    /// <summary>
    /// Strength of the press effect. 0 is no press, 1 is significant press. Values less than 0 or greater than 1 produce exagerated results
    /// </summary>
    public static readonly DependencyProperty PressStrengthProperty = DependencyProperty.RegisterAttached("PressStrength", typeof(double), typeof(TiltEffect), new PropertyMetadata(double.NaN));

    public static double GetPressStrength(DependencyObject source)
    {
      return (double)source.GetValue(PressStrengthProperty);
    }

    public static void SetPressStrength(DependencyObject source, double value)
    {
      source.SetValue(PressStrengthProperty, value);
    }

    /// <summary>
    /// Whether the tilt effect is enabled on a container (and all its children)
    /// </summary>
    public static readonly DependencyProperty IsTiltEnabledProperty = DependencyProperty.RegisterAttached("IsTiltEnabled", typeof(bool), typeof(TiltEffect), new PropertyMetadata(OnIsTiltEnabledChanged));

    public static bool GetIsTiltEnabled(DependencyObject source)
    {
      return (bool)source.GetValue(IsTiltEnabledProperty);
    }

    public static void SetIsTiltEnabled(DependencyObject source, bool value)
    {
      source.SetValue(IsTiltEnabledProperty, value);
    }

    /// <summary>
    /// Suppresses the tilt effect on a single control that would otherwise be tilted
    /// </summary>
    public static readonly DependencyProperty SuppressTiltProperty = DependencyProperty.RegisterAttached("SuppressTilt", typeof(bool), typeof(TiltEffect), null);

    public static bool GetSuppressTilt(DependencyObject source)
    {
      return (bool)source.GetValue(SuppressTiltProperty);
    }

    public static void SetSuppressTilt(DependencyObject source, bool value)
    {
      source.SetValue(SuppressTiltProperty, value);
    }

    /// <summary>
    /// Add / Remove event handlers from the element that has (un)registered for tilting
    /// </summary>
    /// <param name="target">The element that the property is atteched to</param>
    /// <param name="args">Event args</param>
    static void OnIsTiltEnabledChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
      if (target is FrameworkElement)
      {
        // Add / remove our event handler if necessary
        if ((bool) args.NewValue == true)
          (target as FrameworkElement).AddHandler(Control.MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDownHandler), true);
        else
          (target as FrameworkElement).RemoveHandler(Control.MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDownHandler));
      }
    }

    /// <summary>
    /// Handler called for every mouse-down event in the container
    /// </summary>
    /// <param name="sender">Container that originally requested tilt</param>
    /// <param name="e">Event arguments</param>
    static void MouseDownHandler(object sender, MouseButtonEventArgs e)
    {
      TryTiltFromMouseDown(e, sender as FrameworkElement);
    }

    static string storyboardXaml = @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
			<DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.GlobalOffsetZ)"">
				<EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
				<EasingDoubleKeyFrame KeyTime=""00:00:00.1"" Value=""-75""/>
				<EasingDoubleKeyFrame KeyTime=""00:00:00.5"" Value=""0""/>
			</DoubleAnimationUsingKeyFrames>
			<DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"">
				<EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
				<EasingDoubleKeyFrame KeyTime=""00:00:00.1"" Value=""0""/>
				<EasingDoubleKeyFrame KeyTime=""00:00:00.5"" Value=""0""/>
			</DoubleAnimationUsingKeyFrames>
			<DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationY)"">
				<EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
				<EasingDoubleKeyFrame KeyTime=""00:00:00.1"" Value=""0""/>
				<EasingDoubleKeyFrame KeyTime=""00:00:00.5"" Value=""0""/>
			</DoubleAnimationUsingKeyFrames>
		</Storyboard>";

    static Storyboard tiltStoryboard;
    static DoubleKeyFrame depressFrame;
    static DoubleKeyFrame rotationXFrame;
    static DoubleKeyFrame rotationYFrame;

    /// <summary>
    /// Set up the storyboard
    /// </summary>
    static TiltEffect()
    {
      tiltStoryboard = System.Windows.Markup.XamlReader.Load(storyboardXaml) as Storyboard;
      depressFrame = (tiltStoryboard.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1];
      rotationXFrame = (tiltStoryboard.Children[1] as DoubleAnimationUsingKeyFrames).KeyFrames[1];
      rotationYFrame = (tiltStoryboard.Children[2] as DoubleAnimationUsingKeyFrames).KeyFrames[1];
    }

    /// <summary>
    /// Default list of items that are tiltable
    /// </summary>
    static List<Type> tiltableItems = new List<Type>() { typeof(ListBoxItem), typeof(ButtonBase) };

    /// <summary>
    /// Given a mouse-down event, attempts to do a tilt on any control in the visual tree
    /// </summary>
    /// <param name="e">The MouseButtonEventArgs from a mouse-down handler</param>
    /// <param name="tiltContainer">The original container that registered for tilt</param>
    static void TryTiltFromMouseDown(MouseButtonEventArgs e, FrameworkElement tiltContainer)
    {
      foreach (FrameworkElement ancestor in (e.OriginalSource as FrameworkElement).GetVisualAncestors())
      {
        foreach (Type t in tiltableItems)
        {
          if (t.IsAssignableFrom(ancestor.GetType()))
          {
            if ((bool)ancestor.GetValue(SuppressTiltProperty) != true)
            {
              Point p = e.GetPosition(ancestor);
              BeginTilt(ancestor, p, GetTiltStrength(tiltContainer), GetPressStrength(tiltContainer));
              return;
            }
          }
        }
      }
    }

    /// <summary>
    /// Begins a tilt operation on an element
    /// </summary>
    /// <param name="element">The element to tilt</param>
    /// <param name="point">The point at which the 'press' happened, in element's coordinates</param>
    /// <param name="tiltStrength">The default tilt strength, inherited from container (if any)</param>
    /// <param name="pressStrength">The default press strength, inherited from container (if any)</param>
    static void BeginTilt(FrameworkElement element, Point point, double tiltStrength, double pressStrength)
    {
      tiltStoryboard.Stop();
      if (element.GetPlaneProjection(true) == null)
        return;

      Storyboard.SetTarget(tiltStoryboard, element);
      double halfWidth = element.ActualWidth / 2;
      double halfHeight = element.ActualHeight / 2;

      double xAngle = Math.Asin((point.Y - halfHeight) / halfHeight) * RadiansToDegrees;
      double yAngle = Math.Acos((point.X - halfWidth) / halfWidth) * RadiansToDegrees;

      // Get explicit strengths set on the element, if any
      double itemPressStrength = GetPressStrength(element);
      double itemTiltStrength = GetTiltStrength(element);

      // Get the press strength: (i) from item, (ii) from parent, or (iii) default
      if (double.IsNaN(itemPressStrength) != true)
        pressStrength = itemPressStrength;
      else if (double.IsNaN(pressStrength))
        pressStrength = DefaultPressStrength;

      // Get the tilt strength: (i) from item, (ii) from parent, or (iii) default
      if (double.IsNaN(itemTiltStrength) != true)
        tiltStrength = itemTiltStrength;
      else if (double.IsNaN(tiltStrength))
        tiltStrength = DefaultTiltStrength;

      depressFrame.Value = PressStrengthFactor * pressStrength;
      rotationXFrame.Value = xAngle * tiltStrength;
      rotationYFrame.Value = (yAngle - 90) * tiltStrength;

      tiltStoryboard.Begin();
    }
  }
}
