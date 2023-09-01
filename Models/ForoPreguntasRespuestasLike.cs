using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class ForoPreguntasRespuestasLike
    {
        public int IdRespuestaFprl { get; set; }
        public string IdUsuarioFlrp { get; set; }
        public DateTime FechaFlrp { get; set; }
        public int IdFprlikes { get; set; }

        public virtual ForoPreguntasRespuesta IdRespuestaFprlNavigation { get; set; }
        public virtual Usuario IdUsuarioFlrpNavigation { get; set; }
    }
}
