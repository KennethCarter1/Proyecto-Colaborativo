using System;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

namespace Proyecto1
{
    public class Operaciones
    {
        // Método para calcular factorial
        public static int CalcularFactorial(int numero)
        {
            if (numero < 0)
                MessageBox.Show("el factorial no puede ser negativo");
            if (numero <= 1)
                return 1;

            return numero * CalcularFactorial(numero - 1);
        }

        // Método para calcular potencia
        public static double CalcularPotencia(double baseA, double exponente)
        {
            return Math.Pow(baseA, exponente);
        }

        // Método para calcular raíz cuadrada
        public static double CalcularRaiz(double numero)
        {
            if (numero < 0)
                MessageBox.Show("No se puede calcular la raíz de un número negativo");

            return Math.Sqrt(numero);
        }

        // Método para cambiar signo
        public static double CambiarSigno(double numero)
        {
            return -numero;
        }
    }
}