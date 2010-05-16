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
    public class TransitionStoryboardHelper
    {
        public static Storyboard BuildTranslateTransformTransition(UIElement element, Point newLocation, TimeSpan period, IEasingFunction easing)
        {
            Duration duration = new Duration(period);

            // Animate X
            DoubleAnimation translateAnimationX = new DoubleAnimation();
            translateAnimationX.To = newLocation.X;
            translateAnimationX.Duration = duration;

            if (easing != null)
                translateAnimationX.EasingFunction = easing;

            Storyboard.SetTarget(translateAnimationX, element);
            Storyboard.SetTargetProperty(translateAnimationX,
                new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));

            // Animate Y
            DoubleAnimation translateAnimationY = new DoubleAnimation();
            translateAnimationY.To = newLocation.Y;
            translateAnimationY.Duration = duration;

            if (easing != null)
                translateAnimationY.EasingFunction = easing;

            Storyboard.SetTarget(translateAnimationY, element);
            Storyboard.SetTargetProperty(translateAnimationY,
                new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));

            Storyboard sb = new Storyboard();
            sb.AutoReverse = false;
            sb.Duration = duration;
            sb.Children.Add(translateAnimationX);
            sb.Children.Add(translateAnimationY);

            return sb;
        }

        public static Storyboard BuildTransparencyTransition(UIElement element, double toValue, TimeSpan period, bool autoReverse)
        {
            Duration duration = new Duration(period);

            SplineDoubleKeyFrame fromFrame = new SplineDoubleKeyFrame();
            fromFrame.KeySpline = new KeySpline();
            fromFrame.KeySpline.ControlPoint1 = new Point(0, 0); // setup easing
            fromFrame.KeySpline.ControlPoint2 = new Point(0.3, 1.0); // setup easing
            fromFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            fromFrame.Value = element.Opacity;

            SplineDoubleKeyFrame toFrame = new SplineDoubleKeyFrame();
            toFrame.KeySpline = new KeySpline();
            toFrame.KeySpline.ControlPoint1 = new Point(0, 0); // setup easing
            toFrame.KeySpline.ControlPoint2 = new Point(0.3, 1.0); // setup easing
            toFrame.KeyTime = KeyTime.FromTimeSpan(period);
            toFrame.Value = toValue;

            DoubleAnimationUsingKeyFrames opacityAnimation = new DoubleAnimationUsingKeyFrames();
            opacityAnimation.Duration = duration;
            opacityAnimation.KeyFrames.Add(fromFrame);
            opacityAnimation.KeyFrames.Add(toFrame);

            Storyboard.SetTarget(opacityAnimation, element);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("(UIElement.Opacity)"));

            Storyboard sb = new Storyboard();
            sb.AutoReverse = autoReverse;
            sb.Duration = duration;
            sb.Children.Add(opacityAnimation);

            return sb;
        }
    }
}
