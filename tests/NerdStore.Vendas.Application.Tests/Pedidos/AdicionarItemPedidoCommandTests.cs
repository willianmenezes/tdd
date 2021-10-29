using System;
using NerdStore.Vendas.Application.Commands;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar item com command valido")]
        [Trait("Categoria", "Vendas - Pedido commands")]
        public void AdicionarItemPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            //arranje
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto teste", 2, 100);

            //act
            var result = pedidoCommand.EhValido();

            //assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Adicionar item com command invalido")]
        [Trait("Categoria", "Vendas - Pedido commands")]
        public void AdicionarItemPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            //arranje
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            //act
            var result = pedidoCommand.EhValido();

            //assert
            Assert.False(result);
        }
    }
}