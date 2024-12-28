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
    public class DataMateriaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public DataMateriaController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todas as relações Data-Materia.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<DataMateria>>>> Get()
        {
            var resposta = await _unitOfWork.dataMateria.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar uma nova relação Data-Materia.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<DataMateria>>> Add(DataMateria addDataMateria)
        {
            var dataMateria = _mapper.Map<DataMateria>(addDataMateria);
            var resposta = await _unitOfWork.dataMateria.Add(dataMateria);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar uma relação Data-Materia existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<DataMateria>>> Update(DataMateria updateDataMateria)
        {
            var resposta = await _unitOfWork.dataMateria.Update(_mapper.Map<DataMateria>(updateDataMateria), x => x.Id == updateDataMateria.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter uma relação Data-Materia pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<DataMateria>>> GetById(int id)
        {
            var resposta = await _unitOfWork.dataMateria.GetSingle(id);
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
// A classe "DataMateriaController" fornece endpoints para manipular as relações Data-Materia, incluindo adição, atualização e consulta de relações por meio de uma API RESTful.