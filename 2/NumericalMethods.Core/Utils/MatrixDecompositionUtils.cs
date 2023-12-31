﻿using System;
using System.Collections.Generic;

namespace NumericalMethods.Task3.Utils
{
    public static class MatrixDecompositionUtils
    {
        public static IReadOnlyList<double> Solve(double[,] decomposition, IReadOnlyList<double> rightSide)
        {
            _ = decomposition ?? throw new ArgumentNullException(nameof(decomposition));
            _ = rightSide ?? throw new ArgumentNullException(nameof(rightSide));

            var solution = new double[rightSide.Count];
            for (var i = 0; i < rightSide.Count; ++i)//нижний треугольник
            {
                double sum = rightSide[i];

                for (var j = 0; j <= i; ++j)
                    sum -= decomposition[i, j] * solution[j];

                solution[i] = sum / decomposition[i, i];
            }

            for (var i = rightSide.Count - 1; i >= 0; --i)//верхний треугольник
            {
                double sum = solution[i];

                for (var j = i + 1; j < rightSide.Count; ++j)
                    sum -= decomposition[i, j] * solution[j];

                solution[i] = sum;
            }
            if (isFirstRun)
            {
                Console.WriteLine("Solution:");
                for (var i = 0; i < rightSide.Count; ++i)
                    Console.WriteLine(solution[i]);
                Console.WriteLine();
            }
            isFirstRun = false;
            return solution;
        }
        private static bool isFirstRun = true;
    }
}