using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
        public class PasoReceta
        {
            #region Atributos
            private int idReceta;
            private int idPaso;
            private short orden;
            private string descripcion;
            #endregion

            #region Propiedades

            public int IdReceta
            {
                get { return idReceta; }
                set { idReceta = value; }
            }

            public int IdPaso
        {
            get { return idPaso; }
            set { idPaso = value; }
        }

            public short Orden
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
            public PasoReceta()
            {
            }

            #endregion
        }
    
}
