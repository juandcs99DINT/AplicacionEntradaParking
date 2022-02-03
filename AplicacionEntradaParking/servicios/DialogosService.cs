using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AplicacionEntradaParking.servicios
{
    class DialogosService
    {
        public DialogosService() { }

        public string DialogoOpenFile()
        {
            string fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            return fileName;
        }
        public void DialogoError(string mensaje)
        {
            MessageBox.Show(mensaje, "Parking", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void DialogoInformacion(string mensaje)
        {
            MessageBox.Show(mensaje, "Parking", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
