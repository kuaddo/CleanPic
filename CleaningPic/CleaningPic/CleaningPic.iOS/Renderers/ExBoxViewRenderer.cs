using CleaningPic.CustomViews;
using Xamarin.Forms;
using CleaningPic.iOS;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using UIKit;

[assembly: ExportRenderer(typeof(ExBoxView), typeof(ExBoxViewRenderer))]
namespace CleaningPic.iOS
{
    internal class ExBoxViewRenderer : BoxRenderer
    {
        public override void Draw(CGRect rect)
        {
            var exBoxView = (ExBoxView)Element;

            using (var context = UIGraphics.GetCurrentContext())
            {
                var shadowSize = exBoxView.ShadowSize;
                var width  = (int)exBoxView.Width;
                var height = (int)exBoxView.Height;
                var lt = exBoxView.LeftTopRadius;
                var lb = exBoxView.LeftBottomRadius;
                var rt = exBoxView.RightTopRadius;
                var rb = exBoxView.RightBottomRadius;

                // 影
                context.SetFillColor((Color.FromRgba(0, 0, 0, 112)).ToCGColor());
                context.SetLineWidth(0);
                DrawPartialRoundRect(shadowSize, shadowSize, width, height, lt, lb, rt, rb, context);

                // 本体
                context.SetFillColor(exBoxView.Color.ToCGColor());
                context.SetLineWidth(0);
                DrawPartialRoundRect(0, 0, width - shadowSize, height - shadowSize, lt, lb, rt, rb, context);
            }
        }

        private void DrawPartialRoundRect(int l, int r, int t, int b, int lt, int lb, int rt, int rb, CGContext context)
        {
            context.MoveTo(l + lt, t);
            context.AddArcToPoint(l, t, l + lt * 2, t + lt * 2, 90f);
            context.AddLineToPoint(l, b - lb);
            context.AddArcToPoint(l, b - lb * 2, l + lb * 2, b, 90f);
            context.AddLineToPoint(r - rb, b);
            context.AddArcToPoint(r - rb * 2, b - rb * 2, r, b, 90f);
            context.AddLineToPoint(r, t - rt);
            context.AddArcToPoint(r - rt * 2, t, r, t + rt * 2, 90f);
            context.ClosePath();
            context.DrawPath(CGPathDrawingMode.Fill);
        }
    }
}