using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    public static Handler Instance { set; get; }

    [SerializeField] private GameObject loadingPrefab;

    private void Awake() { Instance = this; }

    private void Start()
    {
        StartLoading("Connecting to server...");
    }


    #region Helper
    private void StartLoading(string msg)
    {
        var instance = Instantiate(loadingPrefab, GameObject.Find("Canvas").transform);
        instance.GetComponent<Loading>().Init(msg);
    }
    private void EndLoading()
    {
        try
        {
            GameObject.Find("Canvas").transform.Find("loading").GetComponent<Loading>().Quit();
        }
        catch { }
    }
    #endregion

    #region Request
    //Autentication
    public void SendSignUpRequest(string username, string password, string email)
    {
        StartLoading("Signing up...");
        Client.Instance.SendSignUpRequest(username, password, email);
    }
    public void SendLoginRequest(string usernameEmail, string password)
    {
        StartLoading("Loging in...");
        Client.Instance.SendLoginRequest(usernameEmail, password);
    }

    //Profile
    public void SendFullAccountRequest(string username)
    {
        Client.Instance.SendFullAccountRequest(username);
    }
    public void SendScreennameChangeRequest(string screenname)
    {
        StartLoading("Updating screenname...");
        Client.Instance.SendScreennameChangeRequest(screenname);
    }

    //Friends
    public void SendFriendListRequest()
    {
        Client.Instance.SendFriendListRequest();
    }
    public void SendFriendRequestListRequest()
    {
        Client.Instance.SendFriendRequestListRequest();
    }
    public void SendFriendRequest(string username)
    { 
        StartLoading("Sending request...");
        Client.Instance.SendFriendRequest(username);
    }
    public void SendFriendRequestConfirmation(bool confirmation, string username)
    {
        StartLoading("Sending confirmation...");
        Client.Instance.SendFriendRequestConfirmation(confirmation, username);
    }
    public void SendFriendRemovalRequest(string username)
    {
        StartLoading("Removing friend...");
        Client.Instance.SendFriendRemovalRequest(username);
    }

    //Party 
    public void SendCreatePartyRequest()
    {
        StartLoading("Creating party...");
        Client.Instance.SendCreatePartyRequest();
    }
    public void SendPartyRequest(string receiver)
    {
        StartLoading("Inviting player...");
        Client.Instance.SendPartyRequest(receiver);
    }
    public void SendPartyRequestConfirmation(bool confirmation, string partyToken, string username)
    {
        StartLoading("Sending confirmation...");
        Client.Instance.SendPartyRequestConfirmation(confirmation, partyToken, username);
    }
    public void SendLeavePartyRequest(string partyToken)
    {
        StartLoading("Leaving party...");
        Client.Instance.SendLeavePartyRequest(partyToken);
    }
    public void SendReadyRequest()
    {
        StartLoading("Being ready...");
        Client.Instance.SendReadyRequest();
    }
    #endregion

    #region Response
    //Basic
    public void OnConnect(byte flagEnum, string message)
    {
        EndLoading();
        Entry_Main.Instance.OnConnect(flagEnum, message);
    }

    //Autentication
    public void OnSignUpRequest(byte flagEnum)
    {
        EndLoading();
        Entry_Main.Instance.OnSignUpRequest(flagEnum);
    }
    public void OnLoginRequest(byte flagEnum)
    {
        EndLoading();
        Entry_Main.Instance.OnLoginRequest(flagEnum);
    }

    //Profile
    public void OnFullAccountRequest(byte flagEnum, FullAccount fullAccount)
    {
        if (Profile.Instance != null && Profile.Instance.profile.Equals(fullAccount.GetAccount()))
            Profile.Instance.OnFullAccountRequest(flagEnum, fullAccount);
    }
    public void OnScreenNameChangeRequest(byte flagEnum, string screenname)
    {
        EndLoading();
        if (Profile.Instance != null)
            Profile.Instance.OnScreenNameChangeRequest(flagEnum, screenname);
    }

    //Friends
    public void OnFriendListRequest(List<Account> friendList)
    {
        Friends.Instance.OnFriendListRequest(friendList);
    }
    public void OnFriendRequestListRequest(List<Account> friendRequestList)
    {
        Friends.Instance.OnFriendRequestListRequest(friendRequestList);
    }
    public void OnFriendRequestSender(byte flagEnum)
    {
        EndLoading();
        Friends.Instance.OnFriendRequestSender(flagEnum);
    }
    public void OnFriendRequestReceiver(Account friend)
    {
        Friends.Instance.OnFriendRequestReceiver(friend);
    }
    public void OnFriendRequestConfirmationSender(byte flagEnum, Account sender)
    {
        EndLoading();
        Friends.Instance.OnFriendRequestConfirmationSender(flagEnum, sender);
    }
    public void OnFriendRequestConfirmationReceiver(Account friend, bool confirmation)
    {
        Friends.Instance.OnFriendRequestConfirmationReceiver(friend, confirmation);
    }
    public void OnFriendRemovalRequestSender(byte flagEnum, string receiverUsername)
    {
        EndLoading();
        Friends.Instance.OnFriendRemovalRequestSender(flagEnum, receiverUsername);
    }
    public void OnFriendRemovalRequestReceiver(string senderUsername)
    {
        Friends.Instance.OnFriendRemovalRequestReceiver(senderUsername);
    }
    public void FriendUpdate(Account friend)
    {
        Friends.Instance.OnFriendUpdate(friend);
    }
    public void FriendRequestUpdate(Account friendRequest)
    {
        Friends.Instance.OnFriendRequestUpdate(friendRequest);
    }

    //Party
    public void OnCreatePartyRequest(byte flagEnum, string partyToken)
    {
        EndLoading();
        MainMenu_Play.Instance.OnCreatePartyRequest(flagEnum, partyToken);
    }
    public void OnPartyRequestSender(byte flagEnum)
    {
        EndLoading();
        MainMenu_Play.Instance.OnPartyRequestSender(flagEnum);
    }
    public void OnPartyRequestReceiver(Party partyRequest, string senderUsername)
    {
        MainMenu_Play.Instance.OnPartyRequestReceiver(partyRequest, senderUsername);
    }
    public void OnPartyRequestConfirmationSender(byte flagEnum, string partyToken)
    {
        EndLoading();
        MainMenu_Play.Instance.OnPartyRequestConfirmationSender(flagEnum, partyToken);
    }
    public void OnPartyRequestConfirmationReceiver(bool confirmation, string receiverUsername)
    {
        MainMenu_Play.Instance.OnPartyRequestConfirmationReceiver(confirmation, receiverUsername);
    }
    public void PartyUpdate(Party party)
    {
        MainMenu_Play.Instance.PartyUpdate(party);
    }
    public void PartyRequestUpdate(Party partyRequest)
    {
        MainMenu_Play.Instance.PartyRequestUpdate(partyRequest);
    }
    #endregion
}