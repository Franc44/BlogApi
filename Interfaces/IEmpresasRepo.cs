using System.Collections.Generic;
using BlogApi.Models;

namespace BlogApi.Interfaces
{
    public interface IEmpresaRepo
    {
        IEnumerable<Empresa> GetEmpresas();
        Empresa GetEmpresaPorId(int Id);
    }
}