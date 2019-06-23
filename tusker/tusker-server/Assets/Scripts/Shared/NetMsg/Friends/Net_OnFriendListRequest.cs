using System.Collections.Generic;

[System.Serializable]
public class Net_OnFriendListRequest : NetMsg
{
    public Net_OnFriendListRequest()
    {
        OperationCode = NetOP.OnFriendListRequest;
    }

    public byte FlagEnum { get; set; }

    public List<Account> Friends { get; set; }
}