using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1
{
    public class Visual
    {
        private static Color Rosa = Color.FromArgb(236, 0, 140);
        private static Color gris = Color.FromArgb(217, 217, 217);
        private static Color blanco = Color.FromArgb(255, 255, 255);

        public static void Estilo(Button btn, bool BtnIgual)
        {
            if (BtnIgual)
            {
                btn.BackColor = Rosa;
                btn.ForeColor = blanco;
            }
            else
            {
                btn.BackColor = gris;
                btn.ForeColor = Rosa;
            }
        }

    }
}
