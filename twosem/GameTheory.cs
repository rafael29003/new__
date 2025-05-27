namespace IOiMO
{
    class GameTheory
    {
        private double[,] clean_values;
        private int c_cols;
        private int c_rows;
        bool saddle_point;
        private double[] clean_result;

        private double[,] mixed_values;
        private int m_cols;
        private int m_rows;
        private double[] mixed_result;

        public GameTheory(double[,] clean, double[,] mixed)
        {
            clean_values = clean;
            c_rows = clean_values.GetLength(0);
            c_cols = clean_values.GetLength(1);
            saddle_point = false;
            clean_result = new double[3];

            mixed_values = mixed;
            m_rows = mixed_values.GetLength(0);
            m_cols = mixed_values.GetLength(1);
            mixed_result = new double[7];
        }
        public int getRows_Clean()
        {
            return c_rows;
        }
        public int getCols_Clean()
        {
            return c_cols;
        }
        public int getRows_Mixed()
        {
            return m_rows;
        }
        public int getCols_Mixed()
        {
            return m_cols;
        }
        public void View_Clean()
        {
            Console.WriteLine("Матрица значений: ");
            for (int i = 0; i < this.getRows_Clean(); i++)
            {
                for (int j = 0; j < this.getCols_Clean(); j++)
                {
                    Console.Write(clean_values[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void View_Mixed()
        {
            Console.WriteLine("Матрица значений: ");
            for (int i = 0; i < this.getRows_Mixed(); i++)
            {
                for (int j = 0; j < this.getCols_Mixed(); j++)
                {
                    Console.Write(mixed_values[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        public void Clean_Strategy()
        {
            int row = this.getRows_Clean();
            int col = this.getCols_Clean();

            Vector rowsVal = new Vector(row);
            Vector colsVal = new Vector(col);
            double t_row, t_column;

            //Нахождение мин значения в каждой строке и макс. значения в каждом столбце
            for (int i = 0; i < row; i++)
            {
                t_row = double.MaxValue;

                for (int j = 0; j < col; j++)
                {
                    if (clean_values[i, j] < t_row) t_row = clean_values[i, j];
                }
                rowsVal[i] = t_row;
            }
            for (int i = 0; i < col; i++)
            {
                t_column = double.MinValue;
                for (int j = 0; j < row; j++)
                {
                    if (clean_values[j, i] > t_column) t_column = clean_values[j, i];
                }
                colsVal[i] = t_column;
            }

            //Нахождение минимакса и максимина
            t_column = double.MaxValue;
            t_row = double.MinValue;
            int colId = -1, rowId = -1;

            for (int i = 0; i < row; i++)
            {
                if (rowsVal[i] > t_row)
                {
                    t_row = rowsVal[i];
                    rowId = i;
                }
                if (colsVal[i] < t_column)
                {
                    t_column = colsVal[i];
                    colId = i;
                }
            }

            if (Math.Abs(t_column - t_row) < 0.0001 && Math.Abs(clean_values[rowId, colId] - t_column) < 0.0001)
            {
                saddle_point = true;
                clean_result[0] = t_row;
                clean_result[1] = rowId + 1;
                clean_result[2] = colId + 1;
                Console.WriteLine("Седловая точка в строке {0}, столбце {1}, значение = {2}", clean_result[1], clean_result[2], clean_result[0]);


            }
            else
            {
                Console.WriteLine("Задача не решается чистой стратегией");
            }
        }

        public void Mixed_Strategy()
        {
            double p1, p2, q1, q2;
            double min_cost = double.MaxValue;

            if (getRows_Mixed() != 2 || getCols_Mixed() < 2) Console.WriteLine("Матрица не удовлетворяет условиям");
            for (int i = 0; i < getCols_Mixed() - 1; i++)
            {
                for (int j = i + 1; j < getCols_Mixed(); j++)
                {
                    double cost = 0;
                    p1 = ((mixed_values[1, j] - mixed_values[1, i]) / ((mixed_values[0, i] + mixed_values[1, j]) - (mixed_values[0, j] + mixed_values[1, i])));
                    q1 = ((mixed_values[1, j] - mixed_values[0, j]) / ((mixed_values[0, i] + mixed_values[1, j]) - (mixed_values[0, j] + mixed_values[1, i])));
                    p2 = 1 - p1;
                    q2 = 1 - q1;

                    cost = p1 * (mixed_values[0, i] * q1 + mixed_values[0, j] * q2) + p2 * (mixed_values[1, i] * q1 + mixed_values[1, j] * q2);
                    if (cost < min_cost)
                    {
                        min_cost = cost;
                        mixed_result[0] = p1;
                        mixed_result[1] = q1;
                        mixed_result[2] = p2;
                        mixed_result[3] = q2;
                        mixed_result[4] = min_cost;
                        mixed_result[5] = i+1;
                        mixed_result[6] = j+1;
                    }
                }
            }
            Console.WriteLine("p1 = {0}, q1 = {1}, p2 = {2}, q2 = {3}, v = {4} в столбцах {5} и {6}", mixed_result[0], mixed_result[1], mixed_result[2], mixed_result[3], mixed_result[4], mixed_result[5], mixed_result[6]);
        }
    }
}
