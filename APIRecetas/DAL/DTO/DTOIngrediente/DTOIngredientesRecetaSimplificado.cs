using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOIngrediente
{
    public class DTOIngredientesRecetaSimplificado
    {
        #region Atributos
        private int idIngrediente;
        private double cantidad;
        private string notas;
        #endregion

        #region Propiedades
        public int IdIngrediente
        {
            get { return idIngrediente; }
            set { idIngrediente = value; }
        }

        public double Cantidad
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
        public DTOIngredientesRecetaSimplificado()
        {
        }
        #endregion
    }
}
