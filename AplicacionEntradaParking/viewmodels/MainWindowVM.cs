using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionEntradaParking.viewmodels
{
    class MainWindowVM : ObservableObject
    {
        public MainWindowVM()
        {
            EntradaVehiculoCommand = new RelayCommand(EntradaVehiculo);
        }

        private RelayCommand EntradaVehiculoCommand { get; }


        public void EntradaVehiculo()
        {

        }
    }
}
