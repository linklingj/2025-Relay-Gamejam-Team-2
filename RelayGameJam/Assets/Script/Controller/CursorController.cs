using System.Collections.Generic;
using UnityEngine;

public class CursorController : Singleton<CursorController>
{
    [Header("Prefabs")]
    public GameObject arrowPrefab; //화살표 꼬리
    public GameObject dotPrefab; //중간 연결 점 그래픽

    [Header("Arc Settings")]
    [SerializeField] private float spacing = 70f; //점과 점 사이 거리
    private float arrowAngleAdjustment = 0f;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 30; //처음에 dorPrefab몇 개 소환할건지

    private List<GameObject> pool = new();
    private GameObject arrowInstance;
    private Vector3 arrowDirectionPoint;
    [SerializeField] private RectTransform parent;
    private RectTransform startRect;
    private bool isTargeting = false;

    private void Start()
    {
        arrowInstance = Instantiate(arrowPrefab, parent);
        arrowInstance.gameObject.SetActive(false);
        InitializePool(poolSize);
    }

    public void Targeting(RectTransform cardRect) 
    {
        startRect = cardRect;
        arrowInstance.SetActive(true);
        isTargeting = true;
    }

    public void UnTargeting() //화살표가 꺼짐
    {
        isTargeting = false;
        arrowInstance.SetActive(false);
        foreach (var p in pool)
        {
            p.SetActive(false);
        }
    }

    private void Update()
    {
        if (isTargeting && startRect != null) //타켓팅일 때 마우스의 위치로 화살표가 이동
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 startPos = startRect.position;
            Vector2 midPos = CalculateMidPoint(startPos, mousePos);

            UpdateArc(startPos, midPos, mousePos);
            PositionAndRotateArrow(mousePos);
        }
        
    }

    private void InitializePool(int count) //Pooling
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, parent);
            dot.SetActive(false);
            pool.Add(dot);
        }
    }

    private Vector3 CalculateMidPoint(Vector3 start, Vector3 end) //중간 볼록하게 올라가는 점 계산
    {
        Vector3 mid = (start + end) / 2;
        float arcHeight = Vector3.Distance(start, end) / 2;
        mid.y += arcHeight;
        return mid;
    }

    private void UpdateArc(Vector3 start, Vector3 mid, Vector3 end)
    {
        int numDots = Mathf.CeilToInt(Vector2.Distance(start, mid) / spacing);

        for (int i = 0; i < numDots; i++)
        {
            // 풀 부족 시 자동 확장
            if (i >= pool.Count)
            {
                GameObject extraDot = Instantiate(dotPrefab, transform);
                extraDot.SetActive(false);
                pool.Add(extraDot);
            }

            float t = Mathf.Clamp01(i / (float)numDots);
            Vector2 pos = QuadraticBezierPoint(start, mid, end, t);

            if (i != numDots )
            {
                RectTransform dotRect = pool[i].GetComponent<RectTransform>();
                dotRect.position = pos;
                pool[i].SetActive(true);
            }

            // 방향 벡터용 기준점 저장
            if (i == numDots -  1)
            {
                arrowDirectionPoint = pos;
            }
        }

        // 나머지 dot 끄기
        for (int i = numDots; i < pool.Count; i++)
        {
            pool[i].SetActive(false);
        }
    }

    private Vector3 QuadraticBezierPoint(Vector3 start, Vector3 control, Vector3 end, float t) //dotPrefab 위치 계산
    {
        float u = 1 - t;
        return u * u * start + 2 * u * t * control + t * t * end;
    }

    private void PositionAndRotateArrow(Vector3 arrowPos) //화살표 rotate 및 위치 이동
    {
        RectTransform arrowRect = arrowInstance.GetComponent<RectTransform>();
        arrowRect.position = arrowPos;
        arrowInstance.transform.position = arrowPos;

        Vector3 dir = arrowPos - arrowDirectionPoint;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + arrowAngleAdjustment;

        arrowInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
