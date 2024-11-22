using System.Collections;
using UnityEngine;


public class BoosRoomPlayerController : MonoBehaviour
{
    private float _vertical = 0.0f;
    private float _horizon = 0.0f;
    private float _moveSpeed = 7.0f;
    private Vector3 _direct;
    private Vector3 _lastPosition; // 마지막 위치를 저장할 변수
    private CharacterController _playerController;
    private Animator _animator;
    private bool _isDamaged = false; // 플레이어가 데미지를 받았는지 확인하는 변수
    public int _health = 100; // 플레이어 체력
    public GameObject _sword; // 플레이어의 검에 대한 참조
    private Collider _swordCollider; // 검의 트리거 관리
    public bool _isAttacking = false; // 공격 활성화 상태

    public Transform _target; // 적이 추적할 대상 (플레이어)
    public int _attackDamage = 10; // 공격력
    public GameObject _AttackEffectPrefab; // 보스가 공격시 효과를 위한 프리팹
    private bool _isEffectActive = false; // 이펙트가 활성화되었는지 확인할 변수
    private float _effectCooldown = 1.0f; // 이펙트 재생성 쿨타임

    public GameObject _inventoryUI; // 인벤토리 UI 창 (활성/비활성)
    private bool _isInventoryOpen = false; // 인벤토리 창이 열렸는지 여부

    private int _shieldHitCount = 0; // 방어 상태에서 공격받은 횟수
    private bool _isShieldActive = false; // 방어 상태 여부
    private bool _isWaiting = false; // 대기 상태 여부
    [SerializeField] public HealthBarBossRoom _healthBar; // HealthBar 참조 추가
    private int _leftClickCount = 0; // 왼쪽 마우스 버튼 클릭 횟수
    private int _rightClickCount = 0; // 오른쪽 마우스 버튼 클릭 횟수

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _swordCollider = _sword.GetComponent<Collider>();
        _swordCollider.enabled = false; // 시작 시 검의 트리거 비활성화
    }

    void Move()
    {
        if (_isWaiting || _isAttacking || _isShieldActive) return; // 대기 상태일 때 키 입력 처리 불가

        // 카메라 방향을 기준으로 플레이어 이동 방향을 설정
        Transform cameraTransform = Camera.main.transform; // 메인 카메라의 Transform 가져오기

        // 카메라의 forward 방향에서 Y축을 무시한 이동 방향 계산
        Vector3 forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        // 입력에 따른 이동 방향 설정 (카메라를 기준으로 전/후/좌/우)
        _vertical = Input.GetAxis("Vertical");
        _horizon = Input.GetAxis("Horizontal");

        // 최종 이동 방향 계산 (카메라를 기준으로 방향 정렬)
        _direct = (forward * _vertical + right * _horizon).normalized;

        this.transform.position += _direct * _moveSpeed * Time.deltaTime;

        if (_direct != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(_direct);
            _animator.SetBool("Move", true);
        }

    }

    void Stop()
    {
        if (_direct != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(_direct);
            _animator.SetBool("Move", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag.Contains("Coin")) // 코인이랑 충돌 시 코인을 사라지게 하고 ui 코인 수 증가
        {
            Destroy(collision.gameObject);
            GameManager._Instance.AddCoin(); // 코인 수 증가
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("BossHit"))
        {
            TakeDamage(_attackDamage);
            // 충돌 지점 계산
            Vector3 collisionPoint = collider.ClosestPoint(transform.position);
            // 데미지 효과 프리팹을 충돌 지점에 생성
            Instantiate(_AttackEffectPrefab, collisionPoint, Quaternion.identity);
            StartCoroutine(ResetEffect()); // 쿨타임 후 이펙트 재생성 가능하도록 초기화
        }

    }

    IEnumerator ResetEffect()
    {
        yield return new WaitForSeconds(_effectCooldown); // 쿨타임 대기
        _isEffectActive = false; // 이펙트 초기화
    }


    // 데미지를 받는 메서드
    public void TakeDamage(int damage)
    {
        if (_isWaiting) return; // 대기 상태일 때는 데미지 무시

        if (_isShieldActive)
        {
            _shieldHitCount++;
            Debug.Log($"방어 상태로 {_shieldHitCount}번째 공격을 받았습니다.");
            _animator.SetTrigger("ShieldHit");
            if (_shieldHitCount >= 5)
            {
                StartCoroutine(ShieldBreak());
            }
            // 방어 중이므로 데미지를 받지 않음
            return;
        }

        if (!_isDamaged)
        {
            _isDamaged = true;
            _health -= damage;

            if (_animator != null)
            {
                _animator.SetTrigger("Damaged");
            }

            Debug.Log($"플레이어가 {damage} 데미지를 받았습니다. 남은 체력: {_health}");

            // 체력바 업데이트
            if (_healthBar != null)
            {
                _healthBar.TakeDamage(_health); // 체력바의 TakeDamage에 감소할 데미지 전달
            }

            if (_health == 0)
            {
                Die();
            }

            Invoke("ResetDamage", 3.0f);
        }
    }
    IEnumerator ShieldBreak()
    {
        _isWaiting = true; // 대기 상태 시작
        _isShieldActive = false; // 방어 상태 해제
        _animator.SetBool("Shield", false); // 방어 애니메이션 해제
        _animator.SetTrigger("Hit"); // ShieldOf 애니메이션 실행
        _shieldHitCount = 0; // 방어 중 받은 공격 횟수 초기화

        Debug.Log("방어가 깨졌습니다! 2초 동안 움직일 수 없습니다.");

        yield return new WaitForSeconds(10.0f); // 2초 동안 대기

        _isWaiting = false; // 대기 상태 해제
        Debug.Log("다시 움직일 수 있습니다.");
    }

    // 데미지 상태를 리셋하는 메서드
    private void ResetDamage()
    {
        _isDamaged = false;
    }

    // 플레이어 사망 처리 메서드
    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다.");
        _animator.GetBool("Death");
        // 사망 처리 로직 (게임 오버, 리스폰 등)
        // 싱글톤을 통해 ScenesManager의 OnClickGameOver 호출
        if (ScenesManager._Instance != null)
        {
            ScenesManager._Instance.OnClickGameOver();
        }
        else
        {
            Debug.LogError("ScenesManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerExit(Collider collider)
    {

    }

    // 공격 실행 메서드 (단일 공격과 추가 공격 구분)
    void Attack()
    {
        _isAttacking = true; // 공격 중 상태 설정

        switch (_leftClickCount)
        {
            case 0: // 첫 번째 클릭 시 단일 공격
                _leftClickCount = 1;
                _animator.SetTrigger("Attack"); // 단일 공격 애니메이션 실행
                EnableSwordTrigger();
                StartCoroutine(WaitForComboInput()); // 연계 공격 입력 대기 코루틴
                break;

            case 1: // 두 번째 클릭 시 연계 공격
                _leftClickCount = 2;
                _animator.SetTrigger("Attack2"); // 연계 공격 애니메이션 실행
                EnableSwordTrigger();
                StartCoroutine(WaitForComboInput()); // 연계 공격 입력 대기 코루틴
                break;

            case 2: // 두 번째 클릭 시 연계 공격
                _leftClickCount = 3;
                _animator.SetTrigger("Attack3"); // 연계 공격 애니메이션 실행
                EnableSwordTrigger();
                Invoke("EndComboAttack", 1.5f); // 연계 공격 애니메이션이 끝난 위치를 저장하고 상태 초기화
                break;
        }
    }

    // 연계 공격 입력 대기 코루틴
    IEnumerator WaitForComboInput()
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 동안 연계 공격 입력 대기
        if (_leftClickCount < 3) // 클릭이 부족하다면 종료
        {
            ResetAttackState(); // 공격 상태 초기화
        }
    }

    // 연계 공격이 끝난 위치를 저장하고 공격 상태를 초기화하는 메서드
    public void EndComboAttack()
    {
        ResetAttackState();
        DisableSwordTrigger(); // 공격 종료 시 검의 트리거 비활성화
    }

    // 공격 상태 초기화 메서드
    private void ResetAttackState()
    {
        _isAttacking = false; // 공격 상태 해제
        _leftClickCount = 0; // 왼쪽클릭 카운트 초기화
        _rightClickCount = 0; // 오른쪽클릭 카운트 초기화
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("Attack2");
        _animator.ResetTrigger("Attack3");
        DisableSwordTrigger(); // 공격 종료 시 검의 트리거 비활성화
    }

    void BackJump()
    {
        _animator.SetTrigger("BackJump");
    }

    // 검의 트리거 활성화
    void EnableSwordTrigger()
    {
        _swordCollider.enabled = true;
    }

    // 검의 트리거 비활성화
    void DisableSwordTrigger()
    {
        _swordCollider.enabled = false;
    }

    void Shield()
    {
        if (_isWaiting) return; // 대기 상태일 때 방어 불가
        _isShieldActive = true; // 방어 상태 활성화
        _animator.SetBool("Shield", true);
    }

    private void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;

        if (_isInventoryOpen)
        {
            _inventoryUI.SetActive(true);
            Time.timeScale = 0; // 게임 일시정지
            Cursor.visible = true; // 마우스 커서 표시
            Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
        }
        else
        {
            _inventoryUI.SetActive(false);
            Time.timeScale = 1; // 게임 재개
            Cursor.visible = false; // 마우스 커서 숨김
            Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        }
    }

    private void KeyEventProcess()
    {
        // 키보드 입력 처리 (항상 감지)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BackJump();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Shield();
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            _animator.SetBool("Shield", false);
        }

        // 이동속도 늘리기
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveSpeed = 14.0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed = 7.0f;
        }

        // O키를 눌렀을때 옵션PopUp 열기
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (GameManager._Instance != null)
            {
                GameManager._Instance.ShowOptionPopUp(); // GameManager에서 ShowOptionPopUp호출
            }
        }

        // 게임 스테이지 강제종료  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0; // 게임 일시정지
            Cursor.visible = true; // 마우스 커서를 표시
            Cursor.lockState = CursorLockMode.None; // 마우스잠금을 해제
            if (GameManager._Instance != null)
            {
                GameManager._Instance.GameExitPopUp();
            }
            else
            {
                Debug.LogError("인스턴스를 찾을 수 없습니다.");
            }
        }
    }

    private void MouseEventProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack(); // 마우스 왼쪽 클릭으로 공격
        }


        if (Input.GetMouseButtonDown(1))
        {
            Shield();
        }

        if (Input.GetMouseButtonUp(1))
        {
            _animator.SetBool("Shield", false);
            _isShieldActive = false;
        }
    }

    private void ProcessInput()
    {
        MouseEventProcess();
        KeyEventProcess();

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            Move();
        }
        else
        {
            Stop();
        }
    }

    void Update()
    {
        // 인벤토리 UI 토글 처리
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
            return; // 인벤토리 토글 시 다른 입력은 처리하지 않음
        }

        // 현재 애니메이션 상태를 가져옴
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 공격 애니메이션이 실행 중인지 확인
        bool isAttackAnimationPlaying = stateInfo.IsTag("Attack");

        // 공격 애니메이션 중일 때 이동을 막음
        if (isAttackAnimationPlaying)
        {
            MouseEventProcess();
            Stop(); // 이동을 멈춤
            return;
        }

        // 공격 애니메이션이 아닐 때만 이동 및 키 입력 처리
        ProcessInput();
    }
}