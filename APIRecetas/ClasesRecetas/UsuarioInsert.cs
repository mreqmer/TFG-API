using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
    public class UsuarioInsert
    {
        #region Atributos
        private string firebaseUID;
        private string nombreUsuario;
        private string correoElectronico;
        #endregion

        #region Propiedades

        public string FirebaseUID
        {
            get { return firebaseUID; }
            set { firebaseUID = value; }
        }

        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }

        public string CorreoElectronico
        {
            get { return correoElectronico; }
            set { correoElectronico = value; }
        }

        #endregion

        #region Constructores
        public UsuarioInsert() { }

        public UsuarioInsert(string firebaseUID, string nombreUsuario, string correoElectronico)
        {
            FirebaseUID = firebaseUID;
            NombreUsuario = nombreUsuario;
            CorreoElectronico = correoElectronico;
        }
        #endregion
    }
}
