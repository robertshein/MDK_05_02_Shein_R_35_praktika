using System.Collections.Generic;
using System.Windows;

namespace PermDynamics_Shein
{
    public partial class MainWindow : Window
    {
        public List<Classes.PointInfo> pointsInfo = new List<Classes.PointInfo>();
        public List<Classes.PointInfo> pointsInfo2 = new List<Classes.PointInfo>();
        public MainWindow()
        {
            InitializeComponent();
            OpenPages(pages.main);
        }

        public enum pages
        {
            main,
            chart
        }

        public void OpenPages(pages _pages)
        {
            if (_pages == pages.main)
                frame.Navigate(new Pages.Main(this));
            else if (_pages == pages.chart)
            {
                frame.Navigate(new Pages.Chart(this));
            }
        }
    }
}
