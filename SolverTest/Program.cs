using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Solvers;

using MathNet.Numerics.LinearAlgebra.Solvers;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics;


namespace SolverTest
{
    class Program
    {

        public static double MyCostFunctionMethod(double[] theta)
        {
            // Can add more complicated logic here
        Matrix<Double> featureValues = DenseMatrix.OfArray(new double[,] {
        {1,0.1},
        {1,0.2},
        {1,0.3},
        {1,0.4},
        {1,0.5},
        {1,0.6},
        {1,0.7},
        {1,0.8},
        {1,0.9},
        {1,1},});

            Vector<Double> trueValues = DenseVector.OfArray(new double[] {
0.95166,
   0.96953,
   0.98092,
   0.98811,
   0.99261,
   0.99541,
   0.99716,
   0.99824,
   0.99891,
   0.99932});

            Vector<Double> temptheta = DenseVector.OfArray(theta);
            Console.WriteLine(theta[0].ToString() + " - " + theta[1].ToString());
            Console.WriteLine(Distance.MSE(predictFunction(featureValues, temptheta), trueValues));

            return Distance.MSE(predictFunction(featureValues,temptheta), trueValues);
        }

        static Vector<Double> predictFunction(Matrix<Double> featureValues, Vector<Double> theta)
        {
            
            return sigmoidFunction(featureValues * theta);
        }

        static Vector<Double> sigmoidFunction(Vector<Double> values)
        {
            
            var result = (1 / (values.Multiply(-1).PointwiseExp() + 1));
            Console.WriteLine(result);
            return result;
        }


        static void Main(string[] args)
        {

            double[] xInitial = new double[] { -1, 1 };
            double[] xLower = new double[] { -10, -10 };
            double[] xUpper = new double[] { 10, 10 };

            Func < double[], double> costFunction = MyCostFunctionMethod;

            var solution = NelderMeadSolver.Solve(costFunction, xInitial, xLower, xUpper);
            

            Console.WriteLine(solution.Result);
            Console.WriteLine("solution = {0}", solution.GetSolutionValue(0));
            Console.WriteLine("x = {0}", solution.GetValue(1));
            Console.WriteLine("y = {0}", solution.GetValue(2));

            Console.ReadKey();
        }
    }
}
