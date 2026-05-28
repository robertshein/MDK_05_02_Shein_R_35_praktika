using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PermDynamics_Shein.Pages
{
    public partial class Chart : Page
    {
        public MainWindow mainWindow;

        public double actualHeightCanvas = 0;

        public double maxValue1 = 0;
        double averageValue1 = 0;
        private Line _averageLine1 = null;

        public double maxValue2 = 0;
        double averageValue2 = 0;
        private Line _averageLine2 = null;

        public DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public Chart(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            actualHeightCanvas = mainWindow.Height - 50d;

            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Tick += CreateNewValue;
            dispatcherTimer.Start();

            CreateChart(canvas1, mainWindow.pointsInfo, ref maxValue1, ref _averageLine1);
            ColorChart(canvas1, scroll1, mainWindow.pointsInfo, maxValue1, ref averageValue1, ref _averageLine1, current_value1, average_value1);

            CreateChart(canvas2, mainWindow.pointsInfo2, ref maxValue2, ref _averageLine2);
            ColorChart(canvas2, scroll2, mainWindow.pointsInfo2, maxValue2, ref averageValue2, ref _averageLine2, current_value2, average_value2);
        }

        private void CreateNewValue(object sender, EventArgs e)
        {
            Random random = new Random();

            double value1 = mainWindow.pointsInfo[mainWindow.pointsInfo.Count - 1].value;
            mainWindow.pointsInfo.Add(new Classes.PointInfo(value1 * (random.NextDouble() + 0.5d)));
            ControlCreateChart(canvas1, scroll1, mainWindow.pointsInfo, ref maxValue1, ref averageValue1, ref _averageLine1, current_value1, average_value1);

            double value2 = mainWindow.pointsInfo2[mainWindow.pointsInfo2.Count - 1].value;
            mainWindow.pointsInfo2.Add(new Classes.PointInfo(value2 * (random.NextDouble() + 0.5d)));
            ControlCreateChart(canvas2, scroll2, mainWindow.pointsInfo2, ref maxValue2, ref averageValue2, ref _averageLine2, current_value2, average_value2);
        }

        public void CreateChart(Canvas canvas, List<Classes.PointInfo> points, ref double maxValue, ref Line averageLine)
        {
            canvas.Children.Clear();
            averageLine = null;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].value > maxValue)
                    maxValue = points[i].value;
            }
            for (int i = 0; i < points.Count; i++)
            {
                Line line = new Line();

                line.X1 = i * 20;
                line.X2 = (i + 1) * 20;

                if (i == 0)
                    line.Y1 = actualHeightCanvas;
                else
                    line.Y1 = actualHeightCanvas - ((points[i - 1].value / maxValue) * actualHeightCanvas);

                line.Y2 = actualHeightCanvas - ((points[i].value / maxValue) * actualHeightCanvas);

                line.StrokeThickness = 2;
                points[i].line = line;
                canvas.Children.Add(line);
            }
        }

        public void CreatePoint(Canvas canvas, List<Classes.PointInfo> points, double maxValue)
        {
            Line line = new Line();
            line.X1 = (points.Count - 1) * 20;
            line.X2 = points.Count * 20;
            line.Y1 = actualHeightCanvas - ((points[points.Count - 2].value / maxValue) * actualHeightCanvas);
            line.Y2 = actualHeightCanvas - ((points[points.Count - 1].value / maxValue) * actualHeightCanvas);
            line.StrokeThickness = 2;
            points[points.Count - 1].line = line;
            canvas.Children.Add(line);
        }

        public void ControlCreateChart(Canvas canvas, ScrollViewer scroll, List<Classes.PointInfo> points,
            ref double maxValue, ref double averageValue, ref Line averageLine, Label currentLabel, Label averageLabel)
        {
            double value = points[points.Count - 1].value;
            if (value < maxValue)
                CreatePoint(canvas, points, maxValue);
            else
                CreateChart(canvas, points, ref maxValue, ref averageLine);

            ColorChart(canvas, scroll, points, maxValue, ref averageValue, ref averageLine, currentLabel, averageLabel);
        }

        public void ColorChart(Canvas canvas, ScrollViewer scroll, List<Classes.PointInfo> points,
            double maxValue, ref double averageValue, ref Line averageLine, Label currentLabel, Label averageLabel)
        {
            double value = points[points.Count - 1].value;

            averageValue = 0;
            for (int i = 0; i < points.Count; i++)
                averageValue += points[i].value;
            averageValue = averageValue / points.Count;

            double canvasWidth = points.Count * 20 + 300;
            if (averageLine != null)
                canvas.Children.Remove(averageLine);
            double avgY = actualHeightCanvas - ((averageValue / maxValue) * actualHeightCanvas);
            averageLine = new Line
            {
                X1 = 0,
                X2 = canvasWidth,
                Y1 = avgY,
                Y2 = avgY,
                Stroke = Brushes.Orange,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection { 6, 3 }
            };
            canvas.Children.Add(averageLine);

            for (int i = 0; i < points.Count; i++)
            {
                if (value < averageValue)
                    points[i].line.Stroke = Brushes.Red;
                else
                    points[i].line.Stroke = Brushes.Green;
            }

            canvas.Width = canvasWidth;
            scroll.ScrollToHorizontalOffset(canvasWidth);

            currentLabel.Content = "Тек. знач: " + Math.Round(value, 2);
            averageLabel.Content = "Сред. знач: " + Math.Round(averageValue, 2);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            actualHeightCanvas = mainWindow.Height - 50d;

            CreateChart(canvas1, mainWindow.pointsInfo, ref maxValue1, ref _averageLine1);
            ColorChart(canvas1, scroll1, mainWindow.pointsInfo, maxValue1, ref averageValue1, ref _averageLine1, current_value1, average_value1);

            CreateChart(canvas2, mainWindow.pointsInfo2, ref maxValue2, ref _averageLine2);
            ColorChart(canvas2, scroll2, mainWindow.pointsInfo2, maxValue2, ref averageValue2, ref _averageLine2, current_value2, average_value2);
        }
    }
}
