namespace NerdStore.Vendas.Domain
{
    public enum PedidoStatus
    {
        Rascunho,
        Iniciado,
        Pago = 4,
        Entregue,
        Cancelado
    }
}