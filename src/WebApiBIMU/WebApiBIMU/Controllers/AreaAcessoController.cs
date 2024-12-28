using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.
using System.Reflection;  // Importa o namespace necessário para usar Reflection, que permite inspecionar tipos em tempo de execução.
using WebApiBIMU.Services.EventosService;  // Importa o serviço de eventos para ser usado na UnitOfWork.
using Microsoft.AspNetCore.Http;  // Importa o namespace necessário para a utilização do contexto HTTP.
using Microsoft.AspNetCore.Mvc;  // Importa o namespace necessário para a construção de controladores Web API.
   // Importa o serviço UnitOfWork.
using WebApiBIMU.DTOs;  // Importa os DTOs utilizados na aplicação.

namespace WebApiBIMU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaAcessoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public AreaAcessoController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todas as áreas de acesso.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<AreaAcesso>>>> Get()
        {
            var resposta = await _unitOfWork.areaAcesso.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar uma nova área de acesso.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<AreaAcesso>>> Add(AreaAcesso addAreaAcesso)
        {
            var areaAcesso = _mapper.Map<AreaAcesso>(addAreaAcesso);
            var resposta = await _unitOfWork.areaAcesso.Add(areaAcesso);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar uma área de acesso existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<AreaAcesso>>> Update(AreaAcesso updateAreaAcesso)
        {
            var resposta = await _unitOfWork.areaAcesso.Update(_mapper.Map<AreaAcesso>(updateAreaAcesso), x => x.Id == updateAreaAcesso.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter uma área de acesso pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<AreaAcesso>>> GetById(int id)
        {
            var resposta = await _unitOfWork.areaAcesso.GetSingle(id);
            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }
    }
}

// Resumindo: A interface "IGenericoService" define métodos para gerenciar operações CRUD em uma entidade genérica, incluindo suporte para consultas dinâmicas com filtros, ordenação, inclusão de entidades relacionadas e paginação.
// A interface "IUnitOfWork" é responsável por agrupar múltiplos serviços e gerenciar transações, facilitando o salvamento das mudanças no contexto.
// A classe "UnitOfWork" implementa a interface "IUnitOfWork", inicializa e gerencia o ciclo de vida dos serviços e o contexto de dados.
// A classe "AreaAcessoController" fornece endpoints para manipular as áreas de acesso, incluindo adição, atualização e consulta de áreas por meio de uma API RESTful.
