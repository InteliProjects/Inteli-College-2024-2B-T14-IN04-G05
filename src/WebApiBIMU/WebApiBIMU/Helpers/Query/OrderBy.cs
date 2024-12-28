using System.Linq.Expressions;
using System.Reflection;

namespace WebApiBIMU.Helpers.Query
{
    // Classe genérica OrderBy, onde TEntity é um tipo genérico (ou seja, pode ser qualquer tipo).
    public class OrderBy<TEntity>
    {
        // Construtor que recebe uma expressão e a armazena na propriedade Expression.
        public OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> expression)
        {
            Expression = expression;
        }

        // Construtor que recebe o nome de uma coluna e um booleano indicando se a ordenação é reversa.
        public OrderBy(string columnName, bool reverse)
        {
            Expression = GetOrderBy(columnName, reverse);
        }

        // Propriedade que armazena a expressão usada para ordenar.
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> Expression { get; private set; }

        // Método privado que cria uma expressão de ordenação com base no nome da coluna e na direção (ascendente ou descendente).
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string columnName, bool reverse)
        {
            // Cria o tipo IQueryable para TEntity e define os parâmetros para as expressões.
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = System.Linq.Expressions.Expression.Parameter(typeQueryable, "p");
            var outerExpression = System.Linq.Expressions.Expression.Lambda(argQueryable, argQueryable);

            // Cria uma consulta IQueryable vazia de TEntity.
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            var entityType = typeof(TEntity);
            ParameterExpression arg = System.Linq.Expressions.Expression.Parameter(entityType, "x");

            // Cria a expressão para acessar a propriedade com base no nome da coluna.
            Expression expression = arg;
            string[] properties = columnName.Split('.');
            foreach (string propertyName in properties)
            {
                // Usa Reflection para obter informações sobre a propriedade e cria a expressão para acessá-la.
                PropertyInfo propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expression = System.Linq.Expressions.Expression.Property(expression, propertyInfo);
                entityType = propertyInfo.PropertyType;
            }

            // Cria uma expressão lambda para acessar a propriedade.
            LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(expression, arg);
            string methodName = reverse ? "OrderByDescending" : "OrderBy";

            // Cria a chamada ao método OrderBy ou OrderByDescending usando a expressão criada.
            MethodCallExpression resultExp = System.Linq.Expressions.Expression.Call(typeof(Queryable),
                                                                                     methodName,
                                                                                     new Type[] { typeof(TEntity), entityType },
                                                                                     outerExpression.Body,
                                                                                     System.Linq.Expressions.Expression.Quote(lambda));

            // Cria uma expressão lambda final que será compilada em uma função.
            var finalLambda = System.Linq.Expressions.Expression.Lambda(resultExp, argQueryable);

            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }
    }
}
