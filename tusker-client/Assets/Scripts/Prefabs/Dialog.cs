using UnityEngine;
using UnityEngine.UI;

public static class DialogOp
{
    public const byte ScreenNameChange = 1;
    public const byte LeaveParty = 2;
}

public class Dialog : MonoBehaviour {

	public void Init(byte dialogOp, Object caller)
    {
        EnableInputs(false);

        switch (dialogOp)
        {
            case DialogOp.ScreenNameChange:
                string complement = (caller as GameObject).transform.GetChild(0).Find("ipf_screenname").GetComponent<InputField>().text;
                transform.Find("txt_dialog").GetComponent<Text>().text = string.Format("Change screenname to {0}?", complement);

                transform.Find("btn_yes").GetComponent<Button>().onClick.AddListener(delegate { Quit(); Profile.Instance.ScreenNameDialogResult(complement); });
                transform.Find("btn_no").GetComponent<Button>().onClick.AddListener(delegate { Quit(); Profile.Instance.ScreenNameDialogResult("no"); });
                break;
            case DialogOp.LeaveParty:
                transform.Find("txt_dialog").GetComponent<Text>().text = string.Format("Leave party?");

                transform.Find("btn_yes").GetComponent<Button>().onClick.AddListener(delegate { Quit(); PartyManager.Instance.LeavePartyDialogResult(); });
                transform.Find("btn_no").GetComponent<Button>().onClick.AddListener(delegate { Quit(); });
                break;
        }
    }

    private void EnableInputs(bool value)
    {
        transform.parent.GetComponent<CanvasGroup>().interactable = value;
    }

    private void Quit()
    {
        EnableInputs(true);
        Destroy(gameObject);
    }
}
