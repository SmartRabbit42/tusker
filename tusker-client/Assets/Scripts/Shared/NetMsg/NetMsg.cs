public static class NetOP
{
    //Basic
    public const  byte None = 0;
    public const byte OnConnect = 1;

    //Autentication
    public const byte SignUpRequest = 2;
    public const byte OnSignUpRequest = 3;

    public const byte LoginRequest = 4;    
    public const byte OnLoginRequest = 5;

    //Profile
    public const byte FullAccountRequest = 6;
    public const byte OnFullAccountRequest = 7;

    public const byte ScreennameChangeRequest = 8;
    public const byte OnScreennameChangeRequest = 9;


    //Friends
    public const byte FriendListRequest = 10;
    public const byte OnFriendListRequest = 11;

    public const byte FriendRequestListRequest = 12;
    public const byte OnFriendRequestListRequest = 13;

    public const byte FriendRequest = 14;
    public const byte OnFriendRequestReceiver = 15;
    public const byte OnFriendRequestSender = 16;

    public const byte FriendRemovalRequest = 17;
    public const byte OnFriendRemovalRequestReceiver = 18;
    public const byte OnFriendRemovalRequestSender = 19;

    public const byte FriendRequestConfirmation = 20;
    public const byte OnFriendRequestConfirmationReceiver = 21;
    public const byte OnFriendRequestConfirmationSender = 22;

    public const byte FriendUpdate = 23;
    public const byte FriendRequestUpdate = 24;

    //Party
    public const byte CreatePartyRequest = 25;
    public const byte OnCreatePartyRequest = 26;

    public const byte PartyRequest = 27;
    public const byte OnPartyRequestReceiver = 28;
    public const byte OnPartyRequestSender = 29;

    public const byte PartyRequestConfirmation = 30;
    public const byte OnPartyRequestConfirmationSender = 31;
    public const byte OnPartyRequestConfirmationReceiver = 32;

    public const byte LeavePartyRequest = 33;
    public const byte OnLeavePartyRequest = 34;

    public const byte ReadyRequest = 35;

    public const byte PartyUpdate = 42;
    public const byte PartyRequestUpdate = 43;
}

[System.Serializable]
public abstract class NetMsg
{
	public byte OperationCode { set; get; }

    public NetMsg()
    {
        OperationCode = NetOP.None;
    }
}