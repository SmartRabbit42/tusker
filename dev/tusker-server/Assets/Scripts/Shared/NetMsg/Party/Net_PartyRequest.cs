[System.Serializable]
public class Net_PartyRequest : NetMsg
{
    public Net_PartyRequest()
    {
        OperationCode = NetOP.PartyRequest;
    }

    public string Token { get; set; }
    public string PartyToken { get; set; }

    public string ReceiverUsername { get; set; }
}