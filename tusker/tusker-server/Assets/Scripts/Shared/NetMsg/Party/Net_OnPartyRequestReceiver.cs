[System.Serializable]
public class Net_OnPartyRequestReceiver : NetMsg
{
    public Net_OnPartyRequestReceiver()
    {
        OperationCode = NetOP.OnPartyRequestReceiver;
    }

    public Party PartyRequest { get; set; }
    public string SenderUsername { get; set; }
}