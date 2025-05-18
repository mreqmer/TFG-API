using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesRecetas
{
    public class Likes
    {
        private int idLike;
        private int idReceta;
        private int idUsuario;

        public int IdLike
        {
            get { return idLike; }
            set { idLike = value; }
        }
        public int IdReceta
        {
            get { return idReceta; }
            set { idReceta = value; }
        }
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }
        public Likes() { }

    }
}
