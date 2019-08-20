using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public static Profile Instance { get; set; }

    [SerializeField] private GameObject alertPrefab;
    [SerializeField] private GameObject dialogPrefab;

    private Text username;
    private InputField screenname;
    private Text level;
    private Text cash;

    private Button quit;

    public Account profile;

    public void Init(Account p)
    {
        Instance = this;

        EnableInputs(false);

        name = string.Format("{0}_profile", p.Username);

        var header = transform.GetChild(0);
        username = header.Find("txt_username").GetComponent<Text>();
        level = header.Find("txt_level").GetComponent<Text>();
        screenname = header.Find("ipf_screenname").GetComponent<InputField>();
        quit = header.Find("btn_quit").GetComponent<Button>();

        cash = transform.Find("txt_cash").GetComponent<Text>();

        if (p.Equals(Client.Instance.myAccount))
            screenname.onEndEdit.AddListener(delegate { ChangeScreenName(); });
        else
            screenname.interactable = false;

        quit.onClick.AddListener(() => Quit());

        UpdateValues(p);
        Handler.Instance.SendFullAccountRequest(p.Username);
    }

    public void UpdateValues(Account p)
    {
        username.text = p.Username;
        screenname.text = p.Screenname;
        level.text = p.Level.ToString();

        profile = p;
    }

    private void ChangeScreenName()
    {
        if (screenname.text == profile.Screenname)
            return;

        var instance = Instantiate(dialogPrefab, transform);
        instance.GetComponent<Dialog>().Init(DialogOp.ScreenNameChange, gameObject);
    }
    private void Quit()
    {
        EnableInputs(true);
        Destroy(gameObject);
    }

    #region Helper
    private void Alert(string message)
    {
        var instance = Instantiate(alertPrefab, transform);
        instance.GetComponent<Alert>().Init(message);
    }
    private void EnableInputs(bool value)
    {
        transform.parent.GetComponent<CanvasGroup>().interactable = value;
    }
    #endregion

    #region Response
    public void OnFullAccountRequest(byte flagEnum, FullAccount fa)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert("Failed fullAccountRequest: Unespected error");
                break;
            case 1:
                UpdateValues(fa.GetAccount());

                cash.text = fa.Cash + "c";

                break;
            case 2:
                Alert("Failed fullAccountRequest: Inexistent token");
                break;
            case 3:
                Alert("Failed fullAccountRequest: Inexistent username");
                break;
        }
    }

    public void OnScreenNameChangeRequest(byte flagEnum, string screenname)
    {
        switch (flagEnum)
        {
            case 0:
                Alert("Failed ScreenNameChange: Unespected error");
                this.screenname.text = profile.Screenname;
                break;
            case 1:
                Alert("Successfull ScreenNameChange to " + screenname);
                profile.Screenname = screenname;
                this.screenname.text = screenname;
                break;
            case 2:
                Alert("Failed ScreenNameChange: Inexistent token");
                this.screenname.text = profile.Screenname;
                break;
            case 3:
                Alert("Failed ScreenNameChange: Invalid screenname");
                this.screenname.text = profile.Screenname;
                break;
        }
    }
    public void ScreenNameDialogResult(string screenname)
    {
        switch (screenname)
        {
            default:
                Handler.Instance.SendScreennameChangeRequest(screenname);
                break;
            case "no":
                this.screenname.text = profile.Screenname;
                break;
        }
    }
    #endregion
}
