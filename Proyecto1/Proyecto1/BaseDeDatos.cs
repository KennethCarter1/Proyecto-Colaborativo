using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto1
{
    internal class BaseDeDatos
    {
        private string conectar = @"Server=localhost;
                                    Database=Historial;
                                    TrustServerCertificate=True;
                                    Integrated Security=True;
                                   ";
        private SqlConnection conexion;

        // Conectar
        public bool Conectar()
        {
            try
            {
                conexion = new SqlConnection(conectar);
                conexion.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error(Metodo Conectar) " + ex.Message);
                return false;
            }
        }

        // Cerrar conexión
        public void Cerrar()
        {
            conexion.Close();
        }

        // Ejecutar comandos
        public void Ejecutar(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error(Metodo Ejecutar): " + ex.Message);
               
            }

        }

        // Ejecutar SELECT
        public SqlDataReader Leer(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conexion);
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error (Metodo Leer): " + ex.Message);
                return null;
            }
        }
    }
}
