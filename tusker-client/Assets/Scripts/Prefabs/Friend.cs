using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Friend : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject rightClickMenuPrefab;

    public Account profile;

    private Text username;
    private Text screenname;
    private Image online;

	public void Init(Account p)
    {
        name = p.Username;

        username = transform.Find("txt_username").GetComponent<Text>();
        screenname = transform.Find("txt_screenname").GetComponent<Text>();
        online = transform.Find("img_online").GetComponent<Image>();

        UpdateValues(p);
    }

    public void UpdateValues(Account p)
    {
        username.text = p.Username;
        screenname.text = p.Screenname;
        if (p.ActiveConnection == 0)
            online.color = new Color(0.85f, 0.1f, 0.1f, 1);
        else
            online.color = new Color(0.1f, 0.64f, 0.25f, 1);

        profile = p;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var instance = Instantiate(rightClickMenuPrefab, GameObject.Find("Canvas").transform);
            instance.GetComponent<RightClickMenu>().Init(RightClickMenuOp.friend, gameObject);
        }
    }
}