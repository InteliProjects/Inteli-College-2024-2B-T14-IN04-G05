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
    public class MateriaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public MateriaController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todas as matérias.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<Materia>>>> Get()
        {
            var resposta = await _unitOfWork.materia.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar uma nova matéria.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<Materia>>> Add(Materia addMateria)
        {
            var materia = _mapper.Map<Materia>(addMateria);
            var resposta = await _unitOfWork.materia.Add(materia);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar uma matéria existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<Materia>>> Update(Materia updateMateria)
        {
            var resposta = await _unitOfWork.materia.Update(_mapper.Map<Materia>(updateMateria), x => x.Id == updateMateria.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter uma matéria pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<Materia>>> GetById(int id)
        {
            var resposta = await _unitOfWork.materia.GetSingle(id);
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
// A classe "MateriaController" fornece endpoints para manipular as matérias, incluindo adição, atualização e consulta de matérias por meio de uma API RESTful.