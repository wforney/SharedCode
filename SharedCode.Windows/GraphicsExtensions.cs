// <copyright file="GraphicsExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows
{
    using System.Drawing;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// The graphics extensions class
    /// </summary>
    public static class GraphicsExtensions
    {
        /// <summary>
        /// Draw and fill a rounded rectangle.
        /// </summary>
        /// <param name="graphics">The graphics object to use.</param>
        /// <param name="pen">The pen to use to draw the rounded rectangle. If <code>null</code>, the border is not drawn.</param>
        /// <param name="brush">The brush to fill the rounded rectangle. If <code>null</code>, the internal is not filled.</param>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="horizontalDiameter">Horizontal diameter for the rounded angles.</param>
        /// <param name="verticalDiameter">Vertical diameter for the rounded angles.</param>
        /// <param name="rectAngles">Angles to round.</param>
        public static void DrawAndFillRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            Brush brush,
            Rectangle rectangle,
            int horizontalDiameter,
            int verticalDiameter,
            RectAngles rectAngles)
        {
            // get out data
            var x = rectangle.X;
            var y = rectangle.Y;
            var width = rectangle.Width;
            var height = rectangle.Height;

            // adapt horizontal and vertical diameter if the rectangle is too little
            if (width < horizontalDiameter)
            {
                horizontalDiameter = width;
            }

            if (height < verticalDiameter)
            {
                verticalDiameter = height;
            }

            /*
             * The drawing is the following:
                    *
                    *             a
                    *      P______________Q
                    *    h /              \ b
                    *   W /                \R
                    *    |                  |
                    *  g |                  | c
                    *   V|                  |S
                    *    f\                / d
                    *     U\______________/T
                    *             e
            */
            var tl = (rectAngles & RectAngles.TopLeft) != 0;
            var tr = (rectAngles & RectAngles.TopRight) != 0;
            var br = (rectAngles & RectAngles.BottomRight) != 0;
            var bl = (rectAngles & RectAngles.BottomLeft) != 0;
            var pointP = tl ? new Point(x + horizontalDiameter / 2, y) : new Point(x, y);
            var pointQ = tr ? new Point(x + width - horizontalDiameter / 2 - 1, y) : new Point(x + width - 1, y);
            var pointR = tr ? new Point(x + width - 1, y + verticalDiameter / 2) : pointQ;
            var pointS =
                br
                    ? new Point(x + width - 1, y + height - verticalDiameter / 2 - 1)
                    : new Point(x + width - 1, y + height - 1);
            var pointT = br ? new Point(x + width - horizontalDiameter / 2 - 1) : pointS;
            var pointU = bl ? new Point(x + horizontalDiameter / 2, y + height - 1) : new Point(x, y + height - 1);
            var pointV = bl ? new Point(x, y + height - verticalDiameter / 2 - 1) : pointU;
            var pointW = tl ? new Point(x, y + verticalDiameter / 2) : pointP;

            using (var gp = new GraphicsPath())
            {
                // a
                gp.AddLine(pointP, pointQ);

                // b
                if (tr)
                {
                    gp.AddArc(x + width - horizontalDiameter - 1, y, horizontalDiameter, verticalDiameter, 270, 90);
                }

                // c
                gp.AddLine(pointR, pointS);

                // d
                if (br)
                {
                    gp.AddArc(
                        x + width - horizontalDiameter - 1,
                        y + height - verticalDiameter - 1,
                        horizontalDiameter,
                        verticalDiameter,
                        0,
                        90);
                }

                // e
                gp.AddLine(pointT, pointU);

                // f
                if (bl)
                {
                    gp.AddArc(x, y + height - verticalDiameter - 1, horizontalDiameter, verticalDiameter, 90, 90);
                }

                // g
                gp.AddLine(pointV, pointW);

                // h
                if (tl)
                {
                    gp.AddArc(x, y, horizontalDiameter, verticalDiameter, 180, 90);
                }

                // end
                gp.CloseFigure();

                // draw
                if (brush != null)
                {
                    graphics.FillPath(brush, gp);
                }

                if (pen != null)
                {
                    graphics.DrawPath(pen, gp);
                }
            }
        }
    }
}