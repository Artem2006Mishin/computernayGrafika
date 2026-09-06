namespace computernayGrafika
{
    /// <summary>
    /// Класс для выполнения аффинных преобразований (перемещение, масштабирование, поворот)
    /// с использованием матриц 3×3.
    /// </summary>
    public static class TransformUtils
    {
        /// <summary>
        /// Перемещает точки на (dx, dy).
        /// </summary>
        public static double[,] Move(double[,] points, double dx, double dy)
        {
            // Матрица сдвига 3×3
            double[,] moveMatrix = new double[3, 3]
            {
            { 1, 0, dx },
            { 0, 1, dy },
            { 0, 0, 1 }
            };
            return MatrixUtils.MultiplyMatrix(moveMatrix, points);
        }

        /// <summary>
        /// Масштабирует точки с коэффициентами sx и sy относительно начала координат.
        /// </summary>
        public static double[,] Scale(double[,] points, double sx, double sy)
        {
            double[,] scaleMatrix = new double[3, 3]
            {
            { sx, 0, 0 },
            { 0, sy, 0 },
            { 0, 0, 1 }
            };
            return MatrixUtils.MultiplyMatrix(scaleMatrix, points);
        }

        /// <summary>
        /// Поворачивает точки на угол в градусах (против часовой стрелки) вокруг начала координат.
        /// </summary>
        public static double[,] Rotate(double[,] points, double angleDegrees)
        {
            double angleRad = angleDegrees * Math.PI / 180.0;
            double cosA = Math.Cos(angleRad);
            double sinA = Math.Sin(angleRad);
            double[,] rotateMatrix = new double[3, 3]
            {
            { cosA, -sinA, 0 },
            { sinA,  cosA, 0 },
            { 0,     0,    1 }
            };
            return MatrixUtils.MultiplyMatrix(rotateMatrix, points);
        }

        /// <summary>
        /// Поворачивает точки на угол в градусах относительно заданного центра (cx, cy).
        /// </summary>
        public static double[,] RotateAt(double[,] points, double angleDegrees, double cx, double cy)
        {
            var movedToCenter = Move(points, -cx, -cy);
            var rotated = Rotate(movedToCenter, angleDegrees);
            return Move(rotated, cx, cy);
        }
    }
}
