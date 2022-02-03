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

        public int IniciarEstacionamiento(Estacionamiento estacionamiento, bool vehiculoAsociado)
        {
            int filasAfectadas = 0;
            try
            {
                conexion.Open();
                SqliteCommand comando = conexion.CreateCommand();

                comando.CommandText = vehiculoAsociado ? "INSERT INTO estacionamientos (id_vehiculo, matricula, entrada, tipo) " +
                    "VALUES (@idVehiculo, @matricula, @entrada, @tipo)" : 
                    "INSERT INTO estacionamientos (matricula, entrada, tipo) VALUES (@matricula, @entrada, @tipo)";
                comando.Parameters.Add("@matricula", SqliteType.Text);
                comando.Parameters.Add("@entrada", SqliteType.Text);
                comando.Parameters.Add("@tipo", SqliteType.Text);
                if(vehiculoAsociado)
                {
                    comando.Parameters.Add("@idVehiculo", SqliteType.Integer);
                    comando.Parameters["@idVehiculo"].Value = estacionamiento.IdVehiculo;
                }
                comando.Parameters["@matricula"].Value = estacionamiento.Matricula;
                comando.Parameters["@entrada"].Value = estacionamiento.Entrada;
                comando.Parameters["@tipo"].Value = estacionamiento.Tipo;
                filasAfectadas = comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception)
            {
                dialogosService.DialogoError("Se ha producido un error insertado el nuevo estacionamiento en la BD.");
            }
            return filasAfectadas;
        }
        public Estacionamiento GetEstacionamientoByMatricula(string matricula)
        {
            Estacionamiento estacionamiento = null;
            try
            {
                conexion.Open();
                SqliteCommand comando = conexion.CreateCommand();
                comando.CommandText = "SELECT * FROM estacionamientos WHERE matricula = @matricula";
                comando.Parameters.Add("@matricula", SqliteType.Text);
                comando.Parameters["@matricula"].Value = matricula;
                SqliteDataReader cursorEstacionamientos = comando.ExecuteReader();
                if (cursorEstacionamientos.HasRows)
                {
                    cursorEstacionamientos.Read();
                    if (!cursorEstacionamientos.IsDBNull(1))
                    {
                        estacionamiento = new Estacionamiento(cursorEstacionamientos.GetInt32(0), cursorEstacionamientos.GetInt32(1), cursorEstacionamientos["matricula"] != DBNull.Value ? (string)cursorEstacionamientos["matricula"] : "",
                            cursorEstacionamientos["entrada"] != DBNull.Value ? (string)cursorEstacionamientos["entrada"] : "",
                            cursorEstacionamientos["tipo"] != DBNull.Value ? (string)cursorEstacionamientos["tipo"] : "");
                    }
                    else
                    {
                        estacionamiento = new Estacionamiento(cursorEstacionamientos.GetInt32(0), cursorEstacionamientos["matricula"] != DBNull.Value ? (string)cursorEstacionamientos["matricula"] : "",
                            cursorEstacionamientos["entrada"] != DBNull.Value ? (string)cursorEstacionamientos["entrada"] : "",
                            cursorEstacionamientos["tipo"] != DBNull.Value ? (string)cursorEstacionamientos["tipo"] : "");
                    }
                    cursorEstacionamientos.Close();
                    conexion.Close();
                }
            }
            catch (Exception e)
            {
                dialogosService.DialogoError(e.Message);
            }
            return estacionamiento;
        }
        public int GetCantEstacionamientosTipo(string tipo)
        {
            int cantidad = 0;
            try
            {
                conexion.Open();
                SqliteCommand comando = conexion.CreateCommand();
                comando.CommandText = "SELECT * FROM estacionamientos WHERE tipo = @tipo AND salida IS NULL";
                comando.Parameters.Add("@tipo", SqliteType.Text);
                comando.Parameters["@tipo"].Value = tipo;
                SqliteDataReader cursorEstacionamientos = comando.ExecuteReader();
                if (cursorEstacionamientos.HasRows)
                {
                    while (cursorEstacionamientos.Read())
                    {
                        cantidad++;
                    }
                }
                cursorEstacionamientos.Close();
                conexion.Close();
            }
            catch (Exception e)
            {
                dialogosService.DialogoError(e.Message);
            }
            return cantidad;
        }
    }
}