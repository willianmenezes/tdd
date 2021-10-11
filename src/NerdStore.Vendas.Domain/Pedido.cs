using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public decimal ValorTotal { get; private set; }
        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            _pedidoItems.Add(pedidoItem);
            ValorTotal = PedidoItems.Sum(x => x.ValorUnitario * x.Quantidade);
        }

    }

    public class PedidoItem
    {
        public PedidoItem(Guid produtoId, string nomeProduto, int quantidade, decimal valorUnitario)
        {
            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ProdutoId { get; private set; }
        public string NomeProduto { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
    }
}
