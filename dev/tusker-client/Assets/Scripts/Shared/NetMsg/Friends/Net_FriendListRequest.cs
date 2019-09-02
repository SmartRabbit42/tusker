[System.Serializable]
public class Net_FriendListRequest : NetMsg
{
    public Net_FriendListRequest()
    {
        OperationCode = NetOP.FriendListRequest;
    }

    public string Token { set; get; }
}