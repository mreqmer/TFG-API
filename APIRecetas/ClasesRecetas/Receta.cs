
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
    public class Receta
    {
        #region Atributos
        private int idReceta;
        private string nombreReceta;
        private string descripcion;
        private int? tiempoPreparacion;
        private string dificultad;
        private DateTime fechaCreacion;
        #endregion

        #region Propiedades
        public int IdReceta
        {
            get { return idReceta; }
            set { idReceta = value; }
        }

        public string NombreReceta
        {
            get { return nombreReceta; }
            set { nombreReceta = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public int? TiempoPreparacion
        {
            get { return tiempoPreparacion; }
            set { tiempoPreparacion = value; }
        }

        public string Dificultad
        {
            get { return dificultad; }
            set { dificultad = value; }
        }

        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }
        #endregion

        #region Constructores
        public Receta()
        {
        }

        public Receta(int idReceta, string nombreReceta, string descripcion, int? tiempoPreparacion, string dificultad, DateTime fechaCreacion)
        {
            this.idReceta = idReceta;
            this.nombreReceta = nombreReceta;
            this.descripcion = descripcion;
            this.tiempoPreparacion = tiempoPreparacion;
            this.dificultad = dificultad;
            this.fechaCreacion = fechaCreacion;
        }
        #endregion
    }
}
