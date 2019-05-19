public static class LogManager
{
    #region Server
    public static string ConnectEvent(string user)
    {
        return string.Format("user {0} connected to the server", user);
    }
    public static string DisconnectEvent(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed disconnect update by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull disconnect update by user {0}", user);
            case 2:
                return string.Format("Failed disconnect update by user {0}: Inexistent account", user);
        }
    }

    //Autentication
    public static string SignUpRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed SignUp by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull SignUp by user {0}", user);
            case 2:
                return string.Format("Failed SignUp by user {0}: Invalid email", user);
            case 3:
                return string.Format("Failed SignUp by user {0}: Invalid username", user);
            case 4:
                return string.Format("Failed SignUp by user {0}: Email already in use", user);
            case 5:
                return string.Format("Failed SignUp by user {0}: username already in use", user);
        }
    }
    public static string LoginRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed Login by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull Login by user {0}", user);
            case 2:
                return string.Format("Failed Login by user {0}: Inexistent username or email", user);
            case 3:
                return string.Format("Failed Login by user {0}: Incorrect password", user);
        }
    }

    //Profile
    public static string FullAccountRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed fullAccountRequest by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull fullAccountRequest by user {0}", user);
            case 2:
                return string.Format("Failed fullAccountRequest by user {0}: Inexistent token", user);
            case 3:
                return string.Format("Failed fullAccountRequest by user {0}: Inexistent username", user);
        }
    }
    public static string ScreenNameChangeRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed ScreenNameChange by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull ScreenNameChange by user {0}", user);
            case 2:
                return string.Format("Failed ScreenNameChange by user {0}: Inexistent token", user);
            case 3:
                return string.Format("Failed ScreenNameChange by user {0}: Invalid screenName", user);
        }
    }

    //Friends
    public static string FriendListRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed friendListRequest by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull friendListRequest by user {0}", user);
            case 2:
                return string.Format("Failed friendListRequest by user {0}: Inexistent token", user);
            case 3:
                return string.Format("Failed friendListRequest by user {0}: No friends", user);
        }
    }
    public static string FriendRequestListRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed friendRequestListRequest by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull friendRequestListRequest by user {0}", user);
            case 2:
                return string.Format("Failed friendRequestListRequest by user {0}: Inexistent token", user);
            case 3:
                return string.Format("Failed friendRequestListRequest by user {0}: No friendRequests", user);
        }
    }
    public static string FriendRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed FriendRequest by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull FriendRequest by user {0}", user);
            case 3:
                return string.Format("Failed FriendRequest by user {0}: Inexistent username", user);
            case 4:
                return string.Format("Failed FriendRequest by user {0}: Inexistent token", user);
            case 5:
                return string.Format("Failed FriendRequest by user {0}: Tried to befriend theirself", user);
            case 6:
                return string.Format("Failed FriendRequest by user {0}: Tried to befriend a friend", user);
            case 7:
                return string.Format("Failed FriendRequest by user {0}: Already requested", user);
        }
    }
    public static string FriendRequestConfirmation(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed FriendRequestConfirmation by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull FriendRequestConfirmation by user {0}", user);
            case 3:
                return string.Format("Failed FriendRequestConfirmation by user {0}: Invalid username", user);
            case 4:
                return string.Format("Failed FriendRequestConfirmation by user {0}: Inexistent username", user);
            case 5:
                return string.Format("Failed FriendRequestConfirmation by user {0}: Inexistent token", user);
            case 6:
                return string.Format("Failed FriendRequestConfirmation by user {0}: Tried to befriend theirself", user);
            case 7:
                return string.Format("Failed FriendRequestConfirmation by user {0}: Tried to befriend a friend", user);
        }
    }
    public static string FriendRemovalRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed friendRemovalRequest by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull friendRemovalRequest by user {0}", user);
            case 2:
                return string.Format("Failed friendRemovalRequest by user {0}: Inexistent friendship", user);
        }
    }

    //Party
    public static string CreatePartyRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed createPartyRequest by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull createPartyRequest by user {0}", user);
            case 2:
                return string.Format("Failed createPartyRequest: Inexistent token by user {0}", user);
        }
    }
    public static string PartyRequest(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed partyRequest by user {0}: Unespected error", user);
            case 1:
                return string.Format("Successfull partyRequest by user {0}", user);
            case 2:
                return string.Format("Failed partyRequest by user {0}: Invalid username", user);
            case 3:
                return string.Format("Failed partyRequest by user {0}: Inexistent token", user);
            case 4:
                return string.Format("Failed partyRequest by user {0}: Inexistent username", user);
            case 5:
                return string.Format("Failed partyRequest by user {0}: Trying to invite theirself", user);
            case 6:
                return string.Format("Failed partyRequest by user {0}: Sender not in a party", user);
            case 7:
                return string.Format("Failed partyRequest by user {0}: Receiver not available", user);
            case 8:
                return string.Format("Failed partyRequest by user {0}: Inexistent partyToken", user);
            case 9:
                return string.Format("Failed partyRequest by user {0}: Already requested", user);
        }
    }
    public static string PartyRequestConfirmation(byte flagEnum, string user)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return string.Format("Failed partyRequestConfimation by user {0}: Unespected error", user);
            case 1:
                return string.Format("Succesfull partyRequestConfirmation by user {0}", user);
            case 2:
                return string.Format("Succesfull partyRequestConfirmation by user {0}", user);
            case 3:
                return string.Format("Failed partyRequestConfimation by user {0}: Invalid username", user);
            case 4:
                return string.Format("Failed partyRequestConfimation by user {0}: Inexistent username", user);
            case 5:
                return string.Format("Failed partyRequestConfimation by user {0}: Inexistent token", user);
            case 6:
                return string.Format("Failed partyRequestConfimation by user {0}: Unavailable sender", user);
            case 7:
                return string.Format("Failed partyRequestConfimation by user {0}: Unavailable receiver", user);
            case 8:
                return string.Format("Failed partyRequestConfimation by user {0}: Inexistent partyRequest", user);
            case 9:
                return string.Format("Failed partyRequestConfimation by user {0}: Inexistent party", user);
            case 10:
                return string.Format("Failed partyRequestConfimation by user {0}: Unavailable party", user);
        }
    }
    #endregion

    #region Client
    public static string ConnectEvent(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed connection: Unespected error";
            case 1:
                return "Successfull connection";
            case 2:
                return "Failed connection: Server full";
            case 3:
                return "Failed connection: Server under maintenance";
        }
    }
    public static string DisconnectEvent(byte flagEnum)
    {
        return "";
        //Fazer
    }


    #endregion
}
