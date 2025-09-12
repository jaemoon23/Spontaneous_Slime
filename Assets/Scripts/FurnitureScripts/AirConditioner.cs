using UnityEngine;

public class AirConditioner : MonoBehaviour, ITouchable
{
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject airConditionerWindow;

    public void OnTouch()
    {
        // TODO: Debug log 제거
        Debug.Log("에어컨 터치됨");
        windowPanel.SetActive(true);
        airConditionerWindow.SetActive(true);
    }
}
