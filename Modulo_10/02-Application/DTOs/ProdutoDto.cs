namespace RestFul.DTOs
{
    // DTOs/ProdutoDto.cs — só os campos que o cliente pode enviar
    public record ProdutoDto(string Nome, decimal Preco, string? EmailFornecedor);
}

