using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class Documento
    {
        public int IdDocumento { get; set; }
        public DateTime FechaDoc { get; set; }
        public string UsuarioDoc { get; set; }
        public int? EmpresaDoc { get; set; }
        public string TipoDoc { get; set; }
        public string TituloDoc { get; set; }
        public string DescripcionDoc { get; set; }
        public string FileDoc { get; set; }

        public virtual Empresa EmpresaDocNavigation { get; set; }
        public virtual Usuario UsuarioDocNavigation { get; set; }
    }
}
