[System.Serializable]
public class Account
{
	public int ActiveConnection { get; set; }

    public byte Status { get; set; }

    public string Username { get; set; }
    public string Screenname { get; set; }

    public byte Level { get; set; }

    public string PartyToken { get; set; }
    public byte Priority { get; set; }
    public bool Ready { get; set; }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (obj.GetType() != GetType())
            return false;

        Account acc = (Account)obj;

        if (acc.ActiveConnection != ActiveConnection)
            return false;
        //if (acc.Status != Status)
        //    return false;
        if (acc.Username != Username)
            return false;

        return true;
    }
    public override int GetHashCode()
    {
        int value = 175;
        value += Status.GetHashCode() * 2;
        value += Username.GetHashCode() * 2;
        value += Screenname.GetHashCode() * 2;
        value += Level.GetHashCode() * 2;

        return value;
    }
}
[System.Serializable]
public class FullAccount : Account
{
    public int Cash { get; set; }

    public Account GetAccount()
    {
        Account myAccount = new Account();
        myAccount.ActiveConnection = ActiveConnection;
        myAccount.Status = Status;
        myAccount.Username = Username;
        myAccount.Screenname = Screenname;
        myAccount.Level = Level;
        return myAccount;
    }
}
// Status 0 = Offline
// Status 1 = Online
// Status 2 = On party
// Status 3 = Looking for match
// Status 4 = Champion Select
// Status 5 = On game