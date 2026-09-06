using computernayGrafika;
using System.Globalization;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace computernayGrafika
{
    public partial class Form1 : Form
    {
        // Мировая точка с двойной точностью
        public struct WorldPoint
        {
            public double X;
            public double Y;
            public WorldPoint(double x, double y) { X = x; Y = y; }
        }

        // Исходные мировые координаты (фикcированные по умолчанию)
        private WorldPoint[]? originalPoints;

        // Текущие мировые координаты (после поворота)
        private WorldPoint[]? currentPoints;

        // Список соединений (индексы 1-based в таблице пользователя)
        private (int a, int b)[] edges = new (int, int)[]
        {
            (1,2),(1,18),
            (2,1),(2,3),
            (3,2),(3,4),(3,13),(3,17),(3,20),
            (4,3),(4,5),(4,20),
            (5,4),(5,6),
            (6,5),(6,7),
            (7,6),(7,8),(7,9),(7,20),
            (8,7),(8,9),(8,21),
            (9,7),(9,8),(9,10),(9,19),
            (10,9),(10,11),
            (11,10),(11,12),
            (12,11),(12,13),(12,19),
            (13,12),(13,14),(13,17),(13,19),
            (14,13),(14,15),
            (15,14),(15,16),
            (16,15),(16,17),
            (17,16),(17,18),
            (18,1),(18,17),
            (19,9),(19,12),(19,13),(19,20),(19,21),
            (20,3),(20,4),(20,7),(20,19),(20,21),
            (21,8),(21,19),(21,20)
        };

        // Масштаб (мировые единицы -> пиксели). Вычисляется при перерисовке.
        private double scale = 1.0;

        public Form1()
        {
            InitializeComponent();

            // Подписываем обработчики событий
            pictureBoxCanvas.Resize += (s, e) => Redraw();

            // Значение угла по умолчанию
            txtAngle.Text = "0";

            // Инициализируем новую фигуру
            OriginalPointsInitialize();

            // Копия для накопительных поворотов
            currentPoints = new WorldPoint[originalPoints!.Length];
            Array.Copy(originalPoints, currentPoints, originalPoints.Length);

            Redraw();
        }

        public void OriginalPointsInitialize()
        {
            originalPoints ??= new[]
            {
                new WorldPoint(4, 13),   //1
                new WorldPoint(6, 8),    //2
                new WorldPoint(4, 0),    //3
                new WorldPoint(6, -4),   //4
                new WorldPoint(9, -7),   //5
                new WorldPoint(7, -10),  //6
                new WorldPoint(2, -13),  //7
                new WorldPoint(0, -12),  //8
                new WorldPoint(-2, -13), //9
                new WorldPoint(-7, -10), //10
                new WorldPoint(-9, -7),  //11
                new WorldPoint(-6, -4),  //12
                new WorldPoint(-4, 0),   //13
                new WorldPoint(-6, 8),   //14
                new WorldPoint(-4, 13),  //15
                new WorldPoint(-1, 8),   //16
                new WorldPoint(0, 1),    //17
                new WorldPoint(1, 8),    //18
                new WorldPoint(-1, -7),  //19
                new WorldPoint(1, -7),   //20
                new WorldPoint(0, -9)    //21
            };
        }

        // Преобразование мировой точки в экранную
        private Point WorldToScreen(WorldPoint p)
        {
            // Центр PictureBox в пикселях
            int cx = pictureBoxCanvas.ClientSize.Width / 2;
            int cy = pictureBoxCanvas.ClientSize.Height / 2;

            // 1 вопрос!
            int sx = cx + (int)Math.Round(p.X * scale);
            int sy = cy - (int)Math.Round(p.Y * scale);

            return new Point(sx, sy);
        }

        // Полная перерисовка
        private void Redraw()
        {
            pictureBoxCanvas.Invalidate();
        }

        // Обработчик кнопки "Построить" — сброс фигуры к исходной
        private void BtnDraw_Click(object sender, EventArgs e)
        {
            OriginalPointsInitialize();
            currentPoints = new WorldPoint[originalPoints!.Length];
            Array.Copy(originalPoints, currentPoints, originalPoints.Length);
            Redraw();
        }

        // Обработчик кнопки "Повернуть" — повороты накопительные (вращаем currentPoints)
        private void BtnRotate_Click(object sender, EventArgs e)
        {
            if (currentPoints == null)
            {
                MessageBox.Show("Сначала постройте фигуру.", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!double.TryParse(txtAngle.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double angleDeg))
            {
                MessageBox.Show("Угол должен быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double radians = angleDeg * Math.PI / 180.0;
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            for (int i = 0; i < currentPoints.Length; i++)
            {
                var p = currentPoints[i];
                double xNew = p.X * cos - p.Y * sin;
                double yNew = p.X * sin + p.Y * cos;
                currentPoints[i] = new WorldPoint(xNew, yNew);
            }

            Redraw();
        }

        // Обработчик кнопки "Очистить"
        private void BtnClear_Click(object sender, EventArgs e)
        {
            originalPoints = null;
            currentPoints = null;
            Redraw();
        }

        // Рисование системы координат и фигуры
        private void PictureBoxCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;

            if (w <= 0 || h <= 0)
                return;

            double maxAbsX = 1.0;
            double maxAbsY = 1.0;

            // Use the unrotated original points as reference so rotation doesn't change the scale.
            var referencePoints = originalPoints ?? currentPoints;
            if (referencePoints != null)
            {
                foreach (var p in referencePoints)
                {
                    maxAbsX = Math.Max(maxAbsX, Math.Abs(p.X));
                    maxAbsY = Math.Max(maxAbsY, Math.Abs(p.Y));
                }
            }

            // Make margin proportional to control size so smaller windows get larger margin (more shrink)
            int margin = Math.Max(10, Math.Min(w, h) / 8);

            double scaleX = (w / 2.0 - margin) / maxAbsX;
            double scaleY = (h / 2.0 - margin) / maxAbsY;
            scale = Math.Max(0.000001, Math.Min(scaleX, scaleY)); // protective minimum and fit

            // Apply a small global shrink factor so the figure always has spare space in non-fullscreen.
            const double shrinkFactor = 0.82;
            scale *= shrinkFactor;

            int cx = w / 2;
            int cy = h / 2;

            DrawGrid(g, cx, cy);

            using (var boldPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(boldPen, 0, cy, w, cy); // X axis
                g.DrawLine(boldPen, cx, 0, cx, h); // Y axis

                g.FillEllipse(Brushes.Black, cx - 3, cy - 3, 6, 6);
                g.DrawString("(0,0)", this.Font, Brushes.Black, cx + 5, cy + 5);
            }

            var pts = currentPoints ?? originalPoints;
            if (pts != null && pts.Length > 0)
            {
                // Draw edges
                using (var edgePen = new Pen(Color.Blue, 2))
                {
                    foreach (var ePair in edges)
                    {
                        int ia = ePair.a - 1;
                        int ib = ePair.b - 1;
                        if (ia < 0 || ia >= pts.Length || ib < 0 || ib >= pts.Length) continue;
                        var sa = WorldToScreen(pts[ia]);
                        var sb = WorldToScreen(pts[ib]);
                        g.DrawLine(edgePen, sa, sb);
                    }
                }

                // Draw vertices
                using var brush = new SolidBrush(Color.White);
                using var pen = new Pen(Color.Black, 1);
                using var f = new Font(this.Font.FontFamily, 9);
                using var b = new SolidBrush(Color.Black);

                for (int i = 0; i < pts.Length; i++)
                {
                    var sp = WorldToScreen(pts[i]);
                    int r = 4;
                    g.FillEllipse(brush, sp.X - r, sp.Y - r, r * 2, r * 2);
                    g.DrawEllipse(pen, sp.X - r, sp.Y - r, r * 2, r * 2);
                    g.DrawString((i + 1).ToString(), f, b, sp.X + 6, sp.Y - 6);
                }
            }
        }

        // Рисование тонкой сетки мировых координат вокруг центра
        private void DrawGrid(Graphics g, int cx, int cy)
        {
            // Candidates include fine steps (0.1, 0.2, 0.5...) so integer world coords land on intersections.
            double[] candidates = { 0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100, 200 };
            // Prefer denser grid: target pixel step smaller -> more cells visible
            double minPx = 14, maxPx = 48; // slightly smaller cells for better visibility
            double targetPx = (minPx + maxPx) / 2.0;

            double bestStep = candidates[0];
            double bestScore = double.MaxValue;
            foreach (var s in candidates)
            {
                double px = s * scale;
                double score = Math.Abs(px - targetPx);
                if (score < bestScore) { bestScore = score; bestStep = s; }
            }

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;

            using (var thin = new Pen(Color.FromArgb(240, 240, 240)))
            {
                // Draw vertical grid lines anchored at origin (cx) so integer world coords hit intersections.
                int maxKx = (int)Math.Ceiling((w / 2.0) / (bestStep * scale));
                for (int k = -maxKx; k <= maxKx; k++)
                {
                    if (k == 0) continue; // main axis drawn separately
                    int sx = cx + (int)Math.Round(k * bestStep * scale);
                    g.DrawLine(thin, sx, 0, sx, h);
                }

                // Draw horizontal grid lines anchored at origin (cy).
                int maxKy = (int)Math.Ceiling((h / 2.0) / (bestStep * scale));
                for (int k = -maxKy; k <= maxKy; k++)
                {
                    if (k == 0) continue;
                    int sy = cy - (int)Math.Round(k * bestStep * scale); // world Y positive -> up
                    g.DrawLine(thin, 0, sy, w, sy);
                }
            }
        }
    }
}