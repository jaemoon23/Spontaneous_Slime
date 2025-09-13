using UnityEngine;

public class FlowerPotController : MonoBehaviour, ITouchable
{
    [SerializeField] private EnvironmentManager environmentManager;
    private void OnEnable()
    {
        environmentManager.IsFlower = true;
    }
    public void OnTouch()
    {
        Debug.Log("화분 터치됨");
    }
}
