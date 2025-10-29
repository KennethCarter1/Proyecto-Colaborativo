using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto1
{
    public class Acciones
    {

        private TextBox txtpantalla;
        private Label lblResultado;
        private ListBox lboxcalculos;
        private Panel panelHistorial;
        private Button btnMostrarHistorial;
        private Button btnIgual1;
        private Form formulario;

        public Acciones(Form form, TextBox txt, Label lbl, ListBox lbox, Panel panel, Button btnHistorial, Button btnIgual)
        {

            formulario = form;
            txtpantalla = txt;
            lblResultado = lbl;
            lboxcalculos = lbox;
            panelHistorial = panel;
            btnMostrarHistorial = btnHistorial;
            btnIgual1 = btnIgual;
        }

        public void cargaDatos()
        {
            BaseDeDatos bd = new BaseDeDatos();
            if (bd.Conectar())
                MessageBox.Show("Hecho por Kenneth y Ditzel. Conexión exitosa a la base de datos.");
            else
                MessageBox.Show("Hecho por Kenneth y Ditzel. No se pudo conectar a la base de datos.");
        }


        public void Inicio()
        {
            {
                formulario.BackColor = Color.FromArgb(84, 84, 84);
                lboxcalculos.HorizontalScrollbar = true;

                foreach (Control ctrl in formulario.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        bool boton = (btn == btnIgual1);
                        Visual.Estilo(btn, boton);
                    }
                }

                foreach (Control ctrl in formulario.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        if (btn.Text.All(char.IsDigit) || btn.Text == "." || btn.Text == "*" ||
                            btn.Text == "/" || btn.Text == "+" || btn.Text == "-" || btn.Text == "%")
                        {
                            btn.Click += btnNumero_Click;

                        }
                    }
                }
            }
        }

        private void btnNumero_Click(object sender, EventArgs e)
        {
            detectarObjeto(sender);
        }

        public void eliminarDato()
        {
            if (txtpantalla.Text.Length > 0)
                txtpantalla.Text = txtpantalla.Text.Substring(0, txtpantalla.Text.Length - 1);
        }

        public void borrarTodo()
        {
            txtpantalla.Clear();
            MessageBox.Show("Error");
        }

        public void mostrarDatos()
        {
            try
            {

                lboxcalculos.Items.Clear();

                CalculadoraDB db = new CalculadoraDB();
                List<string> historial = db.ObtenerHistorial();

                foreach (string item in historial)
                {
                    lboxcalculos.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los datos: " + ex.Message);
            }
        }

        public void mostrarHistorial()
        {
            if (btnMostrarHistorial.Text == "Mostrar Calculos")

            {
                panelHistorial.Visible = true;
                formulario.Size = new Size(620, 580);
                btnMostrarHistorial.Text = "Ocultar Calculos";

                mostrarDatos();
            }
            else
            {
                panelHistorial.Visible = false;
                formulario.Size = new Size(299, 580);
                btnMostrarHistorial.Text = "Mostrar Calculos";
            }
        }

        public void raizCuadrada()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtpantalla.Text))
                {
                    MessageBox.Show("Error espacio vacio");
                    txtpantalla.Clear();
                    return;
                }

                bool calcular = Operaciones.CalcularOperacion(txtpantalla.Text);
                if (!calcular)
                {
                    MessageBox.Show("Error en operacion");
                    txtpantalla.Clear();
                    return;
                }

                double numero = Operaciones.resultadoOperacion;
                if (numero >= 0)
                {
                    double resultado = Operaciones.CalcularRaiz(numero);

                    lblResultado.Text = resultado.ToString();
                    GuardarDatos($"√{numero}", resultado.ToString());
                    txtpantalla.Clear();
                }
                else
                {
                    MessageBox.Show("Error");
                    txtpantalla.Clear();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Error");
                txtpantalla.Clear();
            }
        }




        public void factorial()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtpantalla.Text))
                {
                    MessageBox.Show("Error espacio vacio");
                    txtpantalla.Clear();
                    return;
                }

                bool calcular = Operaciones.CalcularOperacion(txtpantalla.Text);
                if (!calcular)
                {
                    MessageBox.Show("Error");
                    txtpantalla.Clear();
                    return;
                }

                double numero = Operaciones.resultadoOperacion;

                if (numero % 1 != 0)
                {
                    MessageBox.Show("Solo se permiten números enteros.");
                    txtpantalla.Clear();
                    return;
                }

                int valor = (int)numero;

                if (valor < 0)
                {
                    MessageBox.Show("El factorial no puede ser negativo.");
                    txtpantalla.Clear();
                    return;
                }

                int resultado = Operaciones.CalcularFactorial(valor);
                lblResultado.Text = resultado.ToString();
                GuardarDatos($"{valor}!", resultado.ToString());
                txtpantalla.Clear();
            }
            catch
            {
                MessageBox.Show("Error");
                txtpantalla.Clear();
            }
        }



        public void cuadrado()
        {
            try
            {
                bool calcular = Operaciones.CalcularOperacion(txtpantalla.Text);
                if (!calcular)
                {
                    MessageBox.Show("Error");
                    txtpantalla.Clear();
                    return;
                }

                double numero = Operaciones.resultadoOperacion;
                double resultado = Operaciones.CalcularPotencia(numero, 2);

                lblResultado.Text = resultado.ToString();
                GuardarDatos($"{numero}²", resultado.ToString());
                txtpantalla.Clear();
            }
            catch
            {
                MessageBox.Show("Error");
                txtpantalla.Clear();
            }
        }



        public void cambioSigno()
        {
            try
            {
                double numero = Convert.ToDouble(txtpantalla.Text);
                txtpantalla.Text = Operaciones.CambiarSigno(numero).ToString();
            }
            catch(Exception)
            {
                MessageBox.Show("Error al cambiar de signo");
            }

        }

        private void GuardarDatos(string operacion, string resultado)
        {
            try
            {
                CalculadoraDB db = new CalculadoraDB();
                db.GuardarOperacion(operacion, Convert.ToDouble(resultado));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message);
            }
        }

        public void mostrarResultado()
        {
            try
            {
                string operacion = txtpantalla.Text;

                // Si no hay nada en pantalla
                if (string.IsNullOrWhiteSpace(operacion))
                {
                    MessageBox.Show("Error");
                    txtpantalla.Clear();
                    return;
                }

                operacion = operacion.Replace("%", "/100");

                bool verificacion = Operaciones.CalcularOperacion(operacion);

                if (verificacion)
                {
                    double resultadoTotal = Operaciones.resultadoOperacion;

                    // Validar NaN o infinito
                    if (double.IsNaN(resultadoTotal) || double.IsInfinity(resultadoTotal))
                    {
                        MessageBox.Show("Error");
                        txtpantalla.Clear();
                        return;
                    }

                    lblResultado.Text = resultadoTotal.ToString();
                    if (operacion.Contains("+") || operacion.Contains("-") || operacion.Contains("*") || operacion.Contains("/"))
                    {
                        GuardarDatos(operacion, resultadoTotal.ToString());
                    }
                    
                    txtpantalla.Clear();
                }
                else
                {
                    MessageBox.Show("Error");
                    txtpantalla.Clear();
                }
            }
            catch
            {
                MessageBox.Show("Error");
                txtpantalla.Clear();
            }
        }



        public void detectarObjeto(Object sender)
        {
            Button btn = (Button)sender;
            txtpantalla.Text += btn.Text;
        }

    }
}
