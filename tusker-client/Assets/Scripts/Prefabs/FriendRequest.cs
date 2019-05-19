using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendRequest : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject rightClickMenuPrefab;

    public Account profile;

    private Text txt_username;
    private Text txt_screenname;

    private Button btn_accept;
    private Button btn_decline;

	public void Init (Account p)
    {
        txt_username = transform.Find("txt_username").GetComponent<Text>();
        txt_screenname = transform.Find("txt_screenname").GetComponent<Text>();

        UpdateValues(p);

        btn_accept = transform.Find("btn_accept").GetComponent<Button>();
        btn_decline = transform.Find("btn_decline").GetComponent<Button>();
        btn_accept.onClick.AddListener(delegate { Handler.Instance.SendFriendRequestConfirmation(true, profile.Username); });
        btn_decline.onClick.AddListener(delegate { Handler.Instance.SendFriendRequestConfirmation(false, profile.Username); });
    }

    public void UpdateValues(Account p)
    {
        name = p.Username;

        txt_username.text = p.Username;
        txt_screenname.text = p.Screenname;

        profile = p;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameObject.Find("friendRequestManager").GetComponent<CanvasGroup>().interactable)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var instance = Instantiate(rightClickMenuPrefab, GameObject.Find("Canvas").transform);
            instance.GetComponent<RightClickMenu>().Init(RightClickMenuOp.friendRequest, gameObject);
        }
    }
}
