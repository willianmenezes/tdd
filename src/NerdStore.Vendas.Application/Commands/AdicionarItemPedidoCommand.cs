using FluentValidation;
using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command, IRequest<bool>
    {
        public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string Nome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarItemPedidoCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AdicionarItemPedidoCommandValidation : AbstractValidator<AdicionarItemPedidoCommand>
    {
        public string IdClienteErroMsg => "Id do cliente invalido.";
        public string IdProdutoErroMsg => "Id do produto invalido.";
        public string NomeErroMsg => "O Nome do produto nao foi informado.";
        public string QtdMaxErroMsg => $"A quantidade maxima de um item e {Pedido.MAX_UNIDADES_ITEM}";
        public string QtdMinErroMsg => $"A quantidade minima de um item e {Pedido.MIN_UNIDADES_ITEM}";
        public string ValorErroMsg => "O valor do item precisa ser maior que 0.";

        public AdicionarItemPedidoCommandValidation()
        {
            RuleFor(x => x.ClienteId)
            .NotEqual(Guid.Empty)
            .WithMessage(IdClienteErroMsg);

            RuleFor(x => x.ProdutoId)
            .NotEqual(Guid.Empty)
            .WithMessage(IdProdutoErroMsg);

            RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage(NomeErroMsg);

            RuleFor(x => x.Quantidade)
            .GreaterThan(0)
            .WithMessage(QtdMinErroMsg)
            .LessThanOrEqualTo(Pedido.MAX_UNIDADES_ITEM)
            .WithMessage(QtdMaxErroMsg);

            RuleFor(x => x.ValorUnitario)
            .GreaterThan(0)
            .WithMessage(ValorErroMsg);
        }
    }
}