using UnityEngine;
using UnityEngine.UI;

public class Entry_Autentication : MonoBehaviour {

    public static Entry_Autentication instance { private set; get; }

    private GameObject login;
    private GameObject signUp;

    private Button submit1;
    private Button submit2;

    private Button createAccount;
    private Button haveAccount;

    private InputField usernameEmail;
    private InputField username;
    private InputField password1;
    private InputField password2;
    private InputField email;

    void Start()
    {
        instance = this;

        login = GameObject.Find("Login");
        signUp = GameObject.Find("SignUp");

        submit1 = GameObject.Find("btn_submit1").GetComponent<Button>();
        submit2 = GameObject.Find("btn_submit2").GetComponent<Button>();

        createAccount = GameObject.Find("btn_createAccount").GetComponent<Button>();
        haveAccount = GameObject.Find("btn_haveAccount").GetComponent<Button>();

        usernameEmail = GameObject.Find("ipf_usernameEmail").GetComponent<InputField>();
        username = GameObject.Find("ipf_username").GetComponent<InputField>();
        password1 = GameObject.Find("ipf_password1").GetComponent<InputField>();
        password2 = GameObject.Find("ipf_password2").GetComponent<InputField>();
        email = GameObject.Find("ipf_email").GetComponent<InputField>();

        submit1.onClick.AddListener(() => Submit1Click());
        submit2.onClick.AddListener(() => Submit2Click());

        createAccount.onClick.AddListener(() => LoadSignUp());
        haveAccount.onClick.AddListener(() => LoadLogin());

        signUp.SetActive(false);
    }

    private void Submit1Click()
    {
        string ue = usernameEmail.text;
        string p = password1.text;
        Handler.Instance.SendLoginRequest(ue, p);
    }
    private void Submit2Click()
    {
        string u = username.text;
        string p = password2.text;
        string e = email.text;
        Handler.Instance.SendSignUpRequest(u, p, e);
    }

    public void LoadSignUp()
    {
        login.SetActive(false);
        signUp.SetActive(true);
    }
    public void LoadLogin()
    {
        signUp.SetActive(false);
        login.SetActive(true);
    }

    public void ClearFields()
    {
        usernameEmail.text = null;
        username.text = null;
        password1.text = null;
        password2.text = null;
        email.text = null;
    }
}
