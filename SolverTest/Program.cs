using System;
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
        static void Main(string[] args)
        {

            double[] xInitial = new double[] { 1, 1 };
            double[] xLower = new double[] { -1, -1 };
            double[] xUpper = new double[] { 10, 10 };

            var solution = NelderMeadSolver.Solve(
            x => (x[1] - x[0]), xInitial, xLower, xUpper);

            Console.WriteLine(solution.Result);
            Console.WriteLine("solution = {0}", solution.GetSolutionValue(0));
            Console.WriteLine("x = {0}", solution.GetValue(1));
            Console.WriteLine("y = {0}", solution.GetValue(2));

            Console.ReadKey();
        }
    }
}
