using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ParcialGrafo
{
    class CGrafo
    {
        public List<CVertice> nodos;
        List<CVertice> parts = new List<CVertice>();
        public int[,] nodosadya = new int[50,50];
        int i, j;
        public CGrafo()
        {
            nodos = new List<CVertice>();
        }

        public CVertice AgregarVertice(string valor)
        {
            CVertice nodo = new CVertice(valor);
            nodos.Add(nodo);
            return nodo;
        }
        public void AgregarVertice(CVertice nuevonodo)
        {
            nodos.Add(nuevonodo);
        }
        public CVertice BuscarVertice(string valor)
        {
            CVertice cV = new CVertice();
            cV = nodos.Find(v => v.Valor == valor);

            return cV;
        }
        public void EliminarVertice(CVertice vertice)
        {
            nodos.Remove(vertice);
            EliminarArco(vertice);
        }
        public void EliminarVertice(string sEvalor)
        {
            if (this.BuscarVertice(sEvalor) != null)
                nodos.Remove(this.BuscarVertice(sEvalor));
        }
        int indice;
        public bool AgregarArco(CVertice origen, CVertice nDestino, int peso)//***
        {
            i = returnindice(origen);
            j = returnindice(nDestino);
            if (origen.ListaAdyacencia.Find(v => v.nDestino == nDestino) == null)
            {
                origen.ListaAdyacencia.Add(new CArco(nDestino, peso));
                nodosadya[i, j] = peso;
                if (Form1.dirigido==false)
                    nodosadya[j, i] = peso;

                return true;
            }
            return false;
        }

        public int returnindice(CVertice n1)
        {
            indice = 0;
            int x = 0;
            foreach (CVertice nodo in nodos)
            {
                if (n1 == nodo)
                {
                    indice = x;
                }
                x++;
            }
            return indice;
        }
        public void EliminarArco(CVertice nOrigen, CVertice nDestino)
        {
            i = returnindice(nOrigen);
            j = returnindice(nDestino);
            nodosadya[i, j] = 0;
            if (Form1.dirigido==false)
                nodosadya[j, i] = 0;
        }
        
        public void EliminarArco(CVertice nBorrar)
        {
            foreach (CVertice nodo in nodos)
                EliminarArista(nodo, nBorrar);
        }
        public bool EliminarArista(CVertice origen, CVertice nDestino)
        {
            CArco arista = origen.ListaAdyacencia.Find(v => v.nDestino == nDestino);
            return origen.ListaAdyacencia.Remove(arista);
        }
        public void ColorearNodo(string valorEscogido)
        {
            CVertice nodoEscogido = this.BuscarVertice(valorEscogido);
            nodoEscogido.Color = Color.Green;
        }
        public string ColorearNodoTest(string valorEscogido)
        {
            CVertice nodoEscogido = this.BuscarVertice(valorEscogido);

            if (nodoEscogido != null)
            {
                nodoEscogido.Color = Color.Orange;
                return "Coloreado";
            }
            else
            {
                return "No existe";
            }
           
        }
        public void ReestablecerColorNodo(string valorEscogido)
        {
            CVertice nodoEscogido = this.BuscarVertice(valorEscogido);
            nodoEscogido.Color = Color.Yellow;
        }
        
        public void DibujarGrafo(Graphics g)
        {
            
            foreach (CVertice nodo in nodos)
                nodo.DibujarArco(g);

            
            foreach (CVertice nodo in nodos)
                nodo.DibujarVertice(g);
        }
        public CVertice DetectarPunto(Point posicionMouse)
        {
            foreach (CVertice nodoActual in nodos)
                if (nodoActual.DetectarPunto(posicionMouse)) return nodoActual;
            return null;
        }
        public void ReestablecerGrafo(Graphics g)
        {
            foreach (CVertice nodo in nodos)
            {
                nodo.Color = Color.Yellow;
                nodo.FontColor = Color.Black;
                foreach (CArco arco in nodo.ListaAdyacencia)
                {
                    arco.grosor_flecha = 2;
                    arco.color = Color.Violet;
                }
            }
            DibujarGrafo(g);
        }
    }
}
