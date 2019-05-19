[System.Serializable]
public class Net_CreatePartyRequest : NetMsg
{
    public Net_CreatePartyRequest()
    {
        OperationCode = NetOP.CreatePartyRequest;
    }

    public string Token { get; set; }
}
