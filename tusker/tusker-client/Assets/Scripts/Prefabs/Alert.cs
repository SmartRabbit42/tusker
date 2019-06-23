using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    private Text alertText;
    private Button ok;

    public void Init(string message)
    {
        enableInputs(false);

        alertText = transform.Find("txt_alert").GetComponent<Text>();
        ok = transform.Find("btn_ok").GetComponent<Button>();

        alertText.text = message;

        ok.onClick.AddListener(() => Quit());
    }
    
    private void Quit()
    {
        enableInputs(true);
        Destroy(gameObject);
    }

    private void enableInputs(bool value)
    {
        transform.parent.GetComponent<CanvasGroup>().interactable = value;
    }
}