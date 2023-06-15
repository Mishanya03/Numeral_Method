using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LeastSquares
{
    private readonly Vector x; // Вектор аргументов
    private readonly Vector y; // Вектор значений функции

    public Vector p; // Вектор параметров
    public delegate double FuncPsi(double func); // Делегат для пси-функций
    public FuncPsi[] func; // Массив пси-функций

    public int n, m; // Количество аргументов и пси-функций

    public LeastSquares(Vector x, Vector y, FuncPsi[] func)
    {
        if (x.Size != y.Size)
            throw new Exception("Количество аргументов не совпадает с количеством значений функции");

        this.x = x.Copy();
        this.y = y.Copy();
        this.n = x.Size;
        this.func = func;
        this.m = func.Length;

        CalculateParameters(); // Вычисление параметров
    }

    private void CalculateParameters()
    {
        Matrix H = new Matrix(n, m); // Создание и заполнение матрицы Н

        for (int i = 0; i < n; i++)
            H.SetRow(i, GetFunc(x[i])); // Заполнение i-й строки матрицы Н

        p = Matrix.Inverse(H.Trans() * H) * H.Trans() * y; // Вычисление вектора параметров
    }

    private Vector GetFunc(double x)
    {
        Vector result = new Vector(m); // Создание результирующего вектора

        for (int i = 0; i < m; i++)
            result[i] = func[i](x); // Вычисление i-го значения пси-функции

        return result;
    }

    public double GetCriteria()
    {
        Vector result = new Vector(n); // Создание результирующего вектора

        for (int i = 0; i < n; i++)
            result[i] = y[i] - p * GetFunc(x[i]); // Вычисление i-го значения критерия

        return result.Norma1(); // Возврат нормы результирующего вектора
    }
}
