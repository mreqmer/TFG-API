using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class DTOLike
    {
        private int idReceta;
        private string uid;

        public int IdReceta
        {
            get { return idReceta; }
            set { idReceta = value; }
        }
        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        public DTOLike() { }

    }
}
