using System;
using System.Collections.Generic;

namespace BlogApi.DTOs
{
    public class EmpresaDTO
    {
        public int IdEmpresa { get; set; }
        public string NombreEmp { get; set; }
        public decimal EstatusEmp { get; set; }
        public string DescripcionEmp { get; set; }
        public DateTime FechaAltaEmp { get; set; }
    }
}