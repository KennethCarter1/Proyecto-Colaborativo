using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Proyecto1
{
    public partial class Calculadora : Form
    {
        string ultimaOperacion = "";
        string connectionString = @"Server=localhost;Database=Historial;TrustServerCertificate=True;Integrated Security=True;";

        public Calculadora()
        {
            InitializeComponent();
            SqlConnection conexion = new SqlConnection(connectionString);
            conexion.Open();
            MessageBox.Show("Hecho por Kenneth y Ditzel Conexion exitosa a la base de datos.");
        }

        private void Calculadora_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(84, 84, 84);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    // Colores
                    if (btn == btnIgual)
                    {
                        btn.BackColor = Color.FromArgb(236, 0, 140);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(217, 217, 217);
                        btn.ForeColor = Color.FromArgb(236, 0, 140);
                    }

                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;

                    // Redondear bordes
                    int radio = 40;
                    GraphicsPath path = new GraphicsPath();
                    Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);
                    path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
                    path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
                    path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
                    path.CloseAllFigures();
                    btn.Region = new Region(path);
                }
            }
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    // Solo números y "." van al evento de agregar al TextBox
                    if (btn.Text.All(char.IsDigit) || btn.Text == "." || btn.Text == "*" || btn.Text == "/" || btn.Text == "+" || btn.Text == "-" || btn.Text == "%")
                    {
                        btn.Click += btnNumero_Click; // Asigna el evento
                    }
                }
            }
        }

        private void btnNumero_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;   // Obtiene el botón presionado
            txtpantalla.Text += btn.Text; // Agrega el texto del botón al TextBox
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtpantalla.Text.Length > 0)
                txtpantalla.Text = txtpantalla.Text.Substring(0, txtpantalla.Text.Length - 1);
        }

        private void btnBorrarElemento_Click(object sender, EventArgs e)
        {
            txtpantalla.Clear();
            lblResultado.Text = "";
        }

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            if (txtpantalla.Text.Length > 0)
                txtpantalla.Text = txtpantalla.Text.Substring(0, txtpantalla.Text.Length - 1);
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            try
            {
                string operacion = txtpantalla.Text;

                if (string.IsNullOrWhiteSpace(operacion))
                {
                    if (string.IsNullOrWhiteSpace(ultimaOperacion))
                    {
                        lblResultado.Text = "0";
                        return;
                    }
                    operacion = ultimaOperacion;
                }
                else
                {
                    ultimaOperacion = operacion;
                }

                // Reemplazar todos los % por /100 antes de calcular
                operacion = operacion.Replace("%", "/100");

                var resultado = new System.Data.DataTable().Compute(operacion, null);
                lblResultado.Text = resultado.ToString();

                // Guardar en la base de datos
                GuardarEnHistorial(operacion, resultado.ToString());

                txtpantalla.Clear();
            }
            catch
            {
                lblResultado.Text = "Error";
                txtpantalla.Clear();
            }
        }

        private void GuardarEnHistorial(string operacion, string resultadoStr)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    // Obtener el próximo ID para operacion
                    int nextOperacionId = ObtenerProximoId("operacion", "id_operacion", conexion);
                    int nextResultadoId = ObtenerProximoId("resultado", "id_resultado", conexion);
                    int nextCalculoId = ObtenerProximoId("calculadora", "id_calculo", conexion);

                    // Insertar en la tabla operacion
                    string queryOperacion = "INSERT INTO operacion (id_operacion, expresion) VALUES (@IdOperacion, @Expresion)";
                    using (SqlCommand comando = new SqlCommand(queryOperacion, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdOperacion", nextOperacionId);
                        comando.Parameters.AddWithValue("@Expresion", operacion);
                        comando.ExecuteNonQuery();
                    }

                    // Insertar en la tabla resultado
                    string queryResultado = "INSERT INTO resultado (id_resultado, resultado) VALUES (@IdResultado, @Resultado)";
                    using (SqlCommand comando = new SqlCommand(queryResultado, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdResultado", nextResultadoId);
                        comando.Parameters.AddWithValue("@Resultado", Convert.ToDouble(resultadoStr));
                        comando.ExecuteNonQuery();
                    }

                    // Insertar en la tabla calculadora - NO pasar la fecha, dejar que la BD use DEFAULT GETDATE()
                    string queryCalculadora = "INSERT INTO calculadora (id_calculo, id_operacion, id_resultado) VALUES (@IdCalculo, @IdOperacion, @IdResultado)";
                    using (SqlCommand comando = new SqlCommand(queryCalculadora, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdCalculo", nextCalculoId);
                        comando.Parameters.AddWithValue("@IdOperacion", nextOperacionId);
                        comando.Parameters.AddWithValue("@IdResultado", nextResultadoId);
                        // No pasamos fecha_operacion para que use el DEFAULT GETDATE()
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en el historial: " + ex.Message);
            }
        }

        private int ObtenerProximoId(string tabla, string columnaId, SqlConnection conexion)
        {
            string query = $"SELECT ISNULL(MAX({columnaId}), 0) + 1 FROM {tabla}";
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        private void btnMasMenos_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtpantalla.Text))
            {
                txtpantalla.Text = (-Convert.ToDouble(txtpantalla.Text)).ToString();
            }
            else if (!string.IsNullOrWhiteSpace(lblResultado.Text))
            {
                lblResultado.Text = (-Convert.ToDouble(lblResultado.Text)).ToString();
            }
        }

        private void btnElevarAlCuadrado_Click(object sender, EventArgs e)
        {
            lblResultado.Text = (Math.Pow(Convert.ToDouble(txtpantalla.Text), 2)).ToString();
            txtpantalla.Clear();
        }

        private void btnFactorial_Click(object sender, EventArgs e)
        {
            try
            {
                int numero = Convert.ToInt32(txtpantalla.Text);

                if (numero < 0)
                {
                    lblResultado.Text = "Error Factorial";
                    return;
                }

                lblResultado.Text = Factorial(numero).ToString();
                txtpantalla.Clear();
            }
            catch
            {
                txtpantalla.Text = "Error Factorial";
                txtpantalla.Clear();
            }
        }

        private int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            return n * Factorial(n - 1);
        }

        private void btnPorcentaje_Click(object sender, EventArgs e)
        {

        }

        private void btnRaizCuadrada_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Sqrt(Convert.ToDouble(txtpantalla.Text)).ToString();
            txtpantalla.Clear();
        }

        private void btnMostrarHistorial_Click(object sender, EventArgs e)
        {
            if (btnMostrarHistorial.Text == "Mostrar Historial")
            {
                panelHistorial.Visible = true;
                this.Size = new Size(620, 580);
                btnMostrarHistorial.Text = "Ocultar Historial";

                // Cargar todas las operaciones directamente
                CargarTodasLasOperaciones();
            }
            else
            {
                panelHistorial.Visible = false;
                this.Size = new Size(299, 580);
                btnMostrarHistorial.Text = "Mostrar Historial";
            }
        }

        private void CargarTodasLasOperaciones()
        {
            try
            {
                lboxcalculos.Items.Clear();

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    // Consulta para obtener todas las operaciones agrupadas por fecha
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

                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        string fechaActual = "";
                        bool primeraFila = true;

                        while (reader.Read())
                        {
                            DateTime fecha = reader.GetDateTime(0); // Fecha sin hora
                            string expresion = reader.GetString(1);
                            double resultado = reader.GetDouble(2);
                            DateTime fechaHoraCompleta = reader.GetDateTime(3); // Fecha con hora

                            string fechaStr = fecha.ToString("dd/MM/yyyy");
                            string horaStr = fechaHoraCompleta.ToString("HH:mm");

                            // Si cambió la fecha, mostrar la fecha como encabezado
                            if (fechaStr != fechaActual || primeraFila)
                            {
                                lboxcalculos.Items.Add($"{fechaStr}");
                                fechaActual = fechaStr;
                                primeraFila = false;
                            }

                            // Mostrar la operación - si la hora es 00:00, no mostrarla
                            string itemOperacion;
                            if (horaStr == "00:00" && fechaHoraCompleta.Second == 0 && fechaHoraCompleta.Minute == 0)
                            {
                                itemOperacion = $"{expresion} = {resultado}";
                            }
                            else
                            {
                                itemOperacion = $"{expresion} = {resultado}";
                            }
                            lboxcalculos.Items.Add(itemOperacion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el historial: " + ex.Message);
            }
        }

        private void lboxcalculos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opcional: Puedes mantener esta funcionalidad si quieres
            // que al hacer clic en una operación pase algo
        }
    }
}