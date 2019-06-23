using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private Text loadingText;

    public void Init(string message)
    {
        name = "loading";

        EnableInputs(false);

        loadingText = transform.Find("txt_loading").GetComponent<Text>();

        loadingText.text = message;
    }

    public void Quit()
    {
        EnableInputs(true);
        Destroy(gameObject);
    }

    private void EnableInputs(bool value)
    {
        transform.parent.GetComponent<CanvasGroup>().interactable = value;
    }
}