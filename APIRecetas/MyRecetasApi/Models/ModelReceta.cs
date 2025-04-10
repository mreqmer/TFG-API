using ClasesRecetas;

namespace MyRecetasApi.Models
{
    public class ModelReceta : Receta
    {
        #region Atributos
        private List<RecetaIngredientes> ingredientes = new List<RecetaIngredientes>();
        private List<PasoReceta> pasos = new List<PasoReceta>();
        private List<RecetaCategoria> categorias = new List<RecetaCategoria>();
        #endregion

        #region Propiedades
        public List<RecetaIngredientes> Ingredientes
        {
            get { return ingredientes; }
            set { ingredientes = value; }
        }

        public List<PasoReceta> Pasos
        {
            get { return pasos; }
            set { pasos = value; }
        }

        public List<RecetaCategoria> Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }
        #endregion
    }
   }
