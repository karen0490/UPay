namespace UPay
{
    public enum UPayAgentState
    {
        AcceptPointOfSaleConnection,
        ReceivePointOfSaleRequestSize,
        ReceivePointOfSaleRequestData,
        SendPointOfSaleRequestToTransactionSwitch,
        ReceiveTransactionSwitchResponse,
        SendTransactionSwitchResponseToPointOfSale,
        ShutdownPointOfSaleConnection
    }
}
