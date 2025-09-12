using UnityEngine;

public class AirConditioner : MonoBehaviour, ITouchable
{
    public void OnTouch()
    {
        Debug.Log("에어컨 터치됨");
    }
}
