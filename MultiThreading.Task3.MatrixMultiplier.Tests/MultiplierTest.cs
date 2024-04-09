using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            // todo: implement a test method to check the size of the matrix which makes parallel multiplication more effective than
            // todo: the regular one
            const int minSize = 50; // Minimum size of the matrix
            const int maxSize = 150; // Maximum size of the matrix
            const int step = 20; // Step size for increasing matrix size
            const int iterations = 3; // Number of iterations for each matrix size

            Console.WriteLine("Testing efficiency...");

            for (int size = minSize; size <= maxSize; size += step)
            {
                double syncAverageTime = 0;
                double parallelAverageTime = 0;

                for (int i = 0; i < iterations; i++)
                {
                    var matrix1 = new Matrix(size, size, randomInit: true);
                    var matrix2 = new Matrix(size, size, randomInit: true);

                    var stopwatch = Stopwatch.StartNew();
                    IMatrix syncResult = new MatricesMultiplier().Multiply(matrix1, matrix2);
                    stopwatch.Stop();
                    syncAverageTime += stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch = Stopwatch.StartNew();
                    IMatrix parallelResult = new MatricesMultiplierParallel().Multiply(matrix1, matrix2);
                    stopwatch.Stop();
                    parallelAverageTime += stopwatch.Elapsed.TotalMilliseconds;
                }

                syncAverageTime /= iterations;
                parallelAverageTime /= iterations;

                Console.WriteLine($"Matrix Size: {size}x{size}, Sync Avg Time: {syncAverageTime} ms, Parallel Avg Time: {parallelAverageTime} ms");

                Assert.IsTrue(syncAverageTime >= parallelAverageTime, "Parallel multiplication should be faster.");
            }
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion
    }
}
