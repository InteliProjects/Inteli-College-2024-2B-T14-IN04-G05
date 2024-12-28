using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
using WebApiBIMU.DTOs;

namespace WebApiBIMU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public EventosController(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<RespostaDeServico<Eventos>>> Get()
        {

            var resposta = await _unitOfWork.eventos.GetAllAsync(null, null, null);

            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método POST para adicionar um novo evento.
        [HttpPost]
        public async Task<ActionResult<RespostaDeServico<Eventos>>> Add(AddEventoDto addEvento)
        {
            var evento = _mapper.Map<Eventos>(addEvento);
            var resposta = await _unitOfWork.eventos.Add(evento);
            if (!resposta.Sucesso)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        // Método PUT para atualizar um evento existente.
        [HttpPut]
        public async Task<ActionResult<RespostaDeServico<Eventos>>> Update(UpdateEventoDto updateEvento)
        {
            var resposta = await _unitOfWork.eventos.Update(_mapper.Map<Eventos>(updateEvento), x => x.Id == updateEvento.Id);
            if (resposta.Dados is null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }

        // Método GET para obter um evento pelo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDeServico<Eventos>>> GetById([FromHeader] int id)
        {
            var resposta = await _unitOfWork.eventos.GetSingle(id);
            if (!resposta.Sucesso)
                return BadRequest(resposta);
            else if (resposta.Dados == null)
                return NotFound(resposta);
            else
                return Ok(resposta);
        }
    }
}
