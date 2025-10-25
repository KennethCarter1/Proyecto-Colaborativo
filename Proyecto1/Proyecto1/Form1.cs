using System;
using System.Windows.Forms;

namespace Proyecto1
{
    public partial class Calculadora : Form
    {
        private Acciones acciones;
        public Calculadora()
        {
            InitializeComponent();
            acciones = new Acciones(this, txtpantalla, lblResultado, lboxcalculos, panelHistorial, btnMostrarHistorial, btnIgual);
            acciones.cargaDatos();
        }

        private void Calculadora_Load(object sender, EventArgs e)
        {
            acciones.Inicio();
        }

        private void btnNumero_Click(object sender, EventArgs e)
        {
            acciones.detectarObjeto(sender);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            acciones.eliminarDato();
        }

        private void btnBorrarElemento_Click(object sender, EventArgs e)
        {
            acciones.borrarTodo();
        }

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            acciones.eliminarDato();
        }


        private void btnIgual_Click(object sender, EventArgs e)
        {
            acciones.mostrarResultado();
        }

        private void btnMasMenos_Click(object sender, EventArgs e)
        {
            acciones.cambioSigno();
        }

        private void btnElevarAlCuadrado_Click(object sender, EventArgs e)
        {
            acciones.cuadrado();
        }

        private void btnFactorial_Click(object sender, EventArgs e)
        {
            acciones.factorial();
        }

        private void btnRaizCuadrada_Click(object sender, EventArgs e)
        {
            acciones.raizCuadrada();
        }

        private void btnMostrarHistorial_Click(object sender, EventArgs e)
        {
            acciones.mostrarHistorial();
        }
    }
}