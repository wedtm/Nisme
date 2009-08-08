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
    /// Interaction logic for NowPlayingBar.xaml
    /// </summary>
    public partial class NowPlayingBar : UserControl
    {
        public MainWindow parent;
        public NowPlayingBar()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parent.PlayNextInQueue();
        }
    }
}
