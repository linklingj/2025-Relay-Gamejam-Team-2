using System.Collections.Generic;
using UnityEngine;

public class ArcController : Singleton<ArcController>
{
    [Header("Prefabs")]
    public GameObject arrowPrefab; //화살표 꼬리
    public GameObject dotPrefab; //중간 연결 점 그래픽

    [Header("Arc Settings")]
    public float spacing = 10f;
    public float arrowAngleAdjustment = 0f;
    public int dotsToSkip = 1;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 50; //처음에 dorPrefab몇 개 소환할건지

    private List<GameObject> pool = new List<GameObject>();
    private GameObject arrowInstance;
    private Vector3 arrowDirectionPoint;
    private Camera camera;
    private Transform startTrans;
    private bool isTargeting = false;

    private void Start()
    {
        camera = Camera.main;
        arrowInstance = Instantiate(arrowPrefab, transform);
        InitializePool(poolSize);
    }

    public void Targeting(Transform owner) //Owner 기준으로 화살표가 활성화됨
    {
        startTrans = owner;
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
        if (isTargeting) //타켓팅일 때 마우스의 위치로 화살표가 이동
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(camera.transform.position.z); // 스크린 → 월드 변환용 z
            mousePos = camera.ScreenToWorldPoint(mousePos);

            Vector3 startPos = startTrans.position;
            Vector3 midPos = CalculateMidPoint(startPos, mousePos);

            UpdateArc(startPos, midPos, mousePos);
            PositionAndRotateArrow(mousePos);
        }
        
    }

    private void InitializePool(int count) //Pooling
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
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
        int numDots = Mathf.CeilToInt(Vector3.Distance(start, mid) / spacing);

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
            Vector3 pos = QuadraticBezierPoint(start, mid, end, t);

            if (i != numDots - dotsToSkip)
            {
                pool[i].transform.position = pos;
                pool[i].SetActive(true);
            }

            // 방향 벡터용 기준점 저장
            if (i == numDots - (dotsToSkip + 1))
            {
                arrowDirectionPoint = pos;
            }
        }

        // 나머지 dot 끄기
        for (int i = numDots - dotsToSkip; i < pool.Count; i++)
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
        arrowInstance.transform.position = arrowPos;

        Vector3 dir = arrowPos - arrowDirectionPoint;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + arrowAngleAdjustment;

        arrowInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
