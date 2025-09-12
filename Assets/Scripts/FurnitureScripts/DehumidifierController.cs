using UnityEngine;

public class DehumidifierController : MonoBehaviour, ITouchable
{
    public void OnTouch()
    {
        Debug.Log("제습기 터치됨");
    }
}
