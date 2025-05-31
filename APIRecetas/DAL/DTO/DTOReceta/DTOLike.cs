using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOReceta
{
    public class DTOLike
    {
        #region Atributos
        private int idReceta;
        private string uid;
        #endregion

        #region Propiedades
        public int IdReceta
        {
            get { return idReceta; }
            set { idReceta = value; }
        }
        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        #endregion
        #region Constructores
        public DTOLike() { }
        #endregion
    }
}
