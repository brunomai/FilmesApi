using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _filmeContext;
        private IMapper _mapper;

        public EnderecoController(FilmeContext filmeContext, IMapper mapper)
        {
            _filmeContext = filmeContext;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto createEnderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(createEnderecoDto);
            _filmeContext.Enderecos.Add(endereco);
            _filmeContext.SaveChanges();
            return CreatedAtAction(nameof(RecuperaEnderecoPorId), new { Id = endereco.Id }, endereco);
        }
        [HttpGet("{id}")]
        public IActionResult RecuperaEnderecoPorId(int id)
        {
            Endereco endereco = _filmeContext.Enderecos.Where(x=> x.Id == id).FirstOrDefault();
            if(endereco == null)             
                return NotFound();

            ReadEnderecoDto readEnderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
            return Ok(readEnderecoDto);
            
        }
        [HttpGet]
        public IEnumerable<ReadEnderecoDto> RecuperaEnderecos()
        {
            return _mapper.Map<List<ReadEnderecoDto>>(_filmeContext.Enderecos);
        }

        [HttpPost("id")]
        public IActionResult AtualizaEndereco(int id, [FromBody] UpdateEnderecoDto updateEnderecoDto)
        {
            Endereco endereco = _filmeContext.Enderecos.Where(x => x.Id == id).FirstOrDefault();
            if (endereco == null)
                return NotFound();
            _mapper.Map(updateEnderecoDto, endereco);
            _filmeContext.SaveChanges();
            return NoContent();

        }

        [HttpDelete("id")]
        public IActionResult DeletaEndereco(int id)
        {
            Endereco endereco = _filmeContext.Enderecos.Where(x => x.Id == id).FirstOrDefault();
            if (endereco == null)
                return NotFound();

            _filmeContext.Remove(endereco);
            _filmeContext.SaveChanges();
            return NoContent();
        }


    }
}
