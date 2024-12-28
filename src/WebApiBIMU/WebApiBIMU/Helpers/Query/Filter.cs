using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.

namespace WebApiBIMU.Helpers.Query
{
    // Classe genérica Filter, onde T é um tipo genérico (ou seja, pode ser qualquer tipo).
    public class Filter<T>
    {
        // Construtor que recebe uma expressão e a armazena na propriedade Expression.
        public Filter(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }

        // Propriedade que armazena a expressão lambda usada para filtrar.
        public Expression<Func<T, bool>> Expression { get; private set; }

        // Método para adicionar uma nova expressão à expressão existente, combinando-as com "AndAlso" (um "E" lógico).
        public void AddExpression(Expression<Func<T, bool>> newExpression)
        {
            // Verifica se a nova expressão é nula, e lança uma exceção caso seja.
            if (newExpression == null) throw new ArgumentNullException(nameof(newExpression), $"{nameof(newExpression)} is null.");

            // Se não houver uma expressão existente, define a expressão atual como a nova expressão.
            if (Expression == null) Expression = newExpression;

            // Cria um novo parâmetro do tipo T para ser usado nas expressões combinadas.
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T));

            // Cria um visitante de expressão para substituir o parâmetro da nova expressão pelo novo parâmetro.
            var leftVisitor = new ReplaceExpressionVisitor(newExpression.Parameters[0], parameter);
            var left = leftVisitor.Visit(newExpression.Body);  // Visita o corpo da nova expressão e aplica a substituição do parâmetro.

            // Cria um visitante de expressão para substituir o parâmetro da expressão existente pelo novo parâmetro.
            var rightVisitor = new ReplaceExpressionVisitor(Expression.Parameters[0], parameter);
            var right = rightVisitor.Visit(Expression.Body);  // Visita o corpo da expressão existente e aplica a substituição do parâmetro.

            // Combina as duas expressões com um "AndAlso" (que é um "E" lógico) e cria uma nova expressão lambda.
            Expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(System.Linq.Expressions.Expression.AndAlso(left, right), parameter);
        }
    }
}

// Resumindo: Esta classe "Filter" permite criar e combinar expressões lambda, que podem ser usadas para filtrar coleções de dados. 
// A expressão resultante é uma combinação de várias condições, unidas com o operador "E" lógico, através do método "AddExpression".
