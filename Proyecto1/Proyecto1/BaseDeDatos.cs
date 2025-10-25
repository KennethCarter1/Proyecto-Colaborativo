using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto1
{
    internal class BaseDeDatos
    {
        private string connectionString = @"Server=localhost;Database=Historial;TrustServerCertificate=True;Integrated Security=True;";
        private SqlConnection conexion;

        // Conectar
        public bool Conectar()
        {
            try
            {
                conexion = new SqlConnection(connectionString);
                conexion.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cerrar conexión
        public void Cerrar()
        {
            if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
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
                MessageBox.Show("Error al obtener historial: " + ex.Message);
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
                MessageBox.Show("Error al obtener historial: " + ex.Message);
                return null;
            }
        }
    }
}
