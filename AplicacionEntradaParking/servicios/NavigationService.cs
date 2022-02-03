using AplicacionEntradaParking.vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionEntradaParking.servicios
{
    class NavigationService
    {
        public void AbrirEntradaVehiculoDialogo()
        {
            EntradaVehiculoDialogo entradaVehiculoDialogo = new EntradaVehiculoDialogo();
            entradaVehiculoDialogo.ShowDialog();
        }
    }
}
