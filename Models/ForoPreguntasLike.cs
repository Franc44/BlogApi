using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class ForoPreguntasLike
    {
        public int IdPreguntaFpl { get; set; }
        public string IdUsuarioFlp { get; set; }
        public DateTime FechaFlp { get; set; }
        public int IdFplikes { get; set; }

        public virtual ForoPregunta IdPreguntaFplNavigation { get; set; }
        public virtual Usuario IdUsuarioFlpNavigation { get; set; }
    }
}
