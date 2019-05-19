[System.Serializable]
public class Net_FriendRequestUpdate : NetMsg
{
    public Net_FriendRequestUpdate()
    {
        OperationCode = NetOP.FriendRequestUpdate;
    }

    public Account FriendRequest { get; set; }
}