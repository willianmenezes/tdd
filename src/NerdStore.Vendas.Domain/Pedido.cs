﻿using NerdStore.Core.DomainObjects;
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
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus PedidoStatus { get; set; }
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        private readonly List<PedidoItem> _pedidoItems;

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if (_pedidoItems.Any(p => p.ProdutoId == pedidoItem.ProdutoId))
            {
                var itemExistente = _pedidoItems.First(p => pedidoItem.ProdutoId == p.ProdutoId);
                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
            }
            else
            {
                _pedidoItems.Add(pedidoItem);
            }

            CalcularValorPedido();
        }

        private void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
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
    }

    public enum PedidoStatus
    {
        Rascunho,
        Iniciado,
        Pago = 4,
        Entregue,
        Cancelado
    }

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
