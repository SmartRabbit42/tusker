[System.Serializable]
public class Net_ReadyRequest : NetMsg
{
    public Net_ReadyRequest()
    {
        OperationCode = NetOP.ReadyRequest;
    }

    public string Token { get; set; }
}