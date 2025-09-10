using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private int exp = 0;
    private int level = 1;
    private int maxExp = 100;

    private bool isLevelUp = false;
    public void OnTouched()
    {
        if (isLevelUp)
        {
            return;
        }
        exp += 10;
        if (exp >= maxExp)
        {
            isLevelUp = true;
            LevelUp();
        }
        Debug.Log($"경험치 :{exp} / {maxExp}");
        Debug.Log($"레벨 :{level}");
    }

    private void LevelUp()
    {
        level += exp / maxExp;
        exp -= maxExp;
        maxExp += 50;
        StartCoroutine(CoScaleUpDown(1f));
    }

    private IEnumerator CoScaleUpDown(float duration)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 2f; // 현재 크기의 두 배
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(endScale, startScale, t);
            yield return null;
        }
        transform.localScale = startScale; // 마지막 보정
        isLevelUp = false;
    }
}
