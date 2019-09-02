[System.Serializable]
public class Net_OnFriendRemovalRequestReceiver : NetMsg
{
    public Net_OnFriendRemovalRequestReceiver()
    {
        OperationCode = NetOP.OnFriendRemovalRequestReceiver;
    }

    public string SenderUsername { set; get; }
}