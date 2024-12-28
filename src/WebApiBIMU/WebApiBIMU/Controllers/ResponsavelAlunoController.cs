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
    public class ResponsavelAlunoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public ResponsavelAlunoController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todos os responsáveis pelo aluno.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<ResponsavelAluno>>>> Get()
        {
            var resposta = await _unitOfWork.responsavelAluno.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar um novo responsável pelo aluno.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<ResponsavelAluno>>> Add(ResponsavelAluno addResponsavelAluno)
        {
            var responsavelAluno = _mapper.Map<ResponsavelAluno>(addResponsavelAluno);
            var resposta = await _unitOfWork.responsavelAluno.Add(responsavelAluno);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar um responsável pelo aluno existente.
        //[HttpPut]
        //public async Task<ActionResult<RespostaDeServico<ResponsavelAluno>>> Update(ResponsavelAluno updateResponsavelAluno)
        //{
        //    var resposta = await _unitOfWork.responsavelAluno.Update(_mapper.Map<ResponsavelAluno>(updateResponsavelAluno), x => x.Id == updateResponsavelAluno.Id);
        //    if (resposta.Dados is null)
        //        return NotFound(resposta);
        //    else
        //        return Ok(resposta);
        //}

        // Método GET para obter um responsável pelo aluno pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<ResponsavelAluno>>> GetById(int id)
        {
            var resposta = await _unitOfWork.responsavelAluno.GetSingle(id);
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
// A classe "UnitOfWork" implementa a interface "IUnitOfWork", iniciali