using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public  class PedidoTests
    {
        [Fact(DisplayName = "Adicionar item nove pedido")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arranje
            var pedido = new Pedido();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);

            // act
            pedido.AdicionarItem(pedidoItem);

            // assert
            Assert.Equal(200, pedido.ValorTotal);
        }
    }
}
