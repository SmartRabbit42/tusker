[System.Serializable]
public class Net_PartyRequestUpdate : NetMsg
{
    public Net_PartyRequestUpdate()
    {
        OperationCode = NetOP.PartyRequestUpdate;
    }

    public Party PartyRequest { get; set; }
}