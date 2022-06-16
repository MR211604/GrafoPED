using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ParcialGrafo
{
    class CVertice
    {
        public string Valor;
        Color color_nodo;
        Size dimensiones;
        Color color_fuente;
        Point _posicion;
        int radio;
        static int size = 70;
        public int grado = 0;
        public List<CArco> ListaAdyacencia;
        Dictionary<string, short> _banderas;
        Dictionary<string, short> _banderas_predeterminado;
        Form1 f1 = new Form1();

        public Color Color
        {
            get { return color_nodo; }
            set { color_nodo = value; }
        }
        public Color FontColor
        {
            get { return color_fuente; }
            set { color_fuente = value; }
        }
        public Point Posicion
        {
            get { return _posicion; }
            set { _posicion = value; }
        }
        public Size Dimensiones
        {
            get { return dimensiones; }
            set
            {
                radio = value.Width / 2;
                dimensiones = value;
            }
        }
        public CVertice(string Valor)
        {
            this.Valor = Valor;
            this.ListaAdyacencia = new List<CArco>();
            this._banderas = new Dictionary<string, short>();
            this._banderas_predeterminado = new Dictionary<string, short>();
            this.Color = Color.Yellow;
            this.Dimensiones = new Size(size, size);
            this.FontColor = Color.Black;
        }
        public CVertice() : this("") { }//constructor por defecto

        public void DibujarVertice(Graphics g)
        {
            SolidBrush b = new SolidBrush(this.color_nodo);
            Rectangle areaNodo = new Rectangle(this._posicion.X - radio, this._posicion.Y - radio, this.dimensiones.Width + 50, this.dimensiones.Height);
            g.FillEllipse(b, areaNodo);
            g.DrawString(this.Valor, new Font("Arial", 8), new SolidBrush(color_fuente), this._posicion.X + 25, this._posicion.Y, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.DrawEllipse(new Pen(Brushes.Black, (float)4), areaNodo);
            b.Dispose();
        }

        public bool DetectarPunto(Point p)
        {
            GraphicsPath posicion = new GraphicsPath();
            posicion.AddEllipse(new Rectangle(this._posicion.X - this.dimensiones.Width / 2, this._posicion.Y - this.dimensiones.Height / 2, this.dimensiones.Width, this.dimensiones.Height));
            bool retval = posicion.IsVisible(p);
            posicion.Dispose();
            return retval;
        }

        public override string ToString()
        {
            return this.Valor;
        }

        public void DibujarArco(Graphics g)
        {
            float distancia;
            int difY, difX;
            foreach (CArco arco in ListaAdyacencia)
            {
                difX = this.Posicion.X - arco.nDestino.Posicion.X;
                difY = this.Posicion.Y - arco.nDestino.Posicion.Y;
                distancia = (float)Math.Sqrt(Math.Pow(difX,2) + Math.Pow(difY,2));
                AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 4, true);
                bigArrow.BaseCap = System.Drawing.Drawing2D.LineCap.Triangle;

                if (Form1.dirigido)//GRAFO DIRIGIDO
                    g.DrawLine(new Pen(new SolidBrush(arco.color), arco.grosor_flecha) { CustomEndCap = bigArrow, Alignment = PenAlignment.Center }, _posicion, new Point(arco.nDestino.Posicion.X + (int)(radio * difX / distancia), arco.nDestino.Posicion.Y + (int)(radio * difY / distancia)));
                else if (Form1.dirigido == false) //GRAFO NO DIRIGIDO
                    g.DrawLine(new Pen(new SolidBrush(arco.color), arco.grosor_flecha), _posicion, new Point(arco.nDestino.Posicion.X + (int)(radio * difX / distancia), arco.nDestino.Posicion.Y + (int)(radio * difY / distancia)));
                else
                    MessageBox.Show(Form1.dirigido.ToString());
                g.DrawString(arco.peso.ToString(), new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Black), this._posicion.X - (int)(difX / 2), this._posicion.Y - (int)(difY / 2), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }
    }
}
