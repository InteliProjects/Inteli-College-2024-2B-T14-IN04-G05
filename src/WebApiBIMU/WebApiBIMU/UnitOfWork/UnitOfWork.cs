using WebApiBIMU.Services.AlunoMateriaService;
using WebApiBIMU.Services.AreaAcessoService;
using WebApiBIMU.Services.AulaService;
using WebApiBIMU.Services.DataMateriaService;
using WebApiBIMU.Services.DiaSemanaService;
using WebApiBIMU.Services.EventosService;
using WebApiBIMU.Services.FreqAulaService;
using WebApiBIMU.Services.HistoricoAcessoService;
using WebApiBIMU.Services.MateriaService;
using WebApiBIMU.Services.PessoasService;
using WebApiBIMU.Services.ResponsavelAlunoService;
using WebApiBIMU.Services.TipoPessoaService;
using WebApiBIMU.Services.UsuarioService;

namespace WebApiBIMU.Services.UnitOfWork
{
    // Classe UnitOfWork que implementa a interface IUnitOfWork, responsável por gerenciar serviços e o contexto de dados.
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;  // Contexto de dados para interagir com o banco de dados.
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.
        private readonly IMapper _mapper;  // Mapeador para realizar conversões entre objetos.

        // Construtor que recebe as dependências necessárias e inicializa o serviço de eventos.
        public UnitOfWork(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            eventos = new EventosService.EventosService(_mapper, _context, _httpContextAccessor);  // Inicializa o serviço de eventos.
            areaAcesso = new AreaAcessoService.AreaAcessoService(_mapper, _context, _httpContextAccessor);
            alunoMateria = new AlunoMateriaService.AlunoMateriaService(_mapper, _context, _httpContextAccessor);
            aula = new AulaService.AulaService(_mapper, _context, _httpContextAccessor);
            dataMateria = new DataMateriaService.DataMateriaService(_mapper, _context, _httpContextAccessor);
            diaSemana = new DiaSemanaService.DiaSemanaService(_mapper, _context, _httpContextAccessor);
            freqAula = new FreqAulaService.FreqAulaService(_mapper, _context, _httpContextAccessor);
            historicoAcesso = new HistoricoAcessoService.HistoricoAcessoService(_mapper, _context, _httpContextAccessor);
            materia = new MateriaService.MateriaService(_mapper, _context, _httpContextAccessor);
            pessoas = new PessoasService.PessoasService(_mapper, _context, _httpContextAccessor);
            responsavelAluno = new ResponsavelAlunoService.ResponsavelAlunoService(_mapper, _context, _httpContextAccessor);
            tipoPessoa = new TipoPessoaService.TipoPessoaService(_mapper, _context, _httpContextAccessor);
            usuario = new UsuarioService.UsuarioService(_mapper, _context, _httpContextAccessor);
        }

        public IEventosService eventos { get; private set; }  // Propriedade que expõe o serviço de eventos.
        public IAreaAcessoService areaAcesso { get; }
        public IAlunoMateriaService alunoMateria { get; }
        public IAulaService aula { get; }
        public IDataMateriaService dataMateria { get; }
        public IDiaSemanaService diaSemana { get; }
        public IFreqAulaService freqAula { get; }
        public IHistoricoAcessoService historicoAcesso { get; }
        public IMateriaService materia { get; }
        public IPessoasService pessoas { get; }
        public IResponsavelAlunoService responsavelAluno { get; }
        public ITipoPessoaService tipoPessoa { get; }
        public IUsuarioService usuario { get; }
        // Método para salvar as mudanças feitas no contexto de dados.
        public int Save()
        {
            return _context.SaveChanges();  // Salva as alterações no banco de dados.
        }

        // Método para liberar recursos usados pelo contexto de dados.
        public void Dispose()
        {
            _context.Dispose();  // Libera o contexto de dados para evitar vazamento de memória.
        }
    }
}
