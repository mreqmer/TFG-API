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
        private string uid;
        private string nombreUsuario;
        private string fotoPerfil;
        #endregion
        #region Propiedades
        public string UID
        {
            get { return uid; }
            set { uid = value; }
        }
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }
        public string FotoPerfil
        {
            get { return fotoPerfil; }
            set { fotoPerfil = value; }
        }
        #endregion

        #region Constructores
        public DTOUpdateUsuario()
        {
        }
        #endregion
    }
}
