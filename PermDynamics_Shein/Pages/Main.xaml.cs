using System;
using System.Windows;
using System.Windows.Controls;

namespace PermDynamics_Shein.Pages
{
    public partial class Main : Page
    {
        public MainWindow mainWindow;
        public Main(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void OpenPageChart(object sender, RoutedEventArgs e)
        {
            float value = Convert.ToInt32(tb_value.Text);
            float value2 = Convert.ToInt32(tb_value2.Text);
            mainWindow.pointsInfo.Add(new Classes.PointInfo(value));
            mainWindow.pointsInfo2.Add(new Classes.PointInfo(value2));
            mainWindow.OpenPages(MainWindow.pages.chart);
        }
    }
}
