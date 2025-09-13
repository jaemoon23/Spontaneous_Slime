using UnityEngine;

public class StoveController : MonoBehaviour, ITouchable
{
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject stoveWindow;
    public void OnTouch()
    {
        Debug.Log("난로 터치됨");
        windowPanel.SetActive(true);
        stoveWindow.SetActive(true);

    }
}
