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
        }

        private Estacionamiento estacionamiento;
        public Estacionamiento Estacionamiento
        {
            get => estacionamiento;
            set => SetProperty(ref estacionamiento, value);
        }

        public void RecibirEstacionamiento()
        {
            Estacionamiento = WeakReferenceMessenger.Default.Send<EstacionamientoMessage>();
        }
    }
}
