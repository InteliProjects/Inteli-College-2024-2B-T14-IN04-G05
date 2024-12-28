using WebApiBIMU.Models;

namespace WebApiBIMU.Services.AuthService
{
    public interface IAuthService
    {
        Task<RespostaDeServico<int>> Registrar(Usuario usuario, string password);
        Task<RespostaDeServico<string>> Login(string username, string password);
        Task<bool> UsuarioExiste(string usuario, int funcId);
        //Task<RespostaDeServico<GetUsuarioDto>> UpdateUsuario(UpdateUsuarioDto updateUsuario);
    }
}
