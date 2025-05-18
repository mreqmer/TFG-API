using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class DTOUsuario
    {
        private int idUsuario;
        private DateTime fechaRegistro;

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

        public DTOUsuario()
        {
        }


    }


}
