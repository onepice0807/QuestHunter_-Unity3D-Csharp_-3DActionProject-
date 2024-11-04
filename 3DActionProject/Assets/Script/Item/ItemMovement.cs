using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public Transform _playerTransform;
    public float _moveSpeed = 1.0f;
    public float _delayBeforeMoving = 1.0f;

    private bool _shouldMove = false;
    private ItemPickup _itemPickup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _itemPickup = GetComponent<ItemPickup>();  // ItemPickup 컴포넌트 가져오기
        if (_itemPickup == null)
        {
            Debug.LogError("ItemPickup 컴포넌트가 아이템에 연결되어 있지 않습니다.");
        }
        Invoke("StartMoving", _delayBeforeMoving);
    }

    void StartMoving()
    {
        _shouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shouldMove && _playerTransform != null)
        {
            // 플레이어 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _moveSpeed * Time.deltaTime);

            // 아이템이 플레이어에 도달하면 획득 처리
            if (Vector3.Distance(transform.position, _playerTransform.position) < 0.5f)
            {
                // 인벤토리에 아이템 추가
                if (_itemPickup != null && _itemPickup._item != null)
                {
                    // 카테고리를 필요에 따라 설정 ("재료" 또는 "무기")
                    
                }

                // 아이템 오브젝트 제거
                Destroy(gameObject);
            }
        }
    }
}