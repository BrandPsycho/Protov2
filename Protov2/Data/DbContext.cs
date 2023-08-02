namespace Protov2.Data
{
    public class DbContext
    {
        public DbContext(string valor) => Valor = valor;

        public string Valor { get; }
        
    }
}
