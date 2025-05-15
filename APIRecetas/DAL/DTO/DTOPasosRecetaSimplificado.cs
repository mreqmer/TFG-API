using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class DTOPasosRecetaSimplificado
    {
        #region Atributos
        private int orden;
        private string descripcion;
        #endregion

        #region Propiedades
        public int Orden
        {
            get { return orden; }
            set { orden = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        #endregion

        #region Constructores
        public DTOPasosRecetaSimplificado()
        {
        }
        #endregion
    }
}
