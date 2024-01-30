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
                CreateMap<Autor, AutorGetDTO>().ForMember(s => s.Libros, options => options.MapFrom(MapAutorDTOLibros));
                CreateMap<LibroDTO, Libro>().ForMember(libro => libro.AutoresLibros, options => options.MapFrom(MapAutoresLibros));
                CreateMap<Libro, LibroGetDTO>().ForMember(libroGetDTO => libroGetDTO.Autores, options => options.MapFrom(MapLibroDTOAutores));
                CreateMap<LibroPatchDTO, Libro>().ReverseMap();
                CreateMap<ComentarioCreacionDTO, Comentario>();
                CreateMap<Comentario, ComentarioDTO>();

        }

        private List<LibroGetDTO> MapAutorDTOLibros(Autor autor, AutorGetDTO autoGetDTO)
        {
            var res = new List<LibroGetDTO>();
            if (autor.AutoresLibros == null) { return res; }

            foreach (var autorLibro in autor.AutoresLibros)
            {
                res.Add(new LibroGetDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }
            return res;
        }

        private List<AutorGetDTO> MapLibroDTOAutores(Libro libro, LibroGetDTO libroGetDTO)
        {
            var res = new List<AutorGetDTO>();
            if (libro.AutoresLibros == null) { return res; }

            foreach (var autorLibro in libro.AutoresLibros)
            {
                res.Add(new AutorGetDTO()
                {
                    Id = autorLibro.AutorId,
                    Name = autorLibro.Autor.Name
                });
            }
            return res;
        }

        private List<AutorLibro> MapAutoresLibros(LibroDTO libroDTO, Libro libro)
        {
            var res = new List<AutorLibro>();
            if (libroDTO.AutoresIds == null) { return res; }

            foreach (var id in libroDTO.AutoresIds)
            {
                res.Add(new AutorLibro() { AutorId = id });
            }
            return res;
        }
    }
}
