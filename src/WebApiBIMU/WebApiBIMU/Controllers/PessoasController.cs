using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.
using System.Reflection;  // Importa o namespace necessário para usar Reflection, que permite inspecionar tipos em tempo de execução.
using WebApiBIMU.Services.EventosService;  // Importa o serviço de eventos para ser usado na UnitOfWork.
using Microsoft.AspNetCore.Http;  // Importa o namespace necessário para a utilização do contexto HTTP.
using Microsoft.AspNetCore.Mvc;  // Importa o namespace necessário para a construção de controladores Web API.
   // Importa o serviço UnitOfWork.
using WebApiBIMU.DTOs;
using WebApiBIMU.DTOs.Pessoas;  // Importa os DTOs utilizados na aplicação.

namespace WebApiBIMU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  // UnitOfWork para acessar serviços e gerenciar transações.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias.
        public PessoasController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método GET para obter todas as pessoas.
        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<IEnumerable<Pessoas>>>> Get()
        {
            var resposta = await _unitOfWork.pessoas.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        [HttpGet("GetAllNomes")]
        public async Task<ActionResult<RespostaDeServico<List<Pessoas>>>> GetAllFunc()
        {
            var tipoCadPessoa = new TipoPessoa();
            Filter<Pessoas> filter = new Filter<Pessoas>(null);
            filter.AddExpression(p => p.TipoPessoa.Tipo_Pessoa_Desc == "Funcionario" || p.TipoPessoa.Tipo_Pessoa_Desc == "Professor");
            var resultado = await _unitOfWork.pessoas.QueryAsync(
                 filter.Expression,
                 null,
                 null,
                 p => new GetNomesPessoasDto
                 {
                     Id = p.Id,
                     Nome = p.Nome,
                 });

            var resposta = new RespostaDeServico<List<GetNomesPessoasDto>>();
            resposta.Dados = resultado.OrderBy(e => e.Nome).ToList();
            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados!.ToList().Count == 0)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar uma nova pessoa.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<Pessoas>>> Add(Pessoas addPessoas)
        {
            var pessoa = _mapper.Map<Pessoas>(addPessoas);
            var resposta = await _unitOfWork.pessoas.Add(pessoa);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar uma pessoa existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<Pessoas>>> Update(Pessoas updatePessoas)
        {
            var resposta = await _unitOfWork.pessoas.Update(_mapper.Map<Pessoas>(updatePessoas), x => x.Id == updatePessoas.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter uma pessoa pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<Pessoas>>> GetById(int id)
        {
            var resposta = await _unitOfWork.pessoas.GetSingle(id);
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
// A classe "PessoasController" fornece endpoints para manipular as pessoas, incluindo adição, atualização e consulta de pessoas por meio de uma API RESTful.
