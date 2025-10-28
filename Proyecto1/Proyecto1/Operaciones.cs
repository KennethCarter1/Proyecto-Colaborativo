using System;
using NCalc;
using System.Windows.Forms;

namespace Proyecto1
{
    public class Operaciones
    {
        // Método para calcular factorial
        public static int CalcularFactorial(int numero)
        {
            if (numero < 0) {
                MessageBox.Show("el factorial no puede ser negativo");
                return 0; 

            }
                
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
            {
                MessageBox.Show("No se puede calcular la raíz de un número negativo");
                return 0;
            }



            return Math.Sqrt(numero);
        }

        // Método para cambiar signo
        public static double CambiarSigno(double numero)
        {
            return numero * -1;
        }

        public static double CalcularOperacion(string expresion)
        {
            try
            {
                Expression e = new Expression(expresion);
                return Convert.ToDouble(e.Evaluate());
            }
            catch
            {
                MessageBox.Show("Error en la operación");
                return 0;
            }
        }

    }
}