using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar voucher tipo valor valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            //Arranje
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Valor, 1, null, 15, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = voucher.ValidarSeAplicavel();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar voucher tipo valor invalido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            //Arranje
            var voucher = new Voucher("", TipoDescontoVoucher.Valor, null, null, 0, DateTime.Now.AddDays(-1), false, true);

            //Act
            var result = voucher.ValidarSeAplicavel();

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(result.Errors.Count, 6);
        }

        [Fact(DisplayName = "Validar voucher tipo porcentagem valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPorcentagem_DeveEstarValido()
        {
            //Arranje
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Porcentagem, 0, 10, 15, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = voucher.ValidarSeAplicavel();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar voucher tipo porcentagem invalido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPorcentagem_DeveEstarInvalido()
        {
            //Arranje
            var voucher = new Voucher("", TipoDescontoVoucher.Porcentagem, null, null, 0, DateTime.Now.AddDays(-1), false, true);

            //Act
            var result = voucher.ValidarSeAplicavel();

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(result.Errors.Count, 6);
        }
    }
}