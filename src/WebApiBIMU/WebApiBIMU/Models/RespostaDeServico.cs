namespace WebApiBIMU.Models
{
    public class RespostaDeServico<T>
    {
        public T? Dados { get; set; }
        public bool Sucesso { get; set; } = true;
        public string? Mensagem { get; set; }
    }
}
