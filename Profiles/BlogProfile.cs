using AutoMapper;
using BlogApi.Models;
using BlogApi.DTOs;

namespace BlogApi.Profiles
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Empresa, EmpresaDTO>();
            CreateMap<EmpresaDTO, Empresa>();
            CreateMap<Usuario,UsuarioLeeDto>();
            CreateMap<UsuarioCreadto, Usuario>();
            CreateMap<Usuario,UsuarioCreadto>();
            CreateMap<ForoPregunta, ForoPreguntaDTO>();
            CreateMap<ForoPreguntaDTO, ForoPregunta>();
            CreateMap<ForoPregunta, ForoPreguntaLeeDto>();
            CreateMap<ForoPreguntaLeeDto, ForoPregunta>();  
            CreateMap<ForoPreguntasRespuesta, ForoPreguntasRespuestaDTO>();
            CreateMap<ForoPreguntasRespuestaDTO, ForoPreguntasRespuesta>();
            CreateMap<Publicacione, PublicacionDTO>();
            CreateMap<PublicacionDTO, Publicacione>();
            CreateMap<PublicacionLeeDto, Publicacione>();
            CreateMap<Publicacione, PublicacionLeeDto>();
            CreateMap<Documento, DocumentoDto>();
            CreateMap<DocumentoDto, Documento>();
            CreateMap<Documento, DocumentoLeeDto>();
            CreateMap<ForoPreguntasLike, FPLikesDto>();
            CreateMap<FPLikesDto, ForoPreguntasLike>();
            CreateMap<ForoPreguntasRespuestasLike, FPRLikesDto>();
            CreateMap<FPRLikesDto, ForoPreguntasRespuestasLike>();
        }
    }
}