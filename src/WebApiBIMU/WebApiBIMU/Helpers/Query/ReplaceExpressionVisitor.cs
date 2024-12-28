using System.Linq.Expressions;  // Importa o namespace necessário para usar expressões lambda.

namespace WebApiBIMU.Helpers.Query
{
    // Classe ReplaceExpressionVisitor, usada para substituir expressões dentro de uma árvore de expressão.
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;  // Armazena a expressão antiga que será substituída.
        private readonly Expression _newValue;  // Armazena a nova expressão que substituirá a antiga.

        // Construtor que recebe a expressão antiga e a nova expressão.
        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        // Método que visita cada nó da árvore de expressão e realiza a substituição se o nó atual for igual à expressão antiga.
        public override Expression Visit(Expression node)
        {
            // Se o nó atual for igual à expressão antiga, retorna a nova expressão.
            if (node == _oldValue) return _newValue;
            // Caso contrário, chama o método base para continuar visitando a árvore.
            return base.Visit(node);
        }
    }
}

// Resumindo: A classe "ReplaceExpressionVisitor" permite substituir uma expressão específica por outra dentro de uma árvore de expressões. 
// Isso é útil quando você deseja modificar uma expressão complexa, como ao combinar várias condições ou adaptar uma expressão existente.
