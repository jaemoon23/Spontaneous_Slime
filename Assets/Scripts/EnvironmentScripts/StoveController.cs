using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoveController : MonoBehaviour, ITouchable
{
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject stoveWindow;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private TextMeshProUGUI stoveText;
    [SerializeField] private TextMeshProUGUI textField;
    private int minStoveStep = 0;
    private int maxStoveStep = 5;
    private string textFormat = "난로: {0}단계";
    private string text = "{0}";

    private void Start()
    {
        var item = DataTableManager.InteriorTable.Get(DataTableIds.InteriorIds[(int)EnvironmentType.Heater]);
        var textData = DataTableManager.StringTable.Get(item.UIText);
        textFormat = textData != null ? textData.Value : this.textFormat;

        plusButton.onClick.AddListener(OnClickPlusStove);
        minusButton.onClick.AddListener(OnClickMinusStove);
        textField.text = $"{environmentManager.StoveStep}";
        stoveText.text = textFormat.Replace(text, environmentManager.StoveStep.ToString());
    }
    private void OnEnable()
    {
        environmentManager.StoveStep = 1;
    }
    public void OnTouch()
    {
        Debug.Log("난로 터치됨");
        windowPanel.SetActive(true);
        environmentManager.ActivatePanel(stoveWindow);
    }
    public void OnClickPlusStove()
    {
        if (environmentManager.StoveStep < maxStoveStep)
        {
            environmentManager.StoveStep += 1;
            textField.text = $"{environmentManager.StoveStep}";
            stoveText.text = textFormat.Replace(text, environmentManager.StoveStep.ToString());
        }
    }

    public void OnClickMinusStove()
    {
        if (environmentManager.StoveStep > minStoveStep)
        {
            environmentManager.StoveStep -= 1;
            textField.text = $"{environmentManager.StoveStep}";
            stoveText.text = textFormat.Replace(text, environmentManager.StoveStep.ToString());
        }
    }
}
