using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class RecetaUsuarioLike : RecetaUsuario
    {
        #region Atributos
        private bool tieneLike;
        #endregion

        #region Propiedades
        public bool TieneLike
        {
            get { return tieneLike; }
            set { tieneLike = value; }
        }
        #endregion

        #region Constructores
        public RecetaUsuarioLike()
        {
        }
        #endregion
    }
}
