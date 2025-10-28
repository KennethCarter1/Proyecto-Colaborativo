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
        private string ultimaOperacion = "";

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

                foreach (Control ctrl in formulario.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        bool boton = (btn == btnIgual1);
                        Visual.AplicarEstiloBoton(btn, boton);
                        //Visual.Redondear(btn, 20);
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
            lblResultado.Text = "";
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
            if (btnMostrarHistorial.Text == "Mostrar Historial")
            {
                panelHistorial.Visible = true;
                formulario.Size = new Size(620, 580);
                btnMostrarHistorial.Text = "Ocultar Historial";

                mostrarDatos();
            }
            else
            {
                panelHistorial.Visible = false;
                formulario.Size = new Size(299, 580);
                btnMostrarHistorial.Text = "Mostrar Historial";
            }
        }

        public void raizCuadrada()
        {
            try
            {
                string resultado1 = Operaciones.CalcularOperacion(txtpantalla.Text).ToString();
                double numero = Convert.ToDouble(resultado1);
                double resultado = Operaciones.CalcularRaiz(numero);
                lblResultado.Text = resultado.ToString();

                // Guardar en historial
                GuardarDatos($"√{numero}", resultado.ToString());

                txtpantalla.Clear();
            }
            catch (Exception ex)
            {
                lblResultado.Text = "Error: " + ex.Message;
                txtpantalla.Clear();
            }
        }

        public void factorial()
        {
            try
            {
                string resultado1 = Operaciones.CalcularOperacion(txtpantalla.Text).ToString();
                int numero = Convert.ToInt32(resultado1);
                int resultado = Operaciones.CalcularFactorial(numero);
                lblResultado.Text = resultado.ToString();

                // Guardar en historial
                GuardarDatos($"{numero}!", resultado.ToString());

                txtpantalla.Clear();
            }
            catch (Exception ex)
            {
                lblResultado.Text = "Error: " + ex.Message;
                txtpantalla.Clear();
            }
        }

        public void cuadrado()
        {
            try
            {
                string resultado1 = Operaciones.CalcularOperacion(txtpantalla.Text).ToString();
                double numero = Convert.ToDouble(resultado1);
                double resultado = Operaciones.CalcularPotencia(numero, 2);
                lblResultado.Text = resultado.ToString();

                // Guardar en historial
                GuardarDatos($"{numero}²", resultado.ToString());

                txtpantalla.Clear();
            }
            catch (Exception ex)
            {
                lblResultado.Text = "Error: " + ex.Message;
                txtpantalla.Clear();
            }
        }


        public void cambioSigno()
        {
            if (txtpantalla.Text != null && txtpantalla.Text.Trim() != "")
            {
                double numero = Convert.ToDouble(txtpantalla.Text);
                txtpantalla.Text = Operaciones.CambiarSigno(numero).ToString();
            }
            else if (lblResultado.Text != null && lblResultado.Text.Trim() != "")
            {
                double numero = Convert.ToDouble(lblResultado.Text);
                lblResultado.Text = Operaciones.CambiarSigno(numero).ToString();
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

                if (operacion == null || operacion.Trim() == "")
                {
                    if (ultimaOperacion == null || ultimaOperacion.Trim() == "")
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


                operacion = operacion.Replace("%", "/100");

                double resultadoTotal = Operaciones.CalcularOperacion(operacion);
                try
                {
                    // Validar si es NaN o infinito
                    if (double.IsNaN(resultadoTotal) || double.IsInfinity(resultadoTotal))
                    {
                        lblResultado.Text = "Error";
                        txtpantalla.Clear();
                        return; 
                    }
                }
                catch
                {
                    lblResultado.Text = "Error";
                    txtpantalla.Clear();
                    return;
                }

                lblResultado.Text = resultadoTotal.ToString();
                GuardarDatos(operacion, resultadoTotal.ToString());
                txtpantalla.Clear();
            }
            catch
            {
                lblResultado.Text = "Error";
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
