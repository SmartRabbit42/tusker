[System.Serializable]
public class Net_SignUpRequest : NetMsg
{
    public Net_SignUpRequest()
    {
        OperationCode = NetOP.SignUpRequest;
    }

    public string Username { set; get; }
    public string Password { set; get; }
    public string Email { set; get; }
}
