using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ParcialGrafo
{
    class CArco
    {
        public CVertice nDestino;//Objeto para referenciar elementos de la clase CVertice
        public int peso;//Atributo para peso
        public float grosor_flecha;//Atributo para indicar el grosor del arco
        public Color color;//Objeto para invocar elementos de la clase Color

        public CArco(CVertice destino) : this(destino, 1)//Constructor con herencia para poder darle valor al arco dentro de la clase Cvertice
        {
            this.nDestino = destino;
        }
        public CArco(CVertice destino, int peso)//Constructor con parametros para dar valores iniciales
        {
            this.nDestino = destino;
            this.peso = peso;
            this.grosor_flecha = 5;
            this.color = Color.Red;//color del arco
        }
    }
}
