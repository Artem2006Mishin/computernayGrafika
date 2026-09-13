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