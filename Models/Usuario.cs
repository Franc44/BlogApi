using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Documentos = new HashSet<Documento>();
            ForoPregunta = new HashSet<ForoPregunta>();
            ForoPreguntasLikes = new HashSet<ForoPreguntasLike>();
            ForoPreguntasRespuesta = new HashSet<ForoPreguntasRespuesta>();
            ForoPreguntasRespuestasLikes = new HashSet<ForoPreguntasRespuestasLike>();
            Publicaciones = new HashSet<Publicacione>();
            Recuperacions = new HashSet<Recuperacion>();
        }

        public string IdUsuario { get; set; }
        public decimal TipoUsua { get; set; }
        public byte[] ContraUsua { get; set; }
        public byte[] NombresUsua { get; set; }
        public byte[] ApellidosUsua { get; set; }
        public DateTime FechaAltaUsua { get; set; }
        public decimal EstatusUsua { get; set; }
        public byte[] EmailUsua { get; set; }
        public byte[] MatriculaUsua { get; set; }
        public int EmpresaUsua { get; set; }
        public byte[] RfcUsua { get; set; }
        public string ProfilePicture { get; set; }

        public virtual Empresa EmpresaUsuaNavigation { get; set; }
        public virtual ICollection<Documento> Documentos { get; set; }
        public virtual ICollection<ForoPregunta> ForoPregunta { get; set; }
        public virtual ICollection<ForoPreguntasLike> ForoPreguntasLikes { get; set; }
        public virtual ICollection<ForoPreguntasRespuesta> ForoPreguntasRespuesta { get; set; }
        public virtual ICollection<ForoPreguntasRespuestasLike> ForoPreguntasRespuestasLikes { get; set; }
        public virtual ICollection<Publicacione> Publicaciones { get; set; }
        public virtual ICollection<Recuperacion> Recuperacions { get; set; }
    }
}
