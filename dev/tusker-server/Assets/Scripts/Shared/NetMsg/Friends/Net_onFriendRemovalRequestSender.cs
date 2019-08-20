[System.Serializable]
public class Net_OnFriendRemovalRequestSender : NetMsg
{
    public Net_OnFriendRemovalRequestSender()
    {
        OperationCode = NetOP.OnFriendRemovalRequestSender;
    }

    public byte FlagEnum { get; set; }

    public string ReceiverUsername { get; set; }
}
// Flag Enum 0 = Unespected error
// Flag Enum 1 = Success
// Flag Enum 2 = Username is not your friend