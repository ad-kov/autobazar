using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using autobazar.ViewModels;

namespace autobazar.Views
{
    /// <summary>
    /// Interaction logic for VozyView.xaml
    /// </summary>
    public partial class VozyView : Window
    {
        public VozyView()
        {
            InitializeComponent();
            this.DataContext = new VozyViewModel();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
        }
    }
}
