using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace ComputerGraphics1
{
	public partial class Form1 : Form
	{
		public class Point3D
		{
			public double X { get; set; }
			public double Y { get; set; }
			public double Z { get; set; }
			public double H { get; set; }
			public Point3D(double x, double y, double z, double h)
			{
				X = x;
				Y = y;
				Z = z;
				H = h;

			}
			public override string ToString()
			{
				return $"X: {X}, Y: {Y}, Z: {Z}, H: {H}";
			}
		}
		private double Angle = Math.PI / 30;
		Point[] points = new Point[_pointNumber];
		Rectangle A = new Rectangle(50, 50, 50, 50);
		Rectangle B = new Rectangle(0, 0, 50, 50);
		private const int _pointNumber = 8;
		private const int _pointNumberTriangle = 5;
		private Point3D[] _points = new Point3D[_pointNumber];
		private Random _random = new Random();
		private Graphics _graphics;
		public Form1()
		{
			InitializeComponent();
		}
		private void BuildLine()
		{
			for (int i = 100; i < 200; i++)
			{
				double x = Map(0.5, 0, 1, 0, i);

				double y = Map(0.75, 0, 1, 0, i);
				Brush br = Brushes.Red;
				_graphics.FillRectangle(br, (float)x, (float)y, 2, 2);
			}

		}
		private void MovePoint(Point3D point)
		{
			double x = point.X;
			double y = point.Y;
			double z = point.Z;

			//OY axis
			point.X = x * Math.Cos(Angle) + z * Math.Sin(Angle);
			point.Z = -x * Math.Sin(Angle) + z * Math.Cos(Angle);
			//OX axis
			//point.Y = y * Math.Cos(Angle) - z * Math.Sin(Angle);
			//point.Z = y * Math.Sin(Angle) + z * Math.Cos(Angle);
			//OZ axis
			/*point.X = x * Math.Cos(Angle) - y * Math.Sin(Angle);
			point.Y = x * Math.Sin(Angle) + y * Math.Cos(Angle);*/
			//point.Y = point.Y * 3;

			/*point.X *= -1;
			point.Z *= -1;*/
		}
		private void DrawPoint(Point3D point, int i)
		{
			if (points.Any(p => (p.X >= pictureBox1.Width || p.Y >= pictureBox1.Height)))
				return;
			double mappedZ = Map(point.Z, 0, 1, 0, pictureBox1.Width)/* + pictureBox1.Width / 2*/;
			double pointSize = Map(1 / point.Z, 0, point.Z, 0, pictureBox1.Width / 10);

			using (StreamWriter sw = new StreamWriter(@"C:\Users\HP-PC\C#programming\garbage\ComputerGraphics1\ComputerGraphics1\size.txt", true))
			{
				sw.WriteLine($"points[{i}]: {_points[i]}, mappedZ: {mappedZ}, pointSize: {pointSize}");
			}

			double x = Map(point.X / point.Z, 0, 1, 0, pictureBox1.Width) + pictureBox1.Width / 2;

			double y = Map(point.Y / point.Z, 0, 1, 0, pictureBox1.Height) + pictureBox1.Height / 2;
			Brush br = Brushes.Red;


			/*_points[i] = new Point3D(x, y, _points[i].Z, _points[i].H);
			points[i] = new Point((int)x, (int)y);*/

			//changed
			_graphics.FillRectangle(br, (float)x, (float)y, (float)pointSize, (float)pointSize);
		}

		private double Map(double n, double start1, double stop1, double start2, double stop2)
		{
			return (((n - start1) / (stop1 - start1)) * (stop2 - start2) /*+ start2*/) / 10;
		}
		private void SavePoint(Point3D point, int i)
		{
			points[i] = MapPointWidth(_points[i]);
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			_graphics.Clear(Color.White);
			//FillPoints();

			TriangleTask();
			//Thread.Sleep(2000);
			pictureBox1.Refresh();
		}
		private void TriangleTask()
		{
			for (int i = 0; i < _pointNumberTriangle; i++)
			{
				MovePoint(_points[i]);
				SavePoint(_points[i], i);
			}
			for (int i = 0; i < _pointNumberTriangle; i++)
			{
				DrawPoint(_points[i], i);
			}
			DrawTriangle();
		}

		private void RectangleTask()
		{
			for (int i = 0; i < _pointNumber; i++)
			{
				MovePoint(_points[i]);
				SavePoint(_points[i], i);
			}
			for (int i = 0; i < _pointNumber; i++)
			{
				DrawPoint(_points[i], i);
			}
			DrawRectangle();
		}
		private Point MapPointWidth(Point3D point)
		{
			return new Point(((int)Map(point.X / point.Z, 0, 1, 0, pictureBox1.Width) + pictureBox1.Width / 2), ((int)Map(point.Y / point.Z, 0, 1, 0, pictureBox1.Height) + pictureBox1.Height / 2));
		}
		private void DrawTriangle()
		{
			//_graphics.DrawRectangle(Pens.Red, A);
			//_graphics.DrawRectangle(Pens.Blue, B);
			using (StreamWriter sw = new StreamWriter(@"C:\Users\HP-PC\C#programming\garbage\ComputerGraphics1\ComputerGraphics1\data.txt", false))
			{
				for (int i = 0; i < _pointNumberTriangle; i++)
				{
					sw.WriteLine($"points[{i}]: {_points[i]}, Map(points[{i}]) = {points[i]}");
				}
			}
			if (points.Any(p => (p.X >= pictureBox1.Width || p.Y >= pictureBox1.Height)))
				return;
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[0]), MapPointWidth(_points[1]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[0]), MapPointWidth(_points[2]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[1]), MapPointWidth(_points[3]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[2]), MapPointWidth(_points[3]));

			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[0]), MapPointWidth(_points[4]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[1]), MapPointWidth(_points[4]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[2]), MapPointWidth(_points[4]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[3]), MapPointWidth(_points[4]));

			//SaveRectangle();
		}
		private void DrawRectangle()
		{
			//_graphics.DrawRectangle(Pens.Red, A);
			//_graphics.DrawRectangle(Pens.Blue, B);
			using (StreamWriter sw = new StreamWriter(@"C:\Users\HP-PC\C#programming\garbage\ComputerGraphics1\ComputerGraphics1\data.txt", false))
			{
				for (int i = 0; i < _pointNumber; i++)
				{
					sw.WriteLine($"points[{i}]: {_points[i]}, Map(points[{i}]) = {points[i]}");
				}
			}
			if (points.Any(p => (p.X >= pictureBox1.Width || p.Y >= pictureBox1.Height)))
				return;
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[0]), MapPointWidth(_points[2]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[0]), MapPointWidth(_points[6]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[4]), MapPointWidth(_points[2]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[6]), MapPointWidth(_points[4]));
			/*_graphics.DrawLine(Pens.Green, points[0], points[6]);
			_graphics.DrawLine(Pens.Green, points[4], points[2]);
			_graphics.DrawLine(Pens.Green, points[6], points[4]);*/
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[1]), MapPointWidth(_points[3]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[1]), MapPointWidth(_points[7]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[5]), MapPointWidth(_points[3]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[7]), MapPointWidth(_points[5]));

			/*_graphics.DrawLine(Pens.Green, points[1], points[3]);
			_graphics.DrawLine(Pens.Green, points[1], points[7]);
			_graphics.DrawLine(Pens.Green, points[5], points[3]);
			_graphics.DrawLine(Pens.Green, points[7], points[5]);*/
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[0]), MapPointWidth(_points[1]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[2]), MapPointWidth(_points[3]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[4]), MapPointWidth(_points[5]));
			_graphics.DrawLine(Pens.Green, MapPointWidth(_points[6]), MapPointWidth(_points[7]));

			/*_graphics.DrawLine(Pens.Green, points[0], points[1]);
			_graphics.DrawLine(Pens.Green, points[2], points[3]);
			_graphics.DrawLine(Pens.Green, points[4], points[5]);
			_graphics.DrawLine(Pens.Green, points[6], points[7]);*/

			//SaveRectangle();
		}
		private void FillPoints()
		{
			//points[0] = new Point(1, 1);
			_points[0] = new Point3D(1, 1, 2, 1);
			//points[1] = new Point(2, 1);
			_points[1] = new Point3D(2, 1, 2, 1);

			//points[2] = new Point(1, 2);
			_points[2] = new Point3D(1, 2, 2, 1);
			//points[3] = new Point(2, 2);
			_points[3] = new Point3D(2, 2, 2, 1);

			//points[4] = new Point(1, 2);
			_points[4] = new Point3D(1, 2, 1, 1);
			//points[5] = new Point(2, 2);
			_points[5] = new Point3D(2, 2, 1, 1);

			//points[6] = new Point(1, 1);
			_points[6] = new Point3D(1, 1, 1, 1);
			//points[7] = new Point(2, 1);
			_points[7] = new Point3D(2, 1, 1, 1);
		}
		private void FillPointsTriangle()
		{
			//points[0] = new Point(1, 1);
			_points[0] = new Point3D(1, 1, 1, 1);
			//points[1] = new Point(2, 1);
			_points[1] = new Point3D(2, 1, 1, 1);

			//points[2] = new Point(1, 2);
			_points[2] = new Point3D(1, 2, 1.5, 1);
			//points[3] = new Point(2, 2);
			_points[3] = new Point3D(2, 2, 1.5, 1);

			//points[4] = new Point(1, 2);
			_points[4] = new Point3D(Math.Sqrt(2), 3 * Math.Sqrt(2), 1, 1);
			//points[5] = new Point(2, 2);
			/*_points[5] = new Point3D(2, 2, 1, 1);

			//points[6] = new Point(1, 1);
			_points[6] = new Point3D(1, 1, 1, 1);
			//points[7] = new Point(2, 1);
			_points[7] = new Point3D(2, 1, 1, 1);*/
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			using (StreamWriter sw = new StreamWriter(@"C:\Users\HP-PC\C#programming\garbage\ComputerGraphics1\ComputerGraphics1\size.txt", false))
			{
				sw.WriteLine();
			}
			pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			_graphics = Graphics.FromImage(pictureBox1.Image);
			//BuildLine();
			FillPointsTriangle();
			timer1.Start();
		}
	}
}
