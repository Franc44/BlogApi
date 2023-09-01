using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class Recuperacion
    {
        public int IdRecuperacion { get; set; }
        public string UsuarioRec { get; set; }
        public string ClaveRec { get; set; }
        public DateTime FechaRec { get; set; }
        public DateTime ExpiracionRec { get; set; }

        public virtual Usuario UsuarioRecNavigation { get; set; }
    }
}
