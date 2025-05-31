using ClasesRecetas;
using DAL.DTO.DTOCategoria;
using DAL.DTO.DTOIngrediente;
using DAL.DTO.DTOPasos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.DTOReceta
{
    public class DTONuevaReceta : Receta
    {
        #region Atributos
        private List<PasoRecetaDTO> pasos;
        private List<DTOIngredientesRecetaSimplificado> ingredientes;
        private List<DTOCategoriaRecetaSimplificada> categorias;
        #endregion

        #region Propiedades
        public List<PasoRecetaDTO> Pasos
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
        }
        #endregion
    }
}
