using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Алгоритм_дейкстры
{
    public partial class Form1 : Form
    {
        private const int radius = 160;

        private Graph _graph; //Искомый объекл класса Граф
        private Graphics _graphics; //Объект графики для отрисовки визуальной части на PictureBox
 
        //Конструктор по умолчанию
        public Form1()
        {
            InitializeComponent();
            _graph = new Graph(); //Создаем новый граф
            _graph.GraphType = GraphType.NonOrientied;
            _graphics = pictureBox1.CreateGraphics(); //Создаем графику для отрисовки
        }

        //При изменении числа вершин
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _graph.Vertexes.Clear(); //Очищаем все вершины перед новым построением
            for (int i = 1; i <= numericUpDown1.Value; i++)
                _graph.Vertexes.Add(new Vertex(i));  //Заново добавляем вершины

            numericUpDown2.Maximum = _graph.Vertexes.Count;
            numericUpDown3.Maximum = _graph.Vertexes.Count;
            numericUpDown7.Maximum = _graph.Vertexes.Count;
            numericUpDown8.Maximum = _graph.Vertexes.Count;

            DrawPolygon(); //Отрисовываем граф
        }

        //Отрисовка графа
        private void DrawPolygon()
        {
            //Очищаем графику для нового рисунка
            _graphics.Clear(BackColor);
            int vertexCount = _graph.Vertexes.Count;
            Point centerOfPictureBox = new Point(pictureBox1.Height / 2 + 40, pictureBox1.Width / 2 - 100); //Центр pictureBox, по нему центрируем круги - вершины
            
            //Массив точек, по которым будут отрисовываться круги-вершины
            List<Point> polygonPoints = new List<Point>();
            double radius = 160; //Радиус от центра 
            
            for (int i = 0; i < vertexCount; i++)
            {
                int pointX = Convert.ToInt32(centerOfPictureBox.X  + radius * Math.Cos((2 * Math.PI * i) / vertexCount));
                int pointY = Convert.ToInt32(centerOfPictureBox.Y + radius * Math.Sin((2 * Math.PI * i) / vertexCount));
                polygonPoints.Add(new Point(pointX, pointY)); //Считаем углы и точки размещения вершин на равных расстояниях
            }

            int nameIndexOfVertex = 1;
            foreach (Point p in polygonPoints)
            {
                _graphics.FillEllipse(new SolidBrush(Color.Blue), p.X, p.Y, 25, 25); //Заполняем круги - вершины
                _graphics.DrawString(nameIndexOfVertex.ToString(), //Пишем на них цифры - номера вершин
                    new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif),13), 
                    new SolidBrush(Color.White), 
                    p.X + 5, 
                    p.Y + 2);

                nameIndexOfVertex++;
            }
        }

        //При нажатии на "добавить путь"
        private void button1_Click(object sender, EventArgs e)
        {
            int leftOffset = 15;
            
            //Создаем ребро, считывая с формы данные
            Edge edge = new Edge
            {
                Cost = Convert.ToInt32(numericUpDown6.Value),
                StartVertex = _graph.Vertexes[Convert.ToInt32(numericUpDown2.Value - 1)],
                EndVertex = _graph.Vertexes[Convert.ToInt32(numericUpDown3.Value - 1)]
            };

            //Добалвяем в граф ребер
            _graph.Edges.Add(edge);

            //Центр pictureBox, по нему центрируем круги - вершины
            Point centerOfPictureBox = new Point(pictureBox1.Height / 2 + 40, pictureBox1.Width / 2 - 100);
            int vertexCount = _graph.Vertexes.Count;

            //Точка у одной вершины для соединения прямой
            Point point1 = new Point(
                Convert.ToInt32(centerOfPictureBox.X + radius * Math.Cos((2 * Math.PI * Convert.ToInt32(numericUpDown2.Value - 1)) / vertexCount)) + leftOffset,
                Convert.ToInt32(centerOfPictureBox.Y + radius * Math.Sin((2 * Math.PI * Convert.ToInt32(numericUpDown2.Value - 1)) / vertexCount)) + leftOffset
                );

            //Точка другой вершины для соединения прямой
            Point point2 = new Point(
               Convert.ToInt32(centerOfPictureBox.X + radius * Math.Cos((2 * Math.PI * Convert.ToInt32(numericUpDown3.Value - 1)) / vertexCount)) + leftOffset,
               Convert.ToInt32(centerOfPictureBox.Y + radius * Math.Sin((2 * Math.PI * Convert.ToInt32(numericUpDown3.Value - 1)) / vertexCount)) + leftOffset
               );

            //Рисуем линию (соединяем прямой - гранью, две вершины
            _graphics.DrawLine(new Pen(new SolidBrush(Color.Red), 2.5f), point1, point2);
            //Пишем поверх грани вес
            _graphics.DrawString(edge.Cost.ToString(),
                 new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 13),
                  new SolidBrush(Color.Green),
                 (point1.X + point2.X) / 2, 
                 (point1.Y + point2.Y) / 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int beginVertex = Convert.ToInt32(numericUpDown7.Value - 1);
            int endVertex = Convert.ToInt32(numericUpDown8.Value - 1);

            if (radioButton1.Checked)
                _graph.GraphType = GraphType.NonOrientied;
            else
                _graph.GraphType = GraphType.Orientied;

            //Создаем объект алгоритма дейкстры на основе графа
            Algorithm deykstra = new Algorithm(_graph);
            string result = String.Empty;

            //Вычисляем минимальные пути до всех вершин
            var mathResult = deykstra.ShortestWaysFromBeginVertexToAllVertexes(beginVertex);
            for (int i = 1; i < mathResult.Count; i++)
                result += mathResult[i] + " ";
            label10.Text = result;

            //Вычисляем кратчайший путь
            var anotherResult = deykstra.ShortestWaysFromBeginVertexToEndVertex(mathResult.ToArray(), beginVertex, endVertex);

            result = String.Empty;
            for (int i = 1; i < anotherResult.Count; i++)
                result += anotherResult[i] + " ";

            label11.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _graphics.Clear(BackColor);
            _graph = new Graph();
        }
    }
}
