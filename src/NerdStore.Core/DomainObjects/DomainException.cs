namespace NerdStore.Core.DomainObjects
{
    public class DomainExeption : Exception
    {
        public DomainExeption() { }

        public DomainExeption(string mensagen) : base(mensagen) { }

        public DomainExeption(string mensagem, Exception innerExeption) : base(mensagem, innerExeption) { }
    }
}
