using System;
using System.Globalization;

namespace computernayGrafika
{
    /// <summary>
    /// Форма для отрисовки персонажа
    /// </summary>
    public partial class DrawingForm : Form
    {
        private double[,]? originalPoints;
        private double[,]? currentPoints;
        private static Random _random = new Random();

        // Список соединений 
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

        // Масштаб для перевода мировых координат в пиксели (вычисляется при перерисовке)
        private double scale = 1.0;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DrawingForm()
        {
            InitializeComponent();

            //FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            pictureBoxCanvas.Resize += (s, e) => Redraw();

            // Значение угла по умолчанию
            txtAngle.Text = "0";

            // Инициализируем новую фигуру
            OriginalPointsInitialize();

            // Копия для накопительных поворотов
            currentPoints = (double[,])originalPoints!.Clone();
            Array.Copy(originalPoints, currentPoints, originalPoints.Length);

            Redraw();
        }

        /// <summary>
        /// Метод для инициализации фигуры
        /// </summary>
        public void OriginalPointsInitialize()
        {
            originalPoints = new double[2, 21]
            {
                { 4, 6, 4, 6, 9, 7, 2, 0, -2, -7, -9, -6, -4, -6, -4, -1, 0, 1, -1, 1, 0 },   // X
                { 13, 8, 0, -4, -7, -10, -13, -12, -13, -10, -7, -4, 0, 8, 13, 8, 1, 8, -7, -7, -9 }    // Y
            };
        }

        /// <summary>
        /// Метод для преобразования мировой точки в экранную
        /// </summary>
        /// <param name="p"> Мировая точка </param>
        /// <returns></returns>
        private Point WorldToScreen(double x, double y)
        {
            int cx = pictureBoxCanvas.ClientSize.Width / 2;
            int cy = pictureBoxCanvas.ClientSize.Height / 2;
            int sx = cx + (int)Math.Round(x * scale);
            int sy = cy - (int)Math.Round(y * scale);
            return new Point(sx, sy);
        }

        /// <summary>
        /// Метод для полной перерисовки
        /// </summary>
        private void Redraw()
        {
            pictureBoxCanvas.Invalidate();
        }

        /// <summary>
        /// Обработчик событий кнопки "Построить".
        /// Рисует исходную фигуру
        /// </summary>
        /// <param name="sender"> Объект-отправитель (кнопка) </param>
        /// <param name="e"> Аргументы события (пусто) </param>
        private void BtnDraw_Click(object sender, EventArgs e)
        {
            OriginalPointsInitialize();
            currentPoints = (double[,])originalPoints!.Clone();
            Redraw();
        }

        /// <summary>
        /// Обработчик событий для кнопки "Повернуть"
        /// </summary>
        /// <param name="sender"> Объект-отправитель (кнопка) </param>
        /// <param name="e"> Аргументы событий (пусто) </param>
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

            int cols = currentPoints.GetLength(1);
            double cx = 0, cy = 0;
            for (int i = 0; i < cols; i++)
            {
                cx += currentPoints[0, i];
                cy += currentPoints[1, i];
            }
            cx /= cols;
            cy /= cols;

            var newCurrwntPoints = MatrixUtils.CreateMatrix(currentPoints);
            newCurrwntPoints = TransformUtils.Move(newCurrwntPoints, -cx, -cy);
            newCurrwntPoints = TransformUtils.Rotate(newCurrwntPoints, angleDeg);
            newCurrwntPoints = TransformUtils.Move(newCurrwntPoints, cx, cy);
            // Применяем поворот
            currentPoints = MatrixUtils.ToPoints(TransformUtils.RotateAt(MatrixUtils.CreateMatrix(currentPoints), angleDeg,
                cx, cy));

            Redraw();
        }

        /// <summary>
        /// Обработчик событий для кнопки "Очистить"
        /// </summary>
        /// <param name="sender"> Объект-отправитель (кнопка) </param>
        /// <param name="e"> Аргументы событий (пусто) </param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            originalPoints = null;
            currentPoints = null;
            Redraw();
        }

        /// <summary>
        /// Метод для рисования системы координат и фигуры
        /// </summary>
        /// <param name="sender"> Объект-отправитель (pictureBox) </param>
        /// <param name="e"> Аргументы событий </param>
        private void PictureBoxCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;

            if (w <= 0 || h <= 0)
                return;

            // Расчитываем макссимальные значения x и y - для масштабирования
            double maxAbsX = 1.0;
            double maxAbsY = 1.0;

            var referencePoints = originalPoints ?? currentPoints;
            if (referencePoints != null)
            {
                int cols = referencePoints.GetLength(1); // количество точек
                for (int i = 0; i < cols; i++)
                {
                    maxAbsX = Math.Max(maxAbsX, Math.Abs(referencePoints[0, i]));
                    maxAbsY = Math.Max(maxAbsY, Math.Abs(referencePoints[1, i]));
                }
            }

            // Высчитываем пределы, в которых будут находится точки
            double scaleX = (w / 2.0 ) / maxAbsX;
            double scaleY = (h / 2.0 ) / maxAbsY;
            scale = scale = Math.Max(0.000001, Math.Min(scaleX, scaleY)) * 0.8; // Более точно вычисляем масштаб

            int cx = w / 2;
            int cy = h / 2;

            DrawGrid(g, cx, cy);

            // Рисование осей
            using (var boldPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(boldPen, 0, cy, w, cy); // X
                g.DrawLine(boldPen, cx, 0, cx, h);
                g.FillEllipse(Brushes.Black, cx - 3, cy - 3, 6, 6);
                g.DrawString("(0,0)", this.Font, Brushes.Black, cx + 5, cy + 5);
            }

            var pts = currentPoints ?? originalPoints;
            if (pts != null && pts.Length > 0)
            {
                int n = pts.GetLength(1); // количество точек

                // Рёбра
                using (var edgePen = new Pen(Color.Blue, 2))
                {
                    foreach (var ePair in edges)
                    {
                        int ia = ePair.a - 1;
                        int ib = ePair.b - 1;
                        if (ia < 0 || ia >= n || ib < 0 || ib >= n) continue;
                        double x1 = pts[0, ia];
                        double y1 = pts[1, ia];
                        double x2 = pts[0, ib];
                        double y2 = pts[1, ib];
                        var sa = WorldToScreen(x1, y1);
                        var sb = WorldToScreen(x2, y2);
                        g.DrawLine(edgePen, sa, sb);
                    }
                }

                // Вершины
                using var brush = new SolidBrush(Color.White);
                using var pen = new Pen(Color.Black, 1);
                using var f = new Font(this.Font.FontFamily, 9);
                using var b = new SolidBrush(Color.Black);

                for (int i = 0; i < n; i++)
                {
                    double x = pts[0, i];
                    double y = pts[1, i];
                    var sp = WorldToScreen(x, y);
                    int r = 4;
                    g.FillEllipse(brush, sp.X - r, sp.Y - r, r * 2, r * 2);
                    g.DrawEllipse(pen, sp.X - r, sp.Y - r, r * 2, r * 2);
                    g.DrawString((i + 1).ToString(), f, b, sp.X + 6, sp.Y - 6);
                }
            }
        }

        /// <summary>
        /// Метод для отрисовки тонкой сетки мировых координат
        /// </summary>
        /// <param name="g"> Объект Graphics для отрисовки </param>
        /// <param name="cx"> Значение центра мировых координадт по x </param>
        /// <param name="cy"> Значение центра мировых координадт по y </param>
        private void DrawGrid(Graphics g, int cx, int cy)
        {
            // Расстояние между линиями при масштабировании экрана
            double bestStep = 1.0;

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;

            // Рисуем линии
            using (var thin = new Pen(Color.FromArgb(240, 240, 240)))
            {
                // Вертикальные (отстоят друг от друга с шагом x)
                int maxKx = (int)Math.Ceiling((w / 2.0) / (bestStep * scale));
                for (int k = -maxKx; k <= maxKx; k++)
                {
                    if (k == 0) continue;
                    int sx = cx + (int)Math.Round(k * bestStep * scale);
                    g.DrawLine(thin, sx, 0, sx, h);
                }

                // Горизонтальные
                int maxKy = (int)Math.Ceiling((h / 2.0) / (bestStep * scale));
                for (int k = -maxKy; k <= maxKy; k++)
                {
                    if (k == 0) continue;
                    int sy = cy - (int)Math.Round(k * bestStep * scale);
                    g.DrawLine(thin, 0, sy, w, sy);
                }
            }
        }

        /// <summary>
        /// Метод для перемещения фигуры
        /// </summary>
        /// <param name="sender"> Объект-отправитель (кнопка) </param>
        /// <param name="e"> Аргументы событий (пусто) </param>
        private void MoveButton_Click(object sender, EventArgs e)
        {
            if (currentPoints == null)
            {
                MessageBox.Show("Сначала постройте фигуру.", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;
            if (w <= 0 || h <= 0) return;

            int cols = currentPoints.GetLength(1);
            double cx = 0, cy = 0;
            for (int i = 0; i < cols; i++)
            {
                cx += currentPoints[0, i];
                cy += currentPoints[1, i];
            }
            cx /= cols;
            cy /= cols;

            double maxX = 0, maxY = 0;
            for (int i = 0; i < cols; i++)
            {
                maxX = Math.Max(maxX, Math.Abs(currentPoints[0, i] - cx));
                maxY = Math.Max(maxY, Math.Abs(currentPoints[1, i] - cy));
            }
            if (maxX < 1e-9 || maxY < 1e-9) return;

            double worldMaxX = (w / 2.0) / scale;
            double worldMaxY = (h / 2.0) / scale;

            double dxLimit = Math.Max(0, worldMaxX - maxX);
            double dyLimit = Math.Max(0, worldMaxY - maxY);

            double dx = (_random.NextDouble() - 0.5) * dxLimit;
            double dy = (_random.NextDouble() - 0.5) * dyLimit;

            var newCurrentPoints = MatrixUtils.CreateMatrix(currentPoints);
            newCurrentPoints = TransformUtils.Move(newCurrentPoints, dx, dy);
            currentPoints = MatrixUtils.ToPoints(newCurrentPoints);

            Redraw();
        }

        /// <summary>
        /// Метод для масштабирования фигуры
        /// </summary>
        /// <param name="sender"> Объект-отправитель (кнопка) </param>
        /// <param name="e"> Аргументы событий (пусто) </param>
        private void ScaleButton_Click(object sender, EventArgs e)
        {
            if (currentPoints == null) return;

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;
            if (w <= 0 || h <= 0) return;

            int cols = currentPoints.GetLength(1);
            double cx = 0, cy = 0;
            for (int i = 0; i < cols; i++)
            {
                cx += currentPoints[0, i];
                cy += currentPoints[1, i];
            }
            cx /= cols;
            cy /= cols;

            double maxX = 0, maxY = 0;
            for (int i = 0; i < cols; i++)
            {
                maxX = Math.Max(maxX, Math.Abs(currentPoints[0, i] - cx));
                maxY = Math.Max(maxY, Math.Abs(currentPoints[1, i] - cy));
            }
            if (maxX < 1e-9 || maxY < 1e-9) return;

            int margin = Math.Max(10, Math.Min(w, h) / 8);
            double worldMaxX = (w / 2.0 - margin) / scale;
            double worldMaxY = (h / 2.0 - margin) / scale;

            double maxScale = Math.Min(worldMaxX / maxX, worldMaxY / maxY);
            double minScale = 0.1;
            maxScale = Math.Max(minScale, maxScale);

            double scaleFactor = minScale + (maxScale - minScale) * _random.NextDouble();

            var newCurrentPoints = MatrixUtils.CreateMatrix(currentPoints);
            newCurrentPoints = TransformUtils.Move(newCurrentPoints, -cx, -cy);
            newCurrentPoints = TransformUtils.Scale(newCurrentPoints, scaleFactor, scaleFactor);
            newCurrentPoints = TransformUtils.Move(newCurrentPoints, cx, cy);
            currentPoints = MatrixUtils.ToPoints(newCurrentPoints);

            Redraw();
        }
    }
}