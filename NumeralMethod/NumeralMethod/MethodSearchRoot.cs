using System;

public delegate double Func(double x);

class MethodSearchRoot
{
    /// <summary>
    /// Метод половинного деления для поиска корня уравнения.
    /// </summary>
    /// <param name="a">Левая граница интервала.</param>
    /// <param name="b">Правая граница интервала.</param>
    /// <param name="eps">Точность вычисления.</param>
    /// <param name="f">Функция, для которой ищется корень.</param>
    /// <returns>Найденный корень уравнения.</returns>
    public static double HalfDivision(double a, double b, double eps, Func f)
    {
        double fa = f(a);
        double fb = f(b);

        if (fa * fb > 0)
        {
            return double.NaN;
        }
        else
        {
            while (b - a > eps)
            {
                double c = (a + b) / 2;
                double fc = f(c);

                if (fa * fc < 0)
                    b = c;
                else
                {
                    a = c;
                    fa = fc;
                }
            }
        }

        return (a + b) / 2;
    }

    /// <summary>
    /// Метод Ньютона для поиска корня уравнения.
    /// </summary>
    /// <param name="t">Начальное приближение.</param>
    /// <param name="eps">Точность вычисления.</param>
    /// <param name="f">Функция, для которой ищется корень.</param>
    /// <returns>Найденный корень уравнения.</returns>
    public static double MethodNewton(double t, double eps, Func f)
    {
        double past_delta = double.MaxValue;
        double delta = 0;

        do
        {
            double ft = f(t);
            double fpt = (f(t + eps) - ft) / eps;
            double x = t - ft / fpt;
            delta = Math.Abs(x - t);
            t = x;

            if (past_delta < delta)
                return double.NaN;

            past_delta = delta;
        } while (delta > eps);

        return t;
    }

    /// <summary>
    /// Метод последовательного приближения для поиска корня уравнения.
    /// </summary>
    /// <param name="t">Начальное приближение.</param>
    /// <param name="eps">Точность вычисления.</param>
    /// <param name="f">Функция, для которой ищется корень.</param>
    /// <returns>Найденный корень уравнения.</returns>
    public static double SequentialApproximation(double t, double eps, Func f)
    {
        double past_delta = double.MaxValue;
        double delta = 0;

        do
        {
            double x = f(t);
            delta = Math.Abs(x - t);
            t = x;

            if (past_delta < delta)
                return double.NaN;

            past_delta = delta;
        } while (delta > eps);

        return t;
    }
}
