using System;
using System.Collections.Generic;

namespace BlogApi.DTOs
{
    public class ForoPreguntaDTO
    {
        public string TituloPregunta { get; set; }
        public string DescripcionPregunta { get; set; }
        public string EtiquetasPregunta { get; set; }
        public short LikesPregunta { get; set; }
        public DateTime FechaPregunta { get; set; }
        public decimal EstatusPregunta { get; set; }
        public string UsuarioPregunta { get; set; }
        public int EmpresaPregunta { get; set; }   
        public DateTime FechaCierre { get; set; }     
    }
}