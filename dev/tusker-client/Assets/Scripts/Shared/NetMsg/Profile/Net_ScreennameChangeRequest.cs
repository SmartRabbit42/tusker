[System.Serializable]
public class Net_ScreennameChangeRequest : NetMsg
{
    public Net_ScreennameChangeRequest()
    {
        OperationCode = NetOP.ScreennameChangeRequest;
    }

    public string NewScreenname { set; get; }
    public string Token { set;get; }
}