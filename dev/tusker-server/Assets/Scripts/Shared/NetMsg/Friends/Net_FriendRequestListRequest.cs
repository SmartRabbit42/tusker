[System.Serializable]
public class Net_FriendRequestListRequest : NetMsg
{
    public Net_FriendRequestListRequest()
    {
        OperationCode = NetOP.FriendRequestListRequest;
    }

    public string Token { set; get; }
}