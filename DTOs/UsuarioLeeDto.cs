using System;

namespace BlogApi.DTOs
{
    public class UsuarioLeeDto
    {
        public string IdUsuario { get; set; }
        public byte[] NombresUsua { get; set; }
        public byte[] ApellidosUsua { get; set; }
        public DateTime FechaAltaUsua { get; set; }
        public decimal EstatusUsua { get; set; }
        public byte[] MatriculaUsua { get; set; }
        public string ProfilePicture { get; set; }

        public string Token { get; set;}
        public DateTime ExpiresDate { get; set; }
        public string Rol { get; set;}
    }
}