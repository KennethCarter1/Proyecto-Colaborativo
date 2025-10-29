using System;
using NCalc;
using System.Windows.Forms;

namespace Proyecto1
{
    public class Operaciones
    {
        public static  double resultadoOperacion;
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
            if (numero >= 0)
            {
                return Math.Sqrt(numero);
                
            }
            return 0;
        }

        // Método para cambiar signo
        public static double CambiarSigno(double numero)
        {
            return numero * -1;
        }

        public static Boolean CalcularOperacion(string expresion)
        {
            try
            {

                Expression e = new Expression(expresion);
                resultadoOperacion = Convert.ToDouble(e.Evaluate());
                return true;
            }
            catch
            {
                MessageBox.Show("Error en la operación");
                return false;
            }
        }

        public static double resultado(double resultado)
        {
            return resultadoOperacion;
        }

    }
}