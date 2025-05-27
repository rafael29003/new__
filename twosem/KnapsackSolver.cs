using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


using IOiMO;

public class KnapsackSolver
{
    private readonly List<Predmet> items;
    private readonly int maxWeight;

    public KnapsackSolver(List<Predmet> items, int maxWeight)
    {
        this.items = items;
        this.maxWeight = maxWeight;
    }

    public class KnapsackResult
    {
        public int MaxValue { get; set; }
        public List<Predmet> SelectedItems { get; set; }
        public int TotalWeight { get; set; }
        public int TotalCost { get; set; }
    }

    public KnapsackResult Solve()
    {
        int n = items.Count;
        int[,] dp = new int[n + 1, maxWeight + 1];

        // Построение таблицы динамического программирования
        for (int i = 1; i <= n; i++)
        {
            for (int w = 1; w <= maxWeight; w++)
            {
                if (items[i - 1].Weight <= w)
                {
                    dp[i, w] = Math.Max(
                        dp[i - 1, w],
                        dp[i - 1, w - items[i - 1].Weight] + items[i - 1].Cost
                    );
                }
                else
                {
                    dp[i, w] = dp[i - 1, w];
                }
            }
        }

        // Восстановление выбранных предметов
        List<Predmet> selected = new List<Predmet>();
        int currentWeight = maxWeight;

        for (int i = n; i > 0; i--)
        {
            if (dp[i, currentWeight] != dp[i - 1, currentWeight])
            {
                selected.Add(items[i - 1]);
                currentWeight -= items[i - 1].Weight;
            }
        }

        return new KnapsackResult
        {
            MaxValue = dp[n, maxWeight],
            SelectedItems = selected,
            TotalWeight = selected.Sum(item => item.Weight),
            TotalCost = selected.Sum(item => item.Cost)
        };
    }
}
