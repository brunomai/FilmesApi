using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : Controller
{
    private FilmeContext _context;
    private IMapper _mapper;
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos ncessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code= "201">Caso o a inserção seja um sucesso</response>
  
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDTO filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }
    /// <summary>
    /// Recupera uma lista de filmes dentro da range estipulada
    /// </summary>
    /// <param name="skip">Indica a posição inicial da lista de filmes</param>
    /// <param name="take">Indica quantos filmes retornarão</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }


    /// <summary>
    /// Recupera filme do BD pelo Id
    /// </summary>
    /// <param name="id">Id do filme a ser procurado</param>
    /// <returns>Retorna o filme buscado</returns>
    /// <response code= "200">Caso o Id exista no BD</response>
    /// <response code= "404">Caso o Id seja inexistente no BD</response>
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filmes => filmes.Id == id);

        if (filme == null)
            return NotFound();
        
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filmeDto);
    }
    /// <summary>
    /// Atualiza o filme no BD atraves do Id
    /// </summary>
    /// <param name="id">Id do tilme a ser atualizado</param>
    /// <param name="filmeDto">Objeto com os campos necessários para atualização de um filme</param>
    /// <returns>sem conteúdo</returns>
    /// <response code="204">Caso o id seja existente na base de dados e o filme tenha sido atualizado</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id,
    [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
            return NotFound();

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Atualiza de forma parcial Filme no banco de Dados usando Id
    /// </summary>
    /// <param name="id">Id do filme a ser atualizado no banco</param>
    /// <param name="patch">Objeto parcial com os campos necessários para atualizaçaõ do filme</param>
    /// <returns>Sem conteúdo de retorno</returns>
    /// <response code= "204">Caso o Id existente na base de dados e o filme tenha sido atualizado</response>
    /// <response code= "404">Caso o Id seja inexistente na base de dados</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AtualizaFilmeParcial(int id,
        JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
            return NotFound();

        var filmeAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(filmeAtualizar, ModelState);

        if (!TryValidateModel(filmeAtualizar))
        {
            return ValidationProblem(ModelState);
        }


        _mapper.Map(filmeAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }
    /// <summary>
    /// Remove um filme do BD através do id
    /// </summary>
    /// <param name="id">Id do filme que deve ser removido</param>
    /// <returns>Sem retorno</returns>
    /// /// <response code= "204">Caso o Id exista no BD e o registro for removido com sucesso</response>
    /// <response code= "404">Caso o Id seja inexistente na base de dados</response>
    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id ==id);

        if(filme == null)
            return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();

    }
}
