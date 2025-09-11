using UnityEngine;

public class SlimeRemoval : MonoBehaviour
{
    SlimeGrowth slimeGrowth;
    private void Start()
    {
        slimeGrowth = GetComponent<SlimeGrowth>();
    }
    public void RemoveSlime()
    {
        Destroy(gameObject);
    }
}
