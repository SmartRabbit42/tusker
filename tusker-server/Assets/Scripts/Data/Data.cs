using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public void Init()
    {
        InitDatabase();
    }
    public void Shutdown()
    {
        ShutdownDatabase();
    }

    private const string MONGO_URI = "mongodb://NedLudd:Leleu2002@ds113765.mlab.com:13765/talesofthedata";
    private const string DATABASE_NAME = "talesofthedata";

    private MongoClient client;
    private MongoServer server;
    private MongoDatabase db;

    private MongoCollection<Model_Account> accounts;
    private MongoCollection<Model_Friend> friends;
    private MongoCollection<Model_FriendRequest> friendRequests;
    private MongoCollection<Model_Party> partys;
    private MongoCollection<Model_PartyRequest> partyRequests;

    private void InitDatabase()
    {
        client = new MongoClient(MONGO_URI);
        server = client.GetServer();
        db = server.GetDatabase(DATABASE_NAME);

        accounts = db.GetCollection<Model_Account>("accounts");
        friends = db.GetCollection<Model_Friend>("friends");
        friendRequests = db.GetCollection<Model_FriendRequest>("friendRequests");
        partys = db.GetCollection<Model_Party>("partys");
        partyRequests = db.GetCollection<Model_PartyRequest>("partyRequests");

        Debug.Log("Database initialized");
    }
    private void ShutdownDatabase()
    {
        client = null;
        server.Shutdown();
        db = null;
    }

    #region Fetch
    private Model_Account FindAccountByObjectId(ObjectId id)
    {
        return accounts.FindOne(Query<Model_Account>.EQ(u => u._id, id));
    }
    private Model_Account FindAccountByEmail(string email)
    {
        return accounts.FindOne(Query<Model_Account>.EQ(u => u.Email, email));
    }
    private Model_Account FindAccountByUsername(string username)
    {
        return accounts.FindOne(Query<Model_Account>.EQ(u => u.Username, username));
    }
    private Model_Account FindAccountByToken(string token)
    {
        return accounts.FindOne(Query<Model_Account>.EQ(u => u.Token, token));
    }
    private Model_Account FindAccountByConnectionId(int connectionId)
    {
        return accounts.FindOne(Query<Model_Account>.EQ(u => u.ActiveConnection, connectionId));
    }
    private List<Account> FindAllFriendsByToken(string token)
    {
        var self = new MongoDBRef("account", FindAccountByToken(token)._id);

        if (self == null)
            return null;

        List<Account> selfFriends = new List<Account>();
        foreach (var f in friends.Find(Query.Or(
            Query<Model_Friend>.EQ(f => f.Sender, self),
            Query<Model_Friend>.EQ(f => f.Receiver, self))))
        {
            var receiver = FindAccountByObjectId(f.Receiver.Id.AsObjectId);
            if (receiver.Token == token)
                selfFriends.Add(FindAccountByObjectId(f.Sender.Id.AsObjectId).GetAccount());
            else
                selfFriends.Add(receiver.GetAccount());
        }
        return selfFriends;
    }
    private List<Account> FindAllFriendsByUsername(string username)
    {
        var self = new MongoDBRef("account", FindAccountByUsername(username)._id);

        List<Account> selfFriends = new List<Account>();
        foreach (var f in friends.Find(Query.Or(
            Query<Model_Friend>.EQ(f => f.Sender, self),
            Query<Model_Friend>.EQ(f => f.Receiver, self))))
        {
            var receiver = FindAccountByObjectId(f.Receiver.Id.AsObjectId);
            if (receiver.Username == username)
                selfFriends.Add(FindAccountByObjectId(f.Sender.Id.AsObjectId).GetAccount());
            else
                selfFriends.Add(receiver.GetAccount());
        }
        return selfFriends;
    }
    private List<Account> FindAllFriendRequestsByToken(string token)
    {
        List<Account> selfFriendRequests = new List<Account>();
        foreach (var f in friendRequests.Find(Query<Model_FriendRequest>.EQ(f => f.Receiver, new MongoDBRef("account", FindAccountByToken(token)._id))))
            selfFriendRequests.Add(FindAccountByObjectId(f.Sender.Id.AsObjectId).GetAccount());
        return selfFriendRequests;
    }
    private List<Account> FindAllFriendRequestsByUsername(string username)
    {
        List<Account> selfFriendRequests = new List<Account>();
        foreach (var f in friendRequests.Find(Query<Model_FriendRequest>.EQ(f => f.Receiver, new MongoDBRef("account", FindAccountByUsername(username)._id))))
            selfFriendRequests.Add(FindAccountByObjectId(f.Sender.Id.AsObjectId).GetAccount());
        return selfFriendRequests;
    }
    private Model_Friend FindFriendship(ObjectId senderId, ObjectId receiverId)
    {
        var query = Query.Or(
            Query.And(
                Query<Model_Friend>.EQ(f => f.Sender, new MongoDBRef("account", senderId)),
                Query<Model_Friend>.EQ(f => f.Receiver, new MongoDBRef("account", receiverId))),
            Query.And(
                Query<Model_Friend>.EQ(f => f.Sender, new MongoDBRef("account", receiverId)),
                Query<Model_Friend>.EQ(f => f.Receiver, new MongoDBRef("account", senderId))));

        return friends.FindOne(query);
    }
    private Model_FriendRequest FindfriendRequest(ObjectId senderId, ObjectId receiverId)
    {
        var query = Query.And(
                Query<Model_FriendRequest>.EQ(f => f.Sender, new MongoDBRef("account", senderId)),
                Query<Model_FriendRequest>.EQ(f => f.Receiver, new MongoDBRef("account", receiverId)));

        return friendRequests.FindOne(query);
    }
    private Model_Party FindPartyByToken(string token)
    {
        return partys.FindOne(Query<Model_Party>.EQ(u => u.Token, token));
    }
    private Model_PartyRequest FindPartyRequest(ObjectId partyId, ObjectId senderId, ObjectId receiverId)
    {
        var query = Query.And(
                Query<Model_PartyRequest>.EQ(p => p.Sender, new MongoDBRef("account", senderId)),
                Query<Model_PartyRequest>.EQ(p => p.Receiver, new MongoDBRef("account", receiverId)),
                Query<Model_PartyRequest>.EQ(p => p.Party, new MongoDBRef("party", partyId)));

        return partyRequests.FindOne(query);
    }
    private List<Account> FindAllPartyRequestsByPartyId(ObjectId partyId)
    {
        List<Account> myPartyRequests = new List<Account>();
        foreach (var f in partyRequests.Find(Query<Model_PartyRequest>.EQ(p => p.Party, new MongoDBRef("party", partyId))))
            myPartyRequests.Add(FindAccountByObjectId(f.Receiver.Id.AsObjectId).GetAccount());
        return myPartyRequests;
    }
    #endregion

    #region Insert
    private void InsertAccount(Model_Account newAccount)
    {
        newAccount.CreatedOn = System.DateTime.Now;
        newAccount.Level = 1;
        newAccount.Cash = 4200;

        accounts.Insert(newAccount);
    }
    private byte InsertFriend(ObjectId senderId, ObjectId receiverId)
    {
        var sender = new MongoDBRef("account", senderId);
        var receiver = new MongoDBRef("account", receiverId);

        if (sender == receiver)
            return 2;

        var query = Query.Or(
            Query.And(
                Query<Model_Friend>.EQ(f => f.Sender, sender),
                Query<Model_Friend>.EQ(f => f.Receiver, receiver)),
            Query.And(
                Query<Model_Friend>.EQ(f => f.Sender, receiver),
                Query<Model_Friend>.EQ(f => f.Receiver, sender)));

        if (friends.FindOne(query) != null)
            return 3;

        Model_Friend newFriendship = new Model_Friend
        {
            Sender = sender,
            Receiver = receiver,
            Since = System.DateTime.Now
        };
        friends.Insert(newFriendship);

        return 1;
    }
    private byte InsertFriendRequest(ObjectId senderId, ObjectId receiverId)
    {
        var sender = new MongoDBRef("account", senderId);
        var receiver = new MongoDBRef("account", receiverId);

        if (sender == receiver)
            return 2;

        var query = Query.And(
            Query<Model_FriendRequest>.EQ(u => u.Sender, sender),
            Query<Model_FriendRequest>.EQ(u => u.Receiver, receiver));

        if (friends.FindOne(query) != null)
            return 3;

        Model_FriendRequest newFriendRequest = new Model_FriendRequest
        {
            Sender = sender,
            Receiver = receiver
        };
        friendRequests.Insert(newFriendRequest);

        return 1;
    }
    private byte InsertParty(string token, ObjectId creatorID)
    {
        var creator = new MongoDBRef("account", creatorID);

        Model_Party newParty = new Model_Party
        {
            Token = token,
            Creator = creator,
            Since = System.DateTime.Now,
            Status = 1
        };
        partys.Insert(newParty);
        return 1;
    }
    private byte InsertPartyRequest(ObjectId partyId, ObjectId senderId, ObjectId receiverId)
    {
        var party = new MongoDBRef("party", partyId);

        var query = Query<Model_Party>.EQ(p => p._id, partyId);
        if (partys.FindOne(query) == null)
            return 2;

        var sender = new MongoDBRef("account", senderId);
        var receiver = new MongoDBRef("account", receiverId);

        if (sender == receiver)
            return 3;

        query = Query.And(
            Query<Model_PartyRequest>.EQ(u => u.Sender, sender),
            Query<Model_PartyRequest>.EQ(u => u.Receiver, receiver));
        if (partyRequests.FindOne(query) != null)
            return 4;

        Model_PartyRequest newPartyRequest = new Model_PartyRequest
        {
            Party = party,
            Sender = sender,
            Receiver = receiver
        };
        partyRequests.Insert(newPartyRequest);

        return 1;
    }
    #endregion

    #region Update
    private void UpdateAccount(Model_Account myAccount)
    {
        IMongoQuery query = null;

        query = Query<Model_Account>.EQ(u => u._id, myAccount._id);

        accounts.Update(query, Update<Model_Account>.Replace(myAccount));
    }
    private void UpdateParty(Model_Party myParty)
    {
        IMongoQuery query = null;

        query = Query<Model_Party>.EQ(u => u._id, myParty._id);

        accounts.Update(query, Update<Model_Party>.Replace(myParty));
    }
    #endregion

    #region Delete
    private void DeleteAccount(ObjectId id)
    {
        accounts.Remove(Query<Model_Account>.EQ(a => a._id, id));
    }
    private void DeleteFriend(ObjectId id)
    {
        friends.Remove(Query<Model_Friend>.EQ(f => f._id, id));
    }
    private void DeleteFriendRequest(ObjectId id)
    {
        friendRequests.Remove(Query<Model_FriendRequest>.EQ(f => f._id, id));
    }
    private void DeletePartyRequest(ObjectId id)
    {
        partyRequests.Remove(Query<Model_PartyRequest>.EQ(p => p._id, id));
    }
    #endregion

    #region Others
    private bool IsFriends(string username1, string username2)
    {
        var friend1 = new MongoDBRef("account", FindAccountByUsername(username1)._id);
        var friend2 = new MongoDBRef("account", FindAccountByUsername(username2)._id);

        if (friend1 == null || friend2 == null)
            return false;

        var query = Query.Or(
            Query.And(
                Query<Model_Friend>.EQ(f => f.Sender, friend1),
                Query<Model_Friend>.EQ(f => f.Receiver, friend2)),
            Query.And(
                Query<Model_Friend>.EQ(f => f.Sender, friend2),
                Query<Model_Friend>.EQ(f => f.Receiver, friend1)));

        return friends.FindOne(query) != null;
    }
    private bool IsRequested(string username1, string username2)
    {
        var sender = new MongoDBRef("account", FindAccountByUsername(username1)._id);
        var receiver = new MongoDBRef("account", FindAccountByUsername(username2)._id);

        if (sender == null || receiver == null)
            return false;

        var query = Query.And(
            Query<Model_FriendRequest>.EQ(f => f.Sender, sender),
            Query<Model_FriendRequest>.EQ(f => f.Receiver, receiver));

        return friendRequests.FindOne(query) != null;
    }
    private void UpdateAccount(Account account, string token)
    {
        /* UPDATE ACCOUNT IN PARTY
        if(account.Status > 1)
        {
            var party = FindPartyByToken(account.PartyToken);

            if(party != null)
            {
                int[] target1 = new int[party.Members.Count];

                bool notCreator = false;
                var creator = FindAccountByObjectId(party.Creator.Id.AsObjectId).GetAccount();
                if (!creator.Equals(account))
                {
                    target1[0] = creator.ActiveConnection;
                    notCreator = true;
                }

                for (int i = 0; i < party.Members.Count; i++)
                {
                    var member = FindAccountByObjectId(party.Members[i].Id.AsObjectId);
                    if (!member.Equals(account))
                        target1[notCreator ? i + 1 : i] = member.ActiveConnection;
                }

                Server.Instance.SendPartyMemberUpdate(account, target1);
            }
        }
        */

        var fs = FindAllFriendsByToken(token);
        int[] target2 = new int[fs.Count];
        for (int i = 0; i < fs.Count; i++)
            target2[i] = fs[i].ActiveConnection;

        var frs = FindAllFriendRequestsByToken(token);
        int[] target3 = new int[frs.Count];
        for (int i = 0; i < frs.Count; i++)
            target3[i] = frs[i].ActiveConnection;

        Server.Instance.SendFriendUpdate(account, target2);
        Server.Instance.SendFriendRequestUpdate(account, target3);
    }
    private void UpdateParty(Party party, ObjectId partyId)
    {
        int[] targets1 = new int[party.MemberCount];
        for (int i = 0; i < party.MemberCount; i++)
            targets1[i] = party.Members[i].ActiveConnection;

        var prs = FindAllPartyRequestsByPartyId(partyId);
        int[] targets2 = new int[prs.Count];
        for (int i = 0; i < prs.Count; i++)
            targets2[i] = prs[i].ActiveConnection;

        Server.Instance.SendPartyUpdate(party, targets1);
        Server.Instance.SendPartyRequestUpdate(party, targets2);
    }
    #endregion

    #region Operations
    public byte DisconnectEvent(int connectionId)
    {
        var myAccount = FindAccountByConnectionId(connectionId);

        if (myAccount == null)
            return 2;

        myAccount.Status = 0;
        myAccount.ActiveConnection = 0;
        UpdateAccount(myAccount);
        UpdateAccount(myAccount.GetAccount(), myAccount.Token);
        return 1;
    }

    public byte SignUpRequest(string username, string password, string email)
    {
        if (!Helper.IsEmail(email))
            return 2;
        if (!Helper.IsUsername(username))
            return 3;
        if (FindAccountByEmail(email) != null)
            return 4;
        if (FindAccountByUsername(username) != null)
            return 5;

        Model_Account newAccount = new Model_Account
        {
            Username = username,
            Screenname = username,
            ShaPassword = password,
            Email = email
        };

        InsertAccount(newAccount);

        return 1;
    }
    public byte LoginRequest(string usernameOrEmail, string password, int cnnId, out Model_Account account)
    {
        IMongoQuery query = null;

        if (Helper.IsEmail(usernameOrEmail))
        {
            query = Query<Model_Account>.EQ(u => u.Email, usernameOrEmail);
            account = accounts.FindOne(query);
            if (account == null)
                return 2;
            else if (account.ShaPassword != password)
            {
                account = null;
                return 3;
            }
        }
        else
        {
            query = Query<Model_Account>.EQ(u => u.Username, usernameOrEmail);
            account = accounts.FindOne(query);
            if (account == null)
                return 2;
            else if (account.ShaPassword != password)
            {
                account = null;
                return 3;
            }
        }

        account.ActiveConnection = cnnId;
        do
            account.Token = Helper.CreateToken(64);
        while (FindAccountByToken(account.Token) != null);
        account.Status = 1;
        account.LastLogin = System.DateTime.Now;

        accounts.Update(query, Update<Model_Account>.Replace(account));
        UpdateAccount(account.GetAccount(), account.Token);
        return 1;
    }

    public byte FullAccountRequest(string username, out FullAccount fullAccount)
    {
        var account = FindAccountByUsername(username);
        if (account == null)
        {
            fullAccount = null;
            return 2;
        }

        fullAccount = account.GetFullAccount();
        return 1;
    }
    public byte ScreennameChangeRequest(string token, string newScreenname)
    {
        if (!Helper.IsUsername(newScreenname))
            return 3;

        IMongoQuery query = null;
        Model_Account account = null;

        query = Query<Model_Account>.EQ(a => a.Token, token);
        if (query == null)
            return 2;

        account = accounts.FindOne(query);
        account.Screenname = newScreenname;

        accounts.Update(query, Update<Model_Account>.Replace(account));
        UpdateAccount(account.GetAccount(), account.Token);
        return 1;
    }

    public byte FriendListRequest(string token, out List<Account> friends)
    {
        friends = FindAllFriendsByToken(token);
        if (friends == null)
            return 2;
        if (friends.Count < 1)
            return 3;

        return 1;
    }
    public byte FriendRequestListRequest(string token, out List<Account> friendRequests)
    {
        friendRequests = FindAllFriendRequestsByToken(token);
        if (friendRequests == null)
            return 2;
        if (friendRequests.Count < 1)
            return 3;

        return 1;
    }
    public byte FriendRequest(string token, string username, out Account friend, out byte id)
    {
        var sender = FindAccountByToken(token);
        var receiver = FindAccountByUsername(username);

        id = 0;
        friend = sender.GetAccount();

        if (!Helper.IsUsername(username))
            return 2;
        if (receiver == null)
            return 3;
        else if (sender == null)
            return 4;
        else if (sender.Username == receiver.Username)
            return 5;
        else if (IsFriends(sender.Username, receiver.Username))
            return 6;
        else if (IsRequested(sender.Username, receiver.Username))
            return 7;

        InsertFriendRequest(sender._id, receiver._id);

        id = (byte)receiver.ActiveConnection;
        return 1;
    }
    public byte FriendRequestConfirmation(string token, string username, bool confirmation, out Account sender, out Account receiver)
    {
        var s = FindAccountByUsername(username);
        sender = s.GetAccount();
        var r = FindAccountByToken(token);
        receiver = r.GetAccount();

        if (!Helper.IsUsername(username))
            return 3;
        else if (r == null)
            return 4;
        else if (s == null)
            return 5;

        DeleteFriendRequest(FindfriendRequest(s._id, r._id)._id);
        try { DeleteFriendRequest(FindfriendRequest(r._id, s._id)._id); } catch { Debug.Log("nao achou"); }

        if (s.Username == r.Username)
            return 6;
        else
            switch (confirmation)
            {
                default:
                case false:
                    return 1;
                case true:
                    switch (InsertFriend(s._id, r._id))
                    {
                        default:
                        case 0:
                            return 0;
                        case 1:
                            return 1;
                        case 2:
                            return 6;
                        case 3:
                            return 7;
                    }
            }
    }
    public byte FriendRemovalRequest(string token, string receiverUsername, out string senderUsername, out byte id)
    {
        var sender = FindAccountByToken(token);
        var receiver = FindAccountByUsername(receiverUsername);

        senderUsername = sender.Username;
        id = (byte)receiver.ActiveConnection;

        var friendship = FindFriendship(sender._id, receiver._id);
        if (friendship == null)
            return 2;

        DeleteFriend(friendship._id);

        return 1;
    }

    public byte CreatePartyRequest(string token, out string partyToken)
    {
        partyToken = null;
        var account = FindAccountByToken(token);
        if (account == null)
            return 2;

        account.Status = 2;
        account.Priority = 2;
        account.PartyToken = partyToken;
        account.Ready = false;

        UpdateAccount(account);

        do
            partyToken = Helper.CreateToken(64);
        while (FindPartyByToken(partyToken) != null);

        InsertParty(partyToken, account._id);

        UpdateAccount(account.GetAccount(), token);
        return 1;
    }
    public byte PartyRequest(string partyToken, string token, string username, out byte id, out Party myParty, out string senderUsername)
    {
        id = 0;
        myParty = null;
        senderUsername = null;

        var sender = FindAccountByToken(token);
        var receiver = FindAccountByUsername(username);
        var party = FindPartyByToken(partyToken);  

        if (!Helper.IsUsername(username))
            return 2;
        if (sender == null)
            return 3;
        else if (receiver == null)
            return 4;
        else if (sender._id == receiver._id)
            return 5;
        else if (sender.Status != 2)
            return 6;
        else if (receiver.Status != 1)
            return 7;
        if (party == null)
            return 8;
        if (FindPartyRequest(party._id, sender._id, receiver._id) != null)
            return 9;

        InsertPartyRequest(party._id, sender._id, receiver._id);

        id = (byte)receiver.ActiveConnection;
        myParty = party.GetParty();
        senderUsername = sender.Username;

        myParty.Members = new Account[6];
        myParty.Members[0] = FindAccountByObjectId(party.Creator.Id.AsObjectId).GetAccount();
        for (int i = 1; i < myParty.MemberCount; i++)
            myParty.Members[i] = FindAccountByObjectId(party.Members[i].Id.AsObjectId).GetAccount();
        return 1;
    }
    public byte PartyRequestConfirmation(bool confirmation, string token, string senderUsername, string partyToken, out byte id, out string username)
    {
        id = 0;
        username = null;

        if (!Helper.IsUsername(senderUsername))
            return 3;

        var sender = FindAccountByUsername(senderUsername);
        var receiver = FindAccountByToken(token);

        if (sender == null)
            return 4;
        if (receiver == null)
            return 5;
        if (sender.Status != 2)
            return 6;
        if (receiver.Status != 1)
            return 7;

        var party = FindPartyByToken(partyToken);

        if (party == null)
            return 9;
        if (party.Status != 1)
            return 10;

        var partyRequest = FindPartyRequest(party._id, sender._id, receiver._id);

        if (partyRequest == null)
            return 8;

        id = (byte)sender.ActiveConnection;
        username = receiver.Username;
        DeletePartyRequest(partyRequest._id);

        if (!confirmation)
            return 2;

        receiver.Status = 2;
        receiver.PartyToken = partyToken;
        receiver.Priority = 0;
        receiver.Ready = false;

        party.Members.Add(new MongoDBRef("account", receiver._id));
        UpdateAccount(receiver);
        UpdateParty(party);

        var myParty = party.GetParty();
        myParty.Members = new Account[6];
        myParty.Members[0] = FindAccountByObjectId(party.Creator.Id.AsObjectId).GetAccount();
        for (int i = 1; i < myParty.MemberCount; i++)
            myParty.Members[i] = FindAccountByObjectId(party.Members[i - 1].Id.AsObjectId).GetAccount();

        UpdateParty(myParty, party._id);
        UpdateAccount(receiver.GetAccount(), receiver.Token);
        return 1;
    }
    #endregion
}
