using System.Collections;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    public Transform _playerTransform; // 플레이어의 위치
    public float _moveSpeed = 0.0f; // 코인이 이동하는 속도
    public float _delayBeforeMoving = 0.0f; // 이동하기 전 대기 시간

    private bool _shouldMove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 일정 시간 후에 코인이 움직이도록 설정
        StartCoroutine(StartMoving());
    }

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(_delayBeforeMoving);
        _shouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shouldMove && _playerTransform != null)
        {
            // 플레이어를 향해 이동
            Vector3 direction = (_playerTransform.position - transform.position).normalized;
            transform.position += direction * _moveSpeed * Time.deltaTime;

            // 일정 거리 이내로 들어오면 코인 제거 및 획득 처리
            if (Vector3.Distance(transform.position, _playerTransform.position) < 0.5f)
            {
                // 플레이어에게 코인 추가
                GameManager._Instance.AddCoin();

                // 코인 오브젝트 제거
                Destroy(gameObject);
            }
        }
    }
}