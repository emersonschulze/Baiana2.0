using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baiana20
{
    public class BarraDeProgresso : ProgressBar

    {
        public BarraDeProgresso()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        public Brush Color { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Color == null)
                Color = Brushes.Green;

            Rectangle rec = e.ClipRectangle;

            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);

            rec.Width = (int) (rec.Width*((double) Value/Maximum)) - 4;
            rec.Height = rec.Height - 4;
            e.Graphics.FillRectangle(Color, 2, 2, rec.Width, rec.Height);
        }
    }
}
