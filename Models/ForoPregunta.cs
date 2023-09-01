using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class ForoPregunta
    {
        public ForoPregunta()
        {
            ForoPreguntasLikes = new HashSet<ForoPreguntasLike>();
            ForoPreguntasRespuesta = new HashSet<ForoPreguntasRespuesta>();
        }

        public int IdPregunta { get; set; }
        public string TituloPregunta { get; set; }
        public string DescripcionPregunta { get; set; }
        public string EtiquetasPregunta { get; set; }
        public short LikesPregunta { get; set; }
        public DateTime FechaPregunta { get; set; }
        public decimal EstatusPregunta { get; set; }
        public string UsuarioPregunta { get; set; }
        public int EmpresaPregunta { get; set; }
        public DateTime? FechaCierre { get; set; }

        public virtual Empresa EmpresaPreguntaNavigation { get; set; }
        public virtual Usuario UsuarioPreguntaNavigation { get; set; }
        public virtual ICollection<ForoPreguntasLike> ForoPreguntasLikes { get; set; }
        public virtual ICollection<ForoPreguntasRespuesta> ForoPreguntasRespuesta { get; set; }
    }
}
