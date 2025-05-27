using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace IOiMO
{
    // Структура для описания минимального элемента
    // матрицы дельта
    public struct Point
    {
        public int i;
        public int j;
        public double value;
        public Point(int input_i, int input_j, double input_value)
        {
            this.i = input_i;
            this.j = input_j;
            this.value = input_value;
        }
    }

    public class TransportTask
    {
        public Vector a;
        public Vector b;
        public Matrix c;
        public Matrix x;
        public Vector v;
        public Vector u;
        public Matrix delta;
        int n;
        int m;
        int num_of_trans;

        public TransportTask(Vector input_a, Vector input_b, Matrix input_c)
        {
            a = input_a.Copy();
            b = input_b.Copy();
            c = input_c.Copy();
            n = a.GetSize();
            m = b.GetSize();
            x = new Matrix(n, m);
            v = new Vector(n);
            u = new Vector(m);
            delta = new Matrix(n, m);
            if (a != b)
                return;
        }

        // Построение начального плана методом минимальной стоимости
        public void InitPlanMinCost()
        {
            Vector a_copy = a.Copy();
            Vector b_copy = b.Copy();
            Matrix c_copy = c.Copy();
            int l = 0; int k = 0;
            double eps = 0.00001;
            while (a_copy.Norma1() > eps && b_copy.Norma1() > eps)
            {
                double c_min = double.MaxValue;
                // Находим минимальную стоимость в матрице с
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (c_copy[i, j] < c_min)
                        {
                            l = i; k = j;
                            c_min = c_copy[i, j];
                        }
                    }
                }
                // Добавляем новое значение в план
                x[l, k] = Math.Min(a_copy[l], b_copy[k]);
                // Считаем количество перевозок
                if (x[l, k] >= 0) num_of_trans++;
                // В зависимости от условия, вычеркиваем
                // строку или стобец в матрице с
                if (x[l, k] == a_copy[l])
                {
                    for (int j = 0; j < m; ++j)
                        c_copy[l, j] = double.NaN;
                }
                else if (x[l, k] == b_copy[k])
                {
                    for (int i = 0; i < n; ++i)
                        c_copy[i, k] = double.NaN;

                }
                else if (x[l, k] == a_copy[l] && x[l, k] == b_copy[k])
                {
                    for (int i = 0; i < n; ++i)
                        c_copy[i, k] = double.NaN;
                    for (int j = 0; j < m; ++j)
                        c_copy[l, j] = double.NaN;
                }
                // Изменяем количество товара у поставщика 
                // и спрос потребителя
                a_copy[l] -= x[l, k];
                b_copy[k] -= x[l, k];
                UpdateTransportCount();
            }
        }

        // Построение начального плана методом северо-западного угла
        public void InitPlan()
        {
            Vector a_copy = a.Copy();
            Vector b_copy = b.Copy();
            int i = 0, j = 0;

            // Обнуляем матрицу перевозок
            x = new Matrix(n, m);

            // Алгоритм северо-западного угла
            while (i < n && j < m)
            {
                double quantity = Math.Min(a_copy[i], b_copy[j]);
                x[i, j] = quantity;

                a_copy[i] -= quantity;
                b_copy[j] -= quantity;

                if (a_copy[i] == 0) i++;
                if (b_copy[j] == 0) j++;
            }

            UpdateTransportCount();
        }

        private void UpdateTransportCount()
        {
            num_of_trans = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (x[i, j] > 0)
                        num_of_trans++;
        }

        // Вычисление критерия
        public double GetCriterion()
        {
            double criterion = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    criterion += c[i, j] * x[i, j];
                }
            }
            return criterion;
        }

        // Проверка плана на оптимальность
        public Point ControlOptimum()
        {
            bool allFound = false;

            // Инициализация потенциалов
            for (int i = 0; i < n; i++) v[i] = double.NaN;
            for (int j = 0; j < m; j++) u[j] = double.NaN;

            // Вычисляем разницу между количеством уравнений и неизвестных
            int diff = (n + m) - num_of_trans;

            // Вводим значение для одного из потенциалов
            for (int i = 0; i < diff; i++) v[i] = 0;

            // Находим потенциалы
            while (!allFound)
            {
                allFound = true;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (x[i, j] > 0)
                        {
                            if (double.IsNaN(u[j]))
                                u[j] = c[i, j] - v[i];
                            else if (double.IsNaN(v[i]))
                                v[i] = c[i, j] - u[j];
                        }
                    }
                }

                // Проверяем, все ли потенциалы найдены
                for (int i = 0; i < n; ++i)
                {
                    if (double.IsNaN(v[i]))
                    {
                        allFound = false;
                        break;
                    }
                }
                for (int j = 0; j < m; ++j)
                {
                    if (double.IsNaN(u[j]))
                    {
                        allFound = false;
                        break;
                    }
                }
            }

            // Строим матрицу дельта и находим минимальный элемент
            double minElem = double.MaxValue;
            int l = 0, k = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    delta[i, j] = c[i, j] - v[i] - u[j];
                    if (delta[i, j] < minElem && x[i, j] == 0)
                    {
                        minElem = delta[i, j];
                        l = i;
                        k = j;
                    }
                }
            }

            return new Point(l, k, minElem);
        }

        // Улучшение плана
        public void ImprovePlan()
        {
            Point optimumPoint = ControlOptimum();

            if (optimumPoint.value < 0)
            {
                // Находим цикл в базисной матрице
                List<(int, int)> cycle = FindCycle(optimumPoint.i, optimumPoint.j);

                if (cycle != null)
                {
                    // Перераспределите поставки по циклу
                    RedistributeSupplies(cycle);
                }
            }
        }

        // Поиск цикла в базисной матрице
        public List<(int, int)> FindCycle(int i, int j)
        {
            // Реализуйте поиск цикла в базисной матрице
            // Это может быть сложно, поэтому упрощенная версия:
            List<(int, int)> cycle = new List<(int, int)>();
            cycle.Add((i, j)); // Начало цикла

            // Простая реализация поиска цикла (не полная)
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < m; y++)
                {
                    if (x != i && y != j && this.x[x, y] > 0)
                    {
                        cycle.Add((x, y));
                        break;
                    }
                }
            }

            return cycle;
        }

        // Перераспределение поставок по циклу
        private void RedistributeSupplies(List<(int, int)> cycle)
        {
            int minDelta = int.MaxValue;
            foreach (var (xIndex, yIndex) in cycle)
            {
                if (x[xIndex, yIndex] < minDelta) minDelta = (int)x[xIndex, yIndex];
            }

            foreach (var (xIndex, yIndex) in cycle)
            {
                if ((xIndex + yIndex) % 2 == 0) // Увеличьте значение в четных ячейках цикла
                    x[xIndex, yIndex] += minDelta;
                else // Уменьшите значение в нечетных ячейках цикла
                    x[xIndex, yIndex] -= minDelta;
            }
        }
    }
}


