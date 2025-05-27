using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace IOiMO
{
    enum COLORS_VERTEX
    {
        WHITE,
        GRAY,
        BLACK
    }

    class Vertex
    {
        private static int IDV = 0;
        private int ID;
        public string label; // Метка (имя вершины)
        private List<Edge> edges; // Список ребер, связанных с вершиной
        public double sumdistance; // Сумма растояний
        public COLORS_VERTEX color; // Цвет вершины
        public Vertex prevvertex; // Ссылка на предшественника
        public bool visited;
        public int num_color;

        public Vertex(string label) // Конструктор
        {
            this.label = label;
            IDV++;
            edges = new List<Edge>();
            sumdistance = Double.MaxValue;
            color = COLORS_VERTEX.WHITE;
            prevvertex = null;
            ID = IDV;
            this.visited = false;
        }

        public int GetID() { return ID; }

        // Получение списка ребер
        public List<Edge> GetEdges() { return edges; }

        public override string ToString()
        {
            string sout = "";
            sout = sout + label;
            sout = sout + "  ID=" + ID.ToString();
            return sout;
        }

        // Просмотр ребер, связанных с вершиной
        public void ViewEdges()
        {
            Console.Write("Edges for {0}", this);
            foreach (Edge curedge in edges)
                Console.Write("  {0}", curedge);
            Console.WriteLine();
        }

        // Добавление ребра
        public bool AddEdge(Edge edge)
        {
            if (edge.BeginPoint != this) return false;
            for (int i = 0; i < edges.Count; i++)
            {
                Edge CurEdge = edges[i];
                if (edge.EndPoint.Equals(CurEdge.EndPoint)) return false;
            }
            edges.Add(edge);
            return true;
        }
    }

    class Edge
    {
        public Vertex BeginPoint; // Начальная вершина
        public Vertex EndPoint;  // Конечная вершина
        public double distance; // Пропускная способность 
        public double current; // Текущий поток

        // Конструктор
        public Edge(Vertex begin, Vertex end, double d)
        {
            this.BeginPoint = begin;
            this.EndPoint = end;
            this.distance = d;
            this.current = 0;
        }

        public override string ToString()
        {
            string sout = "";
            sout = "{" + BeginPoint.label + "  " + EndPoint.label + " p=" + distance.ToString() +
                " current=" + current.ToString() + "}";
            return sout;
        }
    }

    class Graph
    {
        public List<Vertex> allVertexs; // Список всех вершин
        public List<Edge> allEdges; // Список всех ребер

        // Конструктор
        public Graph()
        {
            allVertexs = new List<Vertex>();
            allEdges = new List<Edge>();
        }

        // Добавление ребра
        public bool AddEdge(Vertex v1, Vertex v2, double d)
        {
            if (!allVertexs.Contains(v1)) return false;
            if (!allVertexs.Contains(v2)) return false;
            foreach (Edge cure in v1.GetEdges())
            {
                if (cure.EndPoint.GetID() == v2.GetID()) return false;
            }
            Edge ev1v2 = new Edge(v1, v2, d);
            Edge ev2v1 = new Edge(v2, v1, d);
            v1.GetEdges().Add(ev1v2); v2.GetEdges().Add(ev2v1);
            allEdges.Add(ev1v2);
            return true;
        }

        // Поиск в ширину для транспортной задачи
        public void BFS_Flow(Vertex s)
        {
            Queue<Vertex> Q = new Queue<Vertex>(); // Очередь вершин
                                                   // Инициализация
            foreach (Vertex cv in allVertexs)
            {
                cv.prevvertex = null;
            }
            s.color = COLORS_VERTEX.GRAY;
            Q.Enqueue(s);
            Vertex u, v;
            List<Vertex> used_vertexes = new List<Vertex>();
            // Основной цикл
            while (Q.Count > 0)
            {
                u = Q.Dequeue();                                    // извлекаем вершину из очереди
                if (!used_vertexes.Contains(u))                     // если она не обработна, то добавляем ее в список
                {
                    used_vertexes.Add(u);
                    u.color = COLORS_VERTEX.BLACK;
                }

                foreach (Edge curEdge in u.GetEdges())              // просматриваем смежные вершины
                {
                    if (Math.Abs(curEdge.distance - curEdge.current) < 0.000001)  // Если ребро насыщенное, то его не рассматриваем
                        continue;
                    v = curEdge.EndPoint;
                    if (!used_vertexes.Contains(v))                  // если вершина не обработана, то добавляем
                                                                     // ее в очередь
                    {
                        v.prevvertex = u;                           // предок
                        v.color = COLORS_VERTEX.GRAY;
                        Q.Enqueue(v);
                    }
                }
            }

        }
        public List<Edge> Get_Path_Edges(Vertex s, Vertex v)
        {
            List<Edge> list = new List<Edge>();
            if (v == s)
                return list; 
            Vertex tmp;
            tmp = v;
            while (tmp != null)
            {
                if (tmp == s) return list;
                if(tmp.prevvertex==null) return list;
                Vertex prev_tmp = tmp.prevvertex;
                foreach (Edge curEdge in prev_tmp.GetEdges())
                {
                    if (curEdge.EndPoint == tmp)
                    {
                        list.Add(curEdge);
                    }
                }
                tmp = prev_tmp;
            }
            return new List<Edge>();
        }
        public double FindCurrentFlow(List<Edge> edges)
        {   
            double min_c = double.MaxValue;
            // Находим текущий поток
            foreach(Edge curEdge in edges)
            {   
                // находим разницу между пропускной способностью 
                // и текущим потоком
                double difference = curEdge.distance - curEdge.current;
                if (difference < min_c) 
                    min_c = difference;
            }

            // Меняем значение пропускной способности
            foreach (Edge curEdge in edges)
            {
                curEdge.current += min_c;
                var begin = curEdge.BeginPoint;
                var end = curEdge.EndPoint;
                foreach (var edg in end.GetEdges())
                {
                    if (edg.EndPoint == begin)
                    {
                        edg.current += min_c;
                    }
                }
            }
            return min_c;
        }

        public double FindMxFlow(Vertex s, Vertex t)
        {
            double f = 0;  // результатирующий поток
            // приравниваем текущий поток у всех ребер к нулю
            foreach (Edge curEdge in this.allEdges)
            {
                curEdge.current = 0;
            }
            while(true)
            {
                // Обход в ширину
                BFS_Flow(s);
                // Получаем путь
                List<Edge> cur_path = Get_Path_Edges(s, t);
                // Вывод пути
                cur_path.Reverse();
                // Если путь найден,
                // увеличиваем результатирующий поток
                if (cur_path.Any())
                    f += FindCurrentFlow(cur_path);
                // иначе возвращаем результатирующий поток
                else break;
                Console.WriteLine("Путь");
                foreach (Edge cur_edge in cur_path)
                    Console.WriteLine(cur_edge);
            }
            double sumf = 0;
            foreach(Edge curEdge in s.GetEdges())
            {
                sumf += curEdge.current;
            }
            Console.WriteLine("Max Flow {0}",sumf);
            ViewFlow();
            return sumf;
        }

       public void ViewFlow()
       {
            foreach (Edge curEdge in this.allEdges)
            {
                Console.WriteLine(curEdge);
            }
       }
      
     // Решение задачи коммивояжера МБС
      public List<Vertex> ComivoyagerTask(Vertex start)
        {  
           // Помечаем все вершины как непосещенные
           foreach(Vertex v in allVertexs)
                v.visited = false;
            // Помечаем начальную вершину как текущую
            Vertex cur_v = start;
            // Помечаем текущую вершину как посещенную
            cur_v.visited = true;
            // Расстояние от текущей вершины до близжайшей 
            // смежной вершины
            double minDistance;
            // Ближайшая смежная вершина
            Vertex closest;
            // Списко вершин
            List<Vertex> path = new List<Vertex>(); 
            do 
            {   
                minDistance = Double.MaxValue;
                closest = null;
                path.Add(cur_v);
                // Просматриваем смежные вершины
                foreach (Edge e in cur_v.GetEdges())
                {
                    Vertex u = e.EndPoint;
                    // Если вершина была посещена уже до этого,
                    // ее не рассматриваем
                    if (u.visited) continue;
                    // Если расстояние от текущей вершины 
                    // до смежной минимально, то меняем 
                    // ближайшую смежную вершину и минимальное расстояние
                    if(e.distance < minDistance) 
                    {
                        minDistance = e.distance;
                        closest = u;
                    }
                }
                // Если непосещенная вершина найдена
                if (closest != null)
                {   
                    // Помечаем ее как посещенную
                    closest.visited = true;
                    // Помечаем ее как текущую
                    cur_v = closest;
                }
            }
            while (closest != null);
            return path;
        }
    } 
}
