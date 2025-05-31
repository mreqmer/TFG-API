using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOIngrediente
{
    public class DTOIngredienteReceta
    {
        #region Atributos
        private int idIngrediente;
        private string nombreIngrediente;
        private string categoria;
        private string medida;
        private decimal cantidad;
        private string notas;
        #endregion

        #region Propiedades
        public int IdIngrediente
        {
            get { return idIngrediente; }
            set { idIngrediente = value; }
        }

        public string NombreIngrediente
        {
            get { return nombreIngrediente; }
            set { nombreIngrediente = value; }
        }

        public string Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public string Medida
        {
            get { return medida; }
            set { medida = value; }
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
        public DTOIngredienteReceta()
        {
        }
        #endregion
    }
}
