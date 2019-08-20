using UnityEngine;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour
{
    [SerializeField] GameObject partyMemberPrefab;
    [SerializeField] GameObject dialogPrefab;

    public static PartyManager Instance { get; set; }

    public Party party;

    private Transform players;

    private Dropdown gameMode;
    private Dropdown gameType;
    private Dropdown gameWay;

    private byte localPlayer;

    void Awake(){ Instance = this; }

    public void Init(Party p)
    {
        players = transform.Find("players");

        var header = transform.Find("header");
        gameMode = header.GetChild(0).GetComponent<Dropdown>();
        gameType = header.GetChild(1).GetComponent<Dropdown>();
        gameWay = header.GetChild(2).GetComponent<Dropdown>();

        header.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { LeaveParty(); });

        UpdateParty(p);
    }

    public void UpdateParty(Party p)
    {
        gameMode.value = p.GameMode;
        gameType.value = p.GameType;
        gameWay.value = p.GameWay;

        foreach (Transform pm in players)
            Destroy(pm.gameObject);

        localPlayer = 255;

        for (int i = 0; i < p.MemberCount; i++)
        {
            var playerInstance = Instantiate(partyMemberPrefab, players);

            if (p.Members[i].Equals(Client.Instance.myAccount))
                localPlayer = (byte)i;

            playerInstance.GetComponent<PartyMember>().Init(p.Members[i], i == localPlayer ? 0 : localPlayer == 255 ? i + 1 : i);
        }

        party = p;
    }

    private void LeaveParty()
    {
        var instance = Instantiate(dialogPrefab, GameObject.Find("Canvas").transform);
        instance.GetComponent<Dialog>().Init(DialogOp.LeaveParty, null);
    }
    public void LeavePartyDialogResult()
    {
        Handler.Instance.SendLeavePartyRequest(party.Token);
    }
}
