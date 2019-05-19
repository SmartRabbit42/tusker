[System.Serializable]
public class Net_LeavePartyRequest : NetMsg
{
    public Net_LeavePartyRequest()
    {
        OperationCode = NetOP.LeavePartyRequest;
    }

    public string Token { get; set; }
    public string PartyToken { get; set; }
}