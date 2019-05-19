[System.Serializable]
public class Net_OnPartyRequestConfirmationSender : NetMsg
{
    public Net_OnPartyRequestConfirmationSender()
    {
        OperationCode = NetOP.OnPartyRequestConfirmationSender;
    }

    public byte FlagEnum { get; set; }
    public string PartyToken { get; set; }
}
// FlagEnum 0: Unespected error
// FlagEnum 1: Success
// FlagEnum 2: Success
// FlagEnum 3: Invalid username
// FlagEnum 4: Inexistent username
// FlagEnum 5: Inexistent token
// FlagEnum 6: Sender not available
// FlagEnum 7: Receiver not available
// FlagEnum 8: Inexistent partyRequest
// FlagEnum 9: Inexistent party
// FlagEnum 10: Party not available