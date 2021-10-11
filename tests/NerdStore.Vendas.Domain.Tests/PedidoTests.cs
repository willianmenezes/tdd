﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar item novo pedido")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);

            // act
            pedido.AdicionarItem(pedidoItem);

            // assert
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar item pedido existente")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarQuantidadeSomarValores()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 2, 100);

            // act
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem);

            // assert
            Assert.Equal(400, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(4, pedido.PedidoItems.First(x => x.ProdutoId == produtoId).Quantidade);
        }
    }
}