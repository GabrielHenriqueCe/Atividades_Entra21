using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace RestFul.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaudacaoController : ControllerBase
    {
        public record SaudacaoResposta(string Mensagem, DateTime Hora);

        [HttpGet("{nome}")]
        public IActionResult Saudacao(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("O nome não pode ser vazio");

            return Ok(new SaudacaoResposta($"Olá, {nome}!", DateTime.Now));
        }
    }
}
