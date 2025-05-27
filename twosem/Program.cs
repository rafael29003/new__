using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


using IOiMO;



Vector a = new Vector(new double[] { 100, 200, 150 }); // Запасы
Vector b = new Vector(new double[] { 150, 150, 100 }); // Потребности

// Создание матрицы стоимостей
Matrix c = new Matrix(3, 3);
c[0, 0] = 10; c[0, 1] = 20; c[0, 2] = 15;
c[1, 0] = 30; c[1, 1] = 10; c[1, 2] = 20;
c[2, 0] = 20; c[2, 1] = 15; c[2, 2] = 10;

// Создание задачи транспортного планирования
TransportTask task1 = new TransportTask(a, b, c);

// Формирование начального плана
task1.InitPlan();

// Проверка плана на оптимальность
while (true)
{
    Point optimumPoint = task1.ControlOptimum();

    if (optimumPoint.value >= 0)
    {
        Console.WriteLine("План является оптимальным.");
        break;
    }
    else
    {
        Console.WriteLine($"План можно улучшить. Минимальный элемент: {optimumPoint.value} в точке ({optimumPoint.i}, {optimumPoint.j}).");
        task1.ImprovePlan();

        // Добавьте проверку на зацикленность
        int iterationCount = 0;
        iterationCount++;
        if (iterationCount > 1000) // Остановите цикл после 1000 итераций
        {
            Console.WriteLine("Зацикленность обнаружена. Прекращение цикла.");
            break;
        }
    }
}


// Вывод критерия
Console.WriteLine($"Критерий: {task1.GetCriterion()}");




List<Predmet> items = new List<Predmet>
        {
            new Predmet("Предмет 1", 3, 5),
            new Predmet("Предмет 2", 2, 6),
            new Predmet("Предмет 3", 3, 7),
            new Predmet("Предмет 4", 2, 5),
            new Predmet("Предмет 5", 3, 6),
            new Predmet("Предмет 6", 4, 7)
        };

int maxWeight = 13;

var solver = new KnapsackSolver(items, maxWeight);
var result = solver.Solve();

Console.WriteLine($"Максимальная стоимость: {result.MaxValue}");
Console.WriteLine("Выбранные предметы:");
foreach (var item in result.SelectedItems)
{
    Console.WriteLine($"- {item.Name} (вес: {item.Weight}, стоимость: {item.Cost})");
}
Console.WriteLine($"Вес: {result.TotalWeight}");
Console.WriteLine($"Стоимость: {result.TotalCost}");



Console.WriteLine("Транспортная задача");
Console.WriteLine("Тест №1");
Vector a1 = new Vector(new double[] { 30, 40, 20 });
Vector b1 = new Vector(new double[] { 20, 30, 30, 10 });
Matrix c1 = new Matrix(new double[,] { { 2, 3, 2, 4 }, { 3, 2, 5, 1 }, { 4, 3, 2, 6 } });
Console.WriteLine("Количество товара у поставщиков: {0}", a1);
Console.WriteLine("Спрос потребителей: {0}", b1);
Console.WriteLine("Стоимость перевозки единицы товара:\n{0}", c1);
TransportTask test_1 = new TransportTask(a1, b1, c1);
test_1.InitPlan();
double cr1 = test_1.GetCriterion();
Point min_delta_1 = test_1.ControlOptimum();
Console.WriteLine("Начальный план:\n{0}", test_1.x);
Console.WriteLine("Нулевой критерий = {0}", cr1);
Console.WriteLine("Минимальная дельта: " +
    "i = {0}, j = {1}, значение = {2}", min_delta_1.i, min_delta_1.j, min_delta_1.value);

Console.WriteLine("\nТест №2");
Vector a2 = new Vector(new double[] { 140, 180, 160 });
Vector b2 = new Vector(new double[] { 60, 70, 120, 130, 100 });
Matrix c2 = new Matrix(new double[,] { { 2, 3, 4, 2, 4 }, { 3, 4, 1, 4, 1 }, { 9, 7, 3, 7, 2 } });
Console.WriteLine("Количество товара у поставщиков: {0}", a2);
Console.WriteLine("Спрос потребителей: {0}", b2);
Console.WriteLine("Стоимость перевозки единицы товара:\n{0}", c2);
TransportTask test_2 = new TransportTask(a2, b2, c2);
test_2.InitPlan();
double cr2 = test_2.GetCriterion();
Point min_delta_2 = test_2.ControlOptimum();
Console.WriteLine("Начальный план:\n{0}", test_2.x);
Console.WriteLine("Нулевой критерий = {0}", cr2);
Console.WriteLine("Минимальная дельта: " +
    "i = {0}, j = {1}, значение = {2}", min_delta_2.i, min_delta_2.j, min_delta_2.value);
Console.ReadLine();

Console.WriteLine("Транспортные сети");
Graph N = new Graph();

Vertex s = new Vertex("s");
Vertex v1 = new Vertex("v1");
Vertex v2 = new Vertex("v2");
Vertex v3 = new Vertex("v3");
Vertex v4 = new Vertex("v4");
Vertex t = new Vertex("t");

N.allVertexs.Add(s);
N.allVertexs.Add(v1);
N.allVertexs.Add(v2);
N.allVertexs.Add(v3);
N.allVertexs.Add(v4);
N.allVertexs.Add(t);

N.AddEdge(s, v1, 16);
N.AddEdge(s, v2, 13);
N.AddEdge(v1, v2, 10);
N.AddEdge(v1, v3, 12);
N.AddEdge(v2, v3, 9);
N.AddEdge(v2, v4, 14);
N.AddEdge(v3, v4, 7);
N.AddEdge(v3, t, 20);
N.AddEdge(v4, t, 4);

double max_f = N.FindMxFlow(s, t);
Console.WriteLine(max_f);

Console.WriteLine("Решение задачи коммивояжера МБС");
Graph Cities  = new Graph();

Vertex city_1 = new Vertex("A");
Vertex city_2 = new Vertex("B");
Vertex city_3 = new Vertex("C");
Vertex city_4 = new Vertex("D");
Vertex city_5 = new Vertex("E");

Cities.allVertexs.Add(city_1);
Cities.allVertexs.Add(city_2);
Cities.allVertexs.Add(city_3);
Cities.allVertexs.Add(city_4);
Cities.allVertexs.Add(city_5);


Cities.AddEdge(city_1, city_2, 3);
Cities.AddEdge(city_1, city_3, 6);
Cities.AddEdge(city_1, city_4, 4);
Cities.AddEdge(city_1, city_5, 5);

Cities.AddEdge(city_2, city_3, 7);
Cities.AddEdge(city_2, city_4, 9);
Cities.AddEdge(city_2, city_5, 8);

Cities.AddEdge(city_3, city_4, 6);
Cities.AddEdge(city_3, city_5, 11);

Cities.AddEdge(city_4, city_5, 2);

List<Vertex> path = Cities.ComivoyagerTask(city_1);
foreach(Vertex v in path)
    Console.WriteLine(v.ToString());

Console.WriteLine("Расстановка скобок при умножении матриц");
Vector p = new Vector(new double[] { 5, 10, 6, 8, 3 });
MatrixMultiplication task = new MatrixMultiplication(p);
string answer = task.Solve();
Console.WriteLine(answer);


Console.ReadLine();






