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
    public partial class Login : Form
    {
        string user_verificar;
        string contra_verificar;
        public Login()
        {
            InitializeComponent();
        }

        public void MenuPr()
        {
            Form1 menu = new Form1();
            menu.Show();
            this.Hide();
        }

        private void btn_salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_registrar_Click(object sender, EventArgs e)
        {
            Registro reg = new Registro();
            reg.Show();
            this.Hide();
        }

        private void btn_ingresar_Click(object sender, EventArgs e)
        {
            try
            {
                user_verificar = txb_user.Text;
                contra_verificar = txb_pass.Text;

                StreamReader leer;
                leer = File.OpenText("datos.txt");
                string cadena;
                string[] arreglo = new string[2];
                char[] separador = { '-' };
                bool autorizado = false;
                cadena = leer.ReadLine();
                while (cadena != null && autorizado == false)
                {
                    arreglo = cadena.Split(separador);
                    if (arreglo[0].Trim().Equals(user_verificar) && arreglo[1].Trim().Equals(contra_verificar))
                    {
                        MessageBox.Show("Usuario y contraseñas correctos", "Login Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MenuPr();
                        autorizado = true;
                    }
                    else
                    {
                        cadena = leer.ReadLine();
                    }
                }
                if (autorizado == false)
                {
                    MessageBox.Show("Usuario y/o contraseña incorrectos", "Login Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }

        }
    }
}
