using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public Transform _target; // 적이 추적할 대상 (플레이어)
    public int _attackDamage = 10; // 공격력


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Hit"))
        {
            BossController boss = _target.GetComponent<BossController>();

            // 플레이어가 공격 중일 때 검이 보스와 충돌하면 데미지를 입힘
            if (boss != null)
            {
                boss.TakeDamage(_attackDamage);
                Debug.Log("플레이어가 보스의 공격에 맞았습니다!");

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
