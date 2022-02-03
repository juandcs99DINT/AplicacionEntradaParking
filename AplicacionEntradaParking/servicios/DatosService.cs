using AplicacionEntradaParking.modelos;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionEntradaParking.servicios
{
    class DatosService
    {
        private readonly DialogosService dialogosService = new DialogosService();
        private readonly SqliteConnection conexion = new SqliteConnection("Data Source=" + Properties.Settings.Default.rutaConexionBd + "/parking.db");

        // Para controlar que no haya dos vehículos con la misma matrícula.
        public Vehiculo GetVehiculoByMatricula(string matricula)
        {
            Vehiculo vehiculo = null;
            try
            {
                conexion.Open();
                SqliteCommand comando = conexion.CreateCommand();
                comando.CommandText = "SELECT * FROM vehiculos WHERE matricula = @matricula";
                comando.Parameters.Add("@matricula", SqliteType.Text);
                comando.Parameters["@matricula"].Value = matricula;
                SqliteDataReader cursorVehiculos = comando.ExecuteReader();

                if (cursorVehiculos.HasRows)
                {

                    cursorVehiculos.Read();
                    vehiculo = new Vehiculo(cursorVehiculos.GetInt32(0), cursorVehiculos.GetInt32(1), cursorVehiculos["matricula"] != DBNull.Value ? (string)cursorVehiculos["matricula"] : "",
                          cursorVehiculos.GetInt32(3), cursorVehiculos["modelo"] != DBNull.Value ? (string)cursorVehiculos["modelo"] : "", cursorVehiculos["tipo"] != DBNull.Value ? (string)cursorVehiculos["tipo"] : "");
                }
                cursorVehiculos.Close();
                conexion.Close();
            }
            catch (Exception)
            {
                dialogosService.DialogoError("Error obteniendo el vehículo por matrícula");
            }
            return vehiculo;
        }
    
        public int IniciarEstacionamiento(Estacionamiento estacionamiento)
        {
            conexion.Open();
            SqliteCommand comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO estacionamientos (id_vehiculo, matricula, entrada, tipo) " +
                "VALUES (@idVehiculo, @matricula, @entrada, @tipo)";
            comando.Parameters.Add("@idVehiculo", SqliteType.Integer);
            comando.Parameters.Add("@matricula", SqliteType.Text);
            comando.Parameters.Add("@entrada", SqliteType.Text);
            comando.Parameters.Add("@tipo", SqliteType.Text);
            comando.Parameters["@idVehiculo"].Value = estacionamiento.IdVehiculo ?? 0;
            comando.Parameters["@matricula"].Value = estacionamiento.Matricula;
            comando.Parameters["@entrada"].Value = estacionamiento.Entrada;
            comando.Parameters["@tipo"].Value = estacionamiento.Tipo;
             int filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
            return filasAfectadas;
        }
    }
}