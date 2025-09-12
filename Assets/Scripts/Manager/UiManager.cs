using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private GameObject windowPanel;
    void Start()
    {
        exitButton.onClick.AddListener(() => OnExit());
    }

    private void OnExit()
    {
        windowPanel.SetActive(false);
        for (int i = 0; i < windowPanel.transform.childCount; i++)
        {
            GameObject child = windowPanel.transform.GetChild(i).gameObject; // 자식 오브젝트 가져오기
            if (child == buttonObject)
            {
                continue; // buttonObject 오브젝트는 건너뜀
            }
            child.SetActive(false);
        }
    }

}
