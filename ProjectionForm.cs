using System;

namespace computernayGrafika
{
    /// <summary>
    /// Форма для отображения трёх видов косоугольной проекции кабине
    /// (спереди, сверху, сбоку) одной и той же фигуры.
    /// </summary>
    public partial class ProjectionForm : Form
    {
        private readonly double[,] frontPlane; // 2×N: X, Y
        private readonly double[,] topPlane;   // 2×N: X, Z
        private readonly double[,] sidePlane;  // 2×N: Y, Z
        private readonly (int a, int b)[] edges;

        /// <summary>
        /// Создаёт форму с тремя готовыми (уже спроецированными) плоскими наборами точек.
        /// </summary>
        /// <param name="frontPlane"> 2×N точки вида спереди (X, Y) </param>
        /// <param name="topPlane"> 2×N точки вида сверху (X, Z) </param>
        /// <param name="sidePlane"> 2×N точки вида сбоку (Y, Z) </param>
        /// <param name="edges"> список рёбер (1-based индексы), общий для всех трёх видов </param>
        public ProjectionForm(double[,] frontPlane, double[,] topPlane, double[,] sidePlane, (int a, int b)[] edges)
        {
            InitializeComponent();

            this.frontPlane = frontPlane;
            this.topPlane = topPlane;
            this.sidePlane = sidePlane;
            this.edges = edges;

            pictureBoxFront.Paint += (s, e) => DrawPlane(e.Graphics, pictureBoxFront, this.frontPlane);
            pictureBoxTop.Paint += (s, e) => DrawPlane(e.Graphics, pictureBoxTop, this.topPlane);
            pictureBoxSide.Paint += (s, e) => DrawPlane(e.Graphics, pictureBoxSide, this.sidePlane);

            pictureBoxFront.Resize += (s, e) => pictureBoxFront.Invalidate();
            pictureBoxTop.Resize += (s, e) => pictureBoxTop.Invalidate();
            pictureBoxSide.Resize += (s, e) => pictureBoxSide.Invalidate();
        }

        /// <summary>
        /// Отрисовывает один плоский (2×N) набор точек и рёбер между ними
        /// в пределах переданного PictureBox, с автоматическим масштабированием.
        /// </summary>
        private void DrawPlane(Graphics g, PictureBox box, double[,] plane)
        {
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = box.ClientSize.Width;
            int h = box.ClientSize.Height;
            if (w <= 0 || h <= 0 || plane == null)
                return;

            int n = plane.GetLength(1);

            double maxAbsX = 1.0, maxAbsY = 1.0;
            for (int i = 0; i < n; i++)
            {
                maxAbsX = Math.Max(maxAbsX, Math.Abs(plane[0, i]));
                maxAbsY = Math.Max(maxAbsY, Math.Abs(plane[1, i]));
            }

            double scaleX = (w / 2.0) / maxAbsX;
            double scaleY = (h / 2.0) / maxAbsY;
            double scale = Math.Max(0.000001, Math.Min(scaleX, scaleY)) * 0.8;

            int cx = w / 2;
            int cy = h / 2;

            Point ToScreen(int i) => new Point(
                cx + (int)Math.Round(plane[0, i] * scale),
                cy - (int)Math.Round(plane[1, i] * scale));

            using (var axisPen = new Pen(Color.LightGray, 1))
            {
                g.DrawLine(axisPen, 0, cy, w, cy);
                g.DrawLine(axisPen, cx, 0, cx, h);
            }

            using (var edgePen = new Pen(Color.Blue, 1.5f))
            {
                foreach (var edge in edges)
                {
                    int ia = edge.a - 1;
                    int ib = edge.b - 1;
                    if (ia < 0 || ia >= n || ib < 0 || ib >= n) continue;
                    g.DrawLine(edgePen, ToScreen(ia), ToScreen(ib));
                }
            }

            using (var brush = new SolidBrush(Color.Black))
            {
                for (int i = 0; i < n; i++)
                {
                    var p = ToScreen(i);
                    g.FillEllipse(brush, p.X - 2, p.Y - 2, 4, 4);
                }
            }
        }
    }
}