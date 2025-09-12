using UnityEngine;

public class LightController : MonoBehaviour, ITouchable
{
    public void OnTouch()
    {
        Debug.Log("조명 터치됨");
    }
}
