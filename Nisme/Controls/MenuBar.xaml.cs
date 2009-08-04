using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nisme.Controls
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MainWindow parent;
        public Boolean IsFullscreen = false;
        public MenuBar()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsFullscreen)
            {
                parent.WindowState = WindowState.Normal;
                parent.WindowStyle = WindowStyle.SingleBorderWindow;
                IsFullscreen = false;
            }
            else
            {
                parent.WindowStyle = WindowStyle.None;
                parent.WindowState = WindowState.Maximized;
                IsFullscreen = true;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas me = (Canvas)sender;
            foreach (UIElement el in me.Children)
            {
                Path p = (Path)el;
                p.Fill = Brushes.White;
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
