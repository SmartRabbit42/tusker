[System.Serializable]
public class Net_FriendRemovalRequest : NetMsg
{
    public Net_FriendRemovalRequest()
    {
        OperationCode = NetOP.FriendRemovalRequest;
    }

    public string Token { set; get; }
    public string ReceiverUsername { set; get; }
}