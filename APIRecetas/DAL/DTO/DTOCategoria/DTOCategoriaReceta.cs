using ClasesRecetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOCategoria
{
    public class DTOCategoriaReceta
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
        public DTOCategoriaReceta()
        {
        }

        public DTOCategoriaReceta(int idCategoria, string nombreCategoria)
        {
            this.idCategoria = idCategoria;
            this.nombreCategoria = nombreCategoria;
        }
        #endregion
    }
}
