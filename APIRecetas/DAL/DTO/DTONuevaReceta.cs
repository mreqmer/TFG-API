using ClasesRecetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class DTONuevaReceta : Receta
    {
        #region Atributos
        private List<DTOPasosRecetaSimplificado> pasos;
        private List<DTOIngredientesRecetaSimplificado> ingredientes;
        private List<DTOCategoriaRecetaSimplificada> categorias;
        #endregion

        #region Propiedades
        public List<DTOPasosRecetaSimplificado> Pasos
        {
            get { return pasos; }
            set { pasos = value; }
        }

        public List<DTOIngredientesRecetaSimplificado> Ingredientes
        {
            get { return ingredientes; }
            set { ingredientes = value; }
        }

        public List<DTOCategoriaRecetaSimplificada> Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }
        #endregion

        #region Constructores
        public DTONuevaReceta()
        {
            pasos = new List<DTOPasosRecetaSimplificado>();
            ingredientes = new List<DTOIngredientesRecetaSimplificado>();
            categorias = new List<DTOCategoriaRecetaSimplificada>();
        }
        #endregion
    }
}
