using UnityEngine;

//UI�� ���鶧 SafeArea��ŭ ��������Ǵ� ��ũ��Ʈ��.
public class SafeArea : MonoBehaviour
{
    private RectTransform safeAreaRect;
    private Canvas canvas;
    private Rect lastSafeArea;

    void Start()
    {
        safeAreaRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        OnRectTransformDimensionsChange();
    }

    private void OnRectTransformDimensionsChange() //ȭ���� ���Ҷ� �ڵ����� ȣ��Ǵ� �̺�Ʈ �Լ���.
    {

        if (GetSafeArea() != lastSafeArea && canvas != null)
        {
            lastSafeArea = GetSafeArea();
            UpdateSizeToSafeArea();
        }
    }

    private void UpdateSizeToSafeArea()
    {

        var safeArea = GetSafeArea();
        var inverseSize = new Vector2(1f, 1f) / canvas.pixelRect.size; //0.0���� 1.0���� ����� ��������.
        var newAnchorMin = Vector2.Scale(safeArea.position, inverseSize); //ũ�⸦ �ǹ��ϴ°� �ƴ϶� �� x y z�� inverseSize�� ���ϴ� ���̴�.
        var newAnchorMax = Vector2.Scale(safeArea.position + safeArea.size, inverseSize);

        //��Ŀ ����ȭ -> ��Ŀ�� ���� ��� ���̴�.
        safeAreaRect.anchorMin = newAnchorMin;
        safeAreaRect.anchorMax = newAnchorMax;

        safeAreaRect.offsetMin = Vector2.zero;
        safeAreaRect.offsetMax = Vector2.zero;
    }

    private Rect GetSafeArea()
    {
        return Screen.safeArea;
    }
}
