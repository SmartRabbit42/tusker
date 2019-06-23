using UnityEngine;
using UnityEngine.UI;

public class Entry_Main : MonoBehaviour
{
    public static Entry_Main Instance { set; get; }

    private Text welcomeMessage;
    [SerializeField] private GameObject alertPrefab;
    [SerializeField] private GameObject loadingPrefab;

    private void Awake() { Instance = this; }

    private void Start()
    {
        welcomeMessage = GameObject.Find("txt_message").GetComponent<Text>();
    }

    #region Helper
    private void ChangeWelcomeMessage(string msg)
    {
        welcomeMessage.text = msg;
    }
    private void ChangeWelcomeMessageColor(byte flagEnum)
    {
        switch (flagEnum)
        {
            case 0:
                welcomeMessage.color = new Color32(178, 0, 0, 255);
                break;
            case 1:
                welcomeMessage.color = new Color32(255, 255, 255, 255);
                break;
            case 2:
                welcomeMessage.color = new Color32(80, 95, 175, 255);
                break;
            case 3:
                welcomeMessage.color = new Color32(170, 80, 40, 255);
                break;
        }
    }
    private void Alert(string msg)
    {
        var instance = Instantiate(alertPrefab, GameObject.Find("Canvas").transform);
        instance.GetComponent<Alert>().Init(msg);
    }
    private void ClearFields()
    {
        Entry_Autentication.instance.ClearFields();
    }
    private void LoadLogin()
    {
        Entry_Autentication.instance.LoadLogin();
    }
    #endregion

    #region Response
    public void OnConnect(byte flagEnum, string message)
    {
        ChangeWelcomeMessage(message);
        ChangeWelcomeMessageColor(flagEnum);
    }
    public void OnSignUpRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert(string.Format("Unespected error"));
                break;
            case 1:
                Alert(string.Format("Successfull signUp"));
                ClearFields();
                LoadLogin();
                break;
            case 2:
                Alert(string.Format("Invalid email"));
                break;
            case 3:
                Alert(string.Format("Invalid username"));
                break;
            case 4:
                Alert(string.Format("Email already in use"));
                break;
            case 5:
                Alert(string.Format("Username already in use"));
                break;
            case 6:
                Alert(string.Format("Password too short"));
                break;
        }
    }
    public void OnLoginRequest(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert(string.Format("Unespected error"));
                break;
            case 1:
                Alert(string.Format("Successfull login"));
                break;
            case 2:
                Alert(string.Format("Inexistent username or email"));
                break;
            case 3:
                Alert(string.Format("Incorrect password"));
                break;
            case 4:
                Alert(string.Format("Invalid username or email"));
                break;
            case 5:
                Alert(string.Format("Password too short"));
                break;
        }
    }
    #endregion
}