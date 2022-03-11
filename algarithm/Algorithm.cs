using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Алгоритм_дейкстры
{
    //Класс алгоритма поиска путей
    public class Algorithm
    {
        private int[,] matrix; //Матрица смежности
        private Graph graph; //Исходный граф

        public Algorithm(Graph _graph)
        {
            graph = _graph;
            int matrixSize = graph.Vertexes.Count;
            matrix = new int[matrixSize, matrixSize];       

            //Проходимся по ребрам и выставляем веса соответственно ребрам (остальные равны 0)
            foreach (Edge edge in graph.Edges)
            {
                matrix[edge.StartVertex.Id - 1, edge.EndVertex.Id - 1] = edge.Cost;
                if (graph.GraphType == GraphType.NonOrientied)
                    matrix[edge.EndVertex.Id - 1, edge.StartVertex.Id - 1] = edge.Cost;
            }
        }

        public List<int> ShortestWaysFromBeginVertexToAllVertexes(int BeginVertex)
        {
            int vertexesCount = graph.Vertexes.Count;
            int[] shortestWays = new int[vertexesCount]; //Кратчайшие пути
            int[] visitedVertexes = new int[vertexesCount]; //Посещенные вершины

            //Инициализация вершин и расстояний
            for (int i = 0; i < vertexesCount; i++)
            {
                shortestWays[i] = int.MaxValue;
                visitedVertexes[i] = 1;
            }

            shortestWays[BeginVertex] = 0;

            int minIndex = int.MaxValue;
            do
            {
                minIndex = int.MaxValue;
                int min = int.MaxValue;
                for (int i = 0; i < vertexesCount; i++)
                {
                    // Если вершину ещё не обошли
                    if (visitedVertexes[i] == 1 && shortestWays[i] < min)
                    {
                        min = shortestWays[i];
                        minIndex = i;
                    }
                }

                // Добавляем найденный минимальный вес
                // к текущему весу вершины
                // и сравниваем с текущим минимальным весом вершины
                if (minIndex != int.MaxValue)
                {
                    for (int i = 0; i < vertexesCount; i++)
                    {
                        if (matrix[minIndex, i] > 0)
                        {
                            int temp = min + matrix[minIndex, i];
                            if (temp < shortestWays[i])
                                shortestWays[i] = temp;
                        }
                    }
                    visitedVertexes[minIndex] = 0;
                }
            } while (minIndex < int.MaxValue);

            return shortestWays.ToList();
        }

        // Восстановление пути
        public List<int> ShortestWaysFromBeginVertexToEndVertex(int[] shortestWays, int beginVertex, int endVertex)
        {
            int vertexesCount = graph.Vertexes.Count;
            int[] visitedVertexes = new int[vertexesCount]; // массив посещенных вершин
            visitedVertexes[0] = endVertex + 1; // начальный элемент - конечная вершина
            int indexOfPreviousVertex = 1; // индекс предыдущей вершины
            int weigth = shortestWays[endVertex]; // вес конечной вершины

            while (endVertex != beginVertex) // пока не дошли до начальной вершины
            {
                for (int i = 0; i < vertexesCount; i++) // просматриваем все вершины
                    if (matrix[i, endVertex] != 0) // если связь есть
                    {
                        int temp = weigth - matrix[i, endVertex]; // определяем вес пути из предыдущей вершины
                        if (temp == shortestWays[i]) // если вес совпал с рассчитанным  значит из этой вершины и был переход
                        {
                            weigth = temp;
                            endVertex = i;
                            visitedVertexes[indexOfPreviousVertex] = i + 1;
                            indexOfPreviousVertex++;
                        }
                    }
            }

            var result = visitedVertexes.ToList();
            result.Reverse();
            return result;
        }
    }  
}
