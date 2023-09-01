using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.DTOs
{
    public class UsuarioCreadto
    {
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
        public IFormFile File { get; set; }
    }
}