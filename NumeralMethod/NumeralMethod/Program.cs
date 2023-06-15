using System;
using System.Threading;

class MethodHelp
{
    //Округляет значений вектора до 3 цифр после запятой
    public static Vector VectorRound(Vector vector)
    {
        for (int i = 0; i < vector.Size; i++)
            vector[i] = Math.Round(vector[i], 3);
        return vector;
    }
    //Округляет значений матриц до 3 цифр после запятой
    public static Matrix MatrixRound(Matrix matrix)
    {
        for (int i = 0; i < matrix.Rows; i++)
            for (int j = 0; j < matrix.Columns; j++)
                matrix[i, j] = Math.Round(matrix[i, j], 3);
        return matrix;
    }
}
class Test
{
    public static void TestMethodSearchRoot()
    {
        Console.WriteLine($"Метод половинного деления: {MethodSearchRoot.HalfDivision(1.0, 4.0, 0.0001, Math.Sin)}");
        Console.WriteLine($"Метод половинного деления: {MethodSearchRoot.HalfDivision(0, 3, 0.0001, x => x * x - 1.0)}");
        Console.WriteLine();

        Console.WriteLine($"Метод Ньютона: {MethodSearchRoot.MethodNewton(4, 0.0001, Math.Sin)}");
        Console.WriteLine($"Метод Ньютона: {MethodSearchRoot.MethodNewton(1.1, 0.0001, x => x * x - 1.0)}");
        Console.WriteLine();

        Console.WriteLine($"Метод последовательного приближения: {MethodSearchRoot.SequentialApproximation(1.2, 0.0001, x => x - 2 * (x * x - 1.0))}");
        Console.WriteLine($"Метод последовательного приближения: {MethodSearchRoot.SequentialApproximation(1.2, 0.0001, x => x - 0.5 * (x * x - 1.0))}");
    }
    public static void TestMatrix()
    {
        Matrix matrix1 = new Matrix(3, 3);
        Matrix matrix2 = new Matrix(3, 3);
        Matrix matrix6 = new Matrix(3, 3);
        matrix6[0, 0] = 1; matrix6[0, 1] = -1; matrix6[0, 2] = -1;
        matrix6[1, 0] = 0; matrix6[1, 1] = 1; matrix6[1, 2] = -1;
        matrix6[2, 0] = 0; matrix6[2, 1] = 0; matrix6[2, 2] = 1;

        Vector vector1 = new Vector(3);
        vector1.SetElement(7, 0); vector1.SetElement(2, 1); vector1.SetElement(5, 2);
        Vector vector2 = new Vector(3);
        vector2.SetElement(1, 0); vector2.SetElement(3, 1); vector2.SetElement(0, 2);
        Vector vector3 = new Vector(3);
        vector3.SetElement(8, 0); vector3.SetElement(3, 1); vector3.SetElement(4, 2);



        matrix1.SetColumn(0, vector1); matrix1.SetColumn(1, vector2); matrix1.SetColumn(2, vector3);
        matrix2.SetColumn(0, vector3); matrix2.SetColumn(1, vector2); matrix2.SetColumn(2, vector1);

        Console.WriteLine("Vector 1: {0}", vector1);
        Console.WriteLine("Vector 2: {0}", vector2);
        Console.WriteLine("Vector 3: {0}", vector3);

        Console.WriteLine("Matrix 1:");
        Matrix.PrintMatrix(matrix1);

        Console.WriteLine("\nMatrix 2:");
        Matrix.PrintMatrix(matrix2);

        Console.WriteLine("\nMatrix 6:");
        Matrix.PrintMatrix(matrix6);

        Console.WriteLine("\nMatrix 1 + matrix 2:");
        Matrix rez1 = new Matrix(3, 3);
        rez1 = matrix1 + matrix2;
        Matrix.PrintMatrix(rez1);

        Console.WriteLine("\nMatrix 1 - matrix 2:");
        Matrix rez2 = new Matrix(3, 3);
        rez2 = matrix1 - matrix2;
        Matrix.PrintMatrix(rez2);

        Console.WriteLine("\nMatrix 1 * matrix 2:");
        Matrix rez3 = new Matrix(3, 3);
        rez3 = matrix1 * matrix2;
        Matrix.PrintMatrix(rez3);

        Console.WriteLine("\nMatrix 1 * 5:");
        Matrix rez4 = new Matrix(3, 3);
        rez4 = matrix1 * 5;
        Matrix.PrintMatrix(rez4);

        Console.WriteLine("\n5.5 * matrix 1:");
        Matrix rez5 = new Matrix(3, 3);
        rez5 = 5.5 * matrix1;
        Matrix.PrintMatrix(rez5);
        
        Vector vector4 = new Vector(3);
        vector4.SetElement(1, 0); vector4.SetElement(2, 1); vector4.SetElement(5, 2);
        Vector vector5 = new Vector(3);
        vector5.SetElement(0, 0); vector5.SetElement(3, 1); vector5.SetElement(1, 2);
        Vector vector6 = new Vector(3);
        vector6.SetElement(0, 0); vector6.SetElement(0, 1); vector6.SetElement(4, 2);

        Console.WriteLine("\nVector 4: {0}", vector4);
        Console.WriteLine("Vector 5: {0}", vector5);
        Console.WriteLine("Vector 6: {0}", vector6);

        Matrix matrix3 = new Matrix(3, 3);
        matrix3.SetColumn(0, vector4); matrix3.SetColumn(1, vector5); matrix3.SetColumn(2, vector6);


        Console.WriteLine("----------\nНижний треугольник:");
        Matrix.PrintMatrix(matrix3);
        Console.WriteLine(" + {0} = {1}", vector1, Matrix.SolveLU_DownTriangle(matrix3, vector1));
        
        Console.WriteLine("----------\nВерхний треугольник:");
        Matrix.PrintMatrix(matrix6);
        Console.WriteLine(" + {0} = {1}", vector2, Matrix.SolveLU_UpTriangle(matrix6, vector2));

        Console.WriteLine("---------");

        Vector vector7 = new Vector(3);
        vector7.SetElement(16, 0); vector7.SetElement(81, 1); vector7.SetElement(16, 2);
        Vector vector8 = new Vector(3);
        vector8.SetElement(81, 0); vector8.SetElement(9, 1); vector8.SetElement(25, 2);
        Vector vector9 = new Vector(3);
        vector9.SetElement(16, 0); vector9.SetElement(25, 1); vector9.SetElement(9, 2); 
        Vector vector10 = new Vector(3);
        vector10.SetElement(1, 0); vector10.SetElement(-1, 1); vector10.SetElement(1, 2); 

        Vector vectorB = new Vector(3); vectorB[0] = 4; vectorB[1] = 3; vectorB[2] = 5;
       //vectorB.SetElement(4, 0); vectorB.SetElement(9, 1); vectorB.SetElement(4, 2);

        Matrix matrix4 = new Matrix(3, 3);
        matrix4[0, 0] = 5; matrix4[0, 1] = 1; matrix4[0, 2] = -2;
        matrix4[1, 0] = 1; matrix4[1, 1] = 4; matrix4[1, 2] = -1;
        matrix4[2, 0] = -2; matrix4[2, 1] = -1; matrix4[2, 2] = 6;
        // matrix4.SetColumn(0, vector7); matrix4.SetColumn(1, vector8); matrix4.SetColumn(2, vector9);

        Matrix matrix7 = new Matrix(3, 3);
        matrix7[0, 0] = 1; matrix7[0, 1] = 2; matrix7[0, 2] = 3;
        matrix7[1, 0] = 4; matrix7[1, 1] = 5; matrix7[1, 2] = 6;
        matrix7[2, 0] = 7; matrix7[2, 1] = 8; matrix7[2, 2] = 10;
        Vector vectorB2 = new Vector(3); vectorB2[0] = 3; vectorB2[1] = 6; vectorB2[2] = 9;


        Matrix.PrintMatrix(matrix4);
        
        Console.WriteLine("----------\nМетод Гаусса:");
        Console.WriteLine("Matrix * {0} = {1} {2}", vectorB, Matrix.MethodGauss(matrix4, vectorB), matrix4* Matrix.MethodGauss(matrix4, vectorB));
        
        Console.WriteLine("----------\nМетод квадратного корня:");
        Console.WriteLine("Matrix * {0} = {1} {2}", vectorB, Matrix.MethodSquareRoot(matrix4, vectorB), matrix4 * Matrix.MethodSquareRoot(matrix4, vectorB));

        Console.WriteLine("----------\nМетод Грамма-Шмидта:");
        Console.WriteLine("Matrix * {0} = {1} {2}", vectorB, Matrix.MethodGramSchmidt(matrix4, vectorB), matrix4 * Matrix.MethodGramSchmidt(matrix4, vectorB));

        Console.WriteLine("----------\nМетод последовательных приближений:");
        Console.WriteLine("Matrix * {0} = {1} {2}", vectorB, Matrix.MethodSuccessiveApproximations(matrix4, vectorB), matrix4 * Matrix.MethodSuccessiveApproximations(matrix4, vectorB));
    }
    //Тестирование метода наименьших квадратов
    public static void TestMethodLeastSquares()
    {
        Console.WriteLine("Тестирование МНК:");
        Vector x = new Vector(new double[] { 0, 5, 10, 15, 20, 25 });
        Vector y = new Vector(new double[] { 21, 39, 51, 63, 70, 90 });
        LeastSquares.FuncPsi[] psiArray = new LeastSquares.FuncPsi[] { x => x, x => x*x };

        Console.WriteLine($"X: {x}");
        Console.WriteLine($"Y: {y}");
        Console.WriteLine("Function 1 = x");
        Console.WriteLine("Function 2 = x*x");

        LeastSquares func = new LeastSquares(x, y, psiArray);
        Console.WriteLine("Параметры: {0}", func.p);
        Console.WriteLine("Критерий: {0}", func.GetCriteria());
    }
    //Тестирование интерполирования сплайнами
    public static void TestSplainInterpolation()
    {
        Vector x = new Vector(new double[] { 2, 3, 4, 5, 6 });
        Vector y = new Vector(new double[] { 0.693, 1.099, 1.386, 1.609, 1.792});

        Console.WriteLine($"X: {x}");
        Console.WriteLine($"Y: {y}");

        SplineInterpolation func = new SplineInterpolation(x, y);
        double xt = 3.5;
        double yt = func.GetInterpolation(xt);

        Console.WriteLine($"xt: {xt}");
        Console.WriteLine($"yt: {yt}");
    }
    //Тестирование интегралов
    public static void TestIntegral() 
    {
        Console.WriteLine("\nТестирование интегралов:");
        Console.WriteLine("Функция: x^3-3x^2+2x-27");
        int a = 2;
        int b = 6;
        Console.WriteLine($"Нижний предел: {a}");
        Console.WriteLine($"Верхний предел: {b}");

        Console.WriteLine("\nМетод прямоугольника");
        double result = Integrals.MethodRectangle(a, b, 0.001, x => x * x * x - 3 * x * x + 2 * x - 27);
        Console.WriteLine($"Ответ: {result}");

        Console.WriteLine("\nМетод трапеции");
        double result1 = Integrals.MethodTrapezoid(a, b, 0.001, x => x * x * x - 3 * x * x + 2 * x - 27);
        Console.WriteLine($"Ответ: {result1}");

        Console.WriteLine("\nМетод Симпсона");
        double result2 = Integrals.MethodSimpsons(a, b, 0.001, x => x * x * x - 3 * x * x + 2 * x - 27);
        Console.WriteLine($"Ответ: {result2}");

        Console.WriteLine("\nПроверка двойного интеграла:");
        double x0 = 0;
        double x1 = 2;
        double y0 = 0;
        double y1 = 2;
        double eps = 0.01;

        double result14 = Integrals.MethodSimpsonDouble(x0, x1, y0, y1, eps, (x, y) => x + y + 1);
        Console.WriteLine("Ответ: x + y + 1 [{0}, {1}] x [{2}, {3}] = {4}", x0, x1, y0, y1, result14);
    }
    //Тестирование дифференциальных уравнений
    public static void TestDifferentialEquations()
    {
        double t0 = 0;
        double tEnd = 1;
        int step = 10;
        Vector xn = new Vector(new double[] { 0, 1 });

        Console.WriteLine("Старт - {0}, Конец - {1}, Шагов - {2}, Начальное состояние - {3}", t0, tEnd, step, xn);
        Console.WriteLine("Analitic tEnd  {0}   {1}", Math.Sin(1.0), Math.Cos(1.0));
        Console.WriteLine("\nМетод Эйлера");
        Matrix resultEuler = DifferentialEquations.MethodEuler(t0, tEnd, xn, step, (t, x) => DifferentialEquations.PravilaDU(t, x));
        resultEuler = MethodHelp.MatrixRound(resultEuler);
        Matrix.PrintMatrix(resultEuler);

        Console.WriteLine("\nМетод Рунга-Кутта 2-го порядка:");
        Matrix resultKutta2 = DifferentialEquations.MethodRungeKutta2(t0, tEnd, xn, step, (t, x) => DifferentialEquations.PravilaDU(t, x));
        resultKutta2 = MethodHelp.MatrixRound(resultKutta2);
        Console.WriteLine($"Ответ:");
        Matrix.PrintMatrix(resultKutta2);

        Console.WriteLine("\nМетод Рунга-Кутта 4-го порядка:");
        Matrix resultKutta4 = DifferentialEquations.MethodRungeKutta4(t0, tEnd, xn, step, (t, x) => DifferentialEquations.PravilaDU(t, x));
        resultKutta4 = MethodHelp.MatrixRound(resultKutta4);
        Console.WriteLine($"Ответ:");
        Matrix.PrintMatrix(resultKutta4);

        Console.WriteLine("\nМетод Адамса 4:");
        Matrix resultAdams = DifferentialEquations.MethodAdams4(t0, tEnd, xn, step, (t, x) => DifferentialEquations.PravilaDU(t, x));
        resultAdams = MethodHelp.MatrixRound(resultAdams);
        Console.WriteLine($"Ответ:");
        Matrix.PrintMatrix(resultAdams);


    }
}
class Program
{
    public static void Main(string[] args)
    {

        //Test.TestMethodSearchRoot();
        //Test.TestMatrix();
        //Test.TestMethodLeastSquares();
        //Test.TestSplainInterpolation();
        //Test.TestIntegral();
        //Test.TestDifferentialEquations();

        Console.ReadKey();
    }
}


//Написать свой класс для больших чисел