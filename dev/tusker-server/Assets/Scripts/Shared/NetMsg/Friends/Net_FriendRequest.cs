[System.Serializable]
public class Net_FriendRequest : NetMsg
{
    public Net_FriendRequest()
    {
        OperationCode = NetOP.FriendRequest;
    }

    public string Token { set; get; }
    public string ReceiverUsername { set; get; }
}