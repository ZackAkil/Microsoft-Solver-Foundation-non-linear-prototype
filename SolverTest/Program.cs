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
using ExtensionMethods;

namespace SolverTest
{
    class Program
    {

        public static double[,] readFileIntoArray(string fileName)
        {
            var items = File.ReadAllLines(fileName) // read lines from file
                    .Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(Double.Parse).ToArray());

            double[][] valuesFromFile = new double[items.Count()][];

            int i = 0;
            foreach (var item in items)
            {
                valuesFromFile[i] = item;
                    i++;
            }


          //  Console.WriteLine(items);

            return JaggedToMultidimensional(valuesFromFile);
        }

        public static double[] readResultFileIntoArray(string fileName)
        {
            var items = File.ReadAllLines(fileName) // read lines from file
                    .Select(Double.Parse).ToArray();

            double[] valuesFromFile = new double[items.Count()];

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

            Vector<Double> temptheta = DenseVector.OfArray(theta);

            return Distance.MSE(predictFunction(featureValues,temptheta), trueValues);
        }

        static Vector<Double> predictFunction(Matrix<Double> featureValues, Vector<Double> theta)
        {
            
            return sigmoidFunction(featureValues * theta);
        }

        static Vector<Double> sigmoidFunction(Vector<Double> values)
        {
            
            return (1 / (values.Multiply(-1).PointwiseExp() + 1));
        }


        static void Main(string[] args)
        {

            double[] xInitial = new double[17] { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 };
            double[] xLower = new double[17];
            xLower.Populate(-5);
            double[] xUpper = new double[17];
            xUpper.Populate(5);

            Matrix<Double> featureValues = DenseMatrix.OfArray(readFileIntoArray("testData"));

            Vector<Double> trueValues = DenseVector.OfArray(readResultFileIntoArray("saveResult"));

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
