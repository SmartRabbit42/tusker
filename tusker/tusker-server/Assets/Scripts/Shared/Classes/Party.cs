[System.Serializable]
public class Party
{
    public Party()
    {
        Status = 1;

        GameMode = 0;
        GameType = 0;
        GameWay = 0;

        MemberCount = 0;
        Members = new Account[6];
    }

    public string Token { get; set; }

    public byte Status { get; set; }

    public byte GameMode { get; set; }
    public byte GameType { get; set; }
    public byte GameWay { get; set; }

    public Account[] Members { set; get; }
    public byte MemberCount { get; set; }
}
//Status 0 = Unespected behaviour
//Status 1 = Chilling
//Status 2 = Finding match
//Status 3 = Champion select
//Status 4 = Game