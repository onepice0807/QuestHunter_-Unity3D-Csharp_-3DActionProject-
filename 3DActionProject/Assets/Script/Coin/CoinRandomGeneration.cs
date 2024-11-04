using UnityEngine;

public class CoinRandomGeneration : MonoBehaviour
{
    public GameObject coinPrefab; // 코인 프리팹
    public int coinCount = 30; // 생성할 코인의 수
    public float spawnRadius = 50f; // 코인이 생성될 반경
    public float spawnInterval = 2f; // 코인 간의 최소 거리
    public LayerMask navMeshLayer; // NavMesh가 있는 레이어

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoinGeneration();
    }

    public void CoinGeneration()
    {

    }


    Vector3 GetRandomPosition()  // 코인들이 랜덤한 위치에 있도록 하는 method
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position; // 이 오브젝트를 중심으로 랜덤 위치 계산
        randomDirection.y = 0; // y값을 0으로 설정하여 NavMesh 상의 위치로 제한
        return randomDirection;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
