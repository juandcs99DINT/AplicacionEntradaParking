using AplicacionEntradaParking.modelos;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionEntradaParking.servicios
{
        public class EstacionamientoMessage : RequestMessage<Estacionamiento> { }
}
