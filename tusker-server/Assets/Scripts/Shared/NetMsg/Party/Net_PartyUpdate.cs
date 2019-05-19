[System.Serializable]
public class Net_PartyUpdate : NetMsg
{
    public Net_PartyUpdate()
    {
        OperationCode = NetOP.PartyUpdate;
    }

    public Party Party { get; set; }
}