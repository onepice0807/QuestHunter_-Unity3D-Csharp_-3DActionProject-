using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _vertical = 0.0f;
    private float _horizon = 0.0f;
    private float _moveSpeed = 7.0f;
    private Vector3 _direct;
    private CharacterController _playerController;
    private Animator _animator;
    private Rigidbody _rigidbody; // 플레이어 점프를 위한 Rigidbody

    public int playerDamage = 1; // 플레이어가 적에게 데미지 부여

    public GameObject inventoryUI; // 인벤토리 UI 창 (활성/비활성)
    private bool isInventoryOpen = false; // 인벤토리 창이 열렸는지 여부

    public bool _isSideScrolling = false;  // 카메라가 횡스크롤 모드인지 여부를 나타내는 변수

    private bool _isOnLadder = false;  // 플레이어가 사다리에 있는지 나타내는 변수


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Move()
    {
        if (_isSideScrolling)
        {
            // 횡스크롤 모드에서의 이동
            float moveX = Input.GetAxisRaw("Horizontal"); // A/D 키로 좌우 이동
            float moveY = 0.0f;

            if (_isOnLadder)
            {
                moveY = Input.GetAxisRaw("Vertical"); // W/S 키로 수직 위아래 이동 활성화 (사다리에서만)
                _direct = new Vector3(0, moveY, moveX).normalized; // Y축 이동만 허용
                this.transform.rotation = Quaternion.Euler(0, -90, 0); // 캐릭터가 사다리를 바라보도록 회전 고정 (사다리 방향에 맞게 수정 필요)
                this.transform.position += _direct * _moveSpeed * Time.deltaTime;
                _animator.SetBool("Move", true);
            }
            else
            {
                _direct = new Vector3(0, moveY, moveX).normalized;
                this.transform.position += _direct * _moveSpeed * Time.deltaTime;

                if (_direct != Vector3.zero)
                {
                    this.transform.rotation = Quaternion.LookRotation(_direct);
                    _animator.SetBool("Move", true);
                }
            }

        }
        else
        {
            _vertical = Input.GetAxis("Vertical");
            _horizon = Input.GetAxis("Horizontal");

            _direct = new Vector3(_horizon, 0.0f, _vertical).normalized;
            this.transform.position += _direct * _moveSpeed * Time.deltaTime;

            if (_direct != Vector3.zero)
            {
                this.transform.rotation = Quaternion.LookRotation(_direct);
                _animator.SetBool("Move", true);
            }
        }

            
    }

    void Stop()
    {
        if (_direct != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(_direct);
        }
        _animator.SetBool("Move", false);
    }

    void Clash()
    {
        _animator.SetBool("Clash", true);
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
        if (collider.CompareTag("Enemy"))
        {
            MonsterController enemy = collider.GetComponent<MonsterController>();
            if (enemy != null && _animator.GetBool("Attack")) // 플레이어가 공격 상태일 때만
            {
                enemy.TakeDamage(playerDamage); // 적에게 데미지 전달
            }
        }

    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
        // 공격 애니메이션 재생과 동시에 적을 타격하는 로직
    }

    void Jump()
    {
        _animator.SetBool("Jump", true);

    }

    void Defend()
    {
        _animator.SetBool("Defend", true);
    }


    private void KeyEventProcess()
    {
        // 키보드 입력 처리 (항상 감지)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _animator.SetBool("Jump", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }

        // 인벤토리 창 열고 닫기
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;  // 인벤토리 창 토글
            inventoryUI.SetActive(isInventoryOpen);
        }

        // 인벤토리 창 닫기 (Esc)
        if (Input.GetKeyDown(KeyCode.Escape) && isInventoryOpen)
        {
            isInventoryOpen = false;
            inventoryUI.SetActive(false);
        }


    }

    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            Move();
        }
        else
        {
            Stop();
        }


        KeyEventProcess();

        
    }
}
