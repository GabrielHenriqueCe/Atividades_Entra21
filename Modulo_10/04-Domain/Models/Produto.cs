using System.ComponentModel.DataAnnotations;

public class Produto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; }

    [Range(0.01, 99999, ErrorMessage = "Preço inválido")]
    public decimal Preco { get; set; }

    [EmailAddress]
    public string? EmailFornecedor { get; set; }
}
