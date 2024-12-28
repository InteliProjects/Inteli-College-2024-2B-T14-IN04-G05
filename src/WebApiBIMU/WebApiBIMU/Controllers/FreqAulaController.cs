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
    public class FreqAulaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public FreqAulaController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todas as frequências de aula.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<FreqAula>>>> Get()
        {
            var resposta = await _unitOfWork.freqAula.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar uma nova frequência de aula.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<FreqAula>>> Add(FreqAula addFreqAula)
        {
            var freqAula = _mapper.Map<FreqAula>(addFreqAula);
            var resposta = await _unitOfWork.freqAula.Add(freqAula);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar uma frequência de aula existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<FreqAula>>> Update(FreqAula updateFreqAula)
        {
            var resposta = await _unitOfWork.freqAula.Update(_mapper.Map<FreqAula>(updateFreqAula), x => x.Id == updateFreqAula.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter uma frequência de aula pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<FreqAula>>> GetById(int id)
        {
            var resposta = await _unitOfWork.freqAula.GetSingle(id);
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
// A classe "FreqAulaController" fornece endpoints para manipular as frequências de aula, incluindo adição, atualização e consulta de frequências por meio de uma API RESTful.
