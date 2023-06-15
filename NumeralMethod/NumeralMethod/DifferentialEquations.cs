using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DifferentialEquations
{
    // Делегат для описания производной
    public delegate Vector Derivative(double t, Vector x);

    // Пример дифференциального уравнения: x'' = -x, преобразован в систему уравнений
    public static Vector PravilaDU(double t, Vector x)
    {
        Vector f = new Vector(2);
        f[0] = x[1];    // x'
        f[1] = -x[0];   // x''
        return f;
    }

    // Метод Эйлера для численного решения дифференциального уравнения
    public static Matrix MethodEuler(double t0, double tEnd, Vector x0, int steps, Derivative f)
    {
        int n = x0.Size;
        Matrix result = new Matrix(n + 1, steps + 1);
        double dt = (tEnd - t0) / steps;
        Vector column = new Vector(n + 1);
        column[0] = t0;
        for (int i = 0; i < n; i++)
            column[i + 1] = x0[i];
        result.SetColumn(0, column);
        Vector xt = x0.Copy();
        double t = t0;
        Vector pr;

        for (int k = 1; k <= steps; k++)
        {
            pr = f(t, xt);
            xt = xt + pr * dt;
            t += dt;
            column[0] = t;
            for (int i = 0; i < n; i++)
                column[i + 1] = xt[i];
            result.SetColumn(k, column);
        }
        return result;
    }

    // Метод Рунге-Кутта 2-го порядка для численного решения дифференциального уравнения
    public static Matrix MethodRungeKutta2(double t0, double tEnd, Vector x0, int steps, Derivative f)
    {
        int n = x0.Size;
        Matrix result = new Matrix(n + 1, steps + 1);
        double dt = (tEnd - t0) / steps;
        Vector column = new Vector(n + 1);
        column[0] = t0;
        for (int i = 0; i < n; i++)
            column[i + 1] = x0[i];
        result.SetColumn(0, column);
        Vector k1, k2, x = x0;
        double t = t0;
        for (int k = 1; k <= steps; k++)
        {
            k1 = f(t, x);
            k2 = f(t + dt, x + k1 * dt);
            x = x + (k1 + k2) * 0.5 * dt;
            t += dt;
            column[0] = t;
            for (int i = 0; i < n; i++)
                column[i + 1] = x[i];
            result.SetColumn(k, column);
        }
        return result;
    }

    // Метод Рунге-Кутта 4-го порядка для численного решения дифференциального уравнения
    public static Matrix MethodRungeKutta4(double t0, double tEnd, Vector x0, int steps, Derivative f)
    {
        int n = x0.Size;
        Matrix result = new Matrix(n + 1, steps + 1);
        double dt = (tEnd - t0) / steps;
        Vector column = new Vector(n + 1);
        column[0] = t0;
        for (int i = 0; i < n; i++)
            column[i + 1] = x0[i];
        result.SetColumn(0, column);

        Vector k1, k2, k3, k4, x = x0;
        double t = t0;
        for (int k = 1; k <= steps; k++)
        {
            k1 = f(t, x);
            k2 = f(t + 0.5 * dt, x + 0.5 * k1 * dt);
            k3 = f(t + 0.5 * dt, x + 0.5 * k2 * dt);
            k4 = f(t + dt, x + k3 * dt);

            x = x + (k1 + 2 * k2 + 2 * k3 + k4) * dt / 6.0;
            t += dt;

            column[0] = t;
            for (int i = 0; i < n; i++)
                column[i + 1] = x[i];
            result.SetColumn(k, column);
        }
        return result;
    }

    // Метод Адамса для численного решения дифференциального уравнения
    public static Matrix MethodAdams4(double t0, double tEnd, Vector x0, int steps, Derivative f)
    {
        double h = (tEnd - t0) / steps;
        Matrix result = new Matrix(steps + 1, x0.Size);
        result.SetRow(0, x0);

        // Рассчитываем первые несколько шагов, используя метод Рунге-Кутты
        Vector xi = x0;
        for (int i = 0; i < 4; i++)
        {
            Vector k1 = h * f(t0 + i * h, xi);
            Vector k2 = h * f(t0 + i * h + h / 2, xi + k1 / 2);
            Vector k3 = h * f(t0 + i * h + h / 2, xi + k2 / 2);
            Vector k4 = h * f(t0 + i * h + h, xi + k3);
            xi = xi + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
            result.SetRow(i + 1, xi);
        }

        // Применяем метод Адамса к остальным шагам
        for (int i = 4; i < steps; i++)
        {
            Vector predictor = xi + h / 24 * (55 * f(t0 + (i - 1) * h, xi) - 59 * f(t0 + (i - 2) * h, result.GetRow(i - 1)) + 37 * f(t0 + (i - 3) * h, result.GetRow(i - 2)) - 9 * f(t0 + (i - 4) * h, result.GetRow(i - 3)));
            xi = xi + h / 24 * (9 * f(t0 + (i + 1) * h, predictor) + 19 * f(t0 + i * h, xi) - 5 * f(t0 + (i - 1) * h, result.GetRow(i - 1)) + f(t0 + (i - 2) * h, result.GetRow(i - 2)));
            result.SetRow(i + 1, xi);
        }

        // Добавляем столбец значений времени в результирующую матрицу
        Vector time = new Vector(steps + 1);
        for (int i = 0; i <= steps; i++)
        {
            time[i] = t0 + i * h;
        }
        result = result.AddColumn(time);

        return result.Trans();
    }
}
