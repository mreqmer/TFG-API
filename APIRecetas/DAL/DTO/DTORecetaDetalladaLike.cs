using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class DTORecetaDetalladaLike : RecetaDTO
    {

        private bool tieneLike;

        public bool TieneLike
        {
            get { return tieneLike; }
            set { tieneLike = value; }
        }

    }
}
