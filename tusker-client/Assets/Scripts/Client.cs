using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    public static Client Instance { get; private set; }

    private const int MAX_USER = 100;
    private const int PORT = 4242;
    private const int BYTE_SIZE = 1024;
    public const string SERVER_IP = "127.0.0.1";

    private byte reliableChannel;
    private int connectionId;
    private int hostId;
    private byte error;

    public Account myAccount;
    private string token;

    private bool isStarted = false;

    #region MonoBehaviour
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Init();
    }
    void Update()
    {
        UpdateMessagePump();
    }
    #endregion

    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(cc, MAX_USER);

        hostId = NetworkTransport.AddHost(topology, 0);

        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log("Connecting from standalone");

        Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
        isStarted = true;
    }
    public void ShutDown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }

    private void UpdateMessagePump()
    {
        if (!isStarted)
            return;

        int recHostId;
        int connectionId;
        int channelId;

        byte[] recBuffer = new byte[BYTE_SIZE];
        int dataSize;

        NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, BYTE_SIZE, out dataSize, out error);
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Disconnected from the server");
                break;
            case NetworkEventType.DataEvent:
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);

                OnData(connectionId, channelId, recHostId, msg);
                break;
            default:
            case NetworkEventType.BroadcastEvent:
                Debug.LogError("Unespected NetworkEventType");
                break;
        }
    }

    #region Receive
    private void OnData(int cnnId, int channelId, int recHostId, NetMsg msg)
    {
        switch (msg.OperationCode)
        {
            default:
            case NetOP.None:
                Debug.LogError("Unespected NET Operation Code");
                break;
            case NetOP.OnConnect:
                OnConnect((Net_OnConnect)msg);
                break;
            
            //Autentication
            case NetOP.OnSignUpRequest:
                OnSignUpRequest((Net_OnSignUpRequest)msg);
                break;
            case NetOP.OnLoginRequest:
                OnLoginRequest((Net_OnLoginRequest)msg);
                break;

            //Profile
            case NetOP.OnFullAccountRequest:
                OnFullAccountRequest((Net_OnFullAccountRequest)msg);
                break;
            case NetOP.OnScreennameChangeRequest:
                OnScreenNameChangeRequest((Net_OnScreennameChangeRequest)msg);
                break;
            
            //Friends
            case NetOP.OnFriendListRequest:
                OnFriendListRequest((Net_OnFriendListRequest)msg);
                break;
            case NetOP.OnFriendRequestListRequest:
                OnFriendRequestListRequest((Net_OnFriendRequestListRequest)msg);
                break;
            case NetOP.OnFriendRequestSender:
                OnFriendRequestSender((Net_OnFriendRequestSender)msg);
                break;
            case NetOP.OnFriendRequestReceiver:
                OnFriendRequestReceiver((Net_OnFriendRequestReceiver)msg);
                break;
            case NetOP.OnFriendRequestConfirmationSender:
                OnFriendRequestConfirmationSender((Net_OnFriendRequestConfirmationSender)msg);
                break;
            case NetOP.OnFriendRequestConfirmationReceiver:
                OnFriendRequestConfirmationReceiver((Net_OnFriendRequestConfirmationReceiver)msg);
                break;
            case NetOP.OnFriendRemovalRequestSender:
                OnFriendRemovalRequestSender((Net_OnFriendRemovalRequestSender)msg);
                break;
            case NetOP.OnFriendRemovalRequestReceiver:
                OnFriendRemovalRequestReceiver((Net_OnFriendRemovalRequestReceiver)msg);
                break;
            case NetOP.FriendUpdate:
                FriendUpdate((Net_FriendUpdate)msg);
                break;
            case NetOP.FriendRequestUpdate:
                FriendRequestUpdate((Net_FriendRequestUpdate)msg);
                break;

            //Party
            case NetOP.OnCreatePartyRequest:
                OnCreatePartyRequest((Net_OnCreatePartyRequest)msg);
                break;
            case NetOP.OnPartyRequestSender:
                OnPartyRequestSender((Net_OnPartyRequestSender)msg);
                break;
            case NetOP.OnPartyRequestReceiver:
                OnPartyRequestReceiver((Net_OnPartyRequestReceiver)msg);
                break;
            case NetOP.OnPartyRequestConfirmationSender:
                OnPartyRequestConfirmationSender((Net_OnPartyRequestConfirmationSender)msg);
                break;
            case NetOP.OnPartyRequestConfirmationReceiver:
                OnPartyRequestConfirmationReceiver((Net_OnPartyRequestConfirmationReceiver)msg);
                break;
            case NetOP.PartyUpdate:
                PartyUpdate((Net_PartyUpdate)msg);
                break;
            case NetOP.PartyRequestUpdate:
                PartyRequestUpdate((Net_PartyRequestUpdate)msg);
                break;
        }
    }

    //Basic
    private void OnConnect(Net_OnConnect oc)
    {
        Debug.Log(LogManager.ConnectEvent(oc.FlagEnum));
        Handler.Instance.OnConnect(oc.FlagEnum, oc.Message);
    }
    private void OnDisconnect()
    {
        //Fazer
    }

    //Autentication
    private void OnSignUpRequest(Net_OnSignUpRequest osr)
    {
        switch (osr.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed SignUp: Unespected error");
                Handler.Instance.OnSignUpRequest(0);
                break;
            case 1:
                Debug.Log("Successfull SignUp");
                Handler.Instance.OnSignUpRequest(1);
                break;
            case 2:
                Debug.Log("Failed SignUp: Invalid email");
                Handler.Instance.OnSignUpRequest(2);
                break;
            case 3:
                Debug.Log("Failed SignUp: Invalid username");
                Handler.Instance.OnSignUpRequest(3);
                break;
            case 4:
                Debug.Log("Failed SignUp: Email already in use");
                Handler.Instance.OnSignUpRequest(4);
                break;
            case 5:
                Debug.Log("Failed SignUp: Username already in use");
                Handler.Instance.OnSignUpRequest(5);
                break;
        }
    }
    private void OnLoginRequest(Net_OnLoginRequest olr)
    {
        switch (olr.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed Login: Unespected error");
                Handler.Instance.OnLoginRequest(0);
                break;
            case 1:
                Debug.Log("Successfull Login");
                Handler.Instance.OnLoginRequest(1);

                token = olr.Token;

                myAccount = olr.Account;

                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                break;
            case 2:
                Debug.Log("Failed Login: Inexistent username or email");
                Handler.Instance.OnLoginRequest(2);
                break;
            case 3:
                Debug.Log("Failed Login: Incorrect password");
                Handler.Instance.OnLoginRequest(3);
                break;
        }
    }

    //Profile
    private void OnFullAccountRequest(Net_OnFullAccountRequest ofar)
    {
        switch (ofar.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed fullAccountRequest: Unespected error");
                Handler.Instance.OnFullAccountRequest(0, null);
                break;
            case 1:
                Debug.Log("Successfull fullAccountRequest");
                Handler.Instance.OnFullAccountRequest(1, ofar.Account);
                break;
            case 2:
                Debug.Log("Failed fullAccountRequest: Inexistent token");
                Handler.Instance.OnFullAccountRequest(2, null);
                break;
            case 3:
                Debug.Log("Failed fullAccountRequest: Inexistent username");
                if(Profile.Instance != null)
                Handler.Instance.OnFullAccountRequest(3, null);
                break;
        }
    }
    private void OnScreenNameChangeRequest(Net_OnScreennameChangeRequest oscr)
    {
        switch (oscr.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed ScreenNameChange: Unespected error");
                Handler.Instance.OnScreenNameChangeRequest(0, oscr.screenname);
                break;
            case 1:
                Debug.Log("Successfull ScreenNameChange");
                myAccount.Screenname = oscr.screenname;
                Handler.Instance.OnScreenNameChangeRequest(1, oscr.screenname);
                break;
            case 2:
                Debug.Log("Failed ScreenNameChange: Inexistent token");
                Handler.Instance.OnScreenNameChangeRequest(2, oscr.screenname);
                break;
            case 3:
                Debug.Log("Failed ScreenNameChange: Invalid screenName");
                Handler.Instance.OnScreenNameChangeRequest(3, oscr.screenname);
                break;
        }
    }

    //Friends
    private void OnFriendListRequest(Net_OnFriendListRequest oflr)
    {
        switch (oflr.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed friendListRequest: Unespected error");
                break;
            case 1:
                Debug.Log("Successfull FriendListRequest");
                Handler.Instance.OnFriendListRequest(oflr.Friends);
                break;
            case 2:
                Debug.Log("Failed FriendListRequest: Inexistent token");
                break;
            case 3:
                Debug.Log("Failed FriendListRequest: No friends");
                break;
        }
    }
    private void OnFriendRequestListRequest(Net_OnFriendRequestListRequest ofrlr)
    {
        switch (ofrlr.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed FriendRequestListRequest: Unespected error");
                break;
            case 1:
                Debug.Log("Successfull FriendRequestListRequest");
                Handler.Instance.OnFriendRequestListRequest(ofrlr.FriendRequests);
                break;
            case 2:
                Debug.Log("Failed FriendRequestListRequest Inexistent token");
                break;
            case 3:
                Debug.Log("Failed FriendRequestListRequest No friendRequests");
                break;
        }
    }
    private void OnFriendRequestSender(Net_OnFriendRequestSender ofrs)
    {
        switch (ofrs.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed friendRequest: Unespected error");
                Handler.Instance.OnFriendRequestSender(0);
                break;
            case 1:
                Debug.Log("Successfull friendRequest");
                Handler.Instance.OnFriendRequestSender(1);
                break;
            case 2:
                Debug.Log("Failed friendRequest: Invalid username");
                Handler.Instance.OnFriendRequestSender(2);
                break;
            case 3:
                Debug.Log("Failed friendRequest: Inexistent username");
                Handler.Instance.OnFriendRequestSender(3);
                break;
            case 4:
                Debug.Log("Failed friendRequest: Inexistent token");
                Handler.Instance.OnFriendRequestSender(4);
                break;
            case 5:
                Debug.Log("Failed friendRequest: Tried to befriend yourself");
                Handler.Instance.OnFriendRequestSender(5);
                break;
            case 6:
                Debug.Log("Failed friendRequest: Tried to befriend a friend");
                Handler.Instance.OnFriendRequestSender(6);
                break;
            case 7:
                Debug.Log("Failed friendRequest: Already requested");
                Handler.Instance.OnFriendRequestSender(7);
                break;
        }
    }
    private void OnFriendRequestReceiver(Net_OnFriendRequestReceiver ofrr)
    {
        Debug.Log("FriendRequest received");
        Handler.Instance.OnFriendRequestReceiver(ofrr.Sender);
    }
    private void OnFriendRequestConfirmationSender(Net_OnFriendRequestConfirmationSender ofrcs)
    {
        switch (ofrcs.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed friendRequestConfirmation: Unespected error");
                Handler.Instance.OnFriendRequestConfirmationSender(0, null);
                break;
            case 1:
                Debug.Log("Successfull friendRequestConfirmation");
                Handler.Instance.OnFriendRequestConfirmationSender(1, ofrcs.Sender);
                break;
            case 2:
                Debug.Log("Successfull friendRequestConfirmation");
                Handler.Instance.OnFriendRequestConfirmationSender(2, ofrcs.Sender);
                break;
            case 3:
                Debug.Log("Failed friendRequestConfirmation: Invalid username");
                Handler.Instance.OnFriendRequestConfirmationSender(3, null);
                break;
            case 4:
                Debug.Log("Failed friendRequestConfirmation: Inexistent token");
                Handler.Instance.OnFriendRequestConfirmationSender(4, null);
                break;
            case 5:
                Debug.Log("Failed friendRequestConfirmation: Inexistent username");
                Handler.Instance.OnFriendRequestConfirmationSender(5, null);
                break;
            case 6:
                Debug.Log("Failed friendRequestConfirmation: Tried to befriend yourself");
                Handler.Instance.OnFriendRequestConfirmationSender(6, null);
                break;
            case 7:
                Debug.Log("Failed friendRequestConfirmation: Tried to befriend a friend");
                Handler.Instance.OnFriendRequestConfirmationSender(7, null);
                break;
        }
    }
    private void OnFriendRequestConfirmationReceiver(Net_OnFriendRequestConfirmationReceiver ofrcr)
    {
        Debug.Log("FriendRequestConfirmation received");
        Handler.Instance.OnFriendRequestConfirmationReceiver(ofrcr.Receiver, ofrcr.Confirmation);
    }
    private void OnFriendRemovalRequestSender(Net_OnFriendRemovalRequestSender ofrrs)
    {
        switch (ofrrs.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed friendRemovalRequest: Unespected error");
                Handler.Instance.OnFriendRemovalRequestSender(0, "");
                break;
            case 1:
                Debug.Log("Successfull friendRemovalRequest");
                Handler.Instance.OnFriendRemovalRequestSender(1, ofrrs.ReceiverUsername);
                break;
            case 2:
                Debug.Log("Failed friendRemovalRequest: Inexistent friendship");
                Handler.Instance.OnFriendRemovalRequestSender(2, "");
                break;
        }
    }
    private void OnFriendRemovalRequestReceiver(Net_OnFriendRemovalRequestReceiver ofrrr)
    {
        Debug.Log("FriendRemovalRequest received");
        Handler.Instance.OnFriendRemovalRequestReceiver(ofrrr.SenderUsername);
    }
    private void FriendUpdate(Net_FriendUpdate fu)
    {
        Handler.Instance.FriendUpdate(fu.Friend);
    }
    private void FriendRequestUpdate(Net_FriendRequestUpdate fru)
    {
        Handler.Instance.FriendRequestUpdate(fru.FriendRequest);
    }

    //Party
    private void OnCreatePartyRequest(Net_OnCreatePartyRequest ocpr)
    {
        switch (ocpr.FlagEnum)
        {
            default:
            case 0:
                Debug.Log("Failed createPartyRequest: Unespected error");
                Handler.Instance.OnCreatePartyRequest(0, null);
                break;
            case 1:
                Debug.Log("Successfull createPartyRequest");
                myAccount.Status = 2;
                Handler.Instance.OnCreatePartyRequest(1, ocpr.PartyToken);
                break;
            case 2:
                Debug.Log("Failed createPartyRequest: Inexistent token");
                Handler.Instance.OnCreatePartyRequest(2, null);
                break;
            case 3:
                Debug.Log("Failed createPartyRequest: Already in a party");
                Handler.Instance.OnCreatePartyRequest(3, null);
                break;
        }
    }
    private void OnPartyRequestSender(Net_OnPartyRequestSender opirr)
    {
        switch (opirr.FlagEnum)
        {
            default:
            case 0:
                Debug.LogError("Failed partyRequest: Unespected error");
                Handler.Instance.OnPartyRequestSender(0);
                break;
            case 1:
                Debug.Log("Successfull partyRequest");
                Handler.Instance.OnPartyRequestSender(1);
                break;
            case 2:
                Debug.Log("Failed partyRequest: Invalid username");
                Handler.Instance.OnPartyRequestSender(2);
                break;
            case 3:
                Debug.Log("Failed partyRequest: Inexistent token");
                Handler.Instance.OnPartyRequestSender(3);
                break;
            case 4:
                Debug.Log("Failed partyRequest: Inexistent username");
                Handler.Instance.OnPartyRequestSender(4);
                break;
            case 5:
                Debug.Log("Failed partyRequest: Trying to invite yourself");
                Handler.Instance.OnPartyRequestSender(5);
                break;
            case 6:
                Debug.Log("Failed partyRequest: You are not in a party");
                Handler.Instance.OnPartyRequestSender(6);
                break;
            case 7:
                Debug.Log("Failed partyRequest: Receiver not available");
                Handler.Instance.OnPartyRequestSender(7);
                break;
            case 8:
                Debug.Log("Failed partyRequest: Inexistent partyToken");
                Handler.Instance.OnPartyRequestSender(8);
                break;
            case 9:
                Debug.Log("Failed partyRequest: Already requested");
                Handler.Instance.OnPartyRequestSender(9);
                break;
        }
    }
    private void OnPartyRequestReceiver(Net_OnPartyRequestReceiver opirr)
    {
        Debug.Log("PartyRequest received");
        Handler.Instance.OnPartyRequestReceiver(opirr.PartyRequest, opirr.SenderUsername);
    }
    private void OnPartyRequestConfirmationSender(Net_OnPartyRequestConfirmationSender oprcs)
    {
        switch (oprcs.FlagEnum)
        {
            case 0:
                Debug.Log("Failed partyRequestConfirmation: Unespected error");
                Handler.Instance.OnPartyRequestConfirmationSender(0, null);
                break;
            case 1:
                Debug.Log("Successfull partyRequestConfirmation");
                myAccount.Status = 2;
                Handler.Instance.OnPartyRequestConfirmationSender(1, oprcs.PartyToken);
                break;
            case 2:
                Debug.Log("Successfull partyRequestConfirmation");
                Handler.Instance.OnPartyRequestConfirmationSender(2, oprcs.PartyToken);
                break;
            case 3:
                Debug.Log("Failed partyRequestConfirmation: Invalid username");
                Handler.Instance.OnPartyRequestConfirmationSender(3, oprcs.PartyToken);
                break;
            case 4:
                Debug.Log("Failed partyRequestConfirmation: Inexistent username");
                Handler.Instance.OnPartyRequestConfirmationSender(4, oprcs.PartyToken);
                break;
            case 5:
                Debug.Log("Failed partyRequestConfirmation: Inexistent token");
                Handler.Instance.OnPartyRequestConfirmationSender(5, oprcs.PartyToken);
                break;
            case 6:
                Debug.Log("Failed partyRequestConfirmation: Sender not available");
                Handler.Instance.OnPartyRequestConfirmationSender(6, oprcs.PartyToken);
                break;
            case 7:
                Debug.Log("Failed partyRequestConfirmation: Receiver not available");
                Handler.Instance.OnPartyRequestConfirmationSender(7, oprcs.PartyToken);
                break;
            case 8:
                Debug.Log("Failed partyRequestConfirmation: Inexistent partyRequest");
                Handler.Instance.OnPartyRequestConfirmationSender(8, oprcs.PartyToken);
                break;
            case 9:
                Debug.Log("Failed partyRequestConfirmation: Inexistent party");
                Handler.Instance.OnPartyRequestConfirmationSender(9, oprcs.PartyToken);
                break;
            case 10:
                Debug.Log("Failed partyRequestConfirmation: Party not available");
                Handler.Instance.OnPartyRequestConfirmationSender(10, oprcs.PartyToken);
                break;
        }
    }
    private void OnPartyRequestConfirmationReceiver(Net_OnPartyRequestConfirmationReceiver oprcr)
    {
        Debug.Log("PartyRequestConfirmation received");
        Handler.Instance.OnPartyRequestConfirmationReceiver(oprcr.Confirmation, oprcr.ReceiverUsername);
    }
    private void PartyUpdate(Net_PartyUpdate pu)
    {
        Handler.Instance.PartyUpdate(pu.Party);
    }
    private void PartyRequestUpdate(Net_PartyRequestUpdate pru)
    {
        Handler.Instance.PartyUpdate(pru.PartyRequest);
    }
    #endregion

    #region Send
    public void SendServer(NetMsg msg)
    {
        byte[] buffer = new byte[BYTE_SIZE];

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        NetworkTransport.Send(hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
    }

    //Autentication
    public void SendSignUpRequest(string u, string p, string e)
    {
        if (!Helper.IsUsername(u))
        {
            Debug.Log("Failed SignUp: Invalid username");
            Handler.Instance.OnSignUpRequest(3);
            return;
        }
        if(p.Length < 3)
        {
            Debug.Log("Failed SignUp: Password is too short");
            Handler.Instance.OnSignUpRequest(6);
            return;
        }
        if (!Helper.IsEmail(e))
        {
            Debug.Log("Failed SignUp: Invalid email");
            Handler.Instance.OnSignUpRequest(2);
            return;
        }

        Net_SignUpRequest sr = new Net_SignUpRequest
        {
            Username = u,
            Password = Helper.Sha256FromString(p),
            Email = e
        };

        SendServer(sr);
    }
    public void SendLoginRequest(string ue, string p)
    {
        if(!Helper.IsEmail(ue) && !Helper.IsUsername(ue))
        {
            Debug.Log("Failed Login: Invalid username or email");
            Handler.Instance.OnLoginRequest(4);
            return;
        }
        if (p.Length < 3)
        {
            Debug.Log("Failed Login: Password is too short");
            Handler.Instance.OnLoginRequest(5);
            return;
        }

        Net_LoginRequest lr = new Net_LoginRequest
        {
            UsernameOrEmail = ue,
            Password = Helper.Sha256FromString(p)
        };

        SendServer(lr);
    }

    //Profile
    public void SendFullAccountRequest(string u)
    {
        Net_FullAccountRequest fap = new Net_FullAccountRequest
        {
            Username = u
        };

        SendServer(fap);
    }
    public void SendScreennameChangeRequest(string screenname)
    {
        if (!Helper.IsUsername(screenname))
        {
            Debug.Log("Failed screenNameChange: Invalid screenname");
            Handler.Instance.OnScreenNameChangeRequest(3, screenname);
            return;
        }

        Net_ScreennameChangeRequest scr = new Net_ScreennameChangeRequest
        {
            Token = token,
            NewScreenname = screenname
        };

        SendServer(scr);
    }

    //Friends
    public void SendFriendListRequest()
    {
        Net_FriendListRequest flr = new Net_FriendListRequest
        {
            Token = token
        };

        SendServer(flr);
    }
    public void SendFriendRequestListRequest()
    {
        Net_FriendRequestListRequest frlr = new Net_FriendRequestListRequest
        {
            Token = token
        };

        SendServer(frlr);
    }
    public void SendFriendRequest(string friendUsername)
    {
        if(!Helper.IsUsername(friendUsername) && !Helper.IsEmail(friendUsername))
        {
            Debug.Log("Failed FriendRequest: Invalid username");
            Handler.Instance.OnFriendRequestSender(2);
            return;
        }

        Net_FriendRequest fr = new Net_FriendRequest
        {
            ReceiverUsername = friendUsername,
            Token = token
        };

        SendServer(fr);
    }
    public void SendFriendRequestConfirmation(bool c, string su)
    {
        if (!Helper.IsUsername(su))
        {
            Debug.Log("Failed friendRequestConfirmation: Invalid username");
            Handler.Instance.OnFriendRequestConfirmationSender(2, null);
            return;
        }

        Net_FriendRequestConfirmation frc = new Net_FriendRequestConfirmation
        {
            Confirmation = c,
            Token = token,
            SenderUsername = su
        };

        SendServer(frc);
    }
    public void SendFriendRemovalRequest(string ru)
    {
        if (!Helper.IsUsername(ru))
        {
            Debug.Log("Failed FriendRemovalRequest: Invalid username");
            Handler.Instance.OnFriendRemovalRequestSender(2, "");
            return;
        }

        Net_FriendRemovalRequest frr = new Net_FriendRemovalRequest
        {
            ReceiverUsername = ru,
            Token = token
        };

        SendServer(frr);
    }

    //Party
    public void SendCreatePartyRequest()
    {
        Net_CreatePartyRequest cpr = new Net_CreatePartyRequest
        {
            Token = token
        };

        SendServer(cpr);
    }
    public void SendPartyRequest(string ru)
    {
        if (!Helper.IsUsername(ru))
        {
            Debug.Log("Failed PartyRequest: Invalid username");
            Handler.Instance.OnPartyRequestSender(2);
            return;
        }

        Net_PartyRequest pr = new Net_PartyRequest
        {
            ReceiverUsername = ru,
            Token = token,
            PartyToken = PartyManager.Instance.party.Token
        };

        SendServer(pr);
    }
    public void SendPartyRequestConfirmation(bool c, string pt, string su)
    {
        if (!Helper.IsUsername(su))
        {
            Handler.Instance.OnPartyRequestConfirmationSender(2, pt);
            return;
        }

        Net_PartyRequestConfirmation prc = new Net_PartyRequestConfirmation
        {
            Confirmation = c,
            Token = token,
            SenderUsername = su,
            PartyToken = pt
        };

        SendServer(prc);
    }
    public void SendLeavePartyRequest(string pt)
    {
        Net_LeavePartyRequest lpr = new Net_LeavePartyRequest
        {
            Token = token,
            PartyToken = pt
        };

        SendServer(lpr);
    }
    public void SendReadyRequest()
    {
        Net_ReadyRequest rr = new Net_ReadyRequest
        {
            Token = token
        };

        SendServer(rr);
    }
    #endregion
}