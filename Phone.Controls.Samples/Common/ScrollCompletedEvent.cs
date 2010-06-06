using System.Windows.Media;

namespace PhoneControls.Samples
{
    internal delegate void ScrollCompletedEventHandler(object sender, ScrollCompletedEventArgs e);

    internal class ScrollCompletedEventArgs
    {
        public int SelectedIndex;
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
