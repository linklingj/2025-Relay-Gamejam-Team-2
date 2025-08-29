using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Transform canvas;

    public Transform GetCanvasTrans() => canvas; //공용 캔버스 전달
}
