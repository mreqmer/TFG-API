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
    public class DTOReceta : RecetaUsuario
    {
        #region Atributos
        private List<DTOPasoReceta> pasos;
        private List<DTOIngredienteReceta> ingredientes;
        private List<DTOCategoriaReceta> categorias;
        #endregion

        #region Propiedades
        public List<DTOPasoReceta> Pasos
        {
            get { return pasos; }
            set { pasos = value; }
        }

        public List<DTOIngredienteReceta> Ingredientes
        {
            get { return ingredientes; }
            set { ingredientes = value; }
        }

        public List<DTOCategoriaReceta> Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }
        #endregion

        #region Constructores
        public DTOReceta()
        {
        }
        #endregion

    }
}
