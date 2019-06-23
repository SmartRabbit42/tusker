using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour {

    private readonly float[] spawnPoints = { 0.57f, 0.43f, 0.71f, 0.29f, 0.85f, 0.15f };

    private Account playerParty;

    private Text userrname;
    private Text screenname;
    private Text level;

    private Button ready;
    private Button notReady;

    private bool localPlayer;

    public void Init(Account p, int spawnPoint)
    {
        userrname = transform.Find("txt_username").GetComponent<Text>();
        screenname = transform.Find("txt_screenname").GetComponent<Text>();
        level = transform.Find("txt_level").GetComponent<Text>();

        var r = transform.Find("ready");
        ready = r.Find("btn_ready").GetComponent<Button>();
        notReady = r.Find("btn_notReady").GetComponent<Button>();

        UpdateMember(p, spawnPoint);
    }

    public void UpdateMember(Account p, int spawnPoint)
    {
        UpdateValues(p);

        SetReady(p.Ready);

        var rect = GetComponent<RectTransform>();
        rect.anchorMax = rect.anchorMin = new Vector2(spawnPoints[spawnPoint], 0.5f);
        if (spawnPoint == 0)
            localPlayer = true;

        if (localPlayer)
        {
            ready.onClick.AddListener(delegate { Handler.Instance.SendReadyRequest(); });
            notReady.onClick.AddListener(delegate { Handler.Instance.SendReadyRequest(); });
        }
    }
    
    public void SetReady(bool value)
    {
        ready.gameObject.SetActive(!value);
        notReady.gameObject.SetActive(value);
    }

    public void UpdateValues(Account p)
    {
        name = p.Username;

        userrname.text = p.Username;
        screenname.text = p.Screenname;
        level.text = p.Level.ToString();

        GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.2f + (p.Priority * 0.1f));

        playerParty = p;
    }
}
