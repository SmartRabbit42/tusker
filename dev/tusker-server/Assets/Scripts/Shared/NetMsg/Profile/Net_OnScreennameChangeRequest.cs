[System.Serializable]
public class Net_OnScreennameChangeRequest : NetMsg
{
    public Net_OnScreennameChangeRequest()
    {
        OperationCode = NetOP.OnScreennameChangeRequest;
    }

    public byte FlagEnum { set; get; }

    public string screenname { set; get; }
}

//FlagEnum 0 = Unespected Error
//FlagEnum 1 = Success
//FlagEnum 2 = Inexistent username
//FlagEnum 3 = Incorrect token
//FlagEnum 4 = Invalid screenName