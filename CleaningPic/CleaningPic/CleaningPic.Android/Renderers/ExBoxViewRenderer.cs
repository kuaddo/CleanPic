using Android.Graphics;
using CleaningPic.CustomViews;
using CleaningPic.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExBoxView), typeof(ExBoxViewRenderer))]
namespace CleaningPic.Droid
{
    internal class ExBoxViewRenderer : BoxRenderer
    {
        public override void Draw(Canvas canvas)
        {
            var exBoxView = (ExBoxView)Element;

            using (var paint = new Paint())
            {
                // androidのrendererの場合のみ比率を考えなければならない
                var expand = Width / exBoxView.Width;
                var width = (int)(exBoxView.Width * expand);
                var height = (int)(exBoxView.Height * expand);
                var shadowSize = (int)(exBoxView.ShadowSize * expand);

                var lt = exBoxView.LeftTopRadius;
                var lb = exBoxView.LeftBottomRadius;
                var rt = exBoxView.RightTopRadius;
                var rb = exBoxView.RightBottomRadius;

                paint.AntiAlias = true;

                // 影
                paint.Color = (Xamarin.Forms.Color.FromRgba(0, 0, 0, 112)).ToAndroid();
                var rect = new RectF(shadowSize, shadowSize, width, height);
                DrawPartialRoundRect(rect, lt, lb, rt, rb, canvas, paint);

                // 本体
                paint.Color = exBoxView.Color.ToAndroid();
                rect = new RectF(0, 0, width - shadowSize, height - shadowSize);
                DrawPartialRoundRect(rect, lt, lb, rt, rb, canvas, paint);
            }
        }

        private void DrawPartialRoundRect(RectF rect, int lt, int lb, int rt, int rb, Canvas canvas, Paint paint)
        {
            var l = rect.Left;
            var r = rect.Right;
            var t = rect.Top;
            var b = rect.Bottom;
            var kappa_ = (float)(1 - 4 * (System.Math.Sqrt(2) - 1) / 3);

            using (var path = new Path())
            {
                path.MoveTo(l + lt, t);
                path.CubicTo(l + lt * kappa_, t, l, t + lt * kappa_, l, t + lt);
                path.LineTo(l, b - lb);
                path.CubicTo(l, b - lb * kappa_, l + lb * kappa_, b, l + lb, b);
                path.LineTo(r - rb, b);
                path.CubicTo(r - rb * kappa_, b, r, b - rb * kappa_, r, b - rb);
                path.LineTo(r, t + rt);
                path.CubicTo(r, t + rt * kappa_, r - rt * kappa_, t, r - rt, t);
                canvas.DrawPath(path, paint);
            }
        }
    }
}