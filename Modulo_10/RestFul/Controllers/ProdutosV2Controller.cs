using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using RestFul.Data;

namespace RestFul.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/produtos")]
    public class ProdutosV2Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosV2Controller(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os produtos, incluindo o preço com desconto.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetTodos()
        {
            var produtos = await _context.Produtos.ToListAsync();

            var resultado = produtos.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Preco,
                p.EmailFornecedor,
                PrecoComDesconto = Math.Round(p.Preco * 0.9m, 2)
            });

            return Ok(resultado);
        }
    }
}