using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.
using System.Reflection;  // Importa o namespace necessário para usar Reflection, que permite inspecionar tipos em tempo de execução.

namespace WebApiBIMU.Services.GenericosService
{
    // Classe genérica GenericoService que implementa IGenericoService, usada para gerenciar operações básicas em entidades do tipo T.
    public class GenericoService<T> : IGenericoService<T> where T : class
    {
        public readonly IMapper _mapper;  // Mapeador de objetos (ex.: mapeia entre DTOs e entidades).
        public readonly DataContext _context;  // Contexto de dados para interação com o banco de dados.
        public readonly IHttpContextAccessor _httpContextAccessor;  // Acessor para obter informações do contexto HTTP.

        #region Construtor
        // Construtor que recebe as dependências necessárias.
        public GenericoService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        #endregion

        #region Métodos Dinâmicos
        // Método para adicionar uma nova entidade ao banco de dados.
        async Task<RespostaDeServico<T>> IGenericoService<T>.Add(T model, bool salvarRelacionados = true, Expression<Func<T, bool>> filter = null)
        {
            var resposta = new RespostaDeServico<T>();
            try
            {
                // Verifica se já existe um registro que atenda ao filtro.
                if (filter != null)
                {
                    var registro = QueryDb(filter, null, null).ToList();
                    if (registro.Count != 0)
                        throw new Exception("Já Registrado!");
                }

                // Se não for necessário salvar os relacionamentos, define o estado da entidade como "Added".
                if (!salvarRelacionados)
                    _context.Entry(model).State = EntityState.Added;

                _context.Set<T>().Add(model);  // Adiciona a entidade ao contexto.
                await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados.
                _context.Dispose();
                resposta.Dados = model;
                resposta.Mensagem = "Salvo com sucesso";
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;  // Retorna a mensagem de erro, caso ocorra.
            }
            return resposta;
        }

        // Método para deletar uma entidade pelo ID (não implementado completamente).
        Task<RespostaDeServico<T>?> IGenericoService<T>.Delete(int id)
        {
            return null;
        }

        // Método para obter uma única entidade pelo ID.
        public async virtual Task<RespostaDeServico<T?>> GetSingle(int id)
        {
            var resposta = new RespostaDeServico<T>();
            try
            {
                var item = await _context.Set<T>().FindAsync(id);  // Busca a entidade pelo ID.
                resposta.Dados = item;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;  // Retorna a mensagem de erro, caso ocorra.
            }
            return resposta;
        }
        #endregion

        #region Querys Dinâmicas
        // Método para obter uma lista de entidades usando uma expressão lambda como filtro.
        RespostaDeServico<List<T>> IGenericoService<T>.GetLambda(Func<T, bool> expressaoLambda)
        {
            var resposta = new RespostaDeServico<List<T>>();
            try
            {
                resposta.Dados = _context.Set<T>().Where(expressaoLambda).ToList();  // Filtra os dados com base na expressão lambda.
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;  // Retorna a mensagem de erro, caso ocorra.
            }
            return resposta;
        }

        // Método para atualizar uma entidade com base em um filtro.
        async Task<RespostaDeServico<T>> IGenericoService<T>.Update(T update, Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var resposta = new RespostaDeServico<T>();
            try
            {
                var registro = QueryDb(filter, orderBy, includes).ToList().First();  // Busca o registro que será atualizado.
                if (registro is null)
                    throw new Exception("Registro não encontrado!");

                _mapper.Map(update, registro);  // Mapeia as alterações para o registro encontrado.
                await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados.
                resposta.Dados = registro;
                resposta.Mensagem = "Atualizado";
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;  // Retorna a mensagem de erro, caso ocorra.
            }
            return resposta;
        }

        // Método para obter todas as entidades, com suporte para paginação.
        RespostaDeServico<IEnumerable<T>> IGenericoService<T>.GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null, Expression<Func<T, bool>> filter = null, int? page = null)
        {
            var resposta = new RespostaDeServico<IEnumerable<T>>();
            try
            {
                int pageSize = 3;  // Define o tamanho da página.
                int pageNumber = (page ?? 1);  // Define o número da página (caso não seja fornecido, é 1).

                resposta.Dados = QueryDb(null, orderBy, includes)
                                    .Skip((pageSize - 1) * pageNumber)
                                    .Take(pageSize)
                                    .ToList();  // Realiza a paginação e obtém os resultados.
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;  // Retorna a mensagem de erro, caso ocorra.
            }
            return resposta;
        }

        // Método assíncrono para obter todas as entidades com suporte para filtros e ordenação.
        async Task<RespostaDeServico<IEnumerable<T>>> IGenericoService<T>.GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null, Expression<Func<T, bool>> filter = null)
        {
            var resposta = new RespostaDeServico<IEnumerable<T>>();
            try
            {
                resposta.Dados = await QueryDb(filter, orderBy, includes).ToListAsync();

                if (resposta.Dados!.ToList().Count == 0)
                    resposta.Mensagem = "Registro não encontrado";
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;  // Retorna a mensagem de erro, caso ocorra.
            }
            return resposta;
        }

        // Método para realizar uma consulta dinâmica com base em um filtro e ordenação.
        public virtual IEnumerable<T> Query(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return result.ToList();
        }

        // Método assíncrono para realizar uma consulta com seleção de resultados.
        public virtual async Task<IEnumerable<TResult>> QueryAsync<TResult>(
             Expression<Func<T, bool>> filter,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             Func<IQueryable<T>, IQueryable<T>> includes = null,
             Expression<Func<T, TResult>> selector = null)
        {
            var result = QueryDb(filter, orderBy, includes, selector);
            return await result.ToListAsync();
        }

        // Método protegido que cria a consulta dinâmica com suporte para seleção.
        protected IQueryable<TResult> QueryDb<TResult>(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Func<IQueryable<T>, IQueryable<T>> includes,
            Expression<Func<T, TResult>> selector)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);  // Aplica o filtro à consulta.
            }

            if (includes != null)
            {
                query = includes(query);  // Adiciona entidades relacionadas à consulta.
            }

            if (orderBy != null)
            {
                query = orderBy(query);  // Aplica a ordenação à consulta.
            }

            if (selector != null)
            {
                return query.Select(selector);  // Aplica a seleção à consulta.
            }

            return (IQueryable<TResult>)query;
        }

        // Método assíncrono para realizar uma consulta com base em um filtro e ordenação.
        public virtual async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return await result.ToListAsync();
        }

        // Método protegido que cria a consulta dinâmica sem seleção específica.
        protected IQueryable<T> QueryDb(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IQueryable<T>> includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);  // Aplica o filtro à consulta.
            }

            if (includes != null)
            {
                query = includes(query);  // Adiciona entidades relacionadas à consulta.
            }

            if (orderBy != null)
            {
                query = orderBy(query);  // Aplica a ordenação à consulta.
            }

            return query;
        }
        #endregion
    }
}

// Resumindo: A classe "GenericoService" implementa métodos para gerenciar operações CRUD em uma entidade genérica, incluindo suporte para consultas dinâmicas com filtros, ordenação e paginação. Os métodos são assíncronos quando necessário e utilizam o Entity Framework para interagir com o banco de dados.
