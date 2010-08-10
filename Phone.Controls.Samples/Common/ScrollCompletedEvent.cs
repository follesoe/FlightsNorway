using System.Windows.Media;

namespace Phone.Controls.Samples
{
    internal delegate void ScrollCompletedEventHandler(object sender, ScrollCompletedEventArgs e);

    internal class ScrollCompletedEventArgs
    {
        public int SelectedIndex;
        public ScrollCompletedEventArgs()
        {
        }
    }

    internal struct ScrollHost
    {
        public TranslateTransform Transform;
        public double Width;
        public double Padding;
        public double Speed;
        public void Reset() { Width = 0.0; Speed = 1.0; }
    }

}
