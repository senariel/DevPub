using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct WaveSpawnData
    {
        public float delay;
        public List<GameObject> unitPrefabList;
    }

    public GameObject floor;

    [Header("Setting")]
    [SerializeField] protected List<WaveSpawnData> waveList = new();
    [SerializeField] protected Vector3 SpawnDirection;


    protected TileManager _tileManager;
    private List<GameObject> _enemyList = new();

    // Start is called before the first frame update
    void Awake()
    {
        _tileManager = floor?.GetComponent<TileManager>();

        Debug.Log("[GameManager : Awake]");
    }

    void Start()
    {
        if (IsGameStartable() == false)
            return;

        StartStage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 스테이지 시작
    public void StartStage()
    {
        Debug.Log("[StartGame]");

        // 웨이브 시작
        StartWave(0);
    }

    // 게임 시작 가능 여부
    public bool IsGameStartable()
    {
        // 타일관리자 유효성 확인
        if (_tileManager == null || _tileManager.IsGameStartable() == false)
            return false;

        return true;
    }


    // 웨이브 생성
    bool StartWave(int waveIndex)
    {
        Debug.Log("[StartWave] " + waveIndex);
        if (waveList.Count > waveIndex)
        {
            StartCoroutine(SpawnWave(waveList[waveIndex]));
        }

        Debug.Log("[StartWave] end");

        return true;
    }

    // 웨이브 생성하기. 코루틴 반복용
    IEnumerator SpawnWave(WaveSpawnData waveData)
    {
        Transform startTransform = _tileManager.GetStartTransform();

        foreach (GameObject unitPrefab in waveData.unitPrefabList)
        {
            Debug.Log("\tInstantiate [" + unitPrefab + "] " + unitPrefab.transform.forward + " / " + startTransform.forward + " / " + Vector3.forward);

            if (unitPrefab != null)
            {
                // Start Rotation 은 어떻게 가져올까
                GameObject unit = GameObject.Instantiate<GameObject>(
                    unitPrefab,
                    startTransform.position,
                    startTransform.rotation);

                if (unit != null)
                {
                    _enemyList.Add(unit);
                }
            }

            yield return new WaitForSeconds(waveData.delay);
        }

        Debug.Log("[GameManager : SpawnWave] End");
        yield break;
    }

    
}

