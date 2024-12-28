using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.
using System.Reflection;  // Importa o namespace necessário para usar Reflection, que permite inspecionar tipos em tempo de execução.

namespace WebApiBIMU.Services.GenericosService
{
    // Interface genérica IGenericoService, usada para definir operações básicas em entidades do tipo T.
    public interface IGenericoService<T> where T : class
    {
        // Método para obter uma única entidade pelo ID.
        Task<RespostaDeServico<T?>> GetSingle(int id);

        // Método para obter todas as entidades com suporte para ordenação, inclusão de entidades relacionadas e paginação.
        RespostaDeServico<IEnumerable<T>> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null, Expression<Func<T, bool>> filter = null, int? page = null);

        // Método assíncrono para obter todas as entidades com suporte para filtros, ordenação e inclusão de entidades relacionadas.
        Task<RespostaDeServico<IEnumerable<T>>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null, Expression<Func<T, bool>> filter = null);

        // Método assíncrono para realizar uma consulta com seleção de resultados específicos.
        Task<IEnumerable<TResult>> QueryAsync<TResult>(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IQueryable<T>> includes = null,
            Expression<Func<T, TResult>> selector = null);

        // Método assíncrono para realizar uma consulta dinâmica com base em um filtro e ordenação.
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null);

        // Método para atualizar uma entidade com base em um filtro e incluir ordenação e inclusão de entidades relacionadas.
        Task<RespostaDeServico<T>> Update(T updatePessoa, Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null);

        // Método para adicionar uma nova entidade ao banco de dados.
        Task<RespostaDeServico<T>> Add(T model, bool salvarRelacionados = true, Expression<Func<T, bool>> filter = null);

        // Método para deletar uma entidade pelo ID.
        Task<RespostaDeServico<T>?> Delete(int id);

        // Método para obter uma lista de entidades usando uma expressão lambda como filtro.
        RespostaDeServico<List<T>> GetLambda(Func<T, bool> expressaoLambda);
    }
}

// Resumindo: A interface "IGenericoService" define métodos para gerenciar operações CRUD em uma entidade genérica, incluindo suporte para consultas dinâmicas com filtros, ordenação, inclusão de entidades relacionadas e paginação.
