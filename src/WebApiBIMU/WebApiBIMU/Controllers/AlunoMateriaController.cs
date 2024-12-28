using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.
using System.Reflection;  // Importa o namespace necessário para usar Reflection, que permite inspecionar tipos em tempo de execução.
using WebApiBIMU.Services.EventosService;  // Importa o serviço de eventos para ser usado na UnitOfWork.
using Microsoft.AspNetCore.Http;  // Importa o namespace necessário para a utilização do contexto HTTP.
using Microsoft.AspNetCore.Mvc;  // Importa o namespace necessário para a construção de controladores Web API.
   // Importa o serviço UnitOfWork.
using WebApiBIMU.DTOs;

namespace WebApiBIMU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoMateriaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public AlunoMateriaController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todas as relações Aluno-Materia.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<AlunoMateria>>>> Get()
        {
            var resposta = await _unitOfWork.alunoMateria.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar uma nova relação Aluno-Materia.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<AlunoMateria>>> Add(AlunoMateria addAlunoMateria)
        {
            var alunoMateria = _mapper.Map<AlunoMateria>(addAlunoMateria);
            var resposta = await _unitOfWork.alunoMateria.Add(alunoMateria);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar uma relação Aluno-Materia existente.
        //[HttpPut]
        //public async Task<ActionResult<RespostaDeServico<AlunoMateria>>> Update(AlunoMateria updateAlunoMateria)
        //{
        //    var resposta = await _unitOfWork.alunoMateria.Update(_mapper.Map<AlunoMateria>(updateAlunoMateria), x => x.Id == updateAlunoMateria.Id);
        //    if (resposta.Dados is null)
        //        return NotFound(resposta);
        //    else
        //        return Ok(resposta);
        //}

        // Método GET para obter uma relação Aluno-Materia pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<AlunoMateria>>> GetById(int id)
        {
            var resposta = await _unitOfWork.alunoMateria.GetSingle(id);
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
// A classe "AlunoMateriaController" fornece endpoints para manipular as relações Aluno-Materia, incluindo adição, atualização e consulta de relações por meio de uma API RESTful.
