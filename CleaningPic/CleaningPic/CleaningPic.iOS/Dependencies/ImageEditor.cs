using System;
using CleaningPic.iOS;
using CleaningPic.Utils;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageEditor))]
namespace CleaningPic.iOS
{
    public class ImageEditor : IImageEditor
    {
        public byte[] Resize(byte[] data, int size)
        {
            var uIImage = ToUIImage(data);
            var resizedSize = new CGSize(width: size, height: size);

            UIGraphics.BeginImageContextWithOptions(resizedSize, false, 0.0f);
            uIImage.Draw(new CGRect(0, 0, size, size));
            var newUIImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return ToByteArray(newUIImage);
        }

        public byte[] Square(byte[] data)
        {
            var uIImage = ToUIImage(data);
            int x, y;
            var size = Math.Min(uIImage.Size.Width, uIImage.Size.Height);
            if (uIImage.Size.Width > size)
            {
                x = (int)(uIImage.Size.Width - size) / 2;
                y = 0;
            }
            else
            {
                x = 0;
                y = (int)(uIImage.Size.Height - size) / 2;
            }
            var clipRect = new CGSize(width: size, height: size);

            UIGraphics.BeginImageContextWithOptions(clipRect, false, 0.0f);
            uIImage.Draw(new CGRect(x, y, size, size));
            var newUIImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return ToByteArray(newUIImage);
        }

        // 大きい画像だとSquare()の段階でサイズが非常に大きくなってしまうので、こちらの利用推奨
        public byte[] SquareAndResize(byte[] data, int size)
        {
            return null;
            /*var bitmap = ToBitmap(data);
            var ss = Math.Min(bitmap.Width, bitmap.Height);
            int x, y;
            if (bitmap.Width > ss)
            {
                x = (bitmap.Width - ss) / 2;
                y = 0;
            }
            else
            {
                x = 0;
                y = (bitmap.Height - ss) / 2;
            }

            float scale = ((float)size) / ss;
            Matrix matrix = new Matrix();
            matrix.PostScale(scale, scale);
            var square = Bitmap.CreateBitmap(bitmap, x, y, ss, ss, null, true);
            var newBitmap = Bitmap.CreateBitmap(square, 0, 0, ss, ss, matrix, true);
            return ToByteArray(newBitmap);*/
        }

        private UIImage ToUIImage(byte[] bytes)
        {
            if (bytes != null)
                return new UIImage(NSData.FromArray(bytes));
            else
                return null;
        }

        private byte[] ToByteArray(UIImage uIImage)
        {
            using (var data = uIImage.AsJPEG())
            {
                return data.ToArray();
            }
        }
    }
}
