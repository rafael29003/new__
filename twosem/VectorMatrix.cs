using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IOiMO
{
    public class Vector
    {
        protected int size;
        protected double[] data;
        static Random rnd = new Random(); // Для генерации параметров вектора
        public int Size { get { return size; } }

        public Vector(int size)
        {
            this.size = size;
            data = new double[size];
        }      

        public Vector(double[] vector)
        {
            this.size = vector.Length;
            data = new double[size];
            for (int i = 0; i < size; i++) data[i] = vector[i];
        }

        public double[] GetElements()
        {
            return data;
        }
        //Получить элементы вектора

        public int GetSize() { return size; }
        // Получить количесвто элементов вектора

        public double GetElement(int index)
        {
            if (index < 0 || index >= size) return default(double);
            return data[index];
        }
        // Получить элемент вектора по индексу

        public double this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }
        // Получить или присвоить значение элемента вектора по индексу

        public bool SetElement(double el, int index)
        {
            if (index < 0 || index >= size) return false;
            data[index] = el;
            return true;
        }
        // Присвоить значение элемента по индексу

        public Vector Copy()
        {
            Vector copied_vector = new Vector(data);
            return copied_vector;
        }
        // Сделать копию вектора

        public override string ToString() => $"{{{string.Join("; ", this.data)}}}";
        // Оформить вектор для вывода

        public double Norma1()
        {
            double total = 0;
            for (int i = 0; i < size; i++)
                total += data[i] * data[i];
            return Math.Sqrt(total);
        }
        // Евклидова норма вектора 

        public double Norma2()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                if (Math.Abs(data[i]) > s) s = Math.Abs(data[i]);
            return s;
        }
        // Норма Чебышева

        public double Norma3()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                s += Math.Abs(data[i]);
            return s;
        }
        // l1-норма 

         public Vector Normalize()
        {
            Vector result = new Vector(size);
            double length = Norma1();
            for (int i = 0; i < size; i++)
                if (length != 0) result.data[i] = data[i] / length; else result.data[i] = data[i];
            return result;
        }
        // Нормализация вектора 

        public static Vector NormalizeRandom(int size)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++)
                rez.data[i] = (rnd.NextDouble() - 0.5) * 2.0;
            return rez.Normalize();
        }
        // Нормализовать вектор заданной размерности с рандомными параметрами

        public Vector UMinus()
        {
            Vector result = new Vector(size);
            for (int i = 0; i < size; i++) result.data[i] = -data[i];
            return result;
        }
        // Получить отрицательный вектор

        public static Vector operator -(Vector vector) => vector.UMinus();
        // Перегрузка оператора - для унарного минуса

        public Vector Plus(Vector other_vector)
        {
                if (size == other_vector.size)
                {
                    Vector result = new Vector(size);
                    for (int i = 0; i < size; i++)
                        result[i] += data[i] + other_vector.data[i];
                    return result;
                }
                return null;
        }
        // Сложение векторов


        public static Vector operator +(Vector vector_1, Vector vector_2) => vector_1.Plus(vector_2);
        // Перегрузка оператора + для сложения векторов

        public Vector Minus(Vector other_vector)
        {
            if (size == other_vector.size)
            {
                Vector result = new Vector(size);
                for (int i = 0; i < size; i++) result.data[i] = data[i] - other_vector.data[i];
                return result;
            }
            return null;
        }
        // Вычитание векторов 

        public static Vector operator - (Vector vector_1, Vector vector_2) => vector_1.Minus(vector_2);
        // Перегрузка оператора - для вычитания векторов

        public Vector MultiplyScalar(double number)
        {
            Vector result = new Vector(size);
            for (int i = 0; i < size; i++) result.data[i] = data[i] * number;
            return result;
        }
        // Умножение вектора на число 

        public static Vector operator *(Vector vector_1, double num) => vector_1.MultiplyScalar(num);
        // Перегрузка оператора * для умножения вектора на число

        public static Vector operator *(double num, Vector vector_1) => vector_1.MultiplyScalar(num);
        // Перегрузка оператора * для умножения числа на вектор

        public double ScalarMultiply(Vector other_vector)
        {
            if (size != other_vector.size) return 0;
            double total = 0;
            for (int i = 0; i < size; i++)
                total += data[i] * other_vector.data[i];
            return total;
        }
        // Скалярное произведение векторов

        public static double operator *(Vector first_vector, Vector second_vector) => first_vector.ScalarMultiply(second_vector);
        // Перегрузка оператора * для скалярного произведения векторов

        public static Vector Abs(Vector v)
        {
            for(int i = 0; i < v.size; i++)
            { 
                v.data[i] = Math.Abs(v.data[i]);
            }
            return v;
        }
    
    }

    public class Matrix
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
        // Возвращает количесвто строк матрицы

        public int Columns { get { return columns; } }
        // Возвращает количесвто столбцов матрицы

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
        // Возвращает заданную строку матрицы

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
        // Возвращает столбец матрицы 

        public bool SetRow(int index, Vector r)
        {
            if (index < 0 || index > rows) return false;
            if (r.Size != columns) return false;
            for (int k = 0; k < columns; k++) data[index, k] = r[k];
            return true;
        }
        // Присвоить вектор заданной строке матрицы


        public bool SetColumn(int index, Vector c)
        {
            if (index < 0 || index > columns) return false;
            if (c.Size != rows) return false;
            for (int k = 0; k < rows; k++) data[k, index] = c[k];
            return true;
        }
        // Присвоить вектор заданному столбцу матрицы

        public Matrix RemoveRow(int r)
        {
            if (r < 0 || r >= rows) return this;
            int l = 0;
            Matrix modified_matrix = new Matrix(rows - 1, columns);
            for (int i = 0; i < rows; i++)
            {  
                if (i != r)
                {
                    for (int j = 0; j < columns; j++)
                        modified_matrix[l, j] = data[i, j];
                    l++;
                }
            }
            return modified_matrix;
        }

        public Matrix RemoveColumn(int c)
        {
            if (c < 0 || c >= columns) return this;
            Matrix modified_matrix = new Matrix(rows, columns - 1);
            for (int i = 0; i < rows; i++)
            {
                int l = 0;
                for (int j = 0; j < columns; j++)
                {
                    if(j != c)
                    {
                        modified_matrix[i, l] = data[i, j];
                        l++;
                    }
                }
            }
            return modified_matrix;
        }

        public void SwapRows(int r1, int r2)
        {
            if (r1 < 0 || r2 < 0 || r1 >= rows || r2 >= rows || (r1 == r2)) return;
            Vector v1 = GetRow(r1);
            Vector v2 = GetRow(r2);
            SetRow(r2, v1);
            SetRow(r1, v2);
        }
        // Меняет заданные строки матрицы местами

        public Matrix Copy()
        {
            Matrix copied_matrix = new Matrix(rows, columns);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++) copied_matrix[i, j] = data[i, j];
            return copied_matrix;
        }
        // Возвращает копию матрицы

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
        // Возвраащет транспонированную матрицу

        public static Matrix operator +(Matrix m1, Matrix m2)    
        {
            if (m1.rows != m2.rows || m1.columns != m2.columns)
            {
                throw new Exception("Матрицы не совпадают по размерности");
            }

            Matrix result = new Matrix(m1.rows, m1.columns);

            for (int i = 0; i < m1.rows; i++)
            {
                for (int j = 0; j < m1.columns; j++)
                {
                    result[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return result;
        }
        // Сложение матриц

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.rows != m2.rows || m1.columns != m2.columns)
            {
                throw new Exception("Матрицы не совпадают по размерности");
            }

            Matrix result = new Matrix(m1.rows, m1.columns);

            for (int i = 0; i < m1.rows; i++)
            {
                for (int j = 0; j < m1.columns; j++)
                {
                    result[i, j] = m1[i, j] - m2[i, j];
                }
            }
            return result;
        }
        // Вычитание матриц

        public static Matrix operator *(Matrix m, double number)
        {
            for (int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.columns; j++) m[i, j] *= number; 
            }
            return m;

        }
        // Умножение матрицы на число

        public static Matrix operator *(double number, Matrix m)
        {
            for (int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.columns; j++) m[i, j] *= number;
            }
            return m;

        }
        // Умножение числа на матрицу
        
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.columns != m2.rows)
            {
             throw new Exception("Количество столбцов первой матрицы не совпадает с количесвтом строк второй");
            }

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
                      if (Math.Abs(result.data[i,j]) < 0.000001) result.data[i, j] = 0;
                    }
                }
            }

            return result;

        }
        // Произведение матриц

        public static  Vector operator *(Matrix m, Vector v)
        {
            if (m.columns != v.Size) return null;
            Vector result = new Vector(m.rows);
            for (int i = 0; i < m.rows; i++)
            {
                result[i] = 0;
                for (int j = 0; j < m.columns; j++)
                {
                    result[i] += m[i, j] * v[j];
                }
            }
            return result;
        }
        // Умножение матрицы на вектор

        public static Vector operator *(Vector v, Matrix m)
        {
            if (m.columns != v.Size) return null;
            if (m.columns != m.rows) return null;
            Vector result = new Vector(v.Size);
            for (int i = 0; i < m.rows; i++)
            {
                result[i] = 0;
                for (int j = 0; j < m.columns; j++)
                {
                    result[i] += m[j, i] * v[j];
                }
            }
            return result;
        }
        // Умножение вектора на матрицу

        public static Matrix IdentityMatrix(int size)
        {
            Matrix E = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                E[i, i] = 1;
            }
            return E;
        }
        // Формирование единичной матрицы


        public static Vector LowerTriangle(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;
            if (A.Rows != B.Size) return null;

            int num_of_rows = A.Rows;
            double eps = 0.0000000001;
            for (int i = 0; i < num_of_rows; i++)
            {
                if (A[i, i] < eps) return null;
                for (int j =  i + 1; j < num_of_rows; j++)
                {
                    if (A[i, j] > eps) return null;
                }
            }


            Vector x = new Vector(num_of_rows);
            x[0] = B[0] / A[0, 0];
            for (int i = 1; i < num_of_rows; i++)
            {
                double summa = 0;
                for (int j = 0; j < i; j++)
                {
                    summa += A[i, j] * x[j];
                    
                }
                x[i] = (B[i] - summa) / A[i, i];
            }
            return x;
        }
        // Решенеие СЛАУ с нижней треугольной матрицей

        public static Vector UpperTriangle(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;
            if (A.Rows != B.Size) return null;

            int num_of_rows = A.Rows;
            int last_row = num_of_rows - 1;
            double eps = 0.0000000001;
            for (int i = last_row; i < 0; i--)
            {
                if (A[i, i] < eps) return null;
                for (int j = 0; j < i; j++)
                {
                    if (A[i, j] > eps) return null;
                }
            }

            Vector x = new Vector(num_of_rows);
            x[num_of_rows - 1] = B[num_of_rows - 1] / A[num_of_rows - 1, num_of_rows - 1];
            for (int i = num_of_rows - 2; i >= 0; i--)
            {
                double summa = 0;
                for (int j = i + 1; j < num_of_rows; j++)
                {
                    summa += A[i, j] * x[j];

                }
                x[i] = (B[i] - summa) / A[i, i];
            }
            return x;
        }
        // Решенеие СЛАУ с верхней треугольной матрицей
      
        public static Vector GaussMethod(Matrix A, Vector B)
        {
            Matrix copied_A = A.Copy();
            Vector copied_B = B.Copy();
            int rows = A.rows;
            int columns = A.columns;
            double eps = 0.00000000001;

            if (rows != columns) return null;
            if (rows != B.Size) return null;

            int max;
            double tmp;
            
            for (int j = 0; j < columns; j++)
            {
                max = j;
                for (int i = j + 1; i < rows; i++)
                { 
                    if (Math.Abs(copied_A[i, j]) > Math.Abs(copied_A[max, j])) { max = i; }; 
                }

                if (max != j)
                {
                    Vector temp = copied_A.GetRow(max); copied_A.SetRow(max, copied_A.GetRow(j)); copied_A.SetRow(j, temp);
                    tmp = copied_B[max]; copied_B[max] = copied_B[j]; copied_B[j] = tmp;

                }


                if (Math.Abs(copied_A[j, j]) < eps) return null;
                Console.WriteLine(copied_A);

                for (int i = j + 1; i < rows; i++)
                {
                    double multiplier = -copied_A[j, j] / copied_A[i, j];
                    for (int k = 0; k < columns; k++)
                    {
                        copied_A[i, k] *= multiplier;
                        copied_A[i, k] += copied_A[j, k];
                    }
                    copied_B[i]*= multiplier;
                    copied_B[i] += copied_B[j]; 
                }
            }
            return UpperTriangle(copied_A, copied_B);
        }
        // Решение СЛАУ методом Гаусса с выбором главного элемента по столбцу

        public static Matrix MatrixInverse(Matrix A)
        {
            int rows = A.rows;
            int columns = A.columns;
            if (rows != columns) return null;
            Matrix copied_A = A.Copy();

            Matrix result = new Matrix(rows, columns);
            Matrix E = new Matrix(rows, columns);
            for (int i = 0; i < rows; i++)
            {
                E[i, i] = 1;
            }

            double eps = 0.00000000001;
            int max;
            for (int j = 0; j < columns; j++)
            {
                max = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (Math.Abs(copied_A[i, j]) > Math.Abs(copied_A[max, j])) { max = i; };
                }

                if (max != j)
                {
                    Vector temp = copied_A.GetRow(max); copied_A.SetRow(max, copied_A.GetRow(j)); copied_A.SetRow(j, temp);
                    Vector tmp = E.GetRow(max); E.SetRow(max, E.GetRow(j)); E.SetRow(j, tmp);
                }

                if (Math.Abs(copied_A[j, j]) < eps) return null;

                for (int i = j + 1; i < rows; i++)
                {
                    double multiplier = -copied_A[j, j] / copied_A[i, j];
                    for (int k = 0; k < columns; k++)
                    {
                        copied_A[i, k] *= multiplier;
                        copied_A[i, k] += copied_A[j, k];
                        E[i, k] *= multiplier;
                        E[i, k] += E[j, k];
                    }
                }
            }
                for (int i = 0; i < columns; i++)
                {
                    result.SetColumn(i, UpperTriangle(copied_A, E.GetColumn(i)));
                }
            
            return result;

        }
        // Получение обратной матрицы методом Гаусса

        public static Vector ProgonkaV(Vector c, Vector d, Vector e, Vector b)
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

        public static Vector ThomasAlgorithm(double[] e, double[] d, double[] c, Vector B)
        {
            if (d.Length != B.Size) return null;

            double eps = 0.0000000001;
            for (int i = 0; i < e.Length; i++)
            {
                if (Math.Abs(e[i]) < eps) return null;
            }
            for (int i = 0; i < d.Length; i++)
            {
                if (Math.Abs(d[i]) < eps) return null;
            }
            for (int i = 0; i < c.Length; i++)
            {
                if (Math.Abs(c[i]) < eps) return null;
            }

            int n = d.Length;
            bool flag = false;
            for (int i = 1; i < n - 1; i++)
            {
                if (Math.Abs(d[i]) < Math.Abs(e[i - 1]) + Math.Abs(c[i])) return null;
                if (Math.Abs(d[i]) > Math.Abs(e[i - 1]) + Math.Abs(c[i])) flag = true;
            }
            if (!flag) return null;
            
            double[] alpha = new double[n - 1];
            double[] beta = new double[n];

            alpha[0] = -e[0] / d[0];
            beta[0] = B[0] / d[0];
            for (int i = 1; i < n - 1; i++)
            {
                alpha[i] = -e[i] / (d[i] + c[i - 1] * alpha[i - 1]);
                beta[i] = (B[i] - c[i - 1] * beta[i-1]) / (d[i] + c[i - 1] * alpha[i - 1]);
            }
            beta[n-1] = (B[n-1] - c[n-2] * beta[n- 2]) / (d[n-1] + c[n-2] * alpha[n - 2]);

            Vector x = new Vector(B.Size);
            x[n - 1] = beta[n - 1];
            for(int i = n - 2; i >= 0; i--)
            {
                x[i] = alpha[i] * x[i + 1] + beta[i];
            }
            return x;
        }
        //Методы прогонки

        public static Vector SqrtMethod(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;                                       // Проверки
            if (A.Rows != B.Size) return null;
            if (A[0, 0] <= 0) return null;
            for (int i = 0; i < A.Rows; i++)
            {
                for(int j = 0; j < A.Columns; j++)
                {
                    if (A[i, j] != A[j, i])
                    {
                        return null;
                    }
                }
            }

            int n = A.Rows;
            Matrix T = new Matrix(n, n);
        
            T[0, 0] = Math.Sqrt(A[0, 0]);                                               // t11
            for(int j = 1; j < n; j++)  T[0, j] = A[0, j] / T[0, 0];                    // t1j, j > 1
            for (int i = 1; i < n; i++)
            {
                double summa = 0;
                for (int k = 0; k < i; k++)
                {
                    summa += Math.Pow(T[k, i], 2);
                }

                double value = A[i, i] - summa;
                if (value < 0) return null;
                T[i, i] = Math.Sqrt(value);                                             // tii, 1 < i <= n

                for (int j = i + 1; j < n; j++)
                {
                    summa = 0;
                    for (int k = 0; k < i; k++)
                    {
                        summa += T[k, i] * T[k, j];
                    }
                    T[i, j] = (A[i, j] - summa) / T[i, i];                              // tij, i < j
                }
            }

            Matrix TransT = T.Trans();
            Vector y = LowerTriangle(TransT, B);
            Vector x = UpperTriangle(T, y);
            return x;
        }
        //Метод квадратных корней

        public static Vector GramSchmidtMethod(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;                                       
            if (A.Rows != B.Size) return null;

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

                    Vector new_r = a;
                    for (int k = 0; k < i; k++)
                    {
                        new_r -= T[k, i] * R.GetColumn(k);
                    }
                    R.SetColumn(i, new_r);
                }
            }
            Console.WriteLine(R);
            Matrix D = R.Trans() * R;
            for (int i = 0; i < n; i++) D[i, i] = 1 / D[i, i];
            Vector y = R.Trans() * B * D;
            Vector x = UpperTriangle(T, y);
          return x;
        }
        //Метод Грама-Шмидта

        // Метод вращения Гивенса
        public static Vector GivensRotationMethod(Matrix A, Vector B)
        { 
            if (A.Rows != A.Columns) return null;
            if (A.Rows != B.Size) return null;
            
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
            Vector x = UpperTriangle(A,B);
            return x;
        }

        // Метод последовательных приближений
        public static Vector SuccessiveApproximations(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;
            if (A.Rows != B.Size) return null;

            int n = A.Rows;
            double eps = 0.00000001;
            int max;
            double tmp;

            for (int j = 0; j < n; j++)
            {
                max = j;
                for (int i = j + 1; i < n; i++)
                {
                    if (Math.Abs(A[i, j]) > Math.Abs(A[max, j])) { max = i; };
                }

                if (max != j)
                {
                    Vector temp = A.GetRow(max); A.SetRow(max, A.GetRow(j)); A.SetRow(j, temp);
                    tmp = B[max]; B[max] = B[j]; B[j] = tmp;

                }
                if (Math.Abs(A[j, j]) < eps) return null;

            }

            Vector beta = new Vector(n);
            for (int i = 0; i < beta.Size; i++)
            {
                beta[i] = B[i] / A[i, i];
            }

            Matrix alpha = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if (i != j) alpha[i, j] = A[i, j] / A[i, i];
                    else alpha[i, j] = 0;
                }
                beta[i] = B[i] / A[i, i];
            }

            Vector prev_x = beta;
            Vector current_x; 
            Vector delta;

            do
            {  
                current_x = beta - alpha * prev_x;
                delta = current_x - prev_x;
                prev_x = current_x;
            }
            while (delta.Norma1() > eps);

            return prev_x;
        }

        //Метод скорейшего спуска
        public static Vector SteepestDescentMethod(Matrix A, Vector B)
        {
            if (A.Rows != A.Columns) return null;
            if (A.Rows != B.Size) return null;

            Vector x = new Vector(A.Rows);
            for(int i = 0; i < x.Size; i++)
            {
                x[i] = B[i] / A[i, i];
            }
            Vector r = A * x - B;
            double eps = 0.00000001;
           
            while (r.Norma1() >= eps)
            {
                x -= (r * r) / (A * r * r) * r;
                r = A * x - B;
            }

            return x;
        }

        //Метод Леверрье для нахождения коэффициентов характеристического уравнения
        public static double[] LeverrierMethod(Matrix A)
        {
            if (A.Rows != A.Columns) return null;
            int n = A.Rows;
            Matrix A_degree = A;

            double[] p = new double[n];
            double[] s = new double[n];

            for(int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++) s[k] += A_degree[i, i];
                p[k] = s[k];
                for(int i = k - 1; i >= 0; i--)
                {
                    p[k] += s[i] * p[k - i - 1];
                }
                p[k] /= -k - 1;
                A_degree *= A;
            }
            return p;
        }

        public override string ToString()
        {   
            string output_matrix = string.Empty;
            double[] current_row;

            for (int i = 0; i < rows; i++)
            {
                current_row = GetRow(i).GetElements();
                output_matrix += string.Join("\t", current_row) + "\n";
            }

            return output_matrix;
        }
        // Оформить матрицу для вывода
        
    }


    }
