using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiBIMU.Models;
using WebApiBIMU.Data;
using WebApiBIMU.DTOs.Usuarios;
using WebApiBIMU.Services.AuthService;
using WebApiBIMU.Services.UnitOfWork;
using Newtonsoft.Json;
//using WebApiBIMU.Resources.Auth;

namespace WebApiBIMU.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region AuthTeko
        private readonly IAuthService _authRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented // Indentação para facilitar a leitura

            //5858
        };
        public AuthController(IAuthService authRepo, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _authRepo = authRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<RespostaDeServico<UsuarioRegistrarDto>>> Register(UsuarioRegistrarDto request)
        {
            var response = await _authRepo.Registrar(
                new Usuario { Nome = request.Usuario!, Id_Pessoa = (int)request.FuncId! }, request.Password!);
            var jsonDados = new UsuarioRegistrarDto 
            { 
                Id = response.Dados,
                Usuario = request.Usuario,
                Password = request.Password,
            };

            var jsonResult = new RespostaDeServico<UsuarioRegistrarDto>
            {
                Dados = jsonDados,
                Sucesso = response.Sucesso,
                Mensagem = response.Mensagem,
            };


            if (!jsonResult.Sucesso)
            {
                return BadRequest(JsonConvert.SerializeObject(jsonResult, jsonSettings));
            }


            return Ok(JsonConvert.SerializeObject(jsonResult, jsonSettings));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<RespostaDeServico<int>>> Login(UsuarioLoginDto request)
        {

            var response = await _authRepo.Login(request.User, request.Password);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }
            return Ok(JsonConvert.SerializeObject(response, jsonSettings));
        }

        [HttpGet("GetUsuarios")]
        public async Task<ActionResult<RespostaDeServico<List<GetUsuarioDto>>>> GetUsuarios()
        {
            var resposta = await _unitOfWork.usuario.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados!.ToList().Count == 0)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        #endregion
    }
}
