using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    //TODO: 슬라임 관련 기능 구현
    [SerializeField] private string slimeId = "1";

    private void Start()
    {
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
    }

}
