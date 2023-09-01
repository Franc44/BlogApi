using System.Collections.Generic;
using BlogApi.Models;
using System.Linq;
using BlogApi.Interfaces;

namespace BlogApi.Data
{
    public class EmpresaConsultas : IEmpresaRepo
    {
        private readonly BlogHCContext _context;
        public EmpresaConsultas(BlogHCContext context)
        {
            _context = context;
        }
        public Empresa GetEmpresaPorId(int Id)
        {
            return _context.Empresas.FirstOrDefault(x => x.IdEmpresa == Id);
        }

        public IEnumerable<Empresa> GetEmpresas()
        {
            return _context.Empresas.ToList();
        }
    }
}