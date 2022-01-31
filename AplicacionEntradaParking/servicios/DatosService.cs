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
        private readonly SqliteConnection conexion = new SqliteConnection("Data Source=" + Properties.Settings.Default.rutaConexionBd + "/parking.db");

        // MÉTODOS PARA AP1
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
            comando.Parameters["@idVehiculo"].Value = estacionamiento.IdVehiculo;
            comando.Parameters["@matricula"].Value = estacionamiento.Matricula;
            comando.Parameters["@entrada"].Value = estacionamiento.Entrada;
            comando.Parameters["@tipo"].Value = estacionamiento.Tipo;
            int filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
            return filasAfectadas;
        }

        // Para controlar que un mismo vehículo no tenga dos estacionamientos activos al mismo tiempo.
        public Estacionamiento GetEstacionamientoByMatricula(string matricula)
        {
            conexion.Open();
            Estacionamiento estacionamiento = null;
            SqliteCommand comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM estacionamientos WHERE matricula = @matricula";
            SqliteDataReader cursorEstacionamientos = comando.ExecuteReader();

            comando.Parameters.Add("@matricula", SqliteType.Text);
            comando.Parameters["@matricula"].Value = matricula;

            if (cursorEstacionamientos.HasRows)
            {
                cursorEstacionamientos.Read();
                estacionamiento = new Estacionamiento(cursorEstacionamientos.GetInt32(0), cursorEstacionamientos.GetInt32(1), (string)cursorEstacionamientos["matricula"],
                        (string)cursorEstacionamientos["entrada"], (string)cursorEstacionamientos["salida"], cursorEstacionamientos.GetFloat(5), (string)cursorEstacionamientos["tipo"]);
            }
            cursorEstacionamientos.Close();
            conexion.Close();
            return estacionamiento;
        }

    }
}
