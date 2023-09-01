using BlogApi.Tools;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Interfaces;
using AutoMapper;
using BlogApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogApi.Controllers
{
    [Authorize(Roles = "Master")]
    [Route("api/1master1")]
    [Controller]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MasterController : ControllerBase
    {
        private readonly IAccionUsuario _accion;
        public MasterController(IAccionUsuario accion)
        {
            _accion = accion;
        }

        [HttpGet("todo")]
        public ActionResult GetTodo()
        {
            return Ok(_accion.GetAcciones());
        }

    }
}