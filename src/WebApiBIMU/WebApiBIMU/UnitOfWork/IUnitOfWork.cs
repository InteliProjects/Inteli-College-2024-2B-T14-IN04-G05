using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.
using System.Reflection;  // Importa o namespace necessário para usar Reflection, que permite inspecionar tipos em tempo de execução.
using WebApiBIMU.Services.AlunoMateriaService;
using WebApiBIMU.Services.AreaAcessoService;
using WebApiBIMU.Services.AulaService;
using WebApiBIMU.Services.DataMateriaService;
using WebApiBIMU.Services.DiaSemanaService;
using WebApiBIMU.Services.EventosService;  // Importa o serviço de eventos para ser usado na UnitOfWork.
using WebApiBIMU.Services.FreqAulaService;
using WebApiBIMU.Services.HistoricoAcessoService;
using WebApiBIMU.Services.MateriaService;
using WebApiBIMU.Services.PessoasService;
using WebApiBIMU.Services.ResponsavelAlunoService;
using WebApiBIMU.Services.TipoPessoaService;
using WebApiBIMU.Services.UsuarioService;

namespace WebApiBIMU.Services.UnitOfWork
{
    // Interface IUnitOfWork, usada para gerenciar transações e o ciclo de vida dos serviços.
    public interface IUnitOfWork : IDisposable
    {
        IEventosService eventos { get; }  // Propriedade para acessar o serviço de eventos.
        IAreaAcessoService areaAcesso { get; }
        IAlunoMateriaService alunoMateria { get; }
        IAulaService aula { get; }
        IDataMateriaService dataMateria { get; }
        IDiaSemanaService diaSemana { get; }
        IFreqAulaService freqAula { get; }
        IHistoricoAcessoService historicoAcesso { get; }
        IMateriaService materia { get; }
        IPessoasService pessoas { get; }
        IResponsavelAlunoService responsavelAluno { get; }
        ITipoPessoaService tipoPessoa { get; }
        IUsuarioService usuario { get; }
        int Save();  // Método para salvar as mudanças no contexto de dados.
    }
}