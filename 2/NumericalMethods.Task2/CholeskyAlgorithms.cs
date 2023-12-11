using System;
namespace NumericalMethods.Task2
{
    static class CholeskyAlgorithm
    {
        public static double[,] DecomposeBandedMatrix(double[,] matrix, int halfRibbonLength)
        {
            double[,] Initial_matrix = new double[matrix.GetLength(0), halfRibbonLength + 1];
            Initial_matrix = ToBanded(matrix, Initial_matrix, halfRibbonLength,false);

            if (isFirstRun)
            {
                Console.WriteLine("Initial_matrix:");
                for (int i = 0; i < Initial_matrix.GetLength(0); i++)
                {

                    for (int j = 0; j < halfRibbonLength + 1; j++)
                    {
                        Console.Write(Initial_matrix[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            _ = matrix ?? throw new ArgumentNullException(nameof(matrix));
            _ = halfRibbonLength > matrix.GetLength(0) || halfRibbonLength > matrix.GetLength(1) ? throw new ArgumentOutOfRangeException(nameof(halfRibbonLength), "Half Ribbon Length must be greater than zero.") : true;
            var resultMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
            var lMatrix = resultMatrix;//ссылки
            var uMatrix = resultMatrix;
           
            for (var i = 0; i < matrix.GetLength(0); ++i)
            {
                int columnStartsIndex = BottomBandedMatrixUtils.GetColumnStartsIndex(i, halfRibbonLength);
                int columnEndsIndex = GetColumnEndsIndex(i);

                for (var j = columnStartsIndex; j <= i; ++j)
                {
                    double sum = matrix[i, j];

                    for (var k = 0; k < j; ++k)
                        sum -= lMatrix[i, k] * uMatrix[k, j];

                    lMatrix[i, j] = sum;
                }

                for (var j = i + 1; j <= columnEndsIndex; ++j)
                {
                    double sum = matrix[i, j];

                    for (var k = 0; k < i; ++k)
                        sum -= lMatrix[i, k] * uMatrix[k, j];

                    uMatrix[i, j] = sum / lMatrix[i, i];
                }
            }
            var resultBanded = new double[matrix.GetLength(0), halfRibbonLength * 2 + 2];
            resultBanded = ToBanded(resultMatrix, resultBanded, halfRibbonLength,true);
            if (isFirstRun)
            {
                Console.WriteLine("Result matrix:");
                for (int i = 0; i < matrix.GetLength(0); i++)
                {

                    for (int j = 0; j < halfRibbonLength * 2 + 1; j++)
                    {
                        Console.Write(resultBanded[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                isFirstRun = false;
            }
            return resultMatrix;

            int GetColumnEndsIndex(int rowIndex)
                => Math.Min(rowIndex + halfRibbonLength, matrix.GetLength(1) - 1);
        }


        public static double[,] ToBanded(double[,] matrix, double[,] Initial_matrix,int halfRibbonLength,bool flag)
        {
          
            int k = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
            int x =0;
                if (i < halfRibbonLength)
                {
                    for (int j = i; j >= 0; j--)
                    {
                        Initial_matrix[i, halfRibbonLength - i + j] = (double)matrix[i, j];
                    }
                }
                else
                {
                    for (int j = 0; j < Math.Min(halfRibbonLength + 1, i + 1); j++)
                    {
                        Initial_matrix[i, j] = (double)matrix[i, j + k];
                    }
                    k++;
                }
                if (flag)
                {
                    int t = 0;
                    if (matrix.GetLength(0) - i > halfRibbonLength )
                        for (int j = halfRibbonLength + 1; j <= halfRibbonLength * 2; j++)
                        {
                            if (i < matrix.GetLength(0) - halfRibbonLength)
                                Initial_matrix[i, j] = (double)matrix[i, i + j - halfRibbonLength];
                            else
                            {
                                Initial_matrix[i, j] = (double)matrix[i, matrix.GetLength(0) - halfRibbonLength - t];
                                t++;
                            }
                        }
                    else
                    for (int j = i+1; j < matrix.GetLength(0); j++)
                    {
                            Initial_matrix[i, halfRibbonLength + 1 + x] = (double)matrix[i,j];
                    x++;
                    }
                }
            }
            return Initial_matrix;
        }
        private static bool isFirstRun = true;
    }
}