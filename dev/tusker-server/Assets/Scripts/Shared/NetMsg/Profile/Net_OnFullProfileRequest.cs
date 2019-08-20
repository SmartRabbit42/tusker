[System.Serializable]
public class Net_OnFullAccountRequest : NetMsg
{
    public Net_OnFullAccountRequest()
    {
        OperationCode = NetOP.OnFullAccountRequest;
    }

    public byte FlagEnum { get; set; }

    public FullAccount Account { get; set; }
}
// FlagEnum 0 = Unespected error
// FlagEnum 1 = Success
// FlagEnum 2 = Inexistent token
// FlagEnum 3 = Inexistant Username