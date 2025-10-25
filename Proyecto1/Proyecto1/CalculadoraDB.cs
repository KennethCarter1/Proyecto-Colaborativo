using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Proyecto1
{
    public class CalculadoraDB
    {
        public void GuardarOperacion(string operacion, double resultado)
        {
            try
            {
                BaseDeDatos bd = new BaseDeDatos();

                if (bd.verificarConexion())
                {
                    // Insertar en operacion y obtener el ID
                    string queryOperacion = "INSERT INTO operacion (expresion) VALUES (@Expresion)";
                    int idOperacion = bd.RetornarId(queryOperacion, new SqlParameter[]
                    {
                        new SqlParameter("@Expresion", operacion)
                    });

                    // Insertar en resultado y obtener el ID
                    string queryResultado = "INSERT INTO resultado (resultado) VALUES (@Resultado)";
                    int idResultado = bd.RetornarId(queryResultado, new SqlParameter[]
                    {
                         new SqlParameter("@Resultado", resultado)
                    });

                    // Insertar en calculadora
                    string queryCalculadora = "INSERT INTO calculadora (id_operacion, id_resultado) VALUES (@IdOperacion, @IdResultado)";
                    bd.EjecutarQuery(queryCalculadora, new SqlParameter[] {
                        new SqlParameter("@IdOperacion", idOperacion),
                        new SqlParameter("@IdResultado", idResultado)
                    });
                }
                else
                {
                    throw new Exception("No se pudo conectar a la base de datos");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar operación: " + ex.Message);
            }
        }

        public List<string> ObtenerHistorial()
        {
            List<string> historial = new List<string>();
            SqlDataReader reader = null;

            try
            {
                BaseDeDatos bd = new BaseDeDatos();
                if (!bd.verificarConexion())
                {
                    throw new Exception("No hay conexión a la base de datos");
                }

                string query = @"
            SELECT 
                CONVERT(DATE, c.fecha_operacion) as Fecha,
                o.expresion, 
                r.resultado,
                c.fecha_operacion
            FROM calculadora c
            INNER JOIN operacion o ON c.id_operacion = o.id_operacion
            INNER JOIN resultado r ON c.id_resultado = r.id_resultado
            ORDER BY c.fecha_operacion DESC";

                reader = bd.EjecutarReader(query);

                string fechaActual = "";
                bool primeraFila = true;

                while (reader.Read())
                {
                    DateTime fecha = reader.GetDateTime(0);
                    string expresion = reader.GetString(1);
                    double resultado = reader.GetDouble(2);
                    DateTime fechaHoraCompleta = reader.GetDateTime(3);

                    string fechaStr = fecha.ToString("dd/MM/yyyy");
                    string horaStr = fechaHoraCompleta.ToString("HH:mm");

                    if (fechaStr != fechaActual || primeraFila)
                    {
                        historial.Add($"{fechaStr}");
                        fechaActual = fechaStr;
                        primeraFila = false;
                    }

                    string itemOperacion = $"{expresion} = {resultado}";
                    historial.Add(itemOperacion);
                }

                return historial;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener historial: " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}