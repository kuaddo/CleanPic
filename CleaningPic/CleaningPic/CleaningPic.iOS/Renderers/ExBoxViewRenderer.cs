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

        private void DrawPartialRoundRect(int l, int t, int r, int b, int lt, int lb, int rt, int rb, CGContext context)
        {
            var kappa_ = (float)(1 - 4 * (System.Math.Sqrt(2) - 1) / 3);

            context.MoveTo(l + lt, t);
            context.AddCurveToPoint(l + lt * kappa_, t, l, t + lt * kappa_, l, t + lt);
            context.AddLineToPoint(l, b - lb);
            context.AddCurveToPoint(l, b - lb * kappa_, l + lb * kappa_, b, l + lb, b);
            context.AddLineToPoint(r - rb, b);
            context.AddCurveToPoint(r - rb * kappa_, b, r, b - rb * kappa_, r, b - rb);
            context.AddLineToPoint(r, t + rt);
            context.AddCurveToPoint(r, t + rt * kappa_, r - rt * kappa_, t, r - rt, t);
            context.ClosePath();
            context.DrawPath(CGPathDrawingMode.Fill);
        }
    }
}