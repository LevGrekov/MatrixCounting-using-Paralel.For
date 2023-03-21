using MatrixCountingInThreads;
using System.Diagnostics;

public static class Programm
{
    public static void Main()
    {
        var a = new SquareMatrix(500).randomize();
        var b = new SquareMatrix(500).randomize();
        double x = 10000.31;

        var A = new SquareMatrix(40).randomize();
        var B = new SquareMatrix(40).randomize();


        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        SquareMatrix.ParalelPlus(a,b);
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0} для паралельного сложения матриц размером {1}", stopwatch.Elapsed,a.C);
        stopwatch.Reset();

        stopwatch.Start();
        var c = a + b;
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0} для обычного сложения матриц размером {1}", stopwatch.Elapsed, a.C);

        Console.WriteLine("---------------------------------------------------------------------");

        stopwatch.Start();
        SquareMatrix.ParalelMultiply(a, b);
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0} для паралельного УМНОЖЕНИЯ матриц размером {1}", stopwatch.Elapsed, a.C);
        stopwatch.Reset();

        stopwatch.Start();
        var d = a * b;
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0} для обычного УМНОЖЕНИЯ матриц размером {1}", stopwatch.Elapsed, a.C);

        Console.WriteLine("---------------------------------------------------------------------");

        stopwatch.Start();
        SquareMatrix.ParalelMultyBySkalyar(a, x);
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0} для паралельного УМНОЖЕНИЯ матрицы размером {1} и скаляра {2}", stopwatch.Elapsed, a.C,x);
        stopwatch.Reset();

        stopwatch.Start();
        var e = a * x;
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0} для обычного УМНОЖЕНИЯ матрицы размером {1} и скаляра {2}", stopwatch.Elapsed, a.C,x);
    }
}

