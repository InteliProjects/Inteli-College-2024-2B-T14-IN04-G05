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
    public class HistoricoAcessoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public HistoricoAcessoController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todos os históricos de acesso.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<HistoricoAcesso>>>> Get()
        {
            var includes = new Includes<HistoricoAcesso>(query =>
            {
                return query.Include(p => p.Area)
                                .Include(p => p.Pessoa)
                                .ThenInclude(p => p.TipoPessoa);
            });

            var resposta = await _unitOfWork.historicoAcesso.GetAllAsync(null, includes.Expression, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar um novo histórico de acesso.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<HistoricoAcesso>>> Add(HistoricoAcesso addHistoricoAcesso)
        {
            var historicoAcesso = _mapper.Map<HistoricoAcesso>(addHistoricoAcesso);
            var resposta = await _unitOfWork.historicoAcesso.Add(historicoAcesso);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar um histórico de acesso existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<HistoricoAcesso>>> Update(HistoricoAcesso updateHistoricoAcesso)
        {
            var resposta = await _unitOfWork.historicoAcesso.Update(_mapper.Map<HistoricoAcesso>(updateHistoricoAcesso), x => x.Id == updateHistoricoAcesso.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter um histórico de acesso pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<HistoricoAcesso>>> GetById(int id)
        {
            var resposta = await _unitOfWork.historicoAcesso.GetSingle(id);
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
// A classe "HistoricoAcessoController" fornece endpoints para manipular os históricos de acesso, incluindo adição, atualização e consulta de históricos por meio de uma API RESTful.
