using UnityEngine;

public class FlowerPotController : MonoBehaviour, ITouchable
{
    public void OnTouch()
    {
        Debug.Log("화분 터치됨");
    }
}
