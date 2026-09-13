namespace computernayGrafika
{
    /// <summary>
    /// Ось вращения в 3D.
    /// </summary>
    public enum Axis
    {
        X,
        Y,
        Z
    }

    /// <summary>
    /// Точка зрения для косоугольной проекции кабине: с какой стороны смотрим на объект,
    /// т.е. какая из осей играет роль "глубины", уходящей от плоскости проекции.
    /// </summary>
    public enum CabinetView
    {
        Front, // Глубина по Z (вид спереди, плоскость XY)
        Top, // Глубина по Y (вид сверху, плоскость XZ)
        Side // Глубина по X (вид сбоку, плоскость YZ)
    }

    /// <summary>
    /// Класс для выполнения аффинных преобразований (перемещение, масштабирование, поворот)
    /// в 3D с использованием однородных матриц 4×4.
    /// </summary>
    public static class TransformUtils
    {
        /// <summary>
        /// Перемещает точки на (dx, dy, dz).
        /// </summary>
        public static double[,] Move(double[,] points, double dx, double dy, double dz)
        {
            double[,] moveMatrix = new double[4, 4]
            {
                { 1, 0, 0, dx },
                { 0, 1, 0, dy },
                { 0, 0, 1, dz },
                { 0, 0, 0, 1 }
            };
            return MatrixUtils.MultiplyMatrix(moveMatrix, points);
        }

        /// <summary>
        /// Масштабирует точки с коэффициентами sx, sy, sz относительно начала координат.
        /// </summary>
        public static double[,] Scale(double[,] points, double sx, double sy, double sz)
        {
            double[,] scaleMatrix = new double[4, 4]
            {
                { sx, 0, 0, 0 },
                { 0, sy, 0, 0 },
                { 0, 0, sz, 0 },
                { 0, 0, 0, 1 }
            };
            return MatrixUtils.MultiplyMatrix(scaleMatrix, points);
        }

        /// <summary>
        /// Поворачивает точки на угол в градусах вокруг оси X (через начало координат).
        /// </summary>
        public static double[,] RotateX(double[,] points, double angleDegrees)
        {
            double a = angleDegrees * Math.PI / 180.0;
            double cosA = Math.Cos(a);
            double sinA = Math.Sin(a);
            double[,] m = new double[4, 4]
            {
                { 1, 0,     0,     0 },
                { 0, cosA, -sinA,  0 },
                { 0, sinA,  cosA,  0 },
                { 0, 0,     0,     1 }
            };
            return MatrixUtils.MultiplyMatrix(m, points);
        }

        /// <summary>
        /// Поворачивает точки на угол в градусах вокруг оси Y (через начало координат).
        /// </summary>
        public static double[,] RotateY(double[,] points, double angleDegrees)
        {
            double a = angleDegrees * Math.PI / 180.0;
            double cosA = Math.Cos(a);
            double sinA = Math.Sin(a);
            double[,] m = new double[4, 4]
            {
                {  cosA, 0, sinA, 0 },
                {  0,    1, 0,    0 },
                { -sinA, 0, cosA, 0 },
                {  0,    0, 0,    1 }
            };
            return MatrixUtils.MultiplyMatrix(m, points);
        }

        /// <summary>
        /// Поворачивает точки на угол в градусах вокруг оси Z (через начало координат).
        /// Совпадает с поворотом из исходной 2D-версии.
        /// </summary>
        public static double[,] RotateZ(double[,] points, double angleDegrees)
        {
            double a = angleDegrees * Math.PI / 180.0;
            double cosA = Math.Cos(a);
            double sinA = Math.Sin(a);
            double[,] m = new double[4, 4]
            {
                { cosA, -sinA, 0, 0 },
                { sinA,  cosA, 0, 0 },
                { 0,     0,    1, 0 },
                { 0,     0,    0, 1 }
            };
            return MatrixUtils.MultiplyMatrix(m, points);
        }

        /// <summary>
        /// Строит косоугольную проекцию (Кабине)
        /// </summary>
        /// <param name="points"> однородная матрица точек 4×N </param>
        /// <param name="view"> с какой стороны строится проекция (какая ось — "глубина") </param>
        /// <param name="alphaDegrees"> угол наклона оси глубины к горизонтали, градусы </param>
        /// <param name="distortionFactor"> коэффициент искажения глубины (0.5 — кабине, 1 — кавалье) </param>
        public static double[,] CabinetProjection(double[,] points, CabinetView view, double alphaDegrees, double distortionFactor = 0.5)
        {
            double a = alphaDegrees * Math.PI / 180.0;
            double L = distortionFactor;
            double lc = L * Math.Cos(a);
            double ls = L * Math.Sin(a);

            double[,] m = view switch
            {
                // вид спереди: проекция на плоскость XY, глубина — Z
                CabinetView.Front => new double[4, 4]
                {
                    { 1, 0, lc, 0 },
                    { 0, 1, ls, 0 },
                    { 0, 0, 0,  0 },
                    { 0, 0, 0,  1 }
                },
                // вид сверху: проекция на плоскость XZ, глубина — Y
                CabinetView.Top => new double[4, 4]
                {
                    { 1, lc, 0, 0 },
                    { 0, 0,  0, 0 },
                    { 0, ls, 1, 0 },
                    { 0, 0,  0, 1 }
                },
                // вид сбоку: проекция на плоскость YZ, глубина — X
                CabinetView.Side => new double[4, 4]
                {
                    { 0,  0, 0, 0 },
                    { lc, 1, 0, 0 },
                    { ls, 0, 1, 0 },
                    { 0,  0, 0, 1 }
                },
                _ => throw new ArgumentOutOfRangeException(nameof(view))
            };

            return MatrixUtils.MultiplyMatrix(m, points);
        }

        /// <summary>
        /// Поворачивает точки на угол в градусах вокруг заданной оси относительно
        /// произвольного центра (cx, cy, cz): перенос в центр -> поворот -> перенос обратно.
        /// </summary>
        public static double[,] RotateAt(double[,] points, double angleDegrees, Axis axis, double cx, double cy, double cz)
        {
            var moved = Move(points, -cx, -cy, -cz);
            double[,] rotated = axis switch
            {
                Axis.X => RotateX(moved, angleDegrees),
                Axis.Y => RotateY(moved, angleDegrees),
                Axis.Z => RotateZ(moved, angleDegrees),
                _ => moved
            };
            return Move(rotated, cx, cy, cz);
        }
    }
}