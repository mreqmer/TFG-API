using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOUsuario
{
    public class DTOUsuario
    {
        #region Atributos
        private int idUsuario;
        private DateTime fechaRegistro;
        #endregion

        #region Propiedades
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }
        public DateTime FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }
        #endregion

        #region Constructores
        public DTOUsuario()
        {
        }
        #endregion


    }


}
