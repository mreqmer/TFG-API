using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
    public class RecetaCategoria
    {
        #region Atributos
        private int idReceta;
        private int idCategoria;
        #endregion

        #region Propiedades
        public int IdReceta
        {
            get { return idReceta; }
            set { idReceta = value; }
        }

        public int IdCategoria
        {
            get { return idCategoria; }
            set { idCategoria = value; }
        }


        #endregion

        #region Constructores
        public RecetaCategoria()
        {
        }

        public RecetaCategoria(int idReceta, int idCategoria, string nombreCategoria)
        {
            this.idReceta = idReceta;
            this.idCategoria = idCategoria;
        }
        #endregion
    }
}
