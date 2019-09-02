[System.Serializable]
public class Net_OnCreatePartyRequest : NetMsg
{
    public Net_OnCreatePartyRequest()
    {
        OperationCode = NetOP.OnCreatePartyRequest;
    }

    public byte FlagEnum { get; set; }

    public string PartyToken { get; set; }
}
// Flag Enum 0 = Unespected error
// Flag Enum 1 = Success
// Flag Enum 2 = Inexistant token
// Flag Enum 3 = User already in a party