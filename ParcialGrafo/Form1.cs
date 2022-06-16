using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace ParcialGrafo
{
    public partial class Form1 : Form
    {
        private CGrafo grafo;
        private CVertice nuevoNodo;
        private CVertice NodoOrigen;
        private CVertice NodoDestino;
        private CVertice NodoEliminar;
        private CVertice NdoArco;
        private int var_control = 0;
        private FVertice ventanaVertice;
        private FArco ventanaArco;
        public static Boolean dirigido = true;

        public Form1()
        {
            InitializeComponent();
            resetear();
        }
        public void resetear()
        {
            grafo = new CGrafo();
            nuevoNodo = null;
            var_control = 0;
            ventanaVertice = new FVertice();
            ventanaArco = new FArco();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            rbDirigido.Checked = true;
        }

        private void Pizarra_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                grafo.DibujarGrafo(e.Graphics);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Pizarra_MouseLeave(object sender, EventArgs e)
        {
            Pizarra.Refresh();
        }

        private void Pizarra_MouseUp(object sender, MouseEventArgs e)
        {
            switch (var_control)//con un switch determinamos el que accion realizaremos tomando como referencia la variable de control
            {
                case 1: //Dibuja arco
                    if ((NodoDestino = grafo.DetectarPunto(e.Location)) != null && NodoOrigen != NodoDestino)
                    {
                        ventanaArco.Visible = false;
                        ventanaArco.control = false;
                        ventanaArco.ShowDialog();
                        int peso = 0;
                        if (ventanaArco.control)
                        {
                            peso = int.Parse(ventanaArco.textBox1.Text);
                            if (grafo.AgregarArco(NodoOrigen, NodoDestino,peso))//se procede a crear aristas
                            {
                                NodoOrigen.ListaAdyacencia.Find(v => v.nDestino == NodoDestino).peso = int.Parse(ventanaArco.textBox1.Text);
                            }
                        }
                    }//luego de dibujar el nodo regresamos todos los valores a null para poder ingresar un nuevo nodo
                    var_control = 0;
                    NodoOrigen = null;
                    NodoDestino = null;

                    Pizarra.Refresh();
                    break;
            }
        }

        private void Pizarra_MouseMove(object sender, MouseEventArgs e)
        {
            switch (var_control)
            {
                case 1:  //Dibujar arco
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(4, 4, true);
                    bigArrow.BaseCap = System.Drawing.Drawing2D.LineCap.Triangle;
                    Pizarra.Refresh();
                    if (rbDirigido.Checked)
                    {
                        Pizarra.CreateGraphics().DrawLine(new Pen(Brushes.Violet, 2) { CustomEndCap = bigArrow },
                        NodoOrigen.Posicion, e.Location);
                    }
                    else
                    {
                        Pizarra.CreateGraphics().DrawLine(new Pen(Brushes.Violet, 2),
                            NodoOrigen.Posicion, e.Location);
                    }
                    break;

                case 2: //Crea nuevo nodo
                    if (nuevoNodo != null)
                    {
                        int posX = e.Location.X;
                        int posY = e.Location.Y;

                        if (posX < nuevoNodo.Dimensiones.Width / 2)
                            posX = nuevoNodo.Dimensiones.Width / 2;
                        else if (posX > Pizarra.Size.Width - nuevoNodo.Dimensiones.Width / 2)
                            posX = Pizarra.Size.Width - nuevoNodo.Dimensiones.Width / 2;

                        if (posY < nuevoNodo.Dimensiones.Height / 2)
                            posY = nuevoNodo.Dimensiones.Height / 2;
                        else if (posY > Pizarra.Size.Height - nuevoNodo.Dimensiones.Width / 2)
                            posY = Pizarra.Size.Height - nuevoNodo.Dimensiones.Width / 2;

                        nuevoNodo.Posicion = new Point(posX, posY);
                        Pizarra.Refresh();
                        nuevoNodo.DibujarVertice(Pizarra.CreateGraphics());
                    }
                    break;
                case 3:
                    {
                        if (NodoOrigen == null)
                        {
                            NodoOrigen = grafo.DetectarPunto(e.Location);
                        }
                        else if (NodoDestino == null)
                        {
                            if ((NodoDestino = grafo.DetectarPunto(e.Location)) != null)
                            {
                                if (NodoOrigen != NodoDestino)
                                {
                                    if (!grafo.EliminarArista(NodoOrigen, NodoDestino))
                                        grafo.EliminarArista(NodoOrigen, NodoDestino);
                                    Pizarra.Refresh();
                                }

                                var_control = 0;
                                NodoOrigen = null;
                                NodoDestino = null;
                            }
                        }
                        break;
                    }
            }
        }

        private void Pizarra_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)//si se presiona el boton izquierdo del mouse
            {
                if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                {
                    var_control = 1;
                }
                if (nuevoNodo != null && NodoOrigen == null)
                {
                    ventanaVertice.Visible = false;
                    ventanaVertice.control = false;
                    grafo.AgregarVertice(nuevoNodo);
                    ventanaVertice.ShowDialog();

                    if (ventanaVertice.control)
                    {
                        if (grafo.BuscarVertice(ventanaVertice.textBox1.Text) == null)
                        {
                            nuevoNodo.Valor = ventanaVertice.textBox1.Text;
                        }
                        else
                        {
                            MessageBox.Show("El Nodo " + ventanaVertice.textBox1.Text + " ya existe en el grafo", "Error nuevo Nodo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            grafo.EliminarVertice("");
                        }
                    }
                    else
                    {
                        grafo.EliminarVertice("");
                    }
                    nuevoNodo = null;
                    var_control = 0;
                    Pizarra.Refresh();
                }
                if (NodoEliminar != null)
                {
                    NodoEliminar = grafo.DetectarPunto(e.Location);
                    grafo.EliminarVertice(NodoEliminar);
                    grafo.EliminarArco(NodoEliminar);

                    NodoEliminar = null;
                    var_control = 0;
                    Pizarra.Refresh();
                }

                if (NdoArco != null)
                {
                    NdoArco = grafo.DetectarPunto(e.Location);
                    grafo.EliminarArco(NdoArco);

                    NodoEliminar = null;
                    var_control = 0;
                    Pizarra.Refresh();
                }

            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)//si se presiona el boton derechod el mouse
            {
                if (var_control == 0)
                {
                    if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                    {
                        CMSCrearVertice.Text = "Nodo" + NodoOrigen.Valor;
                    }
                    else
                        Pizarra.ContextMenuStrip = this.CMSCrearVertice;
                }
            }
        }

        private void btnAnchura_Click(object sender, EventArgs e)//COLA
        {
            if (txtRecorrido.Text != "")
            {
                try
                {
                    List<CVertice> resultado = new List<CVertice>();
                    Refresh();
                    Queue<CVertice> miCola = new Queue<CVertice>();
                    listBox1.Items.Clear();
                    CVertice nodo = grafo.BuscarVertice(txtRecorrido.Text);

                    if (nodo != null)
                    {
                        miCola.Enqueue(nodo);
                        while (miCola.Count != 0)
                        {
                            nodo = miCola.Dequeue();
                            if (!resultado.Contains(nodo))
                            {
                                resultado.Add(nodo);
                            }
                            foreach (CArco arco in nodo.ListaAdyacencia)
                            {
                                if (!resultado.Contains(arco.nDestino))
                                {
                                    miCola.Enqueue(arco.nDestino);
                                }
                            }
                        }
                        int sumarcos = 0, o, d;
                        List<CVertice> listnodos = new List<CVertice>();
                        foreach (CVertice node in resultado)//indextar lista para dsps recorrer nodosadya(origen,destino)=peso
                        {
                            listBox1.Items.Add(node);
                            //*********
                            listnodos.Add(node);//creo una nueva lista con los nodos                       
                                                //**********
                            grafo.ColorearNodo(node.Valor);
                            Refresh();
                            Thread.Sleep(500);
                            grafo.ReestablecerColorNodo(node.Valor);
                            Refresh();
                            Thread.Sleep(500);
                        }
                        listBox1.Items.Add("");
                        for (int i = 1; i < listnodos.Count; i++)
                        {
                            o = grafo.returnindice(listnodos[i - 1]);
                            d = grafo.returnindice(listnodos[i]);
                            sumarcos += grafo.nodosadya[o, d];
                        }
                        listBox1.Items.Add("Distancia: " + sumarcos);
                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("No existe un nodo con este nombre", "Nodo incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                catch
                {
                    MessageBox.Show("Error al realizar recorrido");
                }
            }
            else
                MessageBox.Show("Ingrese un nodo de inicio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnProfundida_Click(object sender, EventArgs e)//PILA
        {
            if (txtRecorrido.Text != "")
            {
                try
                {
                    List<CVertice> resultado = new List<CVertice>();
                    Refresh();
                    Stack<CVertice> miPila = new Stack<CVertice>();
                    listBox1.Items.Clear();
                    CVertice nodo = grafo.BuscarVertice(txtRecorrido.Text);

                    if (nodo != null)
                    {
                        miPila.Push(nodo);
                        while (miPila.Count != 0)
                        {
                            nodo = miPila.Pop();
                            if (!resultado.Contains(nodo))
                            {
                                resultado.Add(nodo);
                            }
                            foreach (CArco arco in nodo.ListaAdyacencia)
                            {
                                if (!resultado.Contains(arco.nDestino))
                                {
                                    miPila.Push(arco.nDestino);
                                }
                            }
                        }

                        foreach (CVertice node in resultado)
                        {
                            listBox1.Items.Add(node);
                            grafo.ColorearNodo(node.Valor);
                            Refresh();
                            Thread.Sleep(500);
                            grafo.ReestablecerColorNodo(node.Valor);
                            Refresh();
                            Thread.Sleep(500);
                        }

                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("No existe un nodo con este nombre", "Nodo incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                catch
                {
                    MessageBox.Show("Error al mostrar recorrido");
                }
            }
            else
                 MessageBox.Show("Ingrese un nodo de inicio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text != "")
            {
                try
                {
                    string color = grafo.ColorearNodoTest(txtBuscar.Text);
                   
                    if (color != "No existe")
                    {

                        Refresh();
                        Thread.Sleep(1000);
                        grafo.ReestablecerColorNodo(txtBuscar.Text);
                        Refresh();
                        Thread.Sleep(500);
                    }
                    else
                    {
                        MessageBox.Show("No existe un nodo con este nombre", "Nodo incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch
                {
                    MessageBox.Show("Nodo no encontrado");
                }
            }
            else
                MessageBox.Show("Ingrese un valor de busqueda");
        }

        private void nuevoVérticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevoNodo = new CVertice();
            var_control = 2;
        }

        private void eliminarVérticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodoEliminar = new CVertice();
        }

        private void eliminarArcoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var_control = 3;
        }

        private void rbDirigido_CheckedChanged(object sender, EventArgs e)
        {
            dirigido = true;
            Refresh();
        }

        private void rbNoDirigido_CheckedChanged(object sender, EventArgs e)
        {
            dirigido = false;
            Refresh();
        }

        private void btnMMatriz_Click(object sender, EventArgs e)
        {
            int num = grafo.nodos.Count;
            resetFGV();
            int x;
            for (x = 0; x < num; x++)
            {
                c1 = new DataGridViewColumn();
                r1 = new DataGridViewRow();
                c1.HeaderText = grafo.nodos[x].ToString();
                r1.HeaderCell.Value = grafo.nodos[x].ToString();//NOC XQ NO CAMBIA EL NOMBRE DE CABECERA DE LAS FILAS
                c1.CellTemplate = new DataGridViewTextBoxCell();
                c1.Visible = true;
                c1.Width = 50;
                r1.Visible = true;
                r1.Height = 50;
                dataGridView1.Columns.Add(c1);                
                dataGridView1.Rows.Add(r1);
            }
            for (x = 0; x < num; x++)
            {
                for (int y = 0; y < num; y++)
                {
                    dataGridView1.Rows[y].Cells[x].Value = grafo.nodosadya[y, x];
                }
            }
        }
        DataGridViewColumn c1;
        DataGridViewRow r1;
        private void resetFGV()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetFGV();
            int num = grafo.nodos.Count;
            int count = 0;
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            c1 = new DataGridViewColumn();
            c1.HeaderText = "Nodo";
            c1.Width = 50;
            c1.CellTemplate = cell;
            dataGridView1.Columns.Add(c1);            
            c1 = new DataGridViewColumn();
            c1.HeaderText = "Grado";
            c1.Width = 50;
            c1.CellTemplate = cell;
            dataGridView1.Columns.Add(c1);
            
            for (int x = 0; x < num; x++)
            {
                r1 = new DataGridViewRow(); 
                r1.HeaderCell.Value = grafo.nodos[x].ToString();
                r1.Visible = true;
                r1.Height = 50;
                dataGridView1.Rows.Add(r1);
            }
            dargrado();
            foreach (CVertice nodo in grafo.nodos)
            {
                dataGridView1.Rows[count].Cells[0].Value = grafo.nodos[count].ToString();
                dataGridView1.Rows[count].Cells[1].Value = nodo.grado;
                count++;
            }
            
        }
        void dargrado()
        {
            foreach (CVertice nodo in grafo.nodos)
            {
                nodo.grado = 0;
                int numero = grafo.returnindice(nodo);
                if (dirigido)
                {
                    for (int x = 0; x < grafo.nodos.Count; x++)
                    {
                        if (grafo.nodosadya[x, numero] != 0)
                        {
                            nodo.grado++;
                        }
                        if (grafo.nodosadya[numero, x] != 0)
                        {
                            nodo.grado++;
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < grafo.nodos.Count; x++)
                    {
                        if (grafo.nodosadya[x, numero] != 0 || grafo.nodosadya[numero, x] != 0)
                        {
                            nodo.grado++;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            resetFGV();
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            c1 = new DataGridViewColumn();
            c1.HeaderText = "A";
            c1.Visible = true;
            c1.CellTemplate = cell;
            dataGridView1.Columns.Add(c1);
            c1 = new DataGridViewColumn();
            c1.HeaderText = "B";
            c1.Visible = true;
            
            c1.CellTemplate = cell;
            dataGridView1.Columns.Add(c1);
            
            for (int i = 0; i < grafo.nodos.Count; i++)
            {
                for (int j = 0; j < grafo.nodos.Count; j++)
                {
                    if(grafo.nodosadya[j,i]!=0)
                    {
                        r1 = new DataGridViewRow();                        
                        r1.HeaderCell.Value = count;
                        r1.Visible = true;
                        dataGridView1.Rows.Add(r1);
                        dataGridView1.Rows[count].Cells[0].Value = grafo.nodos[j];
                        dataGridView1.Rows[count].Cells[1].Value = grafo.nodos[i];
                        count++;
                    }
                }                
            }
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        /*private void distanciARCOS()
{
int sumarcos = 0;
for (int i = 0; i < grafo.nodos.Count; i++)
{
for (int j = 0; j < grafo.nodos.Count; j++)
{
if (grafo.nodos[j]==recorridoanchura)
{
sumarcos += int.Parse(grafo.nodos[i].Valor);
}
}
}
}*/
    }
}
