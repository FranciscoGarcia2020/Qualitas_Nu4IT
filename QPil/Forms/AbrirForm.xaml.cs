using System.Windows.Controls;

namespace QPil.Forms
{
    public partial class AbrirForm : UserControl
    {
        public AbrirForm()
        {
            InitializeComponent();
            Forms.escanear esc = new Forms.escanear();
            esc.TopLevel = false;
            esc.TopMost = false;
            this.Width = esc.Width * 1.44;
            this.Height = esc.Height * 1.22;
            host.Child = esc;

        }
    }
}
