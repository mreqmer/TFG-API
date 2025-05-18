using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
    public class Usuario
    {
        #region Atributos
        private int idUsuario;
        private string firebaseUID;
        private string correoElectronico;
        private string nombreUsuario;  
        private DateTime fechaRegistro;
        #endregion

        #region Propiedades
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public string FirebaseUID
        {
            get { return firebaseUID; }
            set { firebaseUID = value; }
        }

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

        public DateTime FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }
        #endregion

        #region Constructores
        public Usuario() { }

        #endregion
    }
}
