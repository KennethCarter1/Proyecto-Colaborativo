using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto1
{
    public class CalculadoraDB
    {
        public void GuardarOperacion(string operacion, double resultado)
        {
            BaseDeDatos bd = new BaseDeDatos();

            if (bd.Conectar())
            {
                try
                {
                    // Insertar en operacion
                    string queryOperacion = $"INSERT INTO operacion (expresion) VALUES ('{operacion}')";
                    bd.Ejecutar(queryOperacion);

                    // Obtener el último id_operacion
                    string queryUltimoOperacion = "SELECT MAX(id_operacion) FROM operacion";
                    SqlDataReader readerOp = bd.Leer(queryUltimoOperacion);
                    int idOperacion = 0;
                    if (readerOp.Read())
                        idOperacion = readerOp.GetInt32(0);
                    readerOp.Close();


                    // Insertar en resultado
                    string queryResultado = $"INSERT INTO resultado (resultado) VALUES ({resultado})";
                    bd.Ejecutar(queryResultado);

                    // Obtener el último id_resultado
                    string queryUltimoResultado = "SELECT MAX(id_resultado) FROM resultado";
                    SqlDataReader readerRes = bd.Leer(queryUltimoResultado);
                    int idResultado = 0;
                    if (readerRes.Read())
                        idResultado = readerRes.GetInt32(0);
                    readerRes.Close();

                    // Insertar en calculadora
                    string queryCalculadora = $"INSERT INTO calculadora (id_operacion, id_resultado) VALUES ({idOperacion}, {idResultado})";
                    bd.Ejecutar(queryCalculadora);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener historial: " + ex.Message);
                }
                finally
                {
                    bd.Cerrar();
                }
            }
            else
            {
                MessageBox.Show("Error al obtener historial: ");
            }
        }

        public List<string> ObtenerHistorial()
        {
            List<string> historial = new List<string>();
            BaseDeDatos bd = new BaseDeDatos();

            if (bd.Conectar())
            {
                SqlDataReader reader = null;
                try
                {
                    string query = @"
                    SELECT o.expresion, r.resultado, c.fecha_operacion
                    FROM calculadora c, operacion o, resultado r
                    WHERE c.id_operacion = o.id_operacion
                      AND c.id_resultado = r.id_resultado
                    ORDER BY c.fecha_operacion DESC;
";

                    reader = bd.Leer(query);

                    string fechaActual = "";
                    bool primeraFila = true;

                    while (reader.Read())
                    {
                        string expresion = reader.GetString(0);
                        double resultado = reader.GetDouble(1);
                        DateTime fechaHora = reader.GetDateTime(2);
                        string fecha = fechaHora.ToString("dd/MM/yyyy");

                        // Si cambió la fecha, agregamos la fecha arriba
                        if (fecha != fechaActual || primeraFila)
                        {
                            historial.Add($"---------{fecha}----------");
                            fechaActual = fecha;
                            primeraFila = false;
                        }

                        historial.Add($"{expresion} = {resultado}");
                        historial.Add("-----------------------------------");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener historial: " + ex.Message);
                    
                }
                finally
                {
                    
                    reader.Close();
                    bd.Cerrar();
                }
            }
            else
            {
                MessageBox.Show("Error al obtener historial: ");
            }

            return historial;
        }

    }
}
