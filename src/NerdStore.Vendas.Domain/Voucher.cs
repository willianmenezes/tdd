using FluentValidation;
using FluentValidation.Results;

namespace NerdStore.Vendas.Domain
{
    public class Voucher
    {
        public Voucher(string codigo, TipoDescontoVoucher tipoDescontoVoucher, decimal? valorDesconto, decimal? percentualDesconto, int quantidade, DateTime dataValidade, bool ativo, bool utilizado)
        {
            Codigo = codigo;
            TipoDescontoVoucher = tipoDescontoVoucher;
            ValorDesconto = valorDesconto;
            PercentualDesconto = percentualDesconto;
            Quantidade = quantidade;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
        }

        public string Codigo { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public decimal? PercentualDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }
    }

    public class VoucherAplicavelValidation : AbstractValidator<Voucher>
    {
        public static string CodigoErroMsg => "Voucher sem codigo valido.";
        public static string DataValidadeErroMsg => "Este voucher esta expirado.";
        public static string AtivoErroMsg => "Este voucher nao e mais valido.";
        public static string UtilizadoErroMsg => "Este voucher ja foi utilizado.";
        public static string QuantidadeErroMsg => "Este voucher nao esta mais disponivel.";
        public static string ValorDescontoErroMsg => "O valor do desconto precisa ser superior a 0.";
        public static string PercentualDescontoErroMsg => "O valor da porcentagem de desconto precisa ser superior a 0.";

        public VoucherAplicavelValidation()
        {
            RuleFor(c => c.Codigo)
            .NotEmpty()
            .WithMessage(CodigoErroMsg);

            RuleFor(c => c.DataValidade)
            .Must(DataVencimentoSuperiorAtual)
            .WithMessage(DataValidadeErroMsg);

            RuleFor(c => c.Ativo)
            .Equal(true)
            .WithMessage(AtivoErroMsg);

            RuleFor(c => c.Utilizado)
            .Equal(false)
            .WithMessage(UtilizadoErroMsg);

            RuleFor(c => c.Quantidade)
            .GreaterThan(0)
            .WithMessage(QuantidadeErroMsg);

            When(c => c.TipoDescontoVoucher == TipoDescontoVoucher.Valor, () =>
            {
                RuleFor(c => c.ValorDesconto)
                .NotNull()
                .WithMessage(ValorDescontoErroMsg)
                .GreaterThan(0)
                .WithMessage(ValorDescontoErroMsg);
            });

            When(c => c.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem, () =>
            {
                RuleFor(c => c.PercentualDesconto)
                .NotNull()
                .WithMessage(PercentualDescontoErroMsg)
                .GreaterThan(0)
                .WithMessage(PercentualDescontoErroMsg);
            });
        }

        protected static bool DataVencimentoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }
    }
}