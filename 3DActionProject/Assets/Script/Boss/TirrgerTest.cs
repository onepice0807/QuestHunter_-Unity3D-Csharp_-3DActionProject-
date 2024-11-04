using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TirrgerTest : MonoBehaviour
{
    public Transform _target; // 적이 추적할 대상 (플레이어)
    public int _attackDamage = 10; // 공격력

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // 타겟에 데미지 전달
            BoosRoomPlayerController boosRoomPlayerController = _target.GetComponent<BoosRoomPlayerController>();
            if (boosRoomPlayerController != null)
            {
                boosRoomPlayerController.TakeDamage(_attackDamage);
                Debug.Log("플레이어가 보스의 공격에 맞았습니다!");
            }
        }
        else
        {
            Debug.Log("타겟에 맞지 않았습니다");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
