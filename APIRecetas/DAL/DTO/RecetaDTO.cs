using ClasesRecetas;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class RecetaDTO : RecetaUsuario
    {
        #region Atributos
        private List<PasoRecetaDTO> pasos;
        private List<IngredienteRecetaDTO> ingredientes;
        private List<CategoriaRecetaDTO> categorias;
        #endregion

        #region Propiedades
        public List<PasoRecetaDTO> Pasos
        {
            get { return pasos; }
            set { pasos = value; }
        }

        public List<IngredienteRecetaDTO> Ingredientes
        {
            get { return ingredientes; }
            set { ingredientes = value; }
        }

        public List<CategoriaRecetaDTO> Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }
        #endregion

        #region Constructores
        public RecetaDTO()
        {
        }
        #endregion

    }
}
