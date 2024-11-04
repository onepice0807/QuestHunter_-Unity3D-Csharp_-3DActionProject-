using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public Transform _target; // 적이 추적할 대상 (플레이어)
    private Animator _animator;
    public float _attackRange = 6.5f; // 공격 범위
    public int _attackDamage = 10; // 공격력
    public float _chaseRange = 10.0f; // 추적 범위
    public int _health = 100; // 적의 체력 (3번 맞으면 죽음)
    private bool _isAttacking = false; // 공격 중인지 여부
    private NavMeshAgent _navAgent; // 적의 추적을 위한 NavMeshAgent
    private int _attackCounter = 0; // 공격 카운트, 3번 중 1번은 3개의 공격을 모두 실행
    public GameObject _DamageTextPrefab; // 데미지 텍스트 프리팹
    public Transform _DamageTextSpawnPosition; // 데미지 텍스트가 표시될 위치 (보스 위)
    [SerializeField] Camera _mainCamera; // 메인카메라 참조값
    [SerializeField] public HealthBarBossRoom _healthBar; // HealthBar 참조 추가

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

    public void setTarget(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        if (_target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target.position);

            // 공격 범위 내에 있을 때
            if (distanceToTarget <= _attackRange && !_isAttacking)
            {
                StartCoroutine(AttackSequence());
            }
            // 추적 범위 내에 있을 때 추적
            else if (distanceToTarget <= _chaseRange && distanceToTarget > _attackRange)
            {
                _navAgent.isStopped = false;
                _navAgent.SetDestination(_target.position); // 플레이어 추적
                _animator.SetBool("Move", true); // 이동 애니메이션 설정
            }
            else
            {
                _navAgent.isStopped = true;
                _animator.SetBool("Move", false); // 이동 애니메이션 해제
            }
        }

    }

    IEnumerator AttackSequence()
    {
        _isAttacking = true;
        _navAgent.isStopped = true; // 공격 중에는 이동 멈춤
        _animator.SetBool("Move", false); // 이동 애니메이션 해제

        _attackCounter++;

        // 타겟을 바라보도록 회전
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);

        // 공격 패턴을 순서대로 실행
        if (_attackCounter % 4 == 1) // 첫 번째 공격: Attack2 -> Attack1
        {
            yield return Attack("Attack2");
            yield return Attack("Attack1");
        }
        else if (_attackCounter % 4 == 2) // 두 번째 공격: Attack2 단독
        {
            yield return Attack("Attack2");
        }
        else if (_attackCounter % 4 == 3) // 세 번째 공격: Attack1 단독
        {
            yield return Attack("Attack1");
        }
        else if (_attackCounter % 4 == 0) // 네 번째 공격 후 포효
        {
            yield return Attack("Attack1");
            yield return Attack("Attack3"); // 포효
        }

        yield return new WaitForSeconds(1.5f); // 공격 후 대기 시간

        _isAttacking = false; // 다시 추적할 수 있도록 상태 해제
    }


    IEnumerator Attack(string attackTrigger)
    {
        if (_animator != null)
        {
            _animator.SetTrigger(attackTrigger);
        }

        // 공격 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(1.0f);

    }

    private void OnTriggerEnter(Collider collider)
    {
        // 검이 보스와 충돌한 경우
        if (collider.CompareTag("Sword"))
        {
            TakeDamage(_attackDamage);
        }
    }


    // 보스가 데미지를 받을 때 호출되는 함수
    public void TakeDamage(int damage)
    {
        // 보스가 공격 중이면 데미지를 무효화
        if (_isAttacking)
        {
           // Debug.Log("보스가 공격 중이므로 데미지가 무효화되었습니다.");
            return;
        }
        else
        {
           // Debug.Log($"플레이어가 보스에게 {_attackDamage}만큼 데미지를 입혔습니다");
            ShowDamageText(_attackDamage);
        }

        // 체력 감소
        _health -= damage;

        // 데미지 로그 출력
        Debug.Log($"적이 {damage} 데미지를 받았습니다. 남은 체력: {_health}");

        // 체력바 업데이트
        if (_healthBar != null)
        {
            _healthBar.BossTakeDamage(_health); // 체력바의 TakeDamage에 감소할 데미지 전달
        }

        // 히트 애니메이션 재생
        if (_animator != null)
        {
            _animator.SetTrigger("Damage"); // Hit 애니메이션 트리거 작동
        }

        // 보스 체력이 0 이하일 때 사망 처리
        if (_health <= 0)
        {
            Die();
        }
    }


    // 보스가 죽는 메서드
    private void Die()
    {
        GameManager._Instance.AddMonster();
        Debug.Log("적이 사망했습니다.");
        Destroy(gameObject); // 보스 오브젝트 제거
    }

    public void ShowDamageText(int damage)
    {
        if (_DamageTextPrefab != null && _DamageTextSpawnPosition != null)
        {
            // 데미지 텍스트 생성
            var numberObj = Instantiate(_DamageTextPrefab);

            // 카메라의 위치와 방향 가져오기
            if (_mainCamera != null)
            {
                // 카메라가 보스에게서 보는 방향 계산
                Vector3 directionToCamera = (_mainCamera.transform.position - _DamageTextSpawnPosition.position).normalized;

                // 데미지 텍스트 위치를 보스의 좌측으로 설정
                Vector3 offsetPosition = _DamageTextSpawnPosition.position - (transform.right * 2.5f); // 좌측으로 1.5만큼 이동
                numberObj.transform.position = offsetPosition;

                // 데미지 텍스트가 항상 카메라를 향하도록 회전
                numberObj.transform.LookAt(_mainCamera.transform.position, Vector3.up);
                numberObj.transform.Rotate(_mainCamera.transform.forward); 
            }

            // 데미지 값을 텍스트로 설정
            numberObj.GetComponent<NumberText>().SetNumber(damage);
        }
    }

}
