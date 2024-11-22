using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public Transform _target; // 적이 추적할 대상 (플레이어)
    private Animator _animator;
    public float _attackRange = 12.5f; // 공격 범위
    public int _attackDamage = 10; // 공격력
    public float _chaseRange = 10.0f; // 추적 범위
    public int _health = 100; // 보스의 체력
    private bool _isAttacking = false; // 공격 중인지 여부
    private NavMeshAgent _navAgent; // 적의 추적을 위한 NavMeshAgent
    private int _attackCounter = 0; // 공격 카운트, 3번 중 1번은 3개의 공격을 모두 실행
    private int _damageNullifyCounter = 0; // 데미지 무효화 카운트
    private bool _isPlayerInRange = false; // 플레이어가 공격 범위 내에 있는지 여부
    public Transform _DamageTextSpawnPosition; // 데미지 텍스트가 표시될 위치 (보스 위)
    public GameObject _DamageTextPrefab; // 데미지 텍스트 프리팹
    public GameObject _AttackEffectPrefab; // 보스가 공격시 효과를 위한 프리팹
    public GameObject _DamageEffectPrefab; // 플레이어의 공격을 맞을때 효과를 위한 프리팹
    private bool _isEffectActive = false; // 이펙트가 활성화되었는지 확인할 변수
    private float _effectCooldown = 1.0f; // 이펙트 재생성 쿨타임
    public Transform _FireVFXPosition; // 죽을때 화염이 표시될 위치
    public Transform _FireVFXPosition2; // 죽을때 화염이 표시될 위치
    public Transform _AttackEffectPosition; // 공격 효과가 나타날 위치
    public Transform _AttackEffectPosition2; // 공격 효과가 나타날 위치
    public GameObject _FireVFXPrefab; // 화염 프리팹 (보스가 죽을때 사용)
    [SerializeField] Camera _mainCamera; // 메인카메라 참조값
    [SerializeField] public HealthBarBossRoom _healthBar; // HealthBar 참조 추가

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();

        // NavMeshAgent의 stoppingDistance를 _attackRange로 설정
        _navAgent.stoppingDistance = _attackRange;
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
            // Debug.Log("타겟과의 거리: " + distanceToTarget); // 거리 확인

            // 공격 범위 내에 있을 때
            if (distanceToTarget <= _attackRange)
            {
                if (!_isPlayerInRange) // 플레이어가 처음 공격 범위 내에 들어왔을 때
                {
                    _damageNullifyCounter = 0; // 데미지 무효화 카운트를 초기화
                    _isPlayerInRange = true;
                }

                if (!_isAttacking)
                {
                    StartCoroutine(AttackSequence()); // 공격 실행
                }
            }
            else
            {
                // 플레이어가 공격 범위를 벗어났을 때
                _isPlayerInRange = false; // 범위 외로 나가면 초기화
            }

            // 추적 범위 내에 있지만 공격 범위 바깥에 있을 때 추적
            if (distanceToTarget <= _chaseRange && distanceToTarget > _attackRange)
            {
                _navAgent.isStopped = false;
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

    IEnumerator AttackSequence()
    {
        _isAttacking = true;
        _navAgent.isStopped = true; // 공격 중에는 이동 멈춤
        _animator.SetBool("Move", false); // 이동 애니메이션 해제

        _attackCounter++;

        // 타겟을 정확히 바라보도록 회전
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));

        // 공격 시작 전 잠시 대기
        yield return new WaitForSeconds(0.2f);

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
            _isEffectActive = true;
            // 데미지 적용

            if (_isAttacking)
            {
                if (_damageNullifyCounter < 2)
                {
                    _damageNullifyCounter++; // 무효화 카운트 증가
                    Debug.Log("보스가 공격 중이므로 데미지가 무효화되었습니다. 무효화 카운트: " + _damageNullifyCounter);
                    return; // 무효화 후 종료
                }
                else
                {
                    TakeDamage(_attackDamage);
                    Debug.Log($"플레이어가 보스에게 {_attackDamage}만큼 데미지를 입혔습니다");
                    SoundManager.Instance.Play_PlayerAttackSound();
                    // 충돌 지점에 데미지 효과 생성
                    if (_DamageEffectPrefab != null)
                    {
                        // 충돌 지점 계산
                        Vector3 collisionPoint = collider.ClosestPoint(transform.position);
                        // 데미지 효과 프리팹을 충돌 지점에 생성
                        Instantiate(_DamageEffectPrefab, collisionPoint, Quaternion.identity);
                        StartCoroutine(ResetEffect()); // 쿨타임 후 이펙트 재생성 가능하도록 초기화
                    }
                }
            }
            
        }
    }

    IEnumerator ResetEffect()
    {
        yield return new WaitForSeconds(_effectCooldown); // 쿨타임 대기
        _isEffectActive = false; // 이펙트 초기화
    }

    // 보스가 공격할때 이팩트 효과를 주기위해 사용하는 함수
    public void AttackEffectRight()
    {
        // 화염 효과 생성
        if (_AttackEffectPrefab != null)
        {
            SoundManager.Instance.Play_BossAttackSound();
            Instantiate(_AttackEffectPrefab, _AttackEffectPosition.position, Quaternion.identity); // 오른쪽발 위치에 공격 효과 표시
        }
    }

    public void AttackEffectLeft()
    {
        // 화염 효과 생성
        if (_AttackEffectPrefab != null)
        {
            SoundManager.Instance.Play_BossAttackSound();
            Instantiate(_AttackEffectPrefab, _AttackEffectPosition.position, Quaternion.identity); // 왼쪽발 위치에 공격 효과 표시
        }
    }


    // 보스가 데미지를 받을 때 호출되는 함수
    public void TakeDamage(int damage)
    {
        // 보스가 공격 중이면 데미지를 무효화
        // 보스가 공격 중일 때 데미지 무효화 카운트를 증가

        // 체력 감소
        _health -= damage;

        // 데미지 로그 출력
        Debug.Log($"적이 {damage} 데미지를 받았습니다. 남은 체력: {_health}");
        ShowDamageText(_attackDamage);
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
        Debug.Log("보스가 사망했습니다.");

        // 죽음 애니메이션 실행
        if (_animator != null)
        {
            _animator.SetBool("Die", true);
        }

        // 화염 효과 생성
        if (_FireVFXPrefab != null)
        {
            Instantiate(_FireVFXPrefab, _FireVFXPosition.position, Quaternion.identity); // 첫 번째 위치에 화염 표시
            Instantiate(_FireVFXPrefab, _FireVFXPosition2.position, Quaternion.identity); // 두 번째 위치에 화염 표시
        }

        // 3초 후에 보스를 제거하고 팝업을 표시
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        // 3초 동안 대기
        yield return new WaitForSeconds(3.0f);

        // 보스 오브젝트 제거
        Destroy(gameObject);

        // 팝업 표시 (게임 매니저를 통해 호출)
        ScenesManager._Instance.OnClickGameClear(); // OnClickGameClear 씬 매니저에서 호출
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
                // numberObj.transform.LookAt(_mainCamera.transform.position, Vector3.up);
                //numberObj.transform.Rotate(_mainCamera.transform.forward); 
            }

            // 데미지 값을 텍스트로 설정
            numberObj.GetComponent<NumberText>().SetNumber(damage);
        }
    }

}
