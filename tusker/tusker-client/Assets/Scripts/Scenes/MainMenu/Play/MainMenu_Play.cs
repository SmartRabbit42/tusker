using UnityEngine;

public class MainMenu_Play : MonoBehaviour
{
    [SerializeField] private GameObject alertPrefab;

    public static MainMenu_Play Instance { get; private set; }

    private GameObject partyEntry;
    private GameObject partyMain;

    void Awake() { Instance = this; }

    void Start()
    {
        partyEntry = transform.Find("partyEntry").gameObject;
        partyMain = transform.Find("partyMain").gameObject;

        partyMain.SetActive(false);

        PartyManager.Instance.Init(new Party());
    }

    #region Helper
    private void OpenParty(bool value)
    {
        partyEntry.SetActive(!value);
        partyMain.SetActive(value);
    }
    private void RemovePartyRequest(string partyToken)
    {
        PartyEntry.Instance.RemovePartyRequest(partyToken);
    }
    private void Alert(string message)
    {
        var instance = Instantiate(alertPrefab, GameObject.Find("Canvas").transform);
        instance.GetComponent<Alert>().Init(message);
    }
    #endregion

    #region Response
    public void OnCreatePartyRequest(byte flagEnum, string partyToken)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert("Failed createPartyRequest: Unespected error");
                break;
            case 1:
                Alert("Successfull createPartyRequest");

                Party myParty = new Party();

                Client.Instance.myAccount.Priority = 2;
                myParty.MemberCount++;
                myParty.Members[0] = Client.Instance.myAccount;
                myParty.Token = partyToken;

                PartyManager.Instance.UpdateParty(myParty);
                OpenParty(true);
                break;
            case 2:
                Alert("Failed createPartyRequest: Inexistent token");
                break;
            case 3:
                Alert("Failed createPartyRequest: Already in a party");
                break;
        }
    }
    public void OnPartyRequestSender(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert("Failed partyRequest: Unespected error");
                break;
            case 1:
                Alert("Successfull partyRequest");
                break;
            case 2:
                Alert("Failed partyRequest: Invalid username");
                break;
            case 3:
                Alert("Failed partyRequest: Inexistent token");
                break;
            case 4:
                Alert("Failed partyRequest: Inexistent username");
                break;
            case 5:
                Alert("Failed partyRequest: Trying to invite yourself");
                break;
            case 6:
                Alert("Failed partyRequest: You are not in a party");
                break;
            case 7:
                Alert("Failed partyRequest: Receiver not available");
                break;
            case 8:
                Alert("Failed partyRequest: Inexistent partyToken");
                break;
            case 9:
                Alert("Failed partyRequest: Already requested");
                break;
        }
    }
    public void OnPartyRequestReceiver(Party partyRequest, string senderUsername)
    {
        if (partyRequest == null)
            return;
        Alert(senderUsername + " invited you to a party");

        PartyEntry.Instance.AddPartyRequest(partyRequest, senderUsername);
    }
    public void OnPartyRequestConfirmationSender(byte flagEnum, string partyToken)
    {
        switch (flagEnum)
        {
            case 0:
                Alert("Failed partyRequestConfirmation: Unespected error");
                break;
            case 1:
                Alert("Successfull partyRequestConfirmation");
                RemovePartyRequest(partyToken);

                OpenParty(true);
                break;
            case 2:
                Alert("Successfull partyRequestConfirmation");
                RemovePartyRequest(partyToken);
                break;
            case 3:
                Alert("Failed partyRequestConfirmation: Invalid username");
                RemovePartyRequest(partyToken);
                break;
            case 4:
                Alert("Failed partyRequestConfirmation: Inexistent username");
                RemovePartyRequest(partyToken);
                break;
            case 5:
                Alert("Failed partyRequestConfirmation: Inexistent token");
                RemovePartyRequest(partyToken);
                break;
            case 6:
                Alert("Failed partyRequestConfirmation: Sender not available");
                RemovePartyRequest(partyToken);
                break;
            case 7:
                Alert("Failed partyRequestConfirmation: Receiver not available");
                RemovePartyRequest(partyToken);
                break;
            case 8:
                Alert("Failed partyRequestConfirmation: Inexistent partyRequest");
                RemovePartyRequest(partyToken);
                break;
            case 9:
                Alert("Failed partyRequestConfirmation: Inexistent party");
                RemovePartyRequest(partyToken);
                break;
            case 10:
                Alert("Failed partyRequestConfirmation: Party not available");
                RemovePartyRequest(partyToken);
                break;
        }
    }
    public void OnPartyRequestConfirmationReceiver(bool confirmation, string receiverUsername)
    {
        Alert(string.Format("{0} {1} your party request", receiverUsername, confirmation ? "accepted" : "declined"));
    }
    public void PartyUpdate(Party party)
    {
        PartyEntry.Instance.RemovePartyRequest(party.Token);
        PartyManager.Instance.UpdateParty(party);
    }
    public void PartyRequestUpdate(Party partyRequest)
    {
        PartyEntry.Instance.UpdatePartyRequest(partyRequest);
    }
    #endregion
}