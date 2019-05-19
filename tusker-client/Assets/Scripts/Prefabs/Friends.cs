using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friends : MonoBehaviour
{
    public static Friends Instance { private set; get; }

    [SerializeField] private GameObject alertPrefab;
    [SerializeField] private GameObject friendPrefab;
    [SerializeField] private GameObject friendRequestManager;

    private List<Account> friends = new List<Account>();
    private List<Account> friendRequests = new List<Account>();

    private Button btn_friends;
    private GameObject pan_friends;

    private Transform myFriends;

    private Text myUsername;
    private Text myScreenname;
    private Button quit;

    private InputField findFriend;

    private Button btn_addFriend;
    private Text txt_btn_addFriend;

    void Awake() { Instance = this; }

	void Start () {
        friends = new List<Account>();
        friendRequests = new List<Account>();

        btn_friends = transform.GetChild(0).GetComponent<Button>();
        pan_friends = transform.GetChild(1).gameObject;

        var self = pan_friends.transform.GetChild(0);
        myUsername = self.Find("txt_username").GetComponent<Text>();
        myScreenname = self.Find("txt_screenname").GetComponent<Text>();
        quit = self.Find("btn_quit").GetComponent<Button>();
        btn_addFriend = self.Find("btn_addFriend").GetComponent<Button>();
        txt_btn_addFriend = btn_addFriend.transform.GetChild(0).GetComponent<Text>();

        findFriend = pan_friends.transform.GetChild(2).GetChild(0).GetComponent<InputField>();
        findFriend.onValueChanged.AddListener(delegate { FindFriend(); });

        myFriends = pan_friends.transform.Find("myFriends");

        btn_friends.onClick.AddListener(delegate { OpenFriendsList(true); });
        quit.onClick.AddListener(delegate { OpenFriendsList(false); });
        btn_addFriend.onClick.AddListener(delegate { OpenFriendRequestManager(); });

        OpenFriendsList(false);

        Handler.Instance.SendFriendListRequest();
        Handler.Instance.SendFriendRequestListRequest();
	}
	
    private void FindFriend()
    {
        string friend = findFriend.text;

        foreach(Transform f in myFriends.transform)
        {
            if (!f.name.Contains(friend))
                f.gameObject.SetActive(false);
            else
                f.gameObject.SetActive(true);
        }
    }

    private void OpenFriendRequestManager()
    {
        var instance = Instantiate(friendRequestManager, GameObject.Find("Canvas").transform);
        instance.name = "friendRequestManager";
        instance.GetComponent<FriendRequestManager>().Init(friendRequests);
    }

    #region Helper
    private void Alert(string message)
    {
        Transform p;
        if (FriendRequestManager.Instance != null)
            p = GameObject.Find("friendRequestManager").transform;
        else
            p = GameObject.Find("Canvas").transform;
        var alert = Instantiate(alertPrefab, p);
        alert.GetComponent<Alert>().Init(message);
    }
    private void OpenFriendsList(bool value)
    {
        UpdateSelfValues();
        btn_friends.gameObject.SetActive(!value);
        pan_friends.gameObject.SetActive(value);
    }
    private void UpdateSelfValues()
    {
        myUsername.text = Client.Instance.myAccount.Username;
        myScreenname.text = Client.Instance.myAccount.Screenname;
    }
    private void AddFriend(Account friend)
    {
        var newFriend = Instantiate(friendPrefab, myFriends);
        newFriend.GetComponent<Friend>().Init(friend);
        friends.Add(friend);
    }
    private void RemoveFriend(string friend)
    {
        Destroy(myFriends.Find(friend).gameObject);
        foreach(var f in friends)
            if(f.Username == friend)
            {
                friends.Remove(f);
                return;
            }  
    }
    private void RemoveFriendRequest(Account friendRequest)
    {
        friendRequests.Remove(friendRequest);
        txt_btn_addFriend.text = friendRequests.Count.ToString();
        if (FriendRequestManager.Instance != null)
            FriendRequestManager.Instance.RemoveFriendRequest(friendRequest);
    }
    #endregion

    #region Response
    public void OnFriendListRequest(List<Account> fs)
    {
        foreach (Transform f in myFriends)
            Destroy(f);
        foreach (var f in fs)
            AddFriend(f);
    }
    public void OnFriendRequestListRequest(List<Account> fr)
    {
        friendRequests = fr;
        txt_btn_addFriend.text = friendRequests.Count.ToString();
    }
    public void OnFriendRequestSender(byte flagEnum)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert("Failed friendRequest: Unespected error");
                break;
            case 1:
                Alert("Successfull friendRequest");
                break;
            case 2:
                Alert("Failed friendRequest: Invalid username");
                break;
            case 3:
                Alert("Failed friendRequest: Inexistent username");
                break;
            case 4:
                Alert("Failed friendRequest: Inexistent token");
                break;
            case 5:
                Alert("Failed friendRequest: Tried to befriend yourself");
                break;
            case 6:
                Alert("Failed friendRequest: Tried to befriend a friend");
                break;
            case 7:
                Alert("Failed friendRequest: Already requested");
                break;
        }
    }
    public void OnFriendRequestReceiver(Account f)
    {
        friendRequests.Add(f);
        txt_btn_addFriend.text = friendRequests.Count.ToString();
        if (FriendRequestManager.Instance != null)
            FriendRequestManager.Instance.OnFriendRequestReceiver(f);
    }
    public void OnFriendRequestConfirmationSender(byte flagEnum, Account f)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert("Failed friendRequestConfirmation: Unespected error");
                break;
            case 1:
                RemoveFriendRequest(f);
                break;
            case 2:
                RemoveFriendRequest(f);
                AddFriend(f);
                break;
            case 3:
                Alert("Failed friendRequestConfirmation: Invalid username");
                RemoveFriendRequest(f);
                break;
            case 4:
                Alert("Failed friendRequestConfirmation: Inexistent username");
                RemoveFriendRequest(f);
                break;
            case 5:
                Alert("Failed friendRequestConfirmation: Inexistent token");
                RemoveFriendRequest(f);
                break;
            case 6:
                Alert("Failed friendRequestConfirmation: Tried to befriend yourself");
                RemoveFriendRequest(f);
                break;
            case 7:
                Alert("Failed friendRequestConfirmation: Tried to befriend a friend");
                RemoveFriendRequest(f);
                break;
        }
    }
    public void OnFriendRequestConfirmationReceiver(Account f, bool confirmation)
    {
        if (confirmation)
        {
            Alert(string.Format("{0} accepted your friend request", f.Username));
            AddFriend(f);
            RemoveFriendRequest(f);
        }
        else
            Alert(string.Format("{0} declined your friend request", f.Username));
        RemoveFriendRequest(f);
    }
    public void OnFriendRemovalRequestSender(byte flagEnum, string friendUsername)
    {
        switch (flagEnum)
        {
            default:
            case 0:
                Alert("Failed friendRemovalRequest: Unespected error");
                break;
            case 1:
                Alert("Successfull friendRemovalRequest");
                RemoveFriend(friendUsername);
                break;
            case 2:
                Alert("Failed friendRemovalRequest: Inexistent friendship");
                RemoveFriend(friendUsername);
                break;
        }
    }
    public void OnFriendRemovalRequestReceiver(string friendUsername)
    {
        Alert(string.Format("{0} removed you from their friendList", friendUsername));
        RemoveFriend(friendUsername);
    }
    public void OnFriendUpdate(Account friend)
    {
        for (byte i = 0; i < friends.Count; i++)
            if (friends[i].Username == friend.Username)
                friends[i] = friend;
        myFriends.Find(friend.Username).GetComponent<Friend>().UpdateValues(friend);
        if (Profile.Instance != null && Profile.Instance.profile.Equals(friend))
            Profile.Instance.UpdateValues(friend);
    }
    public void OnFriendRequestUpdate(Account friendRequest)
    {
        for (byte i = 0; i < friendRequests.Count; i++)
            if (friendRequests[i].Username == friendRequest.Username)
                friendRequests[i] = friendRequest;
        if (FriendRequestManager.Instance != null)
            FriendRequestManager.Instance.UpdateFriendRequest(friendRequest);
    }
    #endregion
}
