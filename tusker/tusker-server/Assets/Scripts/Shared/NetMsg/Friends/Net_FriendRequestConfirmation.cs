[System.Serializable]
public class Net_FriendRequestConfirmation : NetMsg
{
    public Net_FriendRequestConfirmation()
    {
        OperationCode = NetOP.FriendRequestConfirmation;
    }

    public bool Confirmation { set; get; }

    public string SenderUsername { set; get; }
    public string Token { set; get; }
}