using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOReceta
{
    public class DTORecetaDetalladaLike : RecetaDTO
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
        public DTORecetaDetalladaLike()
        {
        }
        #endregion

    }
}
