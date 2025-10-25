using System;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto1
{
    internal class BaseDeDatos
    {
        string connectionString = @"Server=localhost;Database=Historial;TrustServerCertificate=True;Integrated Security=True;";

        public bool verificarConexion()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                
               connection.Close();
            }
        }

        public int EjecutarQuery(string query, SqlParameter[] parametros = null)
        {
            SqlConnection conexion = null;
            try
            {
                conexion = new SqlConnection(connectionString);
                conexion.Open();

                SqlCommand comando = new SqlCommand(query, conexion);
                if (parametros != null)
                {
                    comando.Parameters.AddRange(parametros);
                }

                return comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error ejecutando query: " + ex.Message);
            }
            finally
            {
              conexion.Close();
            }
        }
        public int RetornarId(string query, SqlParameter[] parametros = null)
        {
            SqlConnection conexion = null;
            try
            {
                conexion = new SqlConnection(connectionString);
                conexion.Open();

                // Agregar SELECT SCOPE_IDENTITY() al query
                query += "; SELECT SCOPE_IDENTITY();";

                SqlCommand comando = new SqlCommand(query, conexion);
                if (parametros != null)
                {
                    comando.Parameters.AddRange(parametros);
                }

                // Usar ExecuteScalar() para obtener el ID
                return Convert.ToInt32(comando.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error ejecutando query: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                    conexion.Close();
            }
        }
        public SqlDataReader EjecutarReader(string query, SqlParameter[] parametros = null)
        {
            SqlConnection conexion = new SqlConnection(connectionString);
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                if (parametros != null)
                {
                    comando.Parameters.AddRange(parametros);
                }
                return comando.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                conexion.Close();
                throw;
            }
        }
    }
}