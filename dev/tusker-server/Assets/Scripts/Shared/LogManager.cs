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
    public static string OnConnect(byte flagEnum)
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
    public static string OnDisconnect(byte flagEnum)
    {
        return "";
        //Fazer
    }

    //Autentication
    public static string OnSignUpRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed SignUp: Unespected error";
            case 1:
                return "Successfull SignUp";
            case 2:
                return "Failed SignUp: Invalid email";
            case 3:
                return "Failed SignUp: Invalid username";
            case 4:
                return "Failed SignUp: Email already in use";
            case 5:
                return "Failed SignUp: Username already in use";
        }
    }
    public static string OnLoginRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed Login: Unespected error";
            case 1:
                return "Successfull Login";
            case 2:
                return "Failed Login: Inexistent username or email";
            case 3:
                return "Failed Login: Incorrect password";
        }
    }

    //Profile
    public static string OnFullAccountRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed fullAccountRequest: Unespected error";
            case 1:
                return "Successfull fullAccountRequest";
            case 2:
                return "Failed fullAccountRequest: Inexistent token";
            case 3:
                return "Failed fullAccountRequest: Inexistent username";
        }
    }
    public static string OnScreennameChangeRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed ScreenNameChange: Unespected error";
            case 1:
                return "Successfull ScreenNameChange";
            case 2:
                return "Failed ScreenNameChange: Inexistent token";
            case 3:
                return "Failed ScreenNameChange: Invalid screenName";
        }
    }

    //Friends
    public static string OnFriendListRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed friendListRequest: Unespected error";
            case 1:
                return "Successfull FriendListRequest";
            case 2:
                return "Failed FriendListRequest: Inexistent token";
            case 3:
                return "Failed FriendListRequest: No friends";
        }
    }
    public static string OnFriendRequestListRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed FriendRequestListRequest: Unespected error";
            case 1:
                return "Successfull FriendRequestListRequest";
            case 2:
                return "Failed FriendRequestListRequest Inexistent token";
            case 3:
                return "Failed FriendRequestListRequest No friendRequests";
        }
    }
    public static string OnFriendRequestSender(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed friendRequest: Unespected error";
            case 1:
                return "Successfull friendRequest";
            case 2:
                return "Failed friendRequest: Invalid username";
            case 3:
                return "Failed friendRequest: Inexistent username";
            case 4:
                return "Failed friendRequest: Inexistent token";
            case 5:
                return "Failed friendRequest: Tried to befriend yourself";
            case 6:
                return "Failed friendRequest: Tried to befriend a friend";
            case 7:
                return "Failed friendRequest: Already requested";
        }
    }
    public static string OnFriendRequestConfirmationSender(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed friendRequestConfirmation: Unespected error";
            case 1:
                return "Successfull friendRequestConfirmation";
            case 2:
                return "Successfull friendRequestConfirmation";
            case 3:
                return "Failed friendRequestConfirmation: Invalid username";
            case 4:
                return "Failed friendRequestConfirmation: Inexistent token";
            case 5:
                return "Failed friendRequestConfirmation: Inexistent username";
            case 6:
                return "Failed friendRequestConfirmation: Tried to befriend yourself";
            case 7:
                return "Failed friendRequestConfirmation: Tried to befriend a friend";
        }
    }
    public static string OnFriendRemovalRequestSender(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed friendRemovalRequest: Unespected error";
            case 1:
                return "Successfull friendRemovalRequest";
            case 2:
                return "Failed friendRemovalRequest: Inexistent friendship";
        }
    }

    //Party
    public static string OnCreatePartyRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed createPartyRequest: Unespected error";
            case 1:
                return "Successfull createPartyRequest";
            case 2:
                return "Failed createPartyRequest: Inexistent token";
            case 3:
                return "Failed createPartyRequest: Already in a party";
        }
    }
    public static string OnPartyRequestSender(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed partyRequest: Unespected error";
            case 1:
                return "Successfull partyRequest";
            case 2:
                return "Failed partyRequest: Invalid username";
            case 3:
                return "Failed partyRequest: Inexistent token";
            case 4:
                return "Failed partyRequest: Inexistent username";
            case 5:
                return "Failed partyRequest: Trying to invite yourself";
            case 6:
                return "Failed partyRequest: You are not in a party";
            case 7:
                return "Failed partyRequest: Receiver not available";
            case 8:
                return "Failed partyRequest: Inexistent partyToken";
            case 9:
                return "Failed partyRequest: Already requested";
        }
    }
    public static string OnPartyRequestConfirmationSender(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                return "Failed partyRequestConfirmation: Unespected error";
            case 1:
                return "Successfull partyRequestConfirmation";
            case 2:
                return "Successfull partyRequestConfirmation";
            case 3:
                return "Failed partyRequestConfirmation: Invalid username";
            case 4:
                return "Failed partyRequestConfirmation: Inexistent username";
            case 5:
                return "Failed partyRequestConfirmation: Inexistent token";
            case 6:
                return "Failed partyRequestConfirmation: Sender not available";
            case 7:
                return "Failed partyRequestConfirmation: Receiver not available";
            case 8:
                return "Failed partyRequestConfirmation: Inexistent partyRequest";
            case 9:
                return "Failed partyRequestConfirmation: Inexistent party";
            case 10:
                return "Failed partyRequestConfirmation: Party not available";
        }
    }
    #endregion
}
