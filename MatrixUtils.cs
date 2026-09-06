namespace computernayGrafika
{
    /// <summary>
    /// Вспомогательный класс для операций с матрицами.
    /// </summary>
    public static class MatrixUtils
    {
        /// <summary>
        /// Создает матрицу 3×N из матрицы 2×N (X, Y) путем добавления строки единиц.
        /// </summary>
        public static double[,] CreateMatrix(double[,] points2D)
        {
            int rows = points2D.GetLength(0);
            int cols = points2D.GetLength(1);
            if (rows != 2)
                throw new ArgumentException("Матрица должна иметь ровно 2 строки (X и Y)");
            double[,] matrix = new double[3, cols];
            for (int i = 0; i < cols; i++)
            {
                matrix[0, i] = points2D[0, i];
                matrix[1, i] = points2D[1, i];
                matrix[2, i] = 1.0;
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
        /// Преобразует матрицу точек 3×N обратно в матрицу 2×N (X, Y).
        /// </summary>
        public static double[,] ToPoints(double[,] matrix3D)
        {
            int rows = matrix3D.GetLength(0);
            int cols = matrix3D.GetLength(1);
            if (rows != 3)
                throw new ArgumentException("Матрица должна иметь ровно 3 строки (X, Y, 1)");
            double[,] points2D = new double[2, cols];
            for (int i = 0; i < cols; i++)
            {
                points2D[0, i] = matrix3D[0, i];
                points2D[1, i] = matrix3D[1, i];
            }
            return points2D;
        }
    }
}
