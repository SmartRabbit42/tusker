using UnityEngine;
using UnityEngine.UI;

public class PartyRequest : MonoBehaviour {

    public Party party;
    public string senderUsername;

    private Text invitation;
    private Text members;
    private Text gameConfiguration;

	public void Init(Party pr, string su)
    {
        name = "partyInvitation";

        invitation = transform.Find("txt_invitation").GetComponent<Text>();
        members = transform.Find("txt_members").GetComponent<Text>();
        gameConfiguration = transform.Find("txt_gameConfiguration").GetComponent<Text>();

        transform.Find("btn_accept").GetComponent<Button>().onClick.AddListener(delegate { Handler.Instance.SendPartyRequestConfirmation(true, pr.Token, senderUsername); });
        transform.Find("btn_decline").GetComponent<Button>().onClick.AddListener(delegate { Handler.Instance.SendPartyRequestConfirmation(false, pr.Token, senderUsername); });

        invitation.text = su + " invited you to a party";

        senderUsername = su; 
        UpdateValues(pr);
    }

    public void UpdateValues(Party pr)
    {
        members.text = pr.MemberCount + "/6";
        gameConfiguration.text = GameConfiguration(pr.GameMode, pr.GameType, pr.GameWay);

        party = pr;
    }

    private string GameConfiguration(byte mode, byte type, byte way)
    {
        string res = "";
        switch (mode)
        {
            default:
            case 0:
                res += "Autogeneration > ";
                break;
            case 1:
                res += "Creation > ";
                break;
            case 2:
                res += "Party > ";
                break;
        }
        switch (type)
        {
            default:
            case 0:
                res += "Normal > ";
                break;
            case 1:
                res += "Ranked > ";
                break;
            case 2:
                res += "Custom > ";
                break;
        }
        switch (way)
        {
            default:
            case 0:
                res += "ASAS";
                break;
            case 1:
                res += "ASASA";
                break;
            case 2:
                res += "ASASASS";
                break;
        }
        return res;
    }
}
