[System.Serializable]
public class Net_OnPartyRequestConfirmationReceiver : NetMsg
{
    public Net_OnPartyRequestConfirmationReceiver()
    {
        OperationCode = NetOP.OnPartyRequestConfirmationReceiver;
    }

    public string ReceiverUsername { get; set; }
    public bool Confirmation { get; set; }
}