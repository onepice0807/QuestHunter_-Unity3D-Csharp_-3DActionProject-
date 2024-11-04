using UnityEngine;
using System.Collections;

public class ItemBox : MonoBehaviour
{
    private int _hitCount = 0; // 박스가 몇 번 충돌되었는지 카운트 하는 값
    public GameObject _itemPrefab; // 포션 프리팹 저장
    public GameObject _sword; // 검 오브젝트를 직접 참조하는 값
    public Transform _playerTransform; // 플레이어의 위치를 참조하는 값
    public int _itemCount = 5; // 생성할 포션의 개수 (수량)
    public float _itemRiseHeight = 3.0f; // 아이템이 박스에서 나올 때 높이 설정
    public float _timeBeforeMoveToPlayer = 1.0f; // 아이템이 플레이어에게 이동하기 전에 기다리는 시간
    public float _moveSpeed = 1.0f; // 아이템이 플레이어에게 이동하는 속도

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == _sword)
        {
            _hitCount++;
            Debug.Log($"Hit Count: {_hitCount}");

            if (_hitCount >= 3)
            {
                // 박스 파괴
                Destroy(gameObject);



                // 아이템 생성
                Vector3 itemPosition = transform.position + Vector3.up * _itemRiseHeight;
                GameObject itemInstance = Instantiate(_itemPrefab, transform.position, Quaternion.identity);

                // ItemPickup 컴포넌트 확인
                ItemPickup itemPickup = itemInstance.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    
                }

                // 아이템을 플레이어에게 이동시키는 로직
                ItemMovement itemMovement = itemInstance.AddComponent<ItemMovement>();
                itemMovement._playerTransform = _playerTransform;
                itemMovement._moveSpeed = _moveSpeed;
                itemMovement._delayBeforeMoving = _timeBeforeMoveToPlayer;
            }
        }
    }
}