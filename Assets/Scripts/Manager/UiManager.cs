using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject windowPanel;
    void Start()
    {
        exitButton.onClick.AddListener(() => OnExit());
    }

    private void OnExit()
    {
        windowPanel.SetActive(false);
        // 자식 오브젝트 모두 비활성화
        for (int i = 0; i < windowPanel.transform.childCount; i++)
        {
            if (exitButton.gameObject == windowPanel.transform.GetChild(i).gameObject)
            {
                continue;
            }
            windowPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
