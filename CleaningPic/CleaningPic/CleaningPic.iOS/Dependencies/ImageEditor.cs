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

            UIGraphics.BeginImageContext(resizedSize);
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

            UIGraphics.BeginImageContextWithOptions(new CGSize(width: size, height: size), false, 0.0f);
            uIImage.Draw(new CGPoint(x: -x, y: -y));
            var newUIImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return ToByteArray(newUIImage);
        }

        // 大きい画像だとSquare()の段階でサイズが非常に大きくなってしまうので、こちらの利用推奨
        public byte[] SquareAndResize(byte[] data, int targetSize)
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

            UIGraphics.BeginImageContextWithOptions(new CGSize(width: size, height: size), false, 0.0f);
            uIImage.Draw(new CGPoint(x: -x, y: -y));
            var squareUIImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            UIGraphics.BeginImageContext(new CGSize(width: targetSize, height: targetSize));
            squareUIImage.Draw(new CGRect(0, 0, targetSize, targetSize));
            var newUIImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return ToByteArray(newUIImage);
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
