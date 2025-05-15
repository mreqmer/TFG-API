using ClasesRecetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class RecetaUsuario : Receta
    {
        #region Atributos
        private string nombreUsuario;
        #endregion
        #region Propiedades
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }
        #endregion
        #region Constructores
        public RecetaUsuario()
        {
        }
        #endregion
    
    
    }
}
