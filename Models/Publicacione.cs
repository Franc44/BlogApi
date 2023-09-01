using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class Publicacione
    {
        public int IdPublicacion { get; set; }
        public string TituloPubli { get; set; }
        public string TextoPubli { get; set; }
        public string ImagenNombrePubli { get; set; }
        public DateTime FechaPubli { get; set; }
        public string UsuarioPubli { get; set; }
        public int? EmpresaPubli { get; set; }
        public decimal EstatusPubli { get; set; }
        public decimal TipoPubli { get; set; }

        public virtual Empresa EmpresaPubliNavigation { get; set; }
        public virtual Usuario UsuarioPubliNavigation { get; set; }
    }
}
