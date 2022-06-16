using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcialGrafo
{
    public partial class FArco : Form
    {
        public bool control; //variable de control
        public string dato;
        public FArco()
        {
            InitializeComponent();
            control = false;
            dato = "";
            textBox1.Focus();
            textBox1.Clear();
        }

        private void FArco_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            textBox1.Clear();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            dato = textBox1.Text.Trim();

            if ((dato == "") || (dato == " "))
            {
                MessageBox.Show("Debes ingresar un valor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                control = true;
                Hide();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            control = false;
            Hide();
        }

        private void FArco_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void FArco_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAceptar_Click(null, null);
            }
        }
    }
}
