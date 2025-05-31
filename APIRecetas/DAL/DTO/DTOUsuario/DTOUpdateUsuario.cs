using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOUsuario
{
    public class DTOUpdateUsuario
    {
        #region Atributos
        private string correoElectronico;
        private string nombreUsuario;
        #endregion
        #region Propiedades
        public string CorreoElectronico
        {
            get { return correoElectronico; }
            set { correoElectronico = value; }
        }

        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }
        #endregion

        #region Constructores
        public DTOUpdateUsuario()
        {
        }
        #endregion
    }
}
