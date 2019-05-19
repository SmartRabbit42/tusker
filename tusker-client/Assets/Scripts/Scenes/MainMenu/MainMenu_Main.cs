using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Main : MonoBehaviour
{
    [SerializeField] private GameObject profilePrefab;

    private int currentPanel = 1;

    private GameObject[] panels = new GameObject[4];

    private Button play;
    private Button home;
    private Button store;
    private Button info;
    private Button profile;

    void Start()
    {
        play = transform.GetChild(0).GetChild(0).Find("btn_play").GetComponent<Button>();
        home = transform.GetChild(0).GetChild(0).Find("btn_home").GetComponent<Button>();
        store = transform.GetChild(0).GetChild(0).Find("btn_store").GetComponent<Button>();
        info = transform.GetChild(0).GetChild(0).Find("btn_info").GetComponent<Button>();
        profile = transform.GetChild(0).GetChild(0).Find("btn_profile").GetComponent<Button>();

        panels[0] = transform.GetChild(0).GetChild(1).Find("play").gameObject;
        panels[1] = transform.GetChild(0).GetChild(1).Find("home").gameObject;
        panels[2] = transform.GetChild(0).GetChild(1).Find("store").gameObject;
        panels[3] = transform.GetChild(0).GetChild(1).Find("info").gameObject;

        play.onClick.AddListener(() => PlayClick());
        home.onClick.AddListener(() => HomeClick());
        store.onClick.AddListener(() => StoreClick());
        info.onClick.AddListener(() => InfoClick());
        profile.onClick.AddListener(() => ProfileClick());

        for (int i = 0; i < 4; i++)
            if (i != 1)
                panels[i].SetActive(false);
    }

    private void PlayClick()
    {
        if (currentPanel != 0)
        {
            panels[currentPanel].SetActive(false);
            currentPanel = 0;
            panels[currentPanel].SetActive(true);
        }
    }
    private void HomeClick()
    {
        if (currentPanel != 1)
        {
            panels[currentPanel].SetActive(false);
            currentPanel = 1;
            panels[currentPanel].SetActive(true);
        }
    }
    private void StoreClick()
    {
        if (currentPanel != 2)
        {
            panels[currentPanel].SetActive(false);
            currentPanel = 2;
            panels[currentPanel].SetActive(true);
        }
    }
    private void InfoClick()
    {
        if (currentPanel != 3)
        {
            panels[currentPanel].SetActive(false);
            currentPanel = 3;
            panels[currentPanel].SetActive(true);
        }
    }
    private void ProfileClick()
    {
        var instance = Instantiate(profilePrefab, GameObject.Find("Canvas").transform);
        instance.GetComponent<Profile>().Init(Client.Instance.myAccount);
    }
}
