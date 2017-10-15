using Xamarin.Forms;
using CleaningPic.Utils;
using CleaningPic.Droid;
using System;
using Android.Graphics;
using System.IO;

[assembly: Dependency(typeof(ImageEditor))]
namespace CleaningPic.Droid
{
    public class ImageEditor : IImageEditor
    {
        public byte[] Resize(byte[] data, int size)
        {
            var bitmap = ToBitmap(data);
            float scale = ((float)size) / bitmap.Width;
            Matrix matrix = new Matrix();
            matrix.PostScale(scale, scale);

            var newBitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
            return ToByteArray(newBitmap);
        }

        public byte[] Square(byte[] data)
        {
            var bitmap = ToBitmap(data);
            var size = Math.Min(bitmap.Width, bitmap.Height);
            int x, y;
            if (bitmap.Width > size)
            {
                x = (bitmap.Width - size) / 2;
                y = 0;
            }
            else
            {
                x = 0;
                y = (bitmap.Height - size) / 2;
            }
            var newBitmap = Bitmap.CreateBitmap(bitmap, x, y, size, size, null, true);
            return ToByteArray(newBitmap);
        }

        // 大きい画像だとSquare()の段階でサイズが非常に大きくなってしまうので、こちらの利用推奨
        public byte[] SquareAndResize(byte[] data, int size)
        {
            var bitmap = ToBitmap(data);
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
            return ToByteArray(newBitmap);
        }

        private Bitmap ToBitmap(byte[] bytes)
        {
            if (bytes != null)
                return BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            else
                return null;
        }

        private byte[] ToByteArray(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}