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
    public class DiaSemanaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public DiaSemanaController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todos os dias da semana.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<DiaSemana>>>> Get()
        {
            var resposta = await _unitOfWork.diaSemana.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar um novo dia da semana.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<DiaSemana>>> Add(DiaSemana addDiaSemana)
        {
            var diaSemana = _mapper.Map<DiaSemana>(addDiaSemana);
            var resposta = await _unitOfWork.diaSemana.Add(diaSemana);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar um dia da semana existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<DiaSemana>>> Update(DiaSemana updateDiaSemana)
        {
            var resposta = await _unitOfWork.diaSemana.Update(_mapper.Map<DiaSemana>(updateDiaSemana), x => x.Id == updateDiaSemana.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter um dia da semana pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<DiaSemana>>> GetById(int id)
        {
            var resposta = await _unitOfWork.diaSemana.GetSingle(id);
            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }
    }
}

// Resumindo: A interface "IGenericoService" define métodos para gerenciar operações CRUD em uma entidade genérica, incluindo suporte para consultas dinâmicas com filtros, ordenação,