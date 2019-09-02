[System.Serializable]
public class Net_FriendUpdate : NetMsg
{
    public Net_FriendUpdate()
    {
        OperationCode = NetOP.FriendUpdate;
    }

    public Account Friend { get; set; }
}