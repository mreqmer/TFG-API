using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
    public class Ingrediente
    {
        #region Atributos
        private int idIngrediente;
        private string nombreIngrediente;
        private string categoria;
        private string medida;
        #endregion

        #region Propiedades
        public int IdIngrediente
        {
            get { return idIngrediente; }
            set { idIngrediente = value; }
        }

        public string NombreIngrediente
        {
            get { return nombreIngrediente; }
            set { nombreIngrediente = value; }
        }

        public string Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public string Medida
        {
            get { return medida; }
            set { medida = value; }
        }
        #endregion

        #region Constructores
        public Ingrediente()
        {
        }
        #endregion
    }
}
