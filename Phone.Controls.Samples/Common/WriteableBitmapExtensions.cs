namespace System.Windows.Media.Imaging
{
    public static partial class WriteableBitmapExtensions
    {
        public static void BlitBlt(this WriteableBitmap dst, Point pt, WriteableBitmap src, Rect rc)
        {
            // crop rectangle
            if (rc.X + rc.Width > src.PixelWidth)
                rc.Width = src.PixelWidth - rc.X;
            if (rc.Y + rc.Height > src.PixelHeight)
                rc.Height = src.PixelHeight - rc.Y;
            if (pt.X + rc.Width > dst.PixelWidth)
                rc.Width = dst.PixelWidth - pt.X;
            if (pt.Y + rc.Height > dst.PixelHeight)
                rc.Height = dst.PixelHeight - pt.Y;

            // copy rectangle
            int[] srcPixels = src.Pixels;
            int[] dstPixels = dst.Pixels;
            int srcOffset = (int)(src.PixelWidth * rc.Y + rc.X) * 4;
            int dstOffset = (int)(dst.PixelWidth * pt.Y + pt.X) * 4;
            int max = (int)rc.Height;
            int len = (int)rc.Width * 4;

            for (int y = 0; y < max; y++)
            {
                Buffer.BlockCopy(srcPixels, srcOffset, dstPixels, dstOffset, len);
                srcOffset += src.PixelWidth * 4;
                dstOffset += dst.PixelWidth * 4;
            }
        }
    }
}