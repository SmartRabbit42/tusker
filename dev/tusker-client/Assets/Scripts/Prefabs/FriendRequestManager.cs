using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendRequestManager : MonoBehaviour
{
    public static FriendRequestManager Instance { private set; get; }

    [SerializeField] private GameObject friendRequestPrefab;

    private List<Account> friendRequests;

    private Button btn_quit;

    private InputField ipf_requestSender;
    private Button btn_requestSender;

    private Transform friendRequestsList;

	public void Init(List<Account> fr)
    {
        transform.parent.GetComponent<CanvasGroup>().interactable = false;

        Instance = this;

        friendRequests = fr;

        btn_quit = transform.Find("pan_header").Find("btn_quit").GetComponent<Button>();
        btn_quit.onClick.AddListener(() => QuitClick());

        var requestSender = transform.Find("requestSender").GetChild(1);
        ipf_requestSender = requestSender.Find("ipf_requestSender").GetComponent<InputField>(); 
        btn_requestSender = requestSender.Find("btn_requestSender").GetComponent<Button>();
        btn_requestSender.onClick.AddListener(delegate { Handler.Instance.SendFriendRequest(ipf_requestSender.text); });

        var requestReceiver = transform.Find("requestReceiver");
        friendRequestsList = requestReceiver.Find("friendRequestList");

        if(friendRequests != null)
            foreach(Account f in friendRequests)
            {
                var instance = Instantiate(friendRequestPrefab, friendRequestsList);
                instance.GetComponent<FriendRequest>().Init(f);
            }
    }

    private void QuitClick()
    {
        transform.parent.GetComponent<CanvasGroup>().interactable = true;
        Destroy(gameObject);
    }

    public void OnFriendRequestReceiver(Account f)
    {
        friendRequests.Add(f);
        var instance = Instantiate(friendRequestPrefab, friendRequestsList);
        instance.GetComponent<FriendRequest>().Init(f);
    }

    public void RemoveFriendRequest(Account f)
    {
        friendRequests.Remove(f);
        var friendRequest = friendRequestsList.Find(f.Username);
        if (friendRequest != null)
            Destroy(friendRequest.gameObject);
    }

    public void UpdateFriendRequest(Account f)
    {
        for (byte i = 0; i < friendRequests.Count; i++)
            if (friendRequests[i].Username == f.Username)
                friendRequests[i] = f;
        friendRequestsList.Find(f.Username).GetComponent<FriendRequest>().UpdateValues(f);
    }
}
