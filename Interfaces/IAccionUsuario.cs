using System.Collections.Generic;
using BlogApi.Models;

namespace BlogApi.Interfaces
{
    public interface IAccionUsuario
    {
        void Usuario_Modifico_BD(string usuario);
        List<AccionesUsuario> GetAcciones();
    }
}