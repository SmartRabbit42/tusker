using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    public static Server Instance { get; private set; }

    private const int MAX_USER = 100;
    private const int PORT = 4242;
    private const int BYTE_SIZE = 1024;

    private readonly string welcomeMessage = "Welcome to Tales of the Fallen";
    private byte serverStatus = 0;

    private byte reliableChannel;
    private int hostId;

    private bool isStarted = false;
    private byte error;

    private Data data;

    private Dictionary<int, string> clients = new Dictionary<int, string>();

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
        data = new Data();
        data.Init();

        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(cc, MAX_USER);

        hostId = NetworkTransport.AddHost(topology, PORT, null);

        Debug.Log(string.Format("Opening connection on port {0}", PORT));
        serverStatus = 1;
        isStarted = true;
    }
    public void ShutDown()
    {
        serverStatus = 0;
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
                ConnectEvent(connectionId);
                break;
            case NetworkEventType.DisconnectEvent:
                DisconnectEvent(connectionId);
                break;
            case NetworkEventType.DataEvent:
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);

                OnData(connectionId, channelId, msg);
                break;
            default:
            case NetworkEventType.BroadcastEvent:
                Debug.LogError("Unespected NetworkEventType");
                break;
        }
    }

    #region Receive
    private void OnData(int cnnId, int channelId, NetMsg msg)
    {
        switch (msg.OperationCode)
        {
            default:
            case NetOP.None:
                Debug.LogError("Unespected NET Operation Code");
                break;

            //Autentication
            case NetOP.SignUpRequest:
                SignUpRequest(cnnId, (Net_SignUpRequest)msg);
                break;
            case NetOP.LoginRequest:
                LoginRequest(cnnId, (Net_LoginRequest)msg);
                break;

            //Profile
            case NetOP.FullAccountRequest:
                FullAccountRequest(cnnId, (Net_FullAccountRequest)msg);
                break;
            case NetOP.ScreennameChangeRequest:
                ScreenNameChangeRequest(cnnId, (Net_ScreennameChangeRequest)msg);
                break;

            //Friends
            case NetOP.FriendListRequest:
                FriendListRequest(cnnId, (Net_FriendListRequest)msg);
                break;
            case NetOP.FriendRequestListRequest:
                FriendRequestListRequest(cnnId, (Net_FriendRequestListRequest)msg);
                break;
            case NetOP.FriendRequest:
                FriendRequest(cnnId, (Net_FriendRequest)msg);
                break;
            case NetOP.FriendRequestConfirmation:
                FriendRequestConfirmation(cnnId, (Net_FriendRequestConfirmation)msg);
                break;
            case NetOP.FriendRemovalRequest:
                FriendRemovalRequest(cnnId, (Net_FriendRemovalRequest)msg);
                break;

            //Party
            case NetOP.CreatePartyRequest:
                CreatePartyRequest(cnnId, (Net_CreatePartyRequest)msg);
                break;
            case NetOP.PartyRequest:
                PartyRequest(cnnId, (Net_PartyRequest)msg);
                break;
            case NetOP.PartyRequestConfirmation:
                PartyRequestConfirmation(cnnId, (Net_PartyRequestConfirmation)msg);
                break;
            case NetOP.LeavePartyRequest:
                LeavePartyRequest(cnnId, (Net_LeavePartyRequest)msg);
                break;
            case NetOP.ReadyRequest:
                ReadRequest(cnnId, (Net_ReadyRequest)msg);
                break;
        }
    }

    private void ConnectEvent(int cnnId)
    {
        clients.Add(cnnId, "anon" + cnnId);

        Debug.Log(LogManager.ConnectEvent(clients[cnnId]));

        Net_OnConnect oc = new Net_OnConnect
        {
            FlagEnum = serverStatus,
            Message = welcomeMessage
        };

        SendClient(cnnId, oc);
    }
    private void DisconnectEvent(int cnnId)
    {
        byte flagEnum = data.DisconnectEvent(cnnId);
        Debug.Log(LogManager.DisconnectEvent(flagEnum, clients[cnnId]));
        clients.Remove(cnnId);
    }

    //Autentication
    private void SignUpRequest(int cnnId, Net_SignUpRequest sr)
    {
        byte flagEnum = data.SignUpRequest(sr.Username, sr.Password, sr.Email);

        Debug.Log(LogManager.SignUpRequest(flagEnum, clients[cnnId]));

        Net_OnSignUpRequest oca = new Net_OnSignUpRequest
        {
            FlagEnum = flagEnum
        };

        SendClient(cnnId, oca);
    }
    private void LoginRequest(int cnnId, Net_LoginRequest lr)
    {
        Model_Account account;
        byte flagEnum = data.LoginRequest(lr.UsernameOrEmail, lr.Password, cnnId, out account);

        if (flagEnum == 1)
        {
            Debug.Log("user " + clients[cnnId] + " became " + account.Username);
            clients[cnnId] = account.Username;
        }

        Debug.Log(LogManager.LoginRequest(flagEnum, clients[cnnId]));

        Net_OnLoginRequest olr = new Net_OnLoginRequest
        {
            FlagEnum = flagEnum,
            Account = account == null ? null : account.GetAccount(),
            Token = account == null ? null : account.Token
        };

        SendClient(cnnId, olr);
    }

    //Profile
    private void FullAccountRequest(int cnnId, Net_FullAccountRequest far)
    {
        FullAccount fullAccount;
        byte flagEnum = data.FullAccountRequest(far.Username, out fullAccount);

        Debug.Log(LogManager.FullAccountRequest(flagEnum, clients[cnnId]));

        Net_OnFullAccountRequest ofar = new Net_OnFullAccountRequest
        {
            FlagEnum = flagEnum,
            Account = fullAccount
        };

        SendClient(cnnId, ofar);
    }
    private void ScreenNameChangeRequest(int cnnId, Net_ScreennameChangeRequest scr)
    {
        byte flagEnum = data.ScreennameChangeRequest(scr.Token, scr.NewScreenname);

        Debug.Log(LogManager.ScreenNameChangeRequest(flagEnum, clients[cnnId]));

        Net_OnScreennameChangeRequest oscr = new Net_OnScreennameChangeRequest
        {
            FlagEnum = flagEnum,
            screenname = scr.NewScreenname
        };

        SendClient(cnnId, oscr);
    }

    //Friends
    private void FriendListRequest(int cnnId, Net_FriendListRequest flr)
    {
        List<Account> friends;
        byte flagEnum = data.FriendListRequest(flr.Token, out friends);

        Debug.Log(LogManager.FriendListRequest(flagEnum, clients[cnnId]));

        Net_OnFriendListRequest oflr = new Net_OnFriendListRequest
        {
            FlagEnum = flagEnum,
            Friends = friends
        };

        SendClient(cnnId, oflr);
    }
    private void FriendRequestListRequest(int cnnId, Net_FriendRequestListRequest frlr)
    {
        List<Account> friendRequest;
        byte flagEnum = data.FriendRequestListRequest(frlr.Token, out friendRequest);

        Debug.Log(LogManager.FriendRequestListRequest(flagEnum, clients[cnnId]));

        Net_OnFriendRequestListRequest ofrlr = new Net_OnFriendRequestListRequest
        {
            FlagEnum = flagEnum,
            FriendRequests = friendRequest
        };

        SendClient(cnnId, ofrlr);
    }
    private void FriendRequest(int cnnId, Net_FriendRequest fr)
    {
        byte id;
        Account sender;
        byte flagEnum = data.FriendRequest(fr.Token, fr.ReceiverUsername, out sender, out id);

        Debug.Log(LogManager.FriendRequest(flagEnum, clients[cnnId]));

        Net_OnFriendRequestSender ofrs = new Net_OnFriendRequestSender
        {
            FlagEnum = flagEnum
        };

        if (flagEnum == 1)
        {
            Net_OnFriendRequestReceiver ofrr = new Net_OnFriendRequestReceiver
            {
                Sender = sender
            };

            SendClient(id, ofrr);
        }

        SendClient(cnnId, ofrs);
    }
    private void FriendRequestConfirmation(int cnnId, Net_FriendRequestConfirmation frc)
    {
        Account sender, receiver;
        byte flagEnum = data.FriendRequestConfirmation(frc.Token, frc.SenderUsername, frc.Confirmation, out sender, out receiver);

        Debug.Log(LogManager.FriendRequestConfirmation(flagEnum, clients[cnnId]));

        Net_OnFriendRequestConfirmationSender ofrcs = new Net_OnFriendRequestConfirmationSender
        {
            FlagEnum = flagEnum,
            Sender = sender
        };

        if (flagEnum == 1)
        {
            if (frc.Confirmation)
                ofrcs.FlagEnum = 2;

            Net_OnFriendRequestConfirmationReceiver ofrcr = new Net_OnFriendRequestConfirmationReceiver
            {
                Receiver = receiver,
                Confirmation = frc.Confirmation
            };

            SendClient(sender.ActiveConnection, ofrcr);
        }

        SendClient(cnnId, ofrcs);
    }
    private void FriendRemovalRequest(int cnnId, Net_FriendRemovalRequest flr)
    {
        byte id;
        string senderReceiver;
        byte flagEnum = data.FriendRemovalRequest(flr.Token, flr.ReceiverUsername, out senderReceiver, out id);

        Debug.Log(LogManager.FriendRemovalRequest(flagEnum, clients[cnnId]));

        Net_OnFriendRemovalRequestSender ofrrs = new Net_OnFriendRemovalRequestSender
        {
            FlagEnum = flagEnum,
            ReceiverUsername = flr.ReceiverUsername
        };

        if (flagEnum == 1)
        {
            Net_OnFriendRemovalRequestReceiver ofrrr = new Net_OnFriendRemovalRequestReceiver
            {
                SenderUsername = senderReceiver
            };

            SendClient(id, ofrrr);
        }
        
        SendClient(cnnId, ofrrs);
    }

    //Party
    private void CreatePartyRequest(int cnnId, Net_CreatePartyRequest cpr)
    {
        string partyToken;
        byte flagEnum = data.CreatePartyRequest(cpr.Token, out partyToken);

        Debug.Log(LogManager.CreatePartyRequest(flagEnum, clients[cnnId]));

        Net_OnCreatePartyRequest ocpr = new Net_OnCreatePartyRequest
        {
            FlagEnum = flagEnum,
            PartyToken = partyToken
        };

        SendClient(cnnId, ocpr);
    }
    private void PartyRequest(int cnnId, Net_PartyRequest pir)
    {
        byte id;
        string username;
        Party partyRequest;
        byte flagEnum = data.PartyRequest(pir.PartyToken, pir.Token, pir.ReceiverUsername, out id, out partyRequest, out username);

        Debug.Log(LogManager.PartyRequest(flagEnum, clients[cnnId]));

        Net_OnPartyRequestSender opirs = new Net_OnPartyRequestSender
        {
            FlagEnum = flagEnum
        };

        if (flagEnum == 1)
        {
            Net_OnPartyRequestReceiver opirr = new Net_OnPartyRequestReceiver
            {
                PartyRequest = partyRequest,
                SenderUsername = username
            };

            SendClient(id, opirr);
        }

        SendClient(cnnId, opirs);
    }
    private void PartyRequestConfirmation(int cnnId, Net_PartyRequestConfirmation prc)
    {
        string username;
        byte id;
        byte flagEnum = data.PartyRequestConfirmation(prc.Confirmation, prc.Token, prc.SenderUsername, prc.PartyToken, out id, out username);

        Debug.Log(LogManager.PartyRequestConfirmation(flagEnum, clients[cnnId]));

        Net_OnPartyRequestConfirmationSender oprcs = new Net_OnPartyRequestConfirmationSender
        {
            FlagEnum = flagEnum,
            PartyToken = prc.PartyToken
        };

        if (flagEnum == 1 || flagEnum == 2)
        {
            Net_OnPartyRequestConfirmationReceiver oprcr = new Net_OnPartyRequestConfirmationReceiver
            {
                Confirmation = prc.Confirmation,
                ReceiverUsername = username
            };

            SendClient(id, oprcr);
        }
        
        SendClient(cnnId, oprcs);
    }
    private void LeavePartyRequest(int cnnId, Net_LeavePartyRequest lpr)
    {

    }
    private void ReadRequest(int cnnId, Net_ReadyRequest rr)
    {

    }
    #endregion

    #region Send
    public void SendClient(int cnnId, NetMsg msg)
    {
        if (cnnId == 0)
            return;

        byte[] buffer = new byte[BYTE_SIZE];

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        NetworkTransport.Send(hostId, cnnId, reliableChannel, buffer, BYTE_SIZE, out error);
    }

    public void SendFriendUpdate(Account account, int[] targets)
    {
        foreach (int id in targets)
        {
            Net_FriendUpdate fu = new Net_FriendUpdate
            {
                Friend = account
            };

            SendClient(id, fu);
        }
    }
    public void SendFriendRequestUpdate(Account account, int[] targets)
    {
        foreach (int id in targets)
        {
            Net_FriendRequestUpdate fru = new Net_FriendRequestUpdate
            {
                FriendRequest = account
            };

            SendClient(id, fru);
        }
    }
    public void SendPartyUpdate(Party party, int[] targets)
    {
        foreach (int id in targets)
        {
            Net_PartyUpdate pu = new Net_PartyUpdate
            {
                Party = party
            };

            SendClient(id, pu);
        }
    }
    public void SendPartyRequestUpdate(Party party, int[] targets)
    {
        foreach (int id in targets)
        {
            Net_PartyRequestUpdate pru= new Net_PartyRequestUpdate
            {
                PartyRequest = party
            };

            SendClient(id, pru);
        }
    }
    public void SendPartyMemberUpdate(Account account, int[] targets)
    {
        /*
        foreach (int id in targets)
        {
            Net_PartyRequestUpdate pru = new Net_PartyRequestUpdate
            {
                PartyRequest = account
            };

            SendClient(id, pru);
        }
        */
    }
    #endregion
}
