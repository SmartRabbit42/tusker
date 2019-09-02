[System.Serializable]
public class Net_OnLoginRequest : NetMsg
{
    public Net_OnLoginRequest()
    {
        OperationCode = NetOP.OnLoginRequest;
    }

    public byte FlagEnum { set; get; }

    public string Token { get; set; }
    public Account Account { get; set; }
}

// FlagEnum 0 = Unespected Error
// FlagEnum 1 = Success
// FlagEnum 2 = Incorrect username or email
// FlagEnum 3 = Incorrect password