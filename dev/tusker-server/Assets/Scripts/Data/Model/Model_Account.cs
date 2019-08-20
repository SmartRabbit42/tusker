using MongoDB.Bson;
using System;

public class Model_Account
{
    public ObjectId _id;

    public int ActiveConnection { get; set; }
    public string Username { get; set; }
    public string Screenname { get; set; }
    public string Email { get; set; }
    public string ShaPassword { get; set; }

    public byte Status { get; set; }
    public string Token { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastLogin { get; set; }

    public byte Level { get; set; }
    public int Cash { get; set; }

    //Party
    public string PartyToken { get; set; }
    public byte Priority { get; set; }
    public bool Ready { get; set; }

    public Account GetAccount()
    {
        Account myAccount = new Account
        {
            ActiveConnection = ActiveConnection,
            Status = Status,
            Username = Username,
            Screenname = Screenname,
            Level = Level,

            Priority = Priority,
            PartyToken = PartyToken,
            Ready = Ready
        };
        return myAccount;
    }
    public FullAccount GetFullAccount()
    {
        FullAccount myAccount = new FullAccount();
        myAccount.ActiveConnection = ActiveConnection;
        myAccount.Status = Status;
        myAccount.Username = Username;
        myAccount.Screenname = Screenname;
        myAccount.Level = Level;

        myAccount.Cash = Cash;
        return myAccount;
    }
}