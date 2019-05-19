[System.Serializable]
public class Net_OnFriendRequestConfirmationReceiver : NetMsg
{
    public Net_OnFriendRequestConfirmationReceiver()
    {
        OperationCode = NetOP.OnFriendRequestConfirmationReceiver;
    }

    public bool Confirmation { set; get; }

    public Account Receiver { set; get; }
}