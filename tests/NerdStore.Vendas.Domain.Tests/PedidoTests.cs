using NerdStore.Core.DomainObjects;
using System;
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
        [Trait("Categoria", "Vendas - Pedido")]
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
        [Trait("Categoria", "Vendas - Pedido")]
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
            Assert.Equal(4, pedido.PedidoItems.FirstOrDefault(x => x.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionar item pedido existente quantidade nao permitida")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", Pedido.MAX_UNIDADES_ITEM, 100);
            pedido.AdicionarItem(pedidoItem);

            // act && assert
            Assert.Throws<DomainExeption>(() => pedido.AdicionarItem(pedidoItem));
        }

        [Fact(DisplayName = "Atualizar item pedido inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExisteNaLista_DeveRetornarExeption()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Produto teste", Pedido.MAX_UNIDADES_ITEM, 100);

            // act && assert
            Assert.Throws<DomainExeption>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar item pedido valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            // act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // assert
            Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(x => x.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Atualizar item validar valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarValorTotalPedido()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", 5, 100);

            // act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // assert
            Assert.Equal(500, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Remover item inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemInexistente_DeveRetornarExeption()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var novoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 5, 100);

            // act && assert
            Assert.Throws<DomainExeption>(() => pedido.RemoverItem(novoItem));
        }

        [Fact(DisplayName = "Recalcular valor total do pedido ao remover um item")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveRecalcularValorTotalPedido()
        {
            // Arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);
            var novoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 5, 100);
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(novoItem);

            var totalPedido = pedidoItem.ValorUnitario * pedidoItem.Quantidade;

            // act
            pedido.RemoverItem(novoItem);

            // assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            // arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Valor, 15, null, 1, DateTime.Now.AddDays(15), true, false);

            //act
            var result = pedido.AplicarVoucher(voucher);

            //assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher invalido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarErros()
        {
            // arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("", TipoDescontoVoucher.Valor, null, null, 0, DateTime.Now.AddDays(-1), false, true);

            //act
            var result = pedido.AplicarVoucher(voucher);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher tipo valor e descontar do total do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_VoucherTipoValorDesconto_DeveDescontarValorDoTotal()
        {
            // arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Valor, 15, null, 1, DateTime.Now.AddDays(15), true, false);
            var valorDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            // act
            _ = pedido.AplicarVoucher(voucher);

            //assert
            Assert.Equal(pedido.ValorTotal, valorDesconto);
        }

        [Fact(DisplayName = "Aplicar voucher tipo porcentagem e descontar do total do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_VoucherTipoPorcentagemDesconto_DeveDescontarValorDoTotal()
        {
            // arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Porcentagem, null, 10, 1, DateTime.Now.AddDays(15), true, false);
            var valorDesconto = pedido.ValorTotal - (pedido.ValorTotal * voucher.PercentualDesconto / 100);

            // act
            _ = pedido.AplicarVoucher(voucher);

            //assert
            Assert.Equal(pedido.ValorTotal, valorDesconto);
        }

        [Fact(DisplayName = "Aplicar voucher tipo valor com valor maior que o pedido, o valor do pedido deve ser 0")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_VoucherTipoPValorDesconto_DeveZerarPedido()
        {
            // arranje
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 1, 10);
            pedido.AdicionarItem(pedidoItem);
            var voucher = new Voucher("PROMO-100-REAIS", TipoDescontoVoucher.Valor, 100, null, 1, DateTime.Now.AddDays(15), true, false);
            var valorDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            // act
            _ = pedido.AplicarVoucher(voucher);

            //assert
            Assert.Equal(pedido.ValorTotal, 0);
        }

        [Fact(DisplayName = "Alterar itens do pedido o voucher deve ser aplpicado no novo valor do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AtualizarItensPedido_DeveReclacularValorComVoucherAplicado()
        {
            // arranje
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 1, 100);
            pedido.AdicionarItem(pedidoItem);
            var voucher = new Voucher("PROMO-100-REAIS", TipoDescontoVoucher.Valor, 100, null, 1, DateTime.Now.AddDays(15), true, false);
            _ = pedido.AplicarVoucher(voucher);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto teste", 5, 100);

            // act
            pedido.AtualizarItem(pedidoItemAtualizado);

            //assert
            var totalEsperado = pedido.PedidoItems.Sum(x => x.Quantidade * x.ValorUnitario) - voucher.ValorDesconto;

            Assert.Equal(pedido.ValorTotal, totalEsperado);
        }
    }
}
