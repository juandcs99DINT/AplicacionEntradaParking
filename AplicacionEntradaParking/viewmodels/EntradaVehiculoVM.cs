using AplicacionEntradaParking.modelos;
using AplicacionEntradaParking.servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionEntradaParking.viewmodels
{
    class EntradaVehiculoVM : ObservableRecipient
    {
        public EntradaVehiculoVM()
        {
            RecibirEstacionamiento();
            AceptarCommand = new RelayCommand(CerrarEntradaAlParking);
        }

        public RelayCommand AceptarCommand { get; }

        private Estacionamiento estacionamiento;
        public Estacionamiento Estacionamiento
        {
            get => estacionamiento;
            set => SetProperty(ref estacionamiento, value);
        }

        public void CerrarEntradaAlParking() => System.Windows.Application.Current.Shutdown();

        public void RecibirEstacionamiento()
        {
            Estacionamiento = WeakReferenceMessenger.Default.Send<EstacionamientoMessage>();
        }
    }
}
