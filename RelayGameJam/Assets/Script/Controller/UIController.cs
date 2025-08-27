using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Transform canvasTrans;

    public Transform GetCanvasTrans() => canvasTrans; //공용 캔버스 전달
}
