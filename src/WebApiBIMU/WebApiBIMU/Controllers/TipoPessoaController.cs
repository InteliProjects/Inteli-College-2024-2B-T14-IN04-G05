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
    public class TipoPessoaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public TipoPessoaController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todos os tipos de pessoa.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<TipoPessoa>>>> Get()
        {
            var resposta = await _unitOfWork.tipoPessoa.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar um novo tipo de pessoa.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<TipoPessoa>>> Add(TipoPessoa addTipoPessoa)
        {
            var tipoPessoa = _mapper.Map<TipoPessoa>(addTipoPessoa);
            var resposta = await _unitOfWork.tipoPessoa.Add(tipoPessoa);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar um tipo de pessoa existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<TipoPessoa>>> Update(TipoPessoa updateTipoPessoa)
        {
            var resposta = await _unitOfWork.tipoPessoa.Update(_mapper.Map<TipoPessoa>(updateTipoPessoa), x => x.Id == updateTipoPessoa.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter um tipo de pessoa pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<TipoPessoa>>> GetById(int id)
        {
            var resposta = await _unitOfWork.tipoPessoa.GetSingle(id);
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
// A classe "TipoPessoaController" fornece endpoints para manipular os tipos de pessoa, incluindo adição, atualização e consulta de tipos por meio de uma API RESTful.
