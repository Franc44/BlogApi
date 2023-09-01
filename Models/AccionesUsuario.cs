using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class AccionesUsuario
    {
        public int IdAccion { get; set; }
        public string NombreTablaAccion { get; set; }
        public string IdTablaAccion { get; set; }
        public string OperacionAccion { get; set; }
        public string UsuarioAccion { get; set; }
        public DateTime FechaAccion { get; set; }
    }
}
