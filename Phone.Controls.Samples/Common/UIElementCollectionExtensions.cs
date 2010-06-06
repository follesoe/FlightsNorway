namespace System.Windows.Controls
{
    internal static class UIElementCollectionExtensions
    {
        public static double GetItemPosition(this UIElementCollection items, int index)
        {
            double position = 0.0;
            if ((index >= 0) && (index < items.Count))
            {
                for (int i = 0; i != index; i++)
                {
                    FrameworkElement item = (FrameworkElement)items[i];
                    if (null != item)
                        position += item.ActualWidth;
                }
            }

            return position;
        }
    }
}
