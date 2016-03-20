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
using System.IO;

namespace SolverTest
{
    class Program
    {

        public Matrix<Double> featureValues;
        public Vector<Double> trueValues;

        public static double[,] readFileIntoArray()
        {
            var items = File.ReadAllLines("testData") // read lines from file
                    .Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(Double.Parse).ToArray());

            double[][] valuesFromFile = new double[100][];

            int i = 0;
            foreach (var item in items)
            {
                valuesFromFile[i] = item;
                    i++;
            }


          //  Console.WriteLine(items);

            return JaggedToMultidimensional(valuesFromFile);
        }

        public static double[] readResultFileIntoArray()
        {
            var items = File.ReadAllLines("saveResult") // read lines from file
                    .Select(Double.Parse).ToArray();

            double[] valuesFromFile = new double[100];

            int i = 0;
            foreach (var item in items)
            {
                valuesFromFile[i] = item;
                i++;
            }


            //  Console.WriteLine(items);

            return valuesFromFile;
        }

        public static T[,] JaggedToMultidimensional<T>(T[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray.Max(subArray => subArray.Length);
            T[,] array = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = jaggedArray[i][j];
                }
            }
            return array;
        }

        public static double MyCostFunctionMethod(double[] theta,Matrix<Double> featureValues, Vector<Double> trueValues)
        {
            // Can add more complicated logic here



            Vector<Double> temptheta = DenseVector.OfArray(theta);
         //   Console.WriteLine(theta[0].ToString() + " - " + theta[1].ToString());
          //  Console.WriteLine(Distance.MSE(predictFunction(featureValues, temptheta), trueValues));

            return Distance.MSE(predictFunction(featureValues,temptheta), trueValues);
        }

        static Vector<Double> predictFunction(Matrix<Double> featureValues, Vector<Double> theta)
        {
            
            return sigmoidFunction(featureValues * theta);
        }

        static Vector<Double> sigmoidFunction(Vector<Double> values)
        {
            
            var result = (1 / (values.Multiply(-1).PointwiseExp() + 1));
         //   Console.WriteLine(result);
            return result;
        }


        static void Main(string[] args)
        {


            readResultFileIntoArray();
            
            

             readFileIntoArray();

            double[] xInitial = new double[17] { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 };
            double[] xLower = new double[17] { -5,-5,-5,-5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5 };
            double[] xUpper = new double[17] { 5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5 };

           // Func <double[], double> costFunction = MyCostFunctionMethod;

            //var solution = NelderMeadSolver.Solve(costFunction, xInitial, xLower, xUpper);

            Matrix<Double> featureValues = DenseMatrix.OfArray(readFileIntoArray());

            Vector<Double> trueValues = DenseVector.OfArray(readResultFileIntoArray());

            int y = 7;

            var solution = NelderMeadSolver.Solve(x => MyCostFunctionMethod(x,featureValues,trueValues), xInitial, xLower, xUpper);



            Console.WriteLine(solution.Result);
            Console.WriteLine("solution = {0}", solution.GetSolutionValue(0));
            for (int i = 0; i < 17; i++)
            {
                Console.WriteLine("x = {0}", solution.GetValue(i+1));
            }

            

            Console.ReadKey();
        }


    }
}
