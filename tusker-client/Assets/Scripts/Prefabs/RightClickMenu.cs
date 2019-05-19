using UnityEngine;
using UnityEngine.UI;

public static class RightClickMenuOp
{
    public const byte friend = 1;
    public const byte friendRequest = 2;
}

public class RightClickMenu : MonoBehaviour
{
    [SerializeField] GameObject rightClickMenuItemPrefab;
    [SerializeField] GameObject profilePrefab;

    private const int ITEM_HEIGHT = 60;

	public void Init(byte rightClickMenuOp, GameObject caller)
    {
        var rect = GetComponent<RectTransform>();

        Vector2 localpoint;
        RectTransform rectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out localpoint);
        Vector2 normalizedPoint = Rect.PointToNormalized(rectTransform.rect, localpoint);

        rect.anchorMin = new Vector2(normalizedPoint.x, normalizedPoint.y);
        rect.anchorMax = new Vector2(normalizedPoint.x + 0.2f, normalizedPoint.y);

        switch (rightClickMenuOp)
        {
            case RightClickMenuOp.friend:
                rect.sizeDelta = new Vector2(0, 3 * ITEM_HEIGHT);

                var profile = Instantiate(rightClickMenuItemPrefab, transform);
                var invite = Instantiate(rightClickMenuItemPrefab, transform);
                var remove = Instantiate(rightClickMenuItemPrefab, transform);

                profile.GetComponentInChildren<Text>().text = "PROFILE";
                invite.GetComponentInChildren<Text>().text = "INVITE TO PARTY";
                remove.GetComponentInChildren<Text>().text = "REMOVE";

                profile.GetComponent<Button>().onClick.AddListener(delegate { OpenProfile(caller.GetComponent<Friend>().profile); });
                invite.GetComponent<Button>().onClick.AddListener(delegate { Handler.Instance.SendPartyRequest(caller.name); });
                remove.GetComponent<Button>().onClick.AddListener(delegate { Handler.Instance.SendFriendRemovalRequest(caller.name); });

                if (Client.Instance.myAccount.Status != 2)
                    invite.GetComponent<Button>().interactable = false;
                else if (caller.GetComponent<Friend>().profile.Status != 1)
                    invite.GetComponent<Button>().interactable = false;
                break;
            case RightClickMenuOp.friendRequest:
                rect.sizeDelta = new Vector2(0, 3 * ITEM_HEIGHT);

                var profile1 = Instantiate(rightClickMenuItemPrefab, transform);
                var accept = Instantiate(rightClickMenuItemPrefab, transform);
                var decline = Instantiate(rightClickMenuItemPrefab, transform);

                profile1.GetComponentInChildren<Text>().text = "PROFILE";
                accept.GetComponentInChildren<Text>().text = "ACCEPT";
                decline.GetComponentInChildren<Text>().text = "DECLINE";

                profile1.GetComponent<Button>().onClick.AddListener(delegate { OpenProfile(caller.GetComponent<FriendRequest>().profile); });
                accept.GetComponent<Button>().onClick.AddListener(delegate { Handler.Instance.SendFriendRequestConfirmation(true, caller.GetComponent<FriendRequest>().profile.Username); });
                decline.GetComponent<Button>().onClick.AddListener(delegate { Handler.Instance.SendFriendRequestConfirmation(false, caller.GetComponent<FriendRequest>().profile.Username); });

                break;
        }
    }

    private void OpenProfile(Account p)
    {
        var instance = Instantiate(profilePrefab, GameObject.Find("Canvas").transform);
        instance.GetComponent<Profile>().Init(p);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
                Destroy(gameObject);
        }
        else if (Input.GetMouseButtonDown(1))
            Destroy(gameObject);
    }
}
