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
using Lala.API;

namespace Nisme.Controls
{
    /// <summary>
    /// Interaction logic for Playlists.xaml
    /// </summary>
    public partial class Playlists : UserControl
    {
        public MainWindow parent;
        public Playlists()
        {
            InitializeComponent();
        }

        private void PlaylistsContainer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Lala.API.Playlist selected = (Playlist)this.PlaylistsContainer.SelectedItem;
            parent.dataGrid1.ItemsSource = selected.Songs;
            
        }
    }
}
