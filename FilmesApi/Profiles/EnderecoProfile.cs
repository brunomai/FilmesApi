using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;

namespace FilmesApi.Profiles;

public class EnderecoProfile : Profile
{
	public EnderecoProfile()
	{
		CreateMap<CreateCinemaDto, Endereco>();
        CreateMap<UpdateCinemaDto, Endereco>();
        CreateMap<Endereco, ReadEnderecoDto>();
    }
}
