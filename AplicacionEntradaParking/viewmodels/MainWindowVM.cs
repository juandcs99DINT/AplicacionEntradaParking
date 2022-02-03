using AplicacionEntradaParking.modelos;
using AplicacionEntradaParking.servicios;
using Azure.Storage.Blobs;
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
    class MainWindowVM : ObservableObject
    {
        private readonly DatosService datosService = new DatosService();
        private readonly NavigationService navigationService = new NavigationService();
        private readonly DialogosService dialogosService = new DialogosService();
        private readonly AzureComputerVisionService computerService = new AzureComputerVisionService();
        private readonly AzureCustomVisionService customService = new AzureCustomVisionService();
        private readonly AzureBlobStorageService blobStorageService = new AzureBlobStorageService();

        public MainWindowVM()
        {
            RegistrarEnvioEstacionamiento();
            Estacionamiento = new Estacionamiento();
            EntradaVehiculoCommand = new RelayCommand(EntradaVehiculo);
        }

        public RelayCommand EntradaVehiculoCommand { get; }

        private Estacionamiento estacionamiento;
        public Estacionamiento Estacionamiento
        {
            get => estacionamiento;
            set => SetProperty(ref estacionamiento, value);
        }

        private void RegistrarEnvioEstacionamiento()
        {
            WeakReferenceMessenger.Default.Register<MainWindowVM, EstacionamientoMessage>
              (this, (r, m) =>
              {
                  m.Reply(r.Estacionamiento);
              });
        }

        public string ExaminarImagen()
        {
            string urlImagenAzure = "";
            string rutaImagen = dialogosService.DialogoOpenFile();
            if (rutaImagen.Length != 0)
            {
                BlobContainerClient blobContainerClient = blobStorageService.SubirImagenAzure(rutaImagen);
                urlImagenAzure = blobStorageService.ObtenerURLImagenAzure(blobContainerClient, rutaImagen);
            }
            return urlImagenAzure;
        }

        public void EntradaVehiculo()
        {
            string urlImagen = ExaminarImagen();
            Estacionamiento.Tipo = customService.GetTipoVehiculo(urlImagen);
            Estacionamiento.Matricula = computerService.GetMatricula(urlImagen, Estacionamiento.Tipo);
            if (datosService.GetEstacionamientoByMatricula(Estacionamiento.Matricula) == null)
            {
                Vehiculo vehiculo = datosService.GetVehiculoByMatricula(Estacionamiento.Matricula);
                Estacionamiento.Entrada = DateTime.Now.ToString();
                if (vehiculo != null)
                {
                    Estacionamiento.IdVehiculo = vehiculo.IdVehiculo;
                    datosService.IniciarEstacionamiento(Estacionamiento, true);
                }
                else
                {
                    datosService.IniciarEstacionamiento(Estacionamiento, false);
                }
                navigationService.AbrirEntradaVehiculoDialogo();
            }
            else
            {
                dialogosService.DialogoError("Ya hay vehículo dentro con la misma matrícula");
            }
        }
    }
}
