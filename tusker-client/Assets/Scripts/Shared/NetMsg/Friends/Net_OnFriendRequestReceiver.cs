[System.Serializable]
public class Net_OnFriendRequestReceiver : NetMsg
{
    public Net_OnFriendRequestReceiver()
    {
        OperationCode = NetOP.OnFriendRequestReceiver;
    }

    public Account Sender { get; set; }
}