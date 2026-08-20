using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFul.Data;
using RestFul.DTOs;

namespace RestFul.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os produtos cadastrados.
        /// </summary>
        /// <returns>Lista de produtos.</returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Produto>>> GetTodos()
        {
            return await _context.Produtos.ToListAsync();
        }

        /// <summary>
        /// Retorna um produto específico pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>O produto encontrado.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produto>> GetPorId(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            return produto;
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="dto">Dados do produto a ser criado.</param>
        /// <returns>O produto recém-criado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Produto>> Criar(ProdutoDto dto)
        {
            var produto = new Produto
            {
                Nome = dto.Nome,
                Preco = dto.Preco,
                EmailFornecedor = dto.EmailFornecedor
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId), new { id = produto.Id }, produto);
        }

        /// <summary>
        /// Atualiza os dados de um produto existente.
        /// </summary>
        /// <param name="id">ID do produto a ser atualizado.</param>
        /// <param name="dados">Novos dados do produto.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Atualizar(int id, Produto dados)
        {
            var p = await _context.Produtos.FindAsync(id);
            if (p == null) return NotFound();

            p.Nome = dados.Nome;
            p.Preco = dados.Preco;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto a ser removido.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Remover(int id)
        {
            var p = await _context.Produtos.FindAsync(id);
            if (p == null) return NotFound();

            _context.Produtos.Remove(p);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Busca produtos dentro de uma faixa de preço.
        /// </summary>
        /// <param name="precoMin">Preço mínimo (opcional).</param>
        /// <param name="precoMax">Preço máximo (opcional).</param>
        /// <returns>Lista de produtos dentro da faixa informada.</returns>
        [AllowAnonymous]
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Produto>>> BuscarPorPreco(
            [FromQuery] decimal? precoMin,
            [FromQuery] decimal? precoMax)
        {
            var query = _context.Produtos.AsQueryable();

            if (precoMin.HasValue)
                query = query.Where(p => p.Preco >= precoMin.Value);

            if (precoMax.HasValue)
                query = query.Where(p => p.Preco <= precoMax.Value);

            return await query.ToListAsync();
        }
    }
}