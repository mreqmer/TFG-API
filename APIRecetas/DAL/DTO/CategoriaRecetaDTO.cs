using ClasesRecetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CategoriaRecetaDTO
    {
    #region Atributos
    private int idCategoria;
    private string nombreCategoria;
    #endregion

    #region Propiedades
    public int IdCategoria
    {
        get { return idCategoria; }
        set { idCategoria = value; }
    }

    public string NombreCategoria
    {
        get { return nombreCategoria; }
        set { nombreCategoria = value; }
    }
    #endregion

    #region Constructores
    public CategoriaRecetaDTO()
    {
    }

    public CategoriaRecetaDTO(int idCategoria, string nombreCategoria)
    {
        this.idCategoria = idCategoria;
        this.nombreCategoria = nombreCategoria;
    }
        #endregion
    }
   }
