using System;

namespace ClasesRecetas
{
    public class RecetaIngredientes
    {
        #region Atributos
        private int idReceta;
        private int idIngrediente;
        private decimal cantidad;
        private string notas;
        #endregion

        #region Propiedades
        public int IdReceta
        {
            get { return idReceta; }
            set { idReceta = value; }
        }

        public int IdIngrediente
        {
            get { return idIngrediente; }
            set { idIngrediente = value; }
        }


        public decimal Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        public string Notas
        {
            get { return notas; }
            set { notas = value; }
        }
        #endregion

        #region Constructores
        public RecetaIngredientes()
        {
        }
        #endregion
    }
}
