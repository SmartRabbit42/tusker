using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyEntry : MonoBehaviour
{
    [SerializeField] private GameObject partyRequestPrefab;

    public static PartyEntry Instance { get; private set; }

    public List<Party> partyRequests = new List<Party>();

    private Button createParty;

    private Transform partyRequestList;

    private Text inviteNotification;

    void Awake() { Instance = this; }

	void Start () {
        createParty = transform.GetChild(0).Find("btn_createParty").GetComponent<Button>();
        createParty.onClick.AddListener(delegate { Handler.Instance.SendCreatePartyRequest(); });

        partyRequestList = transform.GetChild(2).Find("partyRequestList");

        inviteNotification = GameObject.Find("Canvas").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();

        UpdateInviteNotification();
    }

    public void AddPartyRequest(Party partyRequest, string senderUsername)
    {
        foreach(Transform pr in partyRequestList)
            if (pr.GetComponent<PartyRequest>().party.Equals(partyRequest))
                return;

        partyRequests.Add(partyRequest);
        var partyInvitationInstance = Instantiate(partyRequestPrefab, partyRequestList);
        partyInvitationInstance.GetComponent<PartyRequest>().Init(partyRequest, senderUsername);

        UpdateInviteNotification();
    }
    public void RemovePartyRequest(string partyToken)
    {
        foreach (Transform pr in partyRequestList)
        {
            var party = pr.GetComponent<PartyRequest>().party;
            if (party.Token.Equals(partyToken))
            {
                Destroy(pr.gameObject);
                partyRequests.Remove(party);
            }
        }

        UpdateInviteNotification();
    }
    public void UpdatePartyRequest(Party partyRequest)
    {
        foreach (Transform pr in partyRequestList)
        {
            var party = pr.GetComponent<PartyRequest>();
            if (party.party.Token.Equals(partyRequest.Token))
            {
                party.UpdateValues(partyRequest);
                partyRequests[partyRequests.IndexOf(partyRequest)] = partyRequest;
            }
        }
    }

    private void UpdateInviteNotification()
    {
        inviteNotification.text = partyRequests.Count.ToString();

        if (partyRequests.Count == 0)
            inviteNotification.transform.parent.gameObject.SetActive(false);
        else
            inviteNotification.transform.parent.gameObject.SetActive(true);
    }
}