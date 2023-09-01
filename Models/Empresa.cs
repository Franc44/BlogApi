using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            Documentos = new HashSet<Documento>();
            ForoPregunta = new HashSet<ForoPregunta>();
            Publicaciones = new HashSet<Publicacione>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdEmpresa { get; set; }
        public string NombreEmp { get; set; }
        public decimal EstatusEmp { get; set; }
        public string DescripcionEmp { get; set; }
        public DateTime FechaAltaEmp { get; set; }

        public virtual ICollection<Documento> Documentos { get; set; }
        public virtual ICollection<ForoPregunta> ForoPregunta { get; set; }
        public virtual ICollection<Publicacione> Publicaciones { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
