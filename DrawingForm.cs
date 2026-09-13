using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace computernayGrafika
{
    /// <summary>
    /// Форма для рисования фигуры (3D-тело, полученное выдавливанием 2D-контура)
    /// </summary>
    public partial class DrawingForm : Form
    {
        // число точек исходного 2D-контура (как в базовой версии)
        private const int ContourCount = 21;

        // 3×N (X, Y, Z): опорная (нетронутая) версия фигуры — 42 точки,
        // индексы 0..20 — передняя грань, 21..41 — задняя грань
        private double[,]? originalPoints3D;

        // 3×N (X, Y, Z): текущая (трансформируемая) версия фигуры
        private double[,]? currentPoints;

        private static Random _random = new Random();

        // рёбра исходного 2D-контура (1-based индексы 1..21)
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

        // рёбра 3D-тела: рёбра передней грани + рёбра задней грани + боковые (соединительные) рёбра
        private (int a, int b)[] edges3D;

        // внутренние точки контура, которые НЕ дублируются на обратной стороне:
        // на задней грани не должно быть ни самих этих точек, ни рёбер к ним
        // (ни между собой, ни с другими точками-дублерами), ни бокового ребра к передней точке
        private static readonly HashSet<int> BackFaceExcludedPoints = new HashSet<int> { 8, 19, 20, 21 };

        // точки, чей дублер на задней грани существует, но без бокового ребра
        // (соединения передней точки с её дублером)
        private static readonly HashSet<int> BackFaceNoSideEdge = new HashSet<int> { 17 };

        // конкретные рёбра, которые не нужно строить на задней грани,
        // даже если обе точки не исключены целиком
        private static readonly (int a, int b)[] BackFaceExtraExcludedEdges = new (int, int)[] { (3, 13) };

        private static bool EdgeMatches(int a, int b, (int x, int y) pair)
        {
            return (a == pair.x && b == pair.y) || (a == pair.y && b == pair.x);
        }

        // текущий масштаб перевода мировых координат в экранные (пересчитывается при отрисовке)
        private double scale = 1.0;

        // глубина выдавливания фигуры по оси Z
        private double extrusionDepth = 4.0;

        // коэффициенты кавальерной проекции: доля Z, добавляемая к экранным X и Y,
        // чтобы задняя грань визуально "уходила" в сторону от передней
        private const double ProjKx = 0.5;
        private const double ProjKy = 0.3;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DrawingForm()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;

            pictureBoxCanvas.Resize += (s, e) => Redraw();

            // значения по умолчанию для полей углов и глубины
            txtAngleX.Text = "0";
            txtAngleY.Text = "0";
            txtAngleZ.Text = "0";
            txtDepth.Text = extrusionDepth.ToString(CultureInfo.InvariantCulture);

            edges3D = BuildEdges3D();

            // инициализация опорной фигуры
            OriginalPointsInitialize();

            // копия для рабочей (трансформируемой) фигуры
            currentPoints = (double[,])originalPoints3D!.Clone();

            Redraw();
        }

        /// <summary>
        /// Строит список рёбер 3D-тела из рёбер исходного 2D-контура:
        /// рёбра передней грани, рёбра задней грани (со сдвигом индексов на ContourCount)
        /// и боковые рёбра, соединяющие каждую точку контура с её копией на задней грани.
        /// </summary>
        private (int a, int b)[] BuildEdges3D()
        {
            var list = new List<(int a, int b)>();

            // передняя грань — контур как есть, со всеми соединениями без исключений
            foreach (var e in edges)
                list.Add(e);

            // задняя грань — тот же контур, индексы сдвинуты на ContourCount,
            // но без рёбер, где хотя бы один конец — исключённая внутренняя точка,
            // и без отдельных рёбер из BackFaceExtraExcludedEdges
            foreach (var e in edges)
            {
                if (BackFaceExcludedPoints.Contains(e.a) || BackFaceExcludedPoints.Contains(e.b))
                    continue;
                if (BackFaceExtraExcludedEdges.Any(pair => EdgeMatches(e.a, e.b, pair)))
                    continue;
                list.Add((e.a + ContourCount, e.b + ContourCount));
            }

            // боковые рёбра — продолжение контура вглубь; для исключённых точек
            // заднего дублера нет вообще, а для точек из BackFaceNoSideEdge
            // дублер есть, но без бокового ребра к передней точке
            for (int i = 1; i <= ContourCount; i++)
            {
                if (BackFaceExcludedPoints.Contains(i) || BackFaceNoSideEdge.Contains(i))
                    continue;
                list.Add((i, i + ContourCount));
            }

            return list.ToArray();
        }

        /// <summary>
        /// Инициализирует опорную 3D-фигуру: строит 2D-контур и выдавливает его
        /// на переднюю (Z = +depth/2) и заднюю (Z = -depth/2) грани.
        /// </summary>
        public void OriginalPointsInitialize()
        {
            double[,] contour = new double[2, ContourCount]
            {
                { 4, 6, 4, 6, 9, 7, 2, 0, -2, -7, -9, -6, -4, -6, -4, -1, 0, 1, -1, 1, 0 },   // X
                { 13, 8, 0, -4, -7, -10, -13, -12, -13, -10, -7, -4, 0, 8, 13, 8, 1, 8, -7, -7, -9 }    // Y
            };

            double halfDepth = extrusionDepth / 2.0;
            originalPoints3D = new double[3, ContourCount * 2];

            for (int i = 0; i < ContourCount; i++)
            {
                // передняя грань
                originalPoints3D[0, i] = contour[0, i];
                originalPoints3D[1, i] = contour[1, i];
                originalPoints3D[2, i] = halfDepth;

                // задняя грань — точная копия контура на глубине -halfDepth
                int j = i + ContourCount;
                originalPoints3D[0, j] = contour[0, i];
                originalPoints3D[1, j] = contour[1, i];
                originalPoints3D[2, j] = -halfDepth;
            }
        }

        /// <summary>
        /// Переводит мировую точку (X, Y, Z) в экранные координаты.
        /// Использует кавальерную (псевдо-3D) проекцию: Z частично добавляется к X и Y.
        /// </summary>
        private Point WorldToScreen(double x, double y, double z)
        {
            double px = x + z * ProjKx;
            double py = y + z * ProjKy;

            int cx = pictureBoxCanvas.ClientSize.Width / 2;
            int cy = pictureBoxCanvas.ClientSize.Height / 2;
            int sx = cx + (int)Math.Round(px * scale);
            int sy = cy - (int)Math.Round(py * scale);
            return new Point(sx, sy);
        }

        /// <summary>
        /// Метод для запроса перерисовки
        /// </summary>
        private void Redraw()
        {
            pictureBoxCanvas.Invalidate();
        }

        /// <summary>
        /// Обработчик клика по кнопке "Построить".
        /// Читает глубину выдавливания и заново строит фигуру
        /// </summary>
        private void BtnDraw_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtDepth.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double depth) && depth >= 0)
                extrusionDepth = depth;

            OriginalPointsInitialize();
            currentPoints = (double[,])originalPoints3D!.Clone();
            Redraw();
        }

        /// <summary>
        /// Общий метод поворота вокруг заданной оси на угол, взятый из текстового поля.
        /// Поворот выполняется относительно центра масс текущей фигуры.
        /// </summary>
        /// <param name="axis"> ось вращения (X, Y или Z) </param>
        /// <param name="angleText"> текст с углом в градусах </param>
        private void RotateAxis(Axis axis, string angleText)
        {
            if (currentPoints == null)
            {
                MessageBox.Show("Сначала постройте фигуру.", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!double.TryParse(angleText, NumberStyles.Float, CultureInfo.InvariantCulture, out double angleDeg))
            {
                MessageBox.Show("Угол должен быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int cols = currentPoints.GetLength(1);
            double cx = 0, cy = 0, cz = 0;
            for (int i = 0; i < cols; i++)
            {
                cx += currentPoints[0, i];
                cy += currentPoints[1, i];
                cz += currentPoints[2, i];
            }
            cx /= cols;
            cy /= cols;
            cz /= cols;

            var matrix = MatrixUtils.CreateMatrix(currentPoints);
            matrix = TransformUtils.RotateAt(matrix, angleDeg, axis, cx, cy, cz);
            currentPoints = MatrixUtils.ToPoints(matrix);

            Redraw();
        }

        /// <summary>
        /// Обработчик клика по кнопке "Повернуть X"
        /// </summary>
        private void BtnRotateX_Click(object sender, EventArgs e) => RotateAxis(Axis.X, txtAngleX.Text);

        /// <summary>
        /// Обработчик клика по кнопке "Повернуть Y"
        /// </summary>
        private void BtnRotateY_Click(object sender, EventArgs e) => RotateAxis(Axis.Y, txtAngleY.Text);

        /// <summary>
        /// Обработчик клика по кнопке "Повернуть Z"
        /// </summary>
        private void BtnRotateZ_Click(object sender, EventArgs e) => RotateAxis(Axis.Z, txtAngleZ.Text);

        /// <summary>
        /// Обработчик клика по кнопке "Проекция".
        /// Строит три вида косоугольной проекции кабине (спереди, сверху, сбоку)
        /// текущей фигуры и показывает их на отдельной форме.
        /// </summary>
        private void BtnProjection_Click(object sender, EventArgs e)
        {
            if (currentPoints == null)
            {
                MessageBox.Show("Сначала постройте фигуру.", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            const double alphaDeg = 45.0; // угол наклона оси глубины к горизонтали

            var homogeneous = MatrixUtils.CreateMatrix(currentPoints);

            var frontProjected = MatrixUtils.ToPoints(TransformUtils.CabinetProjection(homogeneous, CabinetView.Front, alphaDeg));
            var topProjected = MatrixUtils.ToPoints(TransformUtils.CabinetProjection(homogeneous, CabinetView.Top, alphaDeg));
            var sideProjected = MatrixUtils.ToPoints(TransformUtils.CabinetProjection(homogeneous, CabinetView.Side, alphaDeg));

            // для каждого вида забираем только те две координаты, которые не обнулились
            var frontPlane = MatrixUtils.ExtractPlane(frontProjected, 0, 1); // X, Y
            var topPlane = MatrixUtils.ExtractPlane(topProjected, 0, 2);     // X, Z
            var sidePlane = MatrixUtils.ExtractPlane(sideProjected, 1, 2);   // Y, Z

            using (var projectionForm = new ProjectionForm(frontPlane, topPlane, sidePlane, edges3D))
            {
                projectionForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// Обработчик клика по кнопке "Очистить"
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            originalPoints3D = null;
            currentPoints = null;
            Redraw();
        }

        /// <summary>
        /// Метод для отрисовки текущей фигуры и сетки
        /// </summary>
        private void PictureBoxCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;

            if (w <= 0 || h <= 0)
                return;

            // границы (в проекции) для автоматического масштабирования
            double maxAbsX = 1.0;
            double maxAbsY = 1.0;

            var referencePoints = originalPoints3D ?? currentPoints;
            if (referencePoints != null)
            {
                int cols = referencePoints.GetLength(1);
                for (int i = 0; i < cols; i++)
                {
                    double px = referencePoints[0, i] + referencePoints[2, i] * ProjKx;
                    double py = referencePoints[1, i] + referencePoints[2, i] * ProjKy;
                    maxAbsX = Math.Max(maxAbsX, Math.Abs(px));
                    maxAbsY = Math.Max(maxAbsY, Math.Abs(py));
                }
            }

            double scaleX = (w / 2.0) / maxAbsX;
            double scaleY = (h / 2.0) / maxAbsY;
            scale = Math.Max(0.000001, Math.Min(scaleX, scaleY)) * 0.8;

            int cx = w / 2;
            int cy = h / 2;

            DrawGrid(g, cx, cy);

            // оси
            using (var boldPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(boldPen, 0, cy, w, cy); // X
                g.DrawLine(boldPen, cx, 0, cx, h);
                g.FillEllipse(Brushes.Black, cx - 3, cy - 3, 6, 6);
                g.DrawString("(0,0)", this.Font, Brushes.Black, cx + 5, cy + 5);
            }

            var pts = currentPoints ?? originalPoints3D;
            if (pts != null && pts.Length > 0)
            {
                int n = pts.GetLength(1);

                // рёбра
                using (var edgePen = new Pen(Color.Blue, 2))
                {
                    foreach (var ePair in edges3D)
                    {
                        int ia = ePair.a - 1;
                        int ib = ePair.b - 1;
                        if (ia < 0 || ia >= n || ib < 0 || ib >= n) continue;
                        var sa = WorldToScreen(pts[0, ia], pts[1, ia], pts[2, ia]);
                        var sb = WorldToScreen(pts[0, ib], pts[1, ib], pts[2, ib]);
                        g.DrawLine(edgePen, sa, sb);
                    }
                }

                // вершины
                using var brush = new SolidBrush(Color.White);
                using var pen = new Pen(Color.Black, 1);
                using var f = new Font(this.Font.FontFamily, 9);
                using var b = new SolidBrush(Color.Black);

                for (int i = 0; i < n; i++)
                {
                    bool isBackFace = i >= ContourCount;
                    int contourNumber = isBackFace ? i - ContourCount + 1 : i + 1;

                    // на обратной стороне исключённых внутренних точек быть не должно
                    if (isBackFace && BackFaceExcludedPoints.Contains(contourNumber))
                        continue;

                    var sp = WorldToScreen(pts[0, i], pts[1, i], pts[2, i]);
                    int r = 4;
                    g.FillEllipse(brush, sp.X - r, sp.Y - r, r * 2, r * 2);
                    g.DrawEllipse(pen, sp.X - r, sp.Y - r, r * 2, r * 2);

                    // подписываем только переднюю грань, чтобы не загромождать рисунок
                    if (!isBackFace)
                        g.DrawString(contourNumber.ToString(), f, b, sp.X + 6, sp.Y - 6);
                }
            }
        }

        /// <summary>
        /// Метод для отрисовки сетки вокруг центра координат
        /// </summary>
        /// <param name="g"> объект Graphics для отрисовки </param>
        /// <param name="cx"> экранный центр системы координат по x </param>
        /// <param name="cy"> экранный центр системы координат по y </param>
        private void DrawGrid(Graphics g, int cx, int cy)
        {
            double bestStep = 1.0;

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;

            using (var thin = new Pen(Color.FromArgb(240, 240, 240)))
            {
                int maxKx = (int)Math.Ceiling((w / 2.0) / (bestStep * scale));
                for (int k = -maxKx; k <= maxKx; k++)
                {
                    if (k == 0) continue;
                    int sx = cx + (int)Math.Round(k * bestStep * scale);
                    g.DrawLine(thin, sx, 0, sx, h);
                }

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
        /// Обработчик клика по кнопке "Переместить" (случайное смещение в плоскости XY)
        /// </summary>
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

            var matrix = MatrixUtils.CreateMatrix(currentPoints);
            matrix = TransformUtils.Move(matrix, dx, dy, 0);
            currentPoints = MatrixUtils.ToPoints(matrix);

            Redraw();
        }

        /// <summary>
        /// Обработчик клика по кнопке "Масштабировать" (случайное масштабирование в плоскости XY)
        /// </summary>
        private void ScaleButton_Click(object sender, EventArgs e)
        {
            if (currentPoints == null) return;

            int w = pictureBoxCanvas.ClientSize.Width;
            int h = pictureBoxCanvas.ClientSize.Height;
            if (w <= 0 || h <= 0) return;

            int cols = currentPoints.GetLength(1);
            double cx = 0, cy = 0, cz = 0;
            for (int i = 0; i < cols; i++)
            {
                cx += currentPoints[0, i];
                cy += currentPoints[1, i];
                cz += currentPoints[2, i];
            }
            cx /= cols;
            cy /= cols;
            cz /= cols;

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

            var matrix = MatrixUtils.CreateMatrix(currentPoints);
            matrix = TransformUtils.Move(matrix, -cx, -cy, -cz);
            matrix = TransformUtils.Scale(matrix, scaleFactor, scaleFactor, 1.0);
            matrix = TransformUtils.Move(matrix, cx, cy, cz);
            currentPoints = MatrixUtils.ToPoints(matrix);

            Redraw();
        }
    }
}