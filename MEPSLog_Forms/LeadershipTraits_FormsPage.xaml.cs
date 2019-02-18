using System;
using System.Collections.Generic;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Extended;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MEPSLog_Forms
{
    public partial class LeadershipTraits_FormsPage : ContentPage
    {

        public int mental = 10;
        public int emotional = 10;
        public int physical = 10;
        public int spiritual = 10;
        public float i = .001f;



        public LeadershipTraits_FormsPage()
        {
            InitializeComponent();
        }

        void DrawClicked(object sender, System.EventArgs e)
        {

            int.TryParse(mentalEntry.Text, out mental);
            int.TryParse(emotionalEntry.Text, out emotional);
            int.TryParse(physicalEntry.Text, out physical);
            int.TryParse(spiritualEntry.Text, out spiritual);


            var canvasView = triangleCanvas;
            canvasView.InvalidateSurface();

        }



        public double ConvertToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            Debug.WriteLine("PAINTING");

            // get the current surface from the event args
            var surface = e.Surface;
            // get the canvas that we can draw on
            var canvas = surface.Canvas;
            // clear the canvas / view
            canvas.Clear(SKColors.Transparent);

            // create the paint for drawing the triangle
            var trianglePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkRed,
                StrokeWidth = 35
            };

            // create the paint for drawing the debug lines
            var linePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 10
            };

            //test vars
            //float mental = .9f, physical = .9f, spiritual = .9f;


            int centerX = e.Info.Width / 2;
            int centerY = e.Info.Height / 2;
            int maxwidth = e.Info.Width - 200;

            //calculate maximum point locations
            SKPoint maxMP = new SKPoint(centerX, centerY - maxwidth / 2);
            SKPoint maxSP = new SKPoint(centerX + (maxwidth / 2), centerY + ( maxwidth / 2 * (float)Math.Tan(Math.PI / 6) ));
            SKPoint maxMS = new SKPoint(centerX - (maxwidth / 2), centerY + ( maxwidth / 2 * (float)Math.Tan(Math.PI / 6) ));


            //calculate point offsets based on MEPS inputs
            float mpOffset = (mental + physical) / 20f;
            float spOffset = (spiritual + physical) / 20f;
            float msOffset = (mental + spiritual) / 20f;


            //calculate actual point locations
            SKPoint actualMP = new SKPoint(centerX, centerY - (maxwidth * mpOffset) / 2) ;
            SKPoint actualSP = new SKPoint((centerX + (maxwidth * spOffset / 2)) , (centerY + (maxwidth * spOffset / 2 * (float)Math.Tan(Math.PI / 6))) );
            SKPoint actualMS = new SKPoint((centerX - (maxwidth * msOffset / 2)) , (centerY + (maxwidth * msOffset / 2 * (float)Math.Tan(Math.PI / 6))) );

            //create paths
            SKPath guidePath = new SKPath();
            SKPath trianglePath = new SKPath();
            SKPath emptyPath = new SKPath();

            //path out guidePath
            guidePath.MoveTo(centerX, centerY);
            guidePath.LineTo(maxMP);
            guidePath.LineTo(maxMS);
            guidePath.LineTo(centerX, centerY);
            guidePath.MoveTo(maxMS);
            guidePath.LineTo(maxSP);
            guidePath.LineTo(centerX, centerY);
            guidePath.MoveTo(maxSP);
            guidePath.LineTo(maxMP);
            guidePath.Close();

            //path out trianglePath
            trianglePath.MoveTo(actualMS);
            trianglePath.LineTo(actualMP);
            trianglePath.LineTo(actualSP);
            trianglePath.LineTo(actualMS);
            trianglePath.Close();

            emptyPath.MoveTo(centerX, centerY);
            emptyPath.Close();

            trianglePath.FillType = SKPathFillType.Winding;

            //draw both paths
            canvas.DrawPath(guidePath, linePaint);
            canvas.DrawPath(trianglePath, trianglePaint);



            #region tenCircles

            ////get centerX, centerY, circleDiameter10, and interval
            //int centerX = e.Info.Width / 2;
            //int centerY = e.Info.Height / 2;
            //int circleDiameter10 = e.Info.Width - 100;
            //int interval = circleDiameter10 / 18;

            //SKRect MakeRect(int i)
            //{
            //    SKRect tempRect = new SKRect(50 + (i * interval),
            //                                 (e.Info.Height / 2) - (circleDiameter10 / 2) + (i * interval),
            //                                 (e.Info.Width - 50) - (i * interval),
            //                                 (e.Info.Height / 2) + (circleDiameter10 / 2) - (i * interval));
            //    return tempRect;
            //}



            //// create the paint for the circle border
            //var testPaint = new SKPaint
            //{
            //    IsAntialias = true,
            //    Style = SKPaintStyle.Stroke,
            //    Color = SKColors.Black,
            //    StrokeWidth = 1
            //};


            //SKRect rect10 = MakeRect(0);
            //SKRect rect9 = MakeRect(1);
            //SKRect rect8 = MakeRect(2);
            //SKRect rect7 = MakeRect(3);
            //SKRect rect6 = MakeRect(4);
            //SKRect rect5 = MakeRect(5);
            //SKRect rect4 = MakeRect(6);
            //SKRect rect3 = MakeRect(7);
            //SKRect rect2 = MakeRect(8);
            //SKRect rect1 = MakeRect(9);



            //float startAngle = 0;
            //float sweepAngle = 360;



            //using (SKPath path = new SKPath())
            //{
            //    path.AddArc(rect10, startAngle, sweepAngle);
            //    path.AddArc(rect9, startAngle, sweepAngle);
            //    path.AddArc(rect8, startAngle, sweepAngle);
            //    path.AddArc(rect7, startAngle, sweepAngle);
            //    path.AddArc(rect6, startAngle, sweepAngle);
            //    path.AddArc(rect5, startAngle, sweepAngle);
            //    path.AddArc(rect4, startAngle, sweepAngle);
            //    path.AddArc(rect3, startAngle, sweepAngle);
            //    path.AddArc(rect2, startAngle, sweepAngle);
            //    path.AddArc(rect1, startAngle, sweepAngle);

            //    canvas.DrawPath(path, testPaint);
            //}

            #endregion



        }
    }
}
