namespace WebApiBIMU.Helpers.Query
{
    // Classe genérica Includes, onde T é um tipo genérico (ou seja, pode ser qualquer tipo).
    public class Includes<T>
    {
        // Construtor que recebe uma função que transforma um IQueryable e a armazena na propriedade Expression.
        public Includes(Func<IQueryable<T>, IQueryable<T>> expression)
        {
            Expression = expression;
        }

        // Propriedade que armazena a função que transforma um IQueryable.
        public Func<IQueryable<T>, IQueryable<T>> Expression { get; private set; }
    }
}
