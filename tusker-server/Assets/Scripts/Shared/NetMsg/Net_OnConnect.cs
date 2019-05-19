[System.Serializable]
public class Net_OnConnect : NetMsg
{
    public Net_OnConnect()
    {
        OperationCode = NetOP.OnConnect;
    }

    public byte FlagEnum { set; get; }

    public string Message { set; get; }
}

// FlagEnum 0 = Unnespected error
// FlagEnum 1 = Server on
// FlagEnum 2 = Server full
// FlagEnum 3 = Server under maintenance