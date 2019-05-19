[System.Serializable]
public class Net_OnPartyRequestSender : NetMsg
{
    public Net_OnPartyRequestSender()
    {
        OperationCode = NetOP.OnPartyRequestSender;
    }

    public byte FlagEnum { get; set; }
}
// FlagEnum 0 = Unespected error
// FlagEnum 1 = Success
// FlagEnum 2 = Invalid username
// FlagEnum 3 = Inexistent token
// FlagEnum 4 = Inexistent username
// FlagEnum 5 = Trying to invite yourself
// FlagEnum 6 = You are not in a party
// FlagEnum 7 = Receiver not available
// FlagEnum 8 = Inexistent partyToken
// FlagEnum 9 = Already requested