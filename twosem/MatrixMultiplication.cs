using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace IOiMO
{
    class MatrixMultiplication
    {   
        // количество перемножаемых матриц
        int n;
        // размерности перемножаемых матриц
        Vector p;
        // Матрица стоимостей 
        Matrix m;
        // Матрица для сохранения k
        int[,] s;
        // Строка для вывода 
        string order = new string("");

        public MatrixMultiplication(Vector sizes)
        { 
            p = sizes;
            n = p.Size - 1;
            m = new Matrix(n, n);
            s = new int[n, n];
        }

        public void MatrixChainOrder()
        {  
            for(int l = 1; l < n; l++) {
                for(int i = 0; i < n - l; i++)
                {
                    int j = i + l;
                    m[i, j] = double.MaxValue;
                    for(int k = i;  k < j; k++)
                    {
                        double q = m[i, k] + m[k + 1, j] + p[i] * p[k + 1] * p[j + 1];
                        if (q < m[i, j])
                        {
                            m[i, j] = q;
                            s[i, j] = k;
                        }
                    }
                }
            }
        }

        public void PrintOptimalParens(int i, int j)
        {
            if (i == j)
            {
                order += "A" + i;
            }
            else
            {
                order += "(";
                PrintOptimalParens(i, s[i, j]);
                order += "*";
                PrintOptimalParens(s[i, j] + 1, j);
                order += ")";
            }
        }

        public string Solve()
        {
            MatrixChainOrder();
            PrintOptimalParens(0, n - 1);
            return order;
        }
    }
}
