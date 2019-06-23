[System.Serializable]
public class Net_PartyRequestConfirmation : NetMsg
{
    public Net_PartyRequestConfirmation()
    {
        OperationCode = NetOP.PartyRequestConfirmation;
    }
    
    public bool Confirmation { get; set; }

    public string Token { get; set; }
    public string PartyToken { get; set; }
    public string SenderUsername { get; set; }
}