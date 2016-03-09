﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Solvers;


namespace SolverTest
{
    class Program
    {

        public static double MyCostFunctionMethod(double[] x)
        {
            // Can add more complicated logic here
            return x[1] - x[0];
        }

        static void Main(string[] args)
        {

            double[] xInitial = new double[] { 1, 1 };
            double[] xLower = new double[] { -1, -1 };
            double[] xUpper = new double[] { 10, 10 };

            Func<double[], double> costFunction = MyCostFunctionMethod;

            var solution = NelderMeadSolver.Solve(costFunction, xInitial, xLower, xUpper);

            Console.WriteLine(solution.Result);
            Console.WriteLine("solution = {0}", solution.GetSolutionValue(0));
            Console.WriteLine("x = {0}", solution.GetValue(1));
            Console.WriteLine("y = {0}", solution.GetValue(2));

            Console.ReadKey();
        }
    }
}
