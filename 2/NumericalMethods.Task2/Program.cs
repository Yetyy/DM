using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using NumericalMethods.Core.Extensions;
using NumericalMethods.Core.Utils;
using NumericalMethods.Core.Utils.Interfaces;
using NumericalMethods.Core.Utils.RandomProviders;
using NumericalMethods.Task3.Utils;

namespace NumericalMethods.Task2
{
    class Program
    {
        private static readonly IRangedRandomProvider<int> _intRandom = new IntegerRandomProvider();

        private static readonly IRangedRandomProvider<double> _doubleRandom = new DoubleRandomProvider();
        
        const double NonZeroEps = 1e-5;

        static double FindAccuracies(int count, int halfRibbonLength, double minValue, double maxValue)
        {
            _ = count < 0 ? throw new ArgumentOutOfRangeException(nameof(count), "The number of elements must not be negative.") : true;
            double[,] matrixWithoutRightSide = _doubleRandom.GenerateBandedSymmetricMatrix(count, count, halfRibbonLength, minValue, maxValue).ToPoorlyConditionedMatrix();
            IReadOnlyList<double> expectRandomSolution = _doubleRandom.Repeat(count, minValue, maxValue).ToArray();
            var rightSideBuilder = new RightSideBuilder(matrixWithoutRightSide);
            IReadOnlyList<double> randomRightSide = rightSideBuilder.Build(expectRandomSolution);
            
            double[,] decomposition = CholeskyAlgorithm.DecomposeBandedMatrix(matrixWithoutRightSide, halfRibbonLength);

            IReadOnlyList<double> solution = MatrixDecompositionUtils.Solve(decomposition, randomRightSide);

            return AccuracyUtils.CalculateAccuracy(expectRandomSolution, solution, NonZeroEps);
        }

        static void Main()
        {
            const int TestCount = 2;
            var testCases = new (int n, int length,int minValue, int maxValue)[]
             {
                (10, 1,-10, 10),
                (10, 3,-10, 10),
                (12, 3,-100, 100),
                (12, 5,-1000, 1000),
                (100, 5,-1000, 1000),
                (1000, 5,-1000, 1000),
             };
            FindAccuracies(10, 2, 1, 10); //Example
            foreach (var (MatrixLength, HalfRibbonLength, minValue, maxValue) in testCases)
            {
                IReadOnlyList<double> accuracies = Enumerable
                    .Range(1, TestCount)
                    .Select(_ => FindAccuracies(MatrixLength, HalfRibbonLength,     minValue,   maxValue))
                    .ToArray();

                Console.WriteLine($"N = {MatrixLength}; HalfRibbonLength = {HalfRibbonLength}; Min = {minValue}; Max = {maxValue};");
                Console.WriteLine($"Средняя относительная погрешность системы: {accuracies.Average()}");
            }

            Console.ReadKey(true);
        }
    }
}