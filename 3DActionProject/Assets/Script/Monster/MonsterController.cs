using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public Transform _target; // 적이 추적할 대상 (플레이어)
    private Animator _animator;
    public float _attackRange = 2.5f; // 공격 범위
    public int _attackDamage = 10; // 공격력
    public float _chaseRange = 10.0f; // 추적 범위
    public int _health = 3; // 적의 체력 (3번 맞으면 죽음)

    private bool _isAttacking = false; // 공격 여부
    private NavMeshAgent _navAgent; // 적의 추적을 위한 NavMeshAgent

    // 델리게이트 및 이벤트 정의 (몬스터 사망 시 알림)
    public delegate void MonsterDeathHandler(GameObject monster);
    public event MonsterDeathHandler OnMonsterDeath; // 몬스터 사망 이벤트

    public bool _isSideScrolling = false;  // 카메라가 횡스크롤 모드인지 여부를 나타내는 변수


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();

        // 횡스크롤 모드일 때 y축 및 x축 회전을 잠그기
        if (_isSideScrolling)
        {
            _navAgent.updateRotation = false; // NavMeshAgent의 회전 비활성화
        }
    }

    public void setTarget(Transform target)
    {
        _target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target.position);

            // 공격 범위 내에 있을 때 공격
            if (distanceToTarget <= _attackRange && !_isAttacking)
            {
                StartCoroutine(Attack());
            }
            // 추적 범위 안에 있을 때 추적
            else if (distanceToTarget <= _chaseRange && distanceToTarget > _attackRange)
            {
                _navAgent.isStopped = false;
                Vector3 targetPosition = _target.position;

                // 횡스크롤 모드에서는 x축 이동을 제한하여 z축만 이동
                if (_isSideScrolling)
                {
                    targetPosition.x = transform.position.x;
                }
                _navAgent.SetDestination(_target.position); // 플레이어를 추적
                _animator.SetBool("Move", true); // 이동 애니메이션 설정
            }
            else
            {
                _navAgent.isStopped = true;
                _animator.SetBool("Move", false); // 이동 애니메이션 해제
            }
        }
    }

    IEnumerator Attack()
    {
        _isAttacking = true;

        // 플레이어를 바라보도록 회전
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);

        // 공격 애니메이션 시작
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }

        // 애니메이션 시간이 끝날 때까지 대기 (예시: 1초)
        yield return new WaitForSeconds(1.0f);

        // 충돌 감지 및 데미지 처리
        PlayerInteraction playerInteraction = _target.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.TakeDamage(_attackDamage); // 데미지를 전달
        }

        // 공격 후 일정 시간 대기
        yield return new WaitForSeconds(0.5f);

        _isAttacking = false;
    }

    // 적이 데미지를 받을 때 호출되는 함수
    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log($"적이 {damage} 데미지를 받았습니다. 남은 체력: {_health}");

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            // 피격 애니메이션 시작
            _animator.SetTrigger("Hit");
        }
    }

    // 적이 죽는 메서드
    private void Die()
    {
        // 몬스터 사망 시 OnMonsterDeath 이벤트 호출
        if (OnMonsterDeath != null)
        {
            OnMonsterDeath(gameObject); // 구독자들에게 몬스터의 죽음을 알림
        }

        // 적 제거 전에 약간의 대기 시간을 추가하여 사망 애니메이션 실행
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // 사망 애니메이션 시작
        _animator.SetTrigger("Die");
        yield return new WaitForSeconds(1.2f); // 사망 애니메이션 재생 시간
        Destroy(gameObject); // 적 오브젝트 제거
        GameManager._Instance.AddMonster();
        Debug.Log("적이 사망했습니다.");
    }
}
