using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public Voucher Voucher { get; private set; }
        public decimal Desconto { get; private set; }
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        private readonly List<PedidoItem> _pedidoItems;

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if (PedidoItemExistente(pedidoItem))
            {
                var quantidadeItens = pedidoItem.Quantidade;

                var itemExistente = _pedidoItems.FirstOrDefault(p => pedidoItem.ProdutoId == p.ProdutoId);

                if (quantidadeItens + itemExistente.Quantidade > MAX_UNIDADES_ITEM)
                    throw new DomainExeption($"Maximo de {MAX_UNIDADES_ITEM} unidades por produto");

                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
            }
            else
            {
                _pedidoItems.Add(pedidoItem);
            }

            CalcularValorPedido();
        }

        public bool PedidoItemExistente(PedidoItem pedidoItem)
        {
            return _pedidoItems.Any(p => p.ProdutoId == pedidoItem.ProdutoId);
        }

        private void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public static Pedido NovoPedidoRascunho(Guid clienteId)
        {
            var pedido = new Pedido
            {
                ClienteId = clienteId
            };

            pedido.TornarRascunho();

            return pedido;
        }

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemExistente(pedidoItem);

            var pedidoExistente = _pedidoItems.FirstOrDefault(x => x.ProdutoId == pedidoItem.ProdutoId);

            _pedidoItems.Remove(pedidoExistente);
            _pedidoItems.Add(pedidoItem);

            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemExistente(pedidoItem);
        }

        private void ValidarPedidoItemExistente(PedidoItem pedidoItem)
        {
            if (!PedidoItemExistente(pedidoItem))
                throw new DomainExeption($"O item nao existe no pedido");

            _pedidoItems.Remove(pedidoItem);

            CalcularValorPedido();
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplicavel();

            if (result.IsValid != true) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularValorTotalDesconto();

            return result;
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Valor)
            {
                if (Voucher.ValorDesconto.HasValue)
                    desconto = Voucher.ValorDesconto.Value;
            }
            else
            {
                if (Voucher.PercentualDesconto.HasValue)
                    desconto = ValorTotal * Voucher.PercentualDesconto.Value / 100;
            }

            ValorTotal -= desconto;

            if (ValorTotal < 0) ValorTotal = 0;

            Desconto = desconto;
        }
    }
}