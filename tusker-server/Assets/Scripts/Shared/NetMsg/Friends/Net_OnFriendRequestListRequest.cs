using System.Collections.Generic;

[System.Serializable]
public class Net_OnFriendRequestListRequest : NetMsg
{
    public Net_OnFriendRequestListRequest()
    {
        OperationCode = NetOP.OnFriendRequestListRequest;
    }

    public byte FlagEnum { get; set; }

    public List<Account> FriendRequests { get; set; }
}