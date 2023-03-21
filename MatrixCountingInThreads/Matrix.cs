using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCountingInThreads
{
    internal class Matrix
    {
        protected double[,]? data;
        public double[,]? Data
        {
            get => data ?? null;
            set => data = value ?? null;
        }
        public int R => Data.GetLength(0); //Rows
        public int C => Data.GetLength(1); //Columns

        public Matrix Transposed => ~this;

        public double this[int i, int j]
        {
            get
            {
                if (i > Data.GetLength(0) || j > Data.GetLength(1) || i < 0 || j < 0)
                {
                    throw new Exception("Некорректный Индекс");
                }
                return Data[i, j];
            }
            set
            {
                if (i > Data.GetLength(0) || j > Data.GetLength(1) || i < 0 || j < 0)
                {
                    throw new Exception("Некорректный Индекс");
                }
                Data[i, j] = value;
            }
        }
        //Конструкторы
        public Matrix()
        {
            Data = new double[1, 1];
            Data[0, 0] = 0;
        }

        public Matrix(int r, int c)
        {
            if (r <= 0 || c <= 0) throw new Exception("incorrect value of dim");
            Data = new double[r, c];
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    Data[i, j] = 0;
                }
            }
        }

        public Matrix(Matrix other)
        {
            Data = (double[,])other.Data.Clone();
        }


        public Matrix(double[,] Arr)
        {
            int r = Arr.GetLength(0);
            int c = Arr.GetLength(1);

            Data = new double[r, c];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    Data[i, j] = Arr[i, j];
                }
            }
        }
        //ToString
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();

            for (int i = 0; i < R; i++)
            {
                for (int j = 0; j < C; j++)
                {
                    if (Math.Abs(Data[i, j]) < 1.00E-9)
                    {
                        res.Append("0 ");
                        continue;
                    }
                    res.Append(Data[i, j]);
                    res.Append(" ");
                }
                res.Append('\n');
            }
            return res.ToString();
        }

        //Операторы
        public static Matrix operator *(Matrix a, double x)
        {
            Matrix res = new Matrix(a.R, a.C);
            for (int i = 0; i < res.R; i++)
            {
                for (int j = 0; j < res.C; j++)
                {
                    res[i, j] = a[i, j] * x;
                }
            }
            return res;
        }
        public static Matrix operator *(double x, Matrix a) => a * x;
        public static Matrix operator /(Matrix a, double x)
        {
            if (x == 0) throw new DivideByZeroException();
            return a * (1 / x);
        }
        public static Matrix operator /(double x, Matrix a) => a * x;
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.R != b.R || a.C != b.C) throw new Exception("Матрицы имеют разные размерности");
            Matrix res = new Matrix(a.R, a.C);
            for (int i = 0; i < res.R; i += 1)
            {
                for (int j = 0; j < res.C; j += 1)
                {
                    res[i, j] = a[i, j] + b[i, j];
                }
            }
            return res;
        }
        public static Matrix operator -(Matrix a, Matrix b) => a + (b * -1);
        public static Matrix operator -(Matrix a) => (a * -1);
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.C != b.R) throw new Exception("Матрицы имеют неккоректные размерности");
            var res = new Matrix(a.R, b.C);
            for (int i = 0; i < a.R; i++)
            {
                for (int j = 0; j < b.C; j++)
                {
                    for (int k = 0; k < a.C; k += 1)
                    {
                        res[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return res;
        }
        public static Matrix operator ~(Matrix a)
        {
            var res = new Matrix(a);
            res.transpose();
            return res;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Matrix x)
            {
                if (R != x.R || C != x.C) return false;
                for (int i = 0; i < R; i++)
                {
                    for (int j = 0; j < C; j++)
                    {
                        if (Math.Abs(this[i, j]) - Math.Abs(x[i, j]) > 10 * double.Epsilon) return false;
                    }
                }
                return true;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data);
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.R != b.R || a.C != b.C) return false;

            for (int i = 0; i < a.R; i++)
            {
                for (int j = 0; j < b.C; j++)
                {
                    if (Math.Abs(a[i, j]) - Math.Abs(b[i, j]) > 10 * double.Epsilon) return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix a, Matrix b) => !(a == b);

        //Методы
        public Matrix transpose()
        {
            int r = R;
            int c = C;
            var res = new double[c, r];
            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    res[i, j] = this[j, i];
                }
            }
            Data = res;
            return this;
        }
        public virtual Matrix randomize(int x, int y)
        {
            var rnd = new Random();
            for (int i = 0; i < R; i++)
            {
                for (int j = 0; j < C; j++)
                {
                    this[i, j] = rnd.Next(x, y);
                }
            }
            return this;
        }
        public virtual Matrix randomize() => randomize(0, 9);
        public Matrix CreateMatrixWithoutColumn(int column)
        {
            if (column < 0 || column >= C)
            {
                throw new ArgumentException("invalid column index");
            }
            var result = new Matrix(R, C - 1);
            for (int i = 0; i < R; i++)
            {
                for (int j = 0; j < C - 1; j++)
                {
                    result[i, j] = j < column ? this[i, j] : this[i, j + 1];
                }
            }
            return result;
        }
        public Matrix CreateMatrixWithoutRow(int row)
        {
            if (row < 0 || row >= R)
            {
                throw new ArgumentException("invalid row index");
            }
            var result = new Matrix(R - 1, C);
            for (int i = 0; i < R - 1; i++)
            {
                for (int j = 0; j < C; j++)
                {
                    result[i, j] = i < row ? this[i, j] : this[i + 1, j];
                }
            }
            return result;
        }
    }
}
