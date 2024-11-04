using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
    private int _hitCount = 0; // 박스가 몇 번 충돌되었는지 카운트 하는 값
    public GameObject _coinPrefab; // 코인 Prefab 저장하는 값
    public GameObject _sword; // 검 오브젝트를 직접 참조하는 값
    public Transform _playerTransform; // 플레이어의 위치를 참조하는 값
    public int _coinCount = 5; // 생성할 코인의 개수
    public float _coinRiseHeight = 3.0f; // 코인이 박스에서 나올 때 높이 설정
    public float _timeBeforeMoveToPlayer = 1.0f; // 코인이 플레이어에게 이동하기 전에 기다리는 시간
    public float _moveSpeed = 1.0f; // 코인이 플레이어에게 이동하는 속도

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{_hitCount}"); // 히트 카운트 초기값 출력
        // 일정 시간 후에 코인이 움직이도록 설정
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == _sword)
        {
            _hitCount++;
            Debug.Log($"Hit Count: {_hitCount}"); // 충돌 시마다 히트 카운트 출력

            if (_hitCount >= 3)
            {
                // 박스 제거
                Destroy(gameObject);

                // 코인 생성
                for (int i = 0; i < _coinCount; i++)
                {
                    Vector3 coinPosition = transform.position + Vector3.up * _coinRiseHeight + Vector3.right * (i - _coinCount / 2);
                    GameObject coinInstance = Instantiate(_coinPrefab, coinPosition, Quaternion.identity);

                    
                    // 코인에 CoinMovement 스크립트 추가
                    CoinMovement coinMovement = coinInstance.AddComponent<CoinMovement>();
                    coinMovement._playerTransform = _playerTransform;
                    coinMovement._moveSpeed = _moveSpeed;
                    coinMovement._delayBeforeMoving = _timeBeforeMoveToPlayer;
                   
                }
            }
        }
    }
}
