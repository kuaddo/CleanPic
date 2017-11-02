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

            using (var path = new Path())
            {
                path.MoveTo(l + lt, t);
                path.ArcTo(new RectF(l ,t, l + lt * 2, t + lt * 2), 270f, -90f);
                path.LineTo(l, b - lb);
                path.ArcTo(new RectF(l, b - lb * 2, l + lb * 2, b), 180f, -90f);
                path.LineTo(r - rb, b);
                path.ArcTo(new RectF(r - rb * 2, b - rb * 2, r, b), 90f, -90f);
                path.LineTo(r, t -rt);
                path.ArcTo(new RectF(r - rt * 2, t, r, t + rt * 2), 0f, -90f);
                canvas.DrawPath(path, paint);
            }
        }
    }
}