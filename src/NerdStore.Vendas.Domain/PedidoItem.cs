using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        public PedidoItem(Guid produtoId, string nomeProduto, int quantidade, decimal valorUnitario)
        {
            if (quantidade > Pedido.MAX_UNIDADES_ITEM)
                throw new DomainExeption($"Maximo de {Pedido.MAX_UNIDADES_ITEM} unidades por produto");

            if (quantidade < Pedido.MIN_UNIDADES_ITEM)
                throw new DomainExeption($"Minimo de {Pedido.MAX_UNIDADES_ITEM} unidades por produto");

            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ProdutoId { get; private set; }
        public string NomeProduto { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public void AdicionarUnidades(int quantidade)
        {
            Quantidade += quantidade;
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}