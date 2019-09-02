[System.Serializable]
public class Net_OnSignUpRequest : NetMsg
{
    public Net_OnSignUpRequest()
    {
        OperationCode = NetOP.OnSignUpRequest;
    }

    public byte FlagEnum { set; get; }
}

// FlagEnum 0 = Unespected error
// FlagEnum 1 = Success
// FlagEnum 2 = Invalid email
// FlagEnum 3 = Invalid username
// FlagEnum 4 = Email already exists
// FlagEnum 5 = Username already exists