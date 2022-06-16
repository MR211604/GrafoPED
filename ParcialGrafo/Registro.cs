using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ParcialGrafo
{
    public partial class Registro : Form
    {
        public Registro()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btn_volver_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }

        private void btn_registrar_Click(object sender, EventArgs e)
        {
            if (txb_pass.Text == txb_rpass.Text) CuentasRepetidas();
            else
            {
                MessageBox.Show("Contraseñas no coinciden", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CuentasRepetidas()
        {
            string userReg = txb_user.Text;
            StreamReader leer;
            leer = File.OpenText("datos.txt");
            string cadena;
            string[] arreglos = new string[1]; // Crea arreglos (secciones)
            char[] separador = { '-' }; // Crea el separador entre user y password
            bool repetido = false;
            cadena = leer.ReadLine();
            while (cadena != null && repetido == false)
            {
                arreglos = cadena.Split(separador);
                if (arreglos[0].Trim().Equals(userReg))
                {
                    MessageBox.Show("Usuario ya registrado. Ingrese otro", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    leer.Close();
                    repetido = true;
                }
                else
                {
                    cadena = leer.ReadLine();
                }
            }
            if (repetido == false)
            {
                leer.Close();
                RegistroCuenta();
            }
        }

        public void RegistroCuenta()
        {
            string userReg = txb_user.Text;
            string passReg = txb_pass.Text;
            StreamWriter registrar = new StreamWriter("datos.txt", true);
            registrar.WriteLine(userReg + "-" + passReg + "\n");
            registrar.Close();
            MessageBox.Show("Usuario registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
