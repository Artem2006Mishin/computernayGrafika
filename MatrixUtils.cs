namespace computernayGrafika
{
    /// <summary>
    /// Вспомогательный класс для операций с матрицами.
    /// Точки хранятся как 3×N (X, Y, Z), для преобразований используются
    /// однородные матрицы 4×N (добавляется строка единиц).
    /// </summary>
    public static class MatrixUtils
    {
        /// <summary>
        /// Создаёт однородную матрицу 4×N из матрицы точек 3×N (X, Y, Z)
        /// путём добавления строки единиц.
        /// </summary>
        public static double[,] CreateMatrix(double[,] points3D)
        {
            int rows = points3D.GetLength(0);
            int cols = points3D.GetLength(1);
            if (rows != 3)
                throw new ArgumentException("Матрица должна иметь ровно 3 строки (X, Y, Z)");
            double[,] matrix = new double[4, cols];
            for (int i = 0; i < cols; i++)
            {
                matrix[0, i] = points3D[0, i];
                matrix[1, i] = points3D[1, i];
                matrix[2, i] = points3D[2, i];
                matrix[3, i] = 1.0;
            }
            return matrix;
        }

        /// <summary>
        /// Умножает матрицу A (размера m×n) на матрицу B (размера n×p).
        /// Возвращает матрицу размера m×p.
        /// </summary>
        public static double[,] MultiplyMatrix(double[,] A, double[,] B)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            int p = B.GetLength(1);
            if (n != B.GetLength(0))
                throw new ArgumentException("Количество столбцов A должно совпадать с количеством строк B");

            double[,] result = new double[m, p];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < p; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < n; k++)
                        sum += A[i, k] * B[k, j];
                    result[i, j] = sum;
                }
            return result;
        }

        /// <summary>
        /// Преобразует однородную матрицу 4×N обратно в матрицу точек 3×N (X, Y, Z).
        /// </summary>
        public static double[,] ToPoints(double[,] matrix4D)
        {
            int rows = matrix4D.GetLength(0);
            int cols = matrix4D.GetLength(1);
            if (rows != 4)
                throw new ArgumentException("Матрица должна иметь ровно 4 строки (X, Y, Z, 1)");
            double[,] points3D = new double[3, cols];
            for (int i = 0; i < cols; i++)
            {
                points3D[0, i] = matrix4D[0, i];
                points3D[1, i] = matrix4D[1, i];
                points3D[2, i] = matrix4D[2, i];
            }
            return points3D;
        }
        /// <summary>
        /// Извлекает две координатные строки (например, X и Y) из матрицы точек 3×N,
        /// формируя плоскую матрицу 2×N. Используется после проекции, когда одна из
        /// координат обнулена и для отрисовки нужны только оставшиеся две.
        /// </summary>
        /// <param name="points"> матрица точек (3×N) </param>
        /// <param name="rowA"> индекс строки, которая станет первой (экранной X) </param>
        /// <param name="rowB"> индекс строки, которая станет второй (экранной Y) </param>
        public static double[,] ExtractPlane(double[,] points, int rowA, int rowB)
        {
            int cols = points.GetLength(1);
            double[,] plane = new double[2, cols];
            for (int i = 0; i < cols; i++)
            {
                plane[0, i] = points[rowA, i];
                plane[1, i] = points[rowB, i];
            }
            return plane;
        }
    }
}