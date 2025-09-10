using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public LayerMask touchLayers;
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, touchLayers))
            {
                hit.collider.SendMessage(Defines.OnTouch, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    
}
