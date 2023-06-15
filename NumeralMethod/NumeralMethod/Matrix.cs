using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Numerics;
using System.Text;

class Matrix
{
    protected int rows, columns;
    protected double[,] data;
    public Matrix(int r, int c)
    {
        this.rows = r; this.columns = c;
        data = new double[rows, columns];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++) data[i, j] = 0;
    }
    public Matrix(double[,] mm)
    {
        this.rows = mm.GetLength(0); this.columns = mm.GetLength(1);
        data = new double[rows, columns];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                data[i, j] = mm[i, j];
    }
    public int Rows { get { return rows; } }
    public int Columns { get { return columns; } }

    public double this[int i, int j]
    {
        get
        {
            if (i < 0 && j < 0 && i >= rows && j >= columns)
            {
                // Console.WriteLine(" Индексы вышли за пределы матрицы ");
                return Double.NaN;
            }
            else
                return data[i, j];
        }
        set
        {
            if (i < 0 && j < 0 && i >= rows && j >= columns)
            {
                //Console.WriteLine(" Индексы вышли за пределы матрицы ");
            }
            else
                data[i, j] = value;
        }
    }
    public Vector GetRow(int r)
    {
        if (r >= 0 && r < rows)
        {
            Vector row = new Vector(columns);
            for (int j = 0; j < columns; j++) row[j] = data[r, j];
            return row;
        }
        return null;
    }
    public Vector GetColumn(int c)
    {
        if (c >= 0 && c < columns)
        {
            Vector column = new Vector(rows);
            for (int i = 0; i < rows; i++) column[i] = data[i, c];
            return column;
        }
        return null;
    }
    public bool SetRow(int index, Vector r)
    {
        if (index < 0 || index > rows) return false;
        if (r.Size != columns) return false;
        for (int k = 0; k < columns; k++) data[index, k] = r[k];
        return true;
    }
    public bool SetColumn(int index, Vector c)
    {
        if (index < 0 || index > columns) return false;
        if (c.Size != rows) return false;
        for (int k = 0; k < rows; k++) data[k, index] = c[k];
        return true;
    }
    public Matrix AddColumn(Vector vector)
    {
        if (rows != vector.Size)
        {
            throw new ArgumentException("The size of the column vector must match the number of rows in the matrix.");
        }

        Matrix result = new Matrix(rows, columns + 1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                result[i, j] = data[i, j];
            }

            result[i, columns] = vector[i];
        }
        return result;
    }

    public void SwapRows(int r1, int r2)
    {
        if (r1 < 0 || r2 < 0 || r1 >= rows || r2 >= rows || (r1 == r2)) return;
        Vector v1 = GetRow(r1);
        Vector v2 = GetRow(r2);
        SetRow(r2, v1);
        SetRow(r1, v2);
    }
    public Matrix Copy()
    {
        Matrix r = new Matrix(rows, columns);
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++) r[i, j] = data[i, j];
        return r;
    }
    public Matrix Trans()
    {
        Matrix transposeMatrix = new Matrix(columns, rows);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                transposeMatrix.data[j, i] = data[i, j];
            }
        }
        return transposeMatrix;
    }
    public double Norma()
    {
        double norma = 0.0;
        // Вычисление суммы квадратов элементов матрицы
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                norma += data[i, j] * data[i, j];
        // Возвращение квадратного корня из суммы квадратов
        return Math.Sqrt(norma);
    }
    public static double Determinant(Matrix m) 
    {
        int n = (int)Math.Sqrt(m.rows);
        double det = 0;
        if (n == 1) { det = m[0, 0]; }
        else if (n == 2) 
            det = m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];
        else 
        {
            for (int j = 0; j < n; j++) 
            {
                Matrix submatrix = new Matrix(n - 1, n - 1);
                for (int i = 1; i < n; i++) 
                {
                    for (int k = 0; k < n; k++) 
                    {
                        if (k < j) { submatrix[i - 1, k] = m[i, k]; } 
                        else if (k > j) { submatrix[i - 1, k - 1] = m[i, k]; } 
                    }
                }
                double count = Math.Pow(-1, j) * m[0, j];
                det += count * Matrix.Determinant(submatrix); 
            }
        }
        return det;
    }
    //Сложение матриц
    public static Matrix operator +(Matrix m1, Matrix m2)
    {
        if (m1.rows != m2.rows || m1.columns != m2.columns)
        {
            throw new Exception("Матрицы не соразмерны");
        }
        Matrix result = new Matrix(m1.rows, m1.columns);


        for (int i = 0; i < m1.rows; i++)
        {
            for (int j = 0; j < m2.columns; j++)
            {
                result[i, j] = m1[i, j] + m2[i, j];
            }
        }
        return result;
    }

    //Вычитание матриц
    public static Matrix operator -(Matrix m1, Matrix m2)
    {
        if (m1.rows != m2.rows || m1.columns != m2.columns)
        {
            throw new Exception("Матрицы не соразмерны");
        }
        Matrix result = new Matrix(m1.rows, m1.columns);

        for (int i = 0; i < m1.rows; i++)
        {
            for (int j = 0; j < m2.columns; j++)
            {
                result[i, j] = m1[i, j] - m2[i, j];
            }
        }
        return result;
    }
    // Перемножение матрицыи вектора (получение вектора)

    public Vector Dot(Vector vector)
    {
        if (columns != vector.Size) return null;
        Vector result = new Vector(rows);
        for (int i = 0; i < rows; i++)
        {
            double sum = 0;
            for (int j = 0; j < columns; j++)
            {
                sum += data[i, j] * vector[j];
            }
            result[i] = sum;
        }
        return result;
    }
    public Matrix Dot(double value)
    {
        Matrix result = new Matrix(rows, columns);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                result[i, j] = data[i, j] * value;
            }
        }
        return result;
    }
    //Матрица на матрицу
    public static Matrix operator *(Matrix m1, Matrix m2)
    {
        if (m1.columns != m2.rows) throw new Exception();

        Matrix result = new Matrix(m1.rows, m2.columns);
        double[] current_row;
        double[] current_column;

        for (int i = 0; i < m1.rows; i++)
        {
            current_row = m1.GetRow(i).GetElements();
            for (int j = 0; j < m2.columns; j++)
            {
                current_column = m2.GetColumn(j).GetElements();
                for (int k = 0; k < m1.columns; k++)
                {
                    result.data[i, j] += current_row[k] * current_column[k];
                    if (Math.Abs(result.data[i, j]) < 0.000001) result.data[i, j] = 0;
                }
            }
        }
        return result;
    }
    //Матрица на вектор
    public static Vector operator *(Matrix matrix, Vector vector) => matrix.Dot(vector);
    public static Vector operator *(Vector vector, Matrix matrix) => matrix.Dot(vector);
    //Умножение матриц на число
    public static Matrix operator *(Matrix matrix, double value) => matrix.Dot(value);
    public static Matrix operator *(double value, Matrix matrix) => matrix.Dot(value);

    //Нижний треугольник
    public static Vector? SolveLU_DownTriangle(Matrix A, Vector B)
    {
        int rows = A.rows;

        if (A.columns != rows || rows != B.Size)
        {
            Console.WriteLine("Не совпадает размерность!");
            return null;
        }
        ///Проверка
        for (int i = 0; i < rows; i++)
        {
            if (A.data[i, i] == 0)
            {
                Console.WriteLine("Центральная вертикаль = 0!");
                return null;
            }
            for (int j = i + 1; j < rows; j++)
            {
                if (Math.Abs(A.data[i, j]) > 0.000000001)
                {
                    Console.WriteLine("В верхнем треугольнике есть значение больше 0!");
                    return null;
                }
            }
        }

        Vector x = new(rows);
        x[0] = B[0] / A.data[0, 0];
        /// Основной цикл
        for (int i = 0; i < rows; i++)
        {
            double sum = 0;
            for (int j = 0; j < i; j++)
            {
                sum += A.data[i, j] * x[j];
            }
            x[i] = (B[i] - sum) / A.data[i, i];
        }
        return x;
    }
    //Верхний треугольник
    public static Vector? SolveLU_UpTriangle(Matrix A, Vector B)
    {

        int rows = A.rows;

        ///Проверка
        if (A.columns != rows || rows != B.Size)
        {
            Console.WriteLine("Не совпадает размерность!");
            return null;
        }

        for (int i = 0; i < rows; i++)
        {
            if (A.data[i, i] == 0)
            {
                Console.WriteLine("Центральная вертикаль = 0!");
                return null;
            }
            for (int j = 0; j < i; j++)
            {
                if (Math.Abs(A.data[i, j]) > 0.000001)
                {
                    Console.WriteLine("В нижнем треугольнике есть значение больше 0!");
                    return null;
                }
            }
        }

        Vector x = new(rows);
        x[rows - 1] = B[rows - 1] / A.data[rows - 1, rows - 1];
        /// Основной цикл
        for (int i = rows - 2; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < rows; j++)
            {
                sum += A.data[i, j] * x[j];
            }
            x[i] = (B[i] - sum) / A.data[i, i];
        }
        return x;
    }

    //Решение системы линейных уравнений методом Гаусса
    public static Vector MethodGauss(Matrix AA, Vector bb)
    {
        // Копируем матрицу А и вектор b
        Matrix A = AA.Copy();
        Vector b = bb.Copy();
        // Устанавливаем количество строк в матрице А
        int rows = A.Rows;
        if (rows != b.Size || rows != A.columns)
        {
            Console.WriteLine("Не совпадает размерность!");
            return null;
        }
        // Проходим по всем строкам матрицы А
        for (int k = 0; k < rows; k++)
        {
            double maxElement = Math.Abs(A[k, k]);
            int maxIndexRow = k;
            //Поиск максимального элемента
            for (int i = k + 1; i < rows; i++)
            {
                if (Math.Abs(A[i, k]) > maxElement)
                {
                    maxElement = Math.Abs(A[i, k]);
                    maxIndexRow = i;
                }
            }
            /// Проверка на ненулевое элемент 
            if (maxElement < 0.00000001)
            {
                throw new Exception("Матрицы невозможно перемножить");
            }
            //Меняем местами строки
            for (int j = k; j < rows; j++)
            {
                (A[maxIndexRow, j], A[k, j]) = (A[k, j], A[maxIndexRow, j]);
                /*
                double tmp = A[maxIndexRow, j];
                A[maxIndexRow, j] = A[k, j];
                A[k, j] = tmp;
                */
            }
            //Меняем местами значения вектора b
            (b[maxIndexRow], b[k]) = (b[k], b[maxIndexRow]);
            /*
            double temp = b[maxIndexRow];
            b[maxIndexRow] = b[k];
            b[k] = temp;
            */

            //Проходим по всем строкам после k-ой
            for (int i = 0; i < rows; i++)
            {
                if (i == k) continue;
                double value = A[i, k] / A[k, k];
                for (int j = k; j < rows; j++)
                    A[i, j] -= A[k, j] * value;
                b[i] -= b[k] * value;
            }
        }
        Vector vector = SolveLU_DownTriangle(A, b) ?? new(rows);
        return vector;
    }
    // Получение обратной матрицы методом Гаусса
    public static Matrix Inverse(Matrix A)
    {
        int rows = A.rows;
        int columns = A.columns;
        if (rows != columns) return null;
        Matrix aCopy = A.Copy();

        Matrix result = new Matrix(rows, columns);
        Matrix E = new Matrix(rows, columns);
        for (int i = 0; i < rows; i++) E[i, i] = 1;

        double eps = 0.000001;
        int max;
        for (int j = 0; j < columns; j++)
        {
            max = j;
            for (int i = j + 1; i < rows; i++)
                if (Math.Abs(aCopy[i, j]) > Math.Abs(aCopy[max, j])) { max = i; };

            if (max != j)
            {
                Vector temp = aCopy.GetRow(max); aCopy.SetRow(max, aCopy.GetRow(j)); aCopy.SetRow(j, temp);
                Vector tmp = E.GetRow(max); E.SetRow(max, E.GetRow(j)); E.SetRow(j, tmp);
            }

            if (Math.Abs(aCopy[j, j]) < eps) return null;

            for (int i = j + 1; i < rows; i++)
            {
                double multiplier = -aCopy[j, j] / aCopy[i, j];
                for (int k = 0; k < columns; k++)
                {
                    aCopy[i, k] *= multiplier;
                    aCopy[i, k] += aCopy[j, k];
                    E[i, k] *= multiplier;
                    E[i, k] += E[j, k];
                }
            }
        }
        for (int i = 0; i < columns; i++)
            result.SetColumn(i, SolveLU_UpTriangle(aCopy, E.GetColumn(i)));

        return result;
    }

    //Вывод матрицы
    public static void PrintMatrix(Matrix matrix)
    {
        for (int i = 0; i < matrix.Rows; i++)
        {
            for (int j = 0; j < matrix.Columns; j++)
            {
                Console.Write("{0}\t", matrix[i, j]);
            }
            Console.WriteLine("");
        }
    }

    //Метод квадратных корней
    public static Vector? MethodSquareRoot(Matrix aa, Vector bb)
    {
        Matrix a = aa.Copy();
        Vector b = bb.Copy();
        int rows = a.Rows;

        Matrix L = new Matrix(rows, rows);
        Matrix Lt = new Matrix(rows, rows);
        Vector y = new Vector(rows);
        Vector x = new Vector(rows);


        //Находим нижнюю треугольную матрицу
        L[0, 0] = Math.Sqrt(a[0, 0]);//Задаем первый элемент матрицы L как корень из первого элемента матрицы А
        for (int i = 1; i < rows; i++)
        {
            double sumElement1 = 0;
            for (int j = 1; j < rows + 1; j++)
            {
                if (j - 1 < i)
                {
                    for (int k = 0; k < j - 1; k++)
                        sumElement1 += L[i, k] * L[j - 1, k];
                    L[i, j - 1] = (a[i, j - 1] - sumElement1) / L[j - 1, j - 1];
                }
            }
            double sumElement2 = 0;
            for (int k = 0; k < i; k++)
            { sumElement2 += L[i, k] * L[i, k]; }
            L[i, i] = Math.Sqrt(a[i, i] - sumElement2);
        }

        //Транспонируем матрицу L и получаем матрицу LT
        Lt = L.Trans();

        //Рассчитываем вектор у
        double summa = 0;
        for (int i = 0; i < rows; i++)
        {
            summa = 0;
            for (int j = 0; j < i; j++)
            {
                summa += (L[i, j] * y[j]);
            }
            y[i] = (b[i] - summa) / L[i, i];
        }

        //Находим значение x
        for (int i = rows - 1; i >= 0; i--)
        {
            summa = 0;
            for (int j = rows - 1; j > i; j--)
            {
                summa += Lt[i, j] * x[j];
            }
            x[i] = (y[i] - summa) / Lt[i, i];
        }

        return x;
    }
    //Метод прогонки
    public static Vector MethodRunThrough(Vector c, Vector d, Vector e, Vector b)
    {
        int n = d.Size;
        if (b.Size != n) return null;
        Vector x = new Vector(n);
        Vector alfa = new Vector(n);
        Vector betta = new Vector(n);
        for (int i = 0; i < n; i++) if (d[i] == 0) return null;
        // Прямой ход
        alfa[1] = -e[0] / d[0];
        betta[1] = b[0] / d[0];
        for (int i = 1; i < n - 1; i++)
        {
            double zn = d[i] + c[i] * alfa[i];
            alfa[i + 1] = -e[i] / zn;
            betta[i + 1] = (-c[i] * betta[i] + b[i]) / zn;
        }
        // Обратный ход
        x[n - 1] = (-c[n - 1] * betta[n - 1] + b[n - 1]) / (d[n - 1] + c[n - 1] * alfa[n - 1]);
        for (int i = n - 2; i >= 0; i--)
            x[i] = alfa[i + 1] * x[i + 1] + betta[i + 1];
        return x;
    }

    // Метод вращения Гивенса
    public static Vector MethodGivensRotation(Matrix A, Vector B)
    {
        if (A.Rows != A.Columns || A.Rows != B.Size) return null;

        int n = A.Rows;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                Matrix Q = new Matrix(n, n);
                for (int k = 0; k < n; k++) Q[k, k] = 1;
                double c = A[i, i] / Math.Sqrt((Math.Pow(A[i, i], 2) + Math.Pow(A[j, i], 2)));
                double s = A[j, i] / Math.Sqrt((Math.Pow(A[i, i], 2) + Math.Pow(A[j, i], 2)));
                Q[i, i] = c; Q[j, j] = c;
                Q[j, i] = -s; Q[i, j] = s;
                A = Q * A;
                B = Q * B;
            }

        }
        return SolveLU_UpTriangle(A, B);
    }


    //Метод Грамма-Шмидта
    public static Vector MethodGramSchmidt(Matrix A, Vector B)
    {
        if (A.Rows != A.Columns || A.Rows != B.Size) return null;

        int n = A.Rows;
        Matrix R = new Matrix(n, n);
        Matrix T = new Matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            T[i, i] = 1;
        }

        R.SetColumn(0, A.GetColumn(0));
        for (int i = 1; i < n; i++)
        {
            for (int j = 0; j < i; j++)
            {
                Vector a = A.GetColumn(i);
                Vector r = R.GetColumn(j);
                T[j, i] = a * r / (r * r);

                Vector rNew = a;
                for (int k = 0; k < i; k++)
                    rNew -= T[k, i] * R.GetColumn(k);

                R.SetColumn(i, rNew);
            }
        }
        Matrix D = R.Trans() * R;
        for (int i = 0; i < n; i++)
        {
            if (D[i, i] == 0) return null;
            D[i, i] = 1 / D[i, i];
        }
        Vector y = R.Trans() * B * D;

        return SolveLU_UpTriangle(T, y);
    }

    //Метод последовательных приближений(Method of Successive Approximations)
    public static Vector MethodSuccessiveApproximations(Matrix aa, Vector bb)
    {
        Matrix A = aa.Copy();
        Vector B = bb.Copy();
        if (A.Rows != A.Columns || A.Rows != B.Size) return null;

        int n = A.Rows;
        double eps = 0.0001;
        int max;

        for (int j = 0; j < n; j++)
        {
            max = j;
            for (int i = j + 1; i < n; i++)
                if (Math.Abs(A[i, j]) > Math.Abs(A[max, j])) max = i;

            if (max != j)
            {
                for (int k = 0; k < n; k++)
                {
                    double temp = A[j, k];
                    A[j, k] = A[max, k];
                    A[max, k] = temp;
                }
                double tempB = B[j];
                B[j] = B[max];
                B[max] = tempB;
            }
            if (Math.Abs(A[j, j]) < eps) return null;
        }

        Vector beta = new Vector(n);
        for (int i = 0; i < beta.Size; i++)
            beta[i] = B[i] / A[i, i];

        Matrix alpha = new Matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            if (A[i, i] == 0) return null;
            for (int j = 0; j < n; j++)
                if (i != j) alpha[i, j] = A[i, j] / A[i, i];
                else alpha[i, j] = 0;

            beta[i] = B[i] / A[i, i];
        }
        if (alpha.Norma() >= 1) return null;
        Vector xPrev = beta;
        Vector xCurrent;
        Vector d;

        do
        {
            xCurrent = beta - alpha.Dot(xPrev);
            d = xCurrent - xPrev;
            xPrev = xCurrent;
        } while (d.Norma1() > eps);

        return xPrev;
    }


}