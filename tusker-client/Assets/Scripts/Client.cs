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

    #region Responses
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
                OnScreennameChangeRequest((Net_OnScreennameChangeRequest)msg);
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
        Debug.Log(LogManager.OnConnect(oc.FlagEnum));
        Handler.Instance.OnConnect(oc.FlagEnum, oc.Message);
    }
    private void OnDisconnect()
    {
        //Fazer
    }

    //Autentication
    private void OnSignUpRequest(Net_OnSignUpRequest osr)
    {
        Debug.Log(LogManager.OnSignUpRequest(osr.FlagEnum));
        Handler.Instance.OnSignUpRequest(osr.FlagEnum);
    }
    private void OnLoginRequest(Net_OnLoginRequest olr)
    {
        Debug.Log(LogManager.OnLoginRequest(olr.FlagEnum));
        Handler.Instance.OnLoginRequest(olr.FlagEnum);

        if (olr.FlagEnum == 1)
        {
            token = olr.Token;

            myAccount = olr.Account;

            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

    //Profile
    private void OnFullAccountRequest(Net_OnFullAccountRequest ofar)
    {
        Debug.Log(LogManager.OnFullAccountRequest(ofar.FlagEnum));
        Handler.Instance.OnFullAccountRequest(ofar.FlagEnum, ofar.Account);
    }
    private void OnScreennameChangeRequest(Net_OnScreennameChangeRequest oscr)
    {
        Debug.Log(LogManager.OnScreennameChangeRequest(oscr.FlagEnum));
        Handler.Instance.OnScreenNameChangeRequest(oscr.FlagEnum, oscr.screenname);

        if (oscr.FlagEnum == 1)
            myAccount.Screenname = oscr.screenname;
    }

    //Friends
    private void OnFriendListRequest(Net_OnFriendListRequest oflr)
    {
        Debug.Log(LogManager.OnFriendListRequest(oflr.FlagEnum));

        if (oflr.FlagEnum == 1)
            Handler.Instance.OnFriendListRequest(oflr.Friends);
    }
    private void OnFriendRequestListRequest(Net_OnFriendRequestListRequest ofrlr)
    {
        Debug.Log(LogManager.OnFriendRequestListRequest(ofrlr.FlagEnum));

        if (ofrlr.FlagEnum == 1)
            Handler.Instance.OnFriendRequestListRequest(ofrlr.FriendRequests);
    }
    private void OnFriendRequestSender(Net_OnFriendRequestSender ofrs)
    {
        Debug.Log(LogManager.OnFriendRequestSender(ofrs.FlagEnum));
        Handler.Instance.OnFriendRequestSender(ofrs.FlagEnum);
    }
    private void OnFriendRequestReceiver(Net_OnFriendRequestReceiver ofrr)
    {
        Debug.Log("FriendRequest received");
        Handler.Instance.OnFriendRequestReceiver(ofrr.Sender);
    }
    private void OnFriendRequestConfirmationSender(Net_OnFriendRequestConfirmationSender ofrcs)
    {
        Debug.Log(LogManager.OnFriendRequestConfirmationSender(ofrcs.FlagEnum));
        Handler.Instance.OnFriendRequestConfirmationSender(ofrcs.FlagEnum, ofrcs.Sender);
    }
    private void OnFriendRequestConfirmationReceiver(Net_OnFriendRequestConfirmationReceiver ofrcr)
    {
        Debug.Log("FriendRequestConfirmation received");
        Handler.Instance.OnFriendRequestConfirmationReceiver(ofrcr.Receiver, ofrcr.Confirmation);
    }
    private void OnFriendRemovalRequestSender(Net_OnFriendRemovalRequestSender ofrrs)
    {
        Debug.Log(LogManager.OnFriendRemovalRequestSender(ofrrs.FlagEnum));
        Handler.Instance.OnFriendRemovalRequestSender(ofrrs.FlagEnum, ofrrs.ReceiverUsername);
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
        Debug.Log(LogManager.OnCreatePartyRequest(ocpr.FlagEnum));
        Handler.Instance.OnCreatePartyRequest(ocpr.FlagEnum, ocpr.PartyToken);

        if (ocpr.FlagEnum == 1)
            myAccount.Status = 2;
    }
    private void OnPartyRequestSender(Net_OnPartyRequestSender opirr)
    {
        Debug.Log(LogManager.OnPartyRequestSender(opirr.FlagEnum));
        Handler.Instance.OnPartyRequestSender(opirr.FlagEnum);
    }
    private void OnPartyRequestReceiver(Net_OnPartyRequestReceiver opirr)
    {
        Debug.Log("PartyRequest received");
        Handler.Instance.OnPartyRequestReceiver(opirr.PartyRequest, opirr.SenderUsername);
    }
    private void OnPartyRequestConfirmationSender(Net_OnPartyRequestConfirmationSender oprcs)
    {
        Debug.Log(LogManager.OnPartyRequestConfirmationSender(oprcs.FlagEnum));
        Handler.Instance.OnPartyRequestConfirmationSender(oprcs.FlagEnum, oprcs.PartyToken);

        if (oprcs.FlagEnum == 1)
            myAccount.Status = 2;
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

    #region Requests
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