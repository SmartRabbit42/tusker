[System.Serializable]
public class Net_OnFriendRequestConfirmationSender : NetMsg
{
    public Net_OnFriendRequestConfirmationSender()
    {
        OperationCode = NetOP.OnFriendRequestConfirmationSender;
    }

    public byte FlagEnum { get; set; }

    public Account Sender { get; set; }
}
// Flag Enum 0 = Unespected error
// Flag Enum 1 = Success
// Flag Enum 2 = Invalid username
// Flag Enum 3 = Inexistent username
// Flag Enum 4 = Inexistent token
// Flag Enum 5 = Tried to befriend theirself
// Flag Enum 6 = Tried to befriend a friend