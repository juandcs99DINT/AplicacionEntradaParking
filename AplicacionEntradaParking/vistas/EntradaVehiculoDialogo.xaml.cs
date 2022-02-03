using AplicacionEntradaParking.viewmodels;
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

namespace AplicacionEntradaParking.vistas
{
    /// <summary>
    /// Lógica de interacción para EntradaVehiculoDialogo.xaml
    /// </summary>
    public partial class EntradaVehiculoDialogo : Window
    {
        private readonly EntradaVehiculoVM vm;
        public EntradaVehiculoDialogo()
        {
            vm = new EntradaVehiculoVM();
            this.DataContext = vm;
            InitializeComponent();
        }
        private void AceptarButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}

