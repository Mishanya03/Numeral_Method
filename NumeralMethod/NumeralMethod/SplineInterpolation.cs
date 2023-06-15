using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Interpolation
{
    private readonly Vector x; // Вектор значений аргумента
    private readonly Vector y; // Вектор значений функции
    private readonly int n; // Количество интервалов
    private Vector a, b, c, d; // Параметры сплайнов

    public Interpolation(Vector x, Vector y)
    {
        this.x = x.Copy();
        this.y = y.Copy();
        if (x.Size != y.Size)
            throw new Exception("Количество аргументов не совпадает с количеством значений функции");

        this.n = x.Size - 1;
        c = new Vector(n);
        CreateSpline();
    }

    // Создание сплайнов
    private void CreateSpline()
    {
        Vector h = new Vector(n); // Вектор длин интервалов

        // Находим параметр а
        a = new Vector(n);
        for (int i = 0; i < n; i++)
            a[i] = y[i + 1];

        // Формируем вектор h
        for (int i = 1; i <= n; i++)
            h[i - 1] = x[i] - x[i - 1];

        // Нижняя диагональ
        Vector A = new Vector(n - 1);
        for (int i = 1; i < n - 1; i++)
            A[i] = h[i];
        A[0] = 0;

        // Средняя диагональ
        Vector C = new Vector(n - 1);
        for (int i = 0; i < n - 1; i++)
            C[i] = 2 * (h[i] + h[i + 1]);

        // Верхняя диагональ
        Vector B = new Vector(n - 1);
        for (int i = 0; i < n - 2; i++)
            B[i] = h[i + 1];
        B[n - 2] = 0;

        // Вектор правых частей
        Vector F = new Vector(n - 1);
        for (int i = 0; i < n - 1; i++)
            F[i] = 6 * ((y[i + 2] - y[i + 1]) / h[i + 1]) - ((y[i + 1] - y[i]) / h[i]);

        Vector rp = Matrix.MethodRunThrough(A, C, B, F);
        for (int i = 0; i < n - 1; i++)
            c[i] = rp[i];

        // Находим параметр d
        d = new Vector(n);
        for (int i = 0; i < n; i++)
        {
            if (i == 0)
                d[i] = (c[i]) / h[i];
            else
                d[i] = (c[i] - c[i - 1]) / h[i];
        }

        // Находим параметр b
        b = new Vector(n);
        for (int i = 0; i < n; i++)
            b[i] = h[i] / 2 * c[i] - h[i] * h[i] / 6 * d[i] + (y[i + 1] - y[i]) / h[i];
    }

    // Интерполяция
    public double GetInterpolation(double xt)
    {
        int interval = 0;
        if (xt < x[0] || xt > x[n])
            return double.NaN;

        int left = 0;
        int right = 1;
        while (right <= n)
        {
            if (xt >= x[left] && xt <= x[right])
                break;

            left = right;
            right++;
            interval++;
        }

        double dx = (xt - x[interval + 1]);
        double fy = a[interval] + b[interval] * dx + c[interval] / 2 * dx * dx + d[interval] / 6 * dx * dx * dx;
        return fy;
    }
}

// Интерполяция сплайнами
class SplineInterpolation
{
    private int n; // Количество узлов
    private readonly Vector x; // Вектор значений аргумента
    private readonly Vector y; // Вектор значений функции
    private Vector m; // Вектор параметров сплайнов
    private Vector h; // Вектор длин интервалов

    public SplineInterpolation(Vector x, Vector y)
    {
        if (x.Size != y.Size)
            throw new ArgumentException("x и y не соразмерны");

        n = x.Size - 1;

        this.x = new Vector(n + 1);
        this.y = new Vector(n + 1);
        this.m = new Vector(n + 1);
        this.h = new Vector(n + 1);

        for (int i = 0; i <= n; i++)
        {
            this.x[i] = x[i];
            this.y[i] = y[i];
        }

        for (int i = 0; i < n; i++)
            h[i] = x[i + 1] - x[i];

        CalculateCoefficients();
    }

    private void CalculateCoefficients()
    {
        Vector alpha = new Vector(n);
        Vector beta = new Vector(n);

        alpha[1] = -h[1] / (2.0 * (h[0] + h[1]));
        beta[1] = 3.0 * ((y[2] - y[1]) / h[1] - (y[1] - y[0]) / h[0]) / (2.0 * (h[0] + h[1]));

        for (int i = 2; i < n; i++)
        {
            alpha[i] = -h[i] / (2.0 * h[i - 1] + 2.0 * h[i] + h[i - 1] * alpha[i - 1]);
            beta[i] = (3.0 * (y[i + 1] - y[i]) / h[i] - 3.0 * (y[i] - y[i - 1]) / h[i - 1] - h[i - 1] * beta[i - 1]) / (2.0 * h[i - 1] + 2.0 * h[i] + h[i - 1] * alpha[i - 1]);
        }

        m[n - 1] = beta[n - 1];

        for (int i = n - 2; i >= 1; i--)
            m[i] = alpha[i] * m[i + 1] + beta[i];

        m[0] = 0.0;
        m[n] = 0.0;
    }

    // Интерполяция
    public double GetInterpolation(double xValue)
    {
        int i = FindSegment(xValue);

        double d = xValue - x[i];
        double a = (m[i + 1] - m[i]) / (6.0 * h[i]);
        double b = m[i] / 2.0;
        double c = (y[i + 1] - y[i]) / h[i] - h[i] * (2.0 * m[i] + m[i + 1]) / 6.0;
        double res = y[i] + d * (c + d * (b + d * a));
        return res;
    }

    private int FindSegment(double xValue)
    {
        int i = 0;
        while (i < n && xValue > x[i + 1])
            i++;

        return i;
    }
}