[System.Serializable]
public class Net_OnFriendRequestSender : NetMsg
{
    public Net_OnFriendRequestSender()
    {
        OperationCode = NetOP.OnFriendRequestSender;
    }

    public byte FlagEnum { get; set; }
}
// Flag Enum 0 = Unespected error
// Flag Enum 1 = Success
// Flag Enum 2 = Invalid username
// Flag Enum 3 = Inexistant username
// Flag Enum 4 = Username is already your friend
// Flag Enum 5 = Username blocked you