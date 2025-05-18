
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
    public class Categoria
    {
        #region Atributos
        private int idCategoria;
        private string nombreCategoria;
        #endregion

        #region Propiedades
        public int IdCategoria
        {
            get { return idCategoria; }
            set { idCategoria = value; }
        }

        public string NombreCategoria
        {
            get { return nombreCategoria; }
            set { nombreCategoria = value; }
        }
        #endregion

        #region Constructores
        public Categoria()
        {
        }

        public Categoria(int idCategoria, string nombre)
        {
            this.idCategoria = idCategoria;
            this.nombreCategoria = nombre;
        }
        #endregion
    }
}
