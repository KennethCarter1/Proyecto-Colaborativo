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
                    string query = $"INSERT INTO operacion VALUES ('{operacion}')";
                    bd.Ejecutar(query);

                    // Obtener el último id_operacion
                    string queryId1 = "SELECT MAX(id_operacion) FROM operacion";
                    SqlDataReader reader = bd.Leer(queryId1);
                    int idOperacion = 0;
                    if (reader.Read())
                        idOperacion = reader.GetInt32(0);
                    reader.Close();


                    // Insertar en resultado
                    string query2 = $"INSERT INTO resultado VALUES ({resultado})";
                    bd.Ejecutar(query2);

                    // Obtener el último id_resultado
                    string queryId2 = "SELECT MAX(id_resultado) FROM resultado";
                    SqlDataReader sdr = bd.Leer(queryId2);
                    int idResultado = 0;
                    if (sdr.Read())
                        idResultado = sdr.GetInt32(0);
                    sdr.Close();

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
                MessageBox.Show("Error al obtener historial");
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
                    string query = 
                    @"
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
                        
                       
                        if (fecha != fechaActual || primeraFila)
                        {
                            historial.Add($"---------{fecha}----------");
                            fechaActual = fecha;
                            primeraFila = false;
                        }

                        historial.Add($"{expresion} = {resultado}");
                        historial.Add("----------------------------------");
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
                MessageBox.Show("Error al Conectar verifique");
            }

            return historial;
        }

    }
}
