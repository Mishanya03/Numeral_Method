using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public delegate double Integral(double x);
public delegate double DoubleIntegral(double x, double y);

class Integrals
{
    // Метод прямоугольника
    public static double MethodRectangle(double a, double b, double eps, Integral f)
    {
        int n = 1;
        double h = (b - a) / n;
        double fpr = f(a) * h;
        double s = fpr;
        double differential;

        do
        {
            n *= 2;
            h = (b - a) / n;
            for (int i = 1; i < n; i += 2)
            {
                s += f(a + i * h);
            }
            (differential, fpr) = (s * h - fpr, s * h);
        } while (Math.Abs(differential) > eps);

        return fpr;
    }

    // Метод трапеции
    public static double MethodTrapezoid(double a, double b, double eps, Integral f)
    {
        int n = 1;
        double h = (b - a) / n;
        double s = f(a) + f(b);
        double fpr = s * h / 2;
        double differential;

        do
        {
            n *= 2;
            h = (b - a) / n;
            for (int i = 1; i < n; i += 2)
            {
                s += f(a + i * h) + f(a + (i + 1) * h);
            }
            (differential, fpr) = (s * (h / 2) - fpr, s * (h / 2));
        } while (Math.Abs(differential) > eps);

        return fpr;
    }

    // Метод Симпсона
    public static double MethodSimpsons(double a, double b, double eps, Integral f)
    {
        int n = 1;
        double h, fpr = 1, sum1, sum2 = 0;
        double s = f(a) + f(b);
        double diff;

        do
        {
            n *= 2;
            h = (b - a) / n;
            diff = fpr;
            sum1 = 0;

            for (int i = 1; i < n; i += 2)
            {
                sum1 += f(a + h * i);
            }

            fpr = h / 3 * (s + 2 * sum2 + 4 * sum1);
            sum2 = sum1 + sum2;
            diff -= fpr;
        } while (Math.Abs(diff) > eps);

        return fpr;
    }

    // Метод Симпсона для двойного интеграла
    public static double MethodSimpsonDouble(double a, double b, double c, double d, double eps, DoubleIntegral f)
    {
        // Начальное количество разбиений по x и y
        int nx = 2;
        int ny = 2;

        // Переменные для хранения предыдущего и текущего значения интеграла
        double fpr = 0.0;
        double s = 0.0;

        // Цикл для постепенного увеличения разбиений для достижения необходимой точности
        while (true)
        {
            // Вычисление шагов разбиения по x и y
            double hx = (b - a) / nx;
            double hy = (d - c) / ny;

            // Переменная для хранения суммы значений функции
            double sum = 0.0;

            for (int i = 0; i <= nx; i++)
            {
                for (int j = 0; j <= ny; j++)
                {
                    // Вычисление веса для текущего узла
                    double w = (i == 0 || i == nx) ? 1 : (i % 2 == 0) ? 2 : 4;
                    double h = (j == 0 || j == ny) ? 1 : (j % 2 == 0) ? 2 : 4;

                    // Вычисление значения функции в текущем узле и добавление к сумме
                    sum += w * h * f(a + i * hx, c + j * hy);
                }
            }

            // Вычисление приближенного значения интеграла
            s = sum * hx * hy / 9;

            // Проверка достижения необходимой точности
            if (Math.Abs(s - fpr) < eps)
            {
                break;
            }

            // Увеличение числа разбиений вдвое для следующей итерации
            nx *= 2;
            ny *= 2;

            // Обновление значения предыдущего интеграла
            fpr = s;
        }

        // Возвращение приближенного значения двойного интеграла
        return s;
    }
}
