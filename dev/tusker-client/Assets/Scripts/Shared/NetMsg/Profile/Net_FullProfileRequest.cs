[System.Serializable]
public class Net_FullAccountRequest : NetMsg
{
    public Net_FullAccountRequest()
    {
        OperationCode = NetOP.FullAccountRequest;
    }

    public string Username { get; set; }
}