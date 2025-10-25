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
            if (bd.verificarConexion())
                MessageBox.Show("Hecho por Kenneth y Ditzel. Conexión exitosa a la base de datos.");
            else
                MessageBox.Show("Hecho por Kenneth y Ditzel. No se pudo conectar a la base de datos.");
        }
        private void btnNumero_Click(object sender, EventArgs e)
        {
            detectarObjeto(sender);
        }

        public void Inicio()
        {
            {
                formulario.BackColor = Color.FromArgb(84, 84, 84);

                foreach (Control ctrl in formulario.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        bool esBtnIgual = (btn == btnIgual1);
                        Visual.AplicarEstiloBoton(btn, esBtnIgual);
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
                MessageBox.Show("Error al cargar el historial: " + ex.Message);
            }
        }

        public void mostrarHistorial()
        {
            if (btnMostrarHistorial.Text == "Mostrar Historial")
            {
                panelHistorial.Visible = true;
                formulario.Size = new Size(620, 580);
                btnMostrarHistorial.Text = "Ocultar Historial";

                CargarTodasLasOperaciones();
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
                double numero = Convert.ToDouble(txtpantalla.Text);
                lblResultado.Text = Operaciones.CalcularRaiz(numero).ToString();
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
                int numero = Convert.ToInt32(txtpantalla.Text);
                lblResultado.Text = Operaciones.CalcularFactorial(numero).ToString();
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
            double numero = Convert.ToDouble(txtpantalla.Text);
            lblResultado.Text = Operaciones.CalcularPotencia(numero, 2).ToString();
            txtpantalla.Clear();
        }

        public void cambioSigno()
        {
            if (!string.IsNullOrWhiteSpace(txtpantalla.Text))
            {
                double numero = Convert.ToDouble(txtpantalla.Text);
                txtpantalla.Text = Operaciones.CambiarSigno(numero).ToString();
            }
            else if (!string.IsNullOrWhiteSpace(lblResultado.Text))
            {
                double numero = Convert.ToDouble(lblResultado.Text);
                lblResultado.Text = Operaciones.CambiarSigno(numero).ToString();
            }
        }

        private void GuardarEnHistorial(string operacion, string resultadoStr)
        {
            try
            {
                CalculadoraDB db = new CalculadoraDB();
                db.GuardarOperacion(operacion, Convert.ToDouble(resultadoStr));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en el historial: " + ex.Message);
            }
        }

        public void mostrarResultado()
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

                operacion = operacion.Replace("%", "/100");

                var resultado = new System.Data.DataTable().Compute(operacion, null);
                lblResultado.Text = resultado.ToString();

                GuardarEnHistorial(operacion, resultado.ToString());

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

        private void CargarTodasLasOperaciones()
        {
            mostrarDatos();
        }

    }
}
