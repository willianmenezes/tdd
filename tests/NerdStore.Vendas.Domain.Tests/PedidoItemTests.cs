using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Criar item pedido com quantidade abaixo do permitido")]
        [Trait("Categoria", "Item pedido Tests")]
        public void AdicionarItemPedido_UnidadesAbaixoDoPermitido_DeveRetornarExeption()
        {
            // Arranje & act & assert
            Assert.Throws<DomainExeption>(() => new PedidoItem(Guid.NewGuid(), "Produto teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));
        }

        [Fact(DisplayName = "Criar item pedido com quantidade acima do permitido")]
        [Trait("Categoria", "Item pedido Tests")]
        public void AdicionarItemPedido_UnidadesAcimaDoPermitido_DeveRetornarExeption()
        {
            // Arranje & act & assert
            Assert.Throws<DomainExeption>(() => new PedidoItem(Guid.NewGuid(), "Produto teste", Pedido.MAX_UNIDADES_ITEM + 1, 100));
        }
    }
}
