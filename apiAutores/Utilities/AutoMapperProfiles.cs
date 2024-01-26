using apiAutores.DTOs;
using apiAutores.Entidades;
using AutoMapper;
using System.Net;

namespace apiAutores.Utilities
{
    public class AutoMapperProfiles: Profile
    { 
            public AutoMapperProfiles()
            {
                CreateMap<AutorDTO, Autor>();
                CreateMap<Autor, AutorGetDTO>();
                CreateMap<LibroDTO, Libro>();
                CreateMap<Libro, LibroGetDTO>();
        }
    }
}
