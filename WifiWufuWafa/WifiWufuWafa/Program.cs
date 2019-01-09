using System;
using System.IO;

namespace WifiWufuWafa
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("programm is start");
            string[] train = File.ReadAllLines("train.txt");
            string[] test = File.ReadAllLines("test.txt");
            float[][] x_train, y_train, x_test, y_test;
            x_train = CreateMxN(train.Length, 7);
            y_train = CreateMxN(1, train.Length);
            x_test = CreateMxN(test.Length, 7);
            y_test = CreateMxN(1, test.Length);

            for (int i = 0; i < train.Length; i++)
            {
                string[] t = train[i].Split('	');
                for (int j = 0; j < 7; j++)
                {
                    x_train[i][j] = float.Parse(t[j]);
                }
                y_train[0][i] = float.Parse(t[7]);
            }

            for (int i = 0; i < test.Length; i++)
            {
                string[] t = test[i].Split('	');
                for (int j = 0; j < 7; j++)
                {
                    x_test[i][j] = float.Parse(t[j]);
                }
                y_test[0][i] = float.Parse(t[7]);
            }

            float[][] vec = LogReg(x_train, y_train, 0.0000003f, 1000);

            for (int i = 0; i < vec.Length; i++)
            {
                string s = "";
                for (int j = 0; j < vec[0].Length; j++)
                {
                    s = s.Insert(s.Length, vec[i][j] + " ");
                }
                Console.WriteLine(s);
            }

            CheckTest(vec, x_test, y_test);

            Console.ReadKey();

            CheckTest(vec, x_train, y_train);

            Console.WriteLine("programm is over");
            Console.ReadKey();
        }

        static void CheckTest(float[][] vec, float[][] x, float[][] y)
        {
            int[] total = new int[4] { 0, 0, 0, 0 };
            for (int i = 0; i < x.Length; i++)
            {
                string s = "";
                float top = 0;
                double topp = 0;
                for (int j = 0; j < vec.Length; j++)
                {
                    s = s.Insert(s.Length, f(Skal(x[i], vec[j])) + " ");
                    if (f(Skal(x[i], vec[j])) > topp)
                    {
                        top = j;
                        topp = f(Skal(x[i], vec[j]));
                    }
                }
                if ((top + 1) == y[0][i]) total[(int)top]++;
                s = s.Insert(s.Length, "     " + y[0][i]);
                Console.WriteLine(s);
            }
            string result = "";
            for (int i = 0; i < total.Length; i++)
            {
                result = result.Insert(result.Length, (i + 1) + ". " + total[i] + "   ");
            }
            int summ = 0;
            foreach (int num in total)
            {
                summ += num;
            }
            Console.WriteLine("Correct: " + result + "\nTotal: " + (float)summ / x.Length);
        }

        static float[][] LogReg(float[][] x, float[][] y, float h, int iter)
        {
            float[][] vec = CreateMxN(4, x[0].Length);
            float[] var = new float[] { 1, 2, 3, 4 };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < iter; j++)
                {
                    float[] sumVec = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
                    for (int g = 0; g < y[0].Length; g++)
                    {
                        if (y[0][g] == var[i])
                        {
                            sumVec = M_M(sumVec, AxM(1 - (float)f(Skal(vec[i], x[g])), x[g]));
                        }
                        else
                        {
                            sumVec = M_M(sumVec, AxM(0 - (float)f(Skal(vec[i], x[g])), x[g]));
                        }
                    }
                    vec[i] = M_M(vec[i], AxM(h, sumVec));
                }
            }
            return vec;
        }

        static double f(float z)
        {
            return (1 / (1 + Math.Exp(-z)));
        }

        static float Skal(float[] m1, float[] m2)
        {
            float c = 0;
            for (int i = 0; i < m1.Length; i++)
            {
                c += m1[i] * m2[i];
            }
            return c;
        }

        static float[] AxM(float a, float[] m)
        {
            float[] m1 = new float[m.Length];
            for (int i = 0; i < m.Length; i++)
            {
                m1[i] = m[i] * a;
            }
            return m1;
        }

        static float[] M_M(float[] a, float[] b)
        {
            float[] c = new float[a.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a[i] + b[i];
            }
            return c;
        }

        static float[] _M(float[] a)
        {
            float[] c = new float[a.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = -a[i];
            }
            return c;
        }

        static float[][] CreateMxN(int m, int n)
        {
            float[][] mn = new float[m][];
            for (int i = 0; i < m; i++)
            {
                mn[i] = new float[n];
            }
            return mn;
        }
    }
}
