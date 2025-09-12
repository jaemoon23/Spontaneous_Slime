using UnityEngine;

public class DehumidifierController : MonoBehaviour, ITouchable
{
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject dehumidifierWindow;

    public void OnTouch()
    {
        // TODO: Debug log 제거
        Debug.Log("제습기 터치됨");
        windowPanel.SetActive(true);
        dehumidifierWindow.SetActive(true);
    }
}
