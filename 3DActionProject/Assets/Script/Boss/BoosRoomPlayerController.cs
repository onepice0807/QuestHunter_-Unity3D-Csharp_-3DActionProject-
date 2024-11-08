using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


public class BoosRoomPlayerController : MonoBehaviour
{
    private float _vertical = 0.0f;
    private float _horizon = 0.0f;
    private float _moveSpeed = 7.0f;
    private Vector3 _direct;
    private Vector3 _lastPosition; // ������ ��ġ�� ������ ����
    private CharacterController _playerController;
    private Animator _animator;
    private bool _isDamaged = false; // �÷��̾ �������� �޾Ҵ��� Ȯ���ϴ� ����
    public int _health = 100; // �÷��̾� ü��
    public GameObject _sword; // �÷��̾��� �˿� ���� ����
    public bool _isAttacking = false; // ���� Ȱ��ȭ ����

    public Transform _target; // ���� ������ ��� (�÷��̾�)
    public int _attackDamage = 10; // ���ݷ�

    public GameObject _inventoryUI; // �κ��丮 UI â (Ȱ��/��Ȱ��)
    private bool _isInventoryOpen = false; // �κ��丮 â�� ���ȴ��� ����

    private int _shieldHitCount = 0; // ��� ���¿��� ���ݹ��� Ƚ��
    private bool _isShieldActive = false; // ��� ���� ����
    private bool _isWaiting = false; // ��� ���� ����
    [SerializeField] public HealthBarBossRoom _healthBar; // HealthBar ���� �߰�
    private int _leftClickCount = 0; // ���� ���콺 ��ư Ŭ�� Ƚ��
    private int _rightClickCount = 0; // ������ ���콺 ��ư Ŭ�� Ƚ��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Move()
    {
        if (_isWaiting || _isAttacking || _isShieldActive) return; // ��� ������ �� Ű �Է� ó�� �Ұ�

        // ī�޶� ������ �������� �÷��̾� �̵� ������ ����
        Transform cameraTransform = Camera.main.transform; // ���� ī�޶��� Transform ��������

        // ī�޶��� forward ���⿡�� Y���� ������ �̵� ���� ���
        Vector3 forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        // �Է¿� ���� �̵� ���� ���� (ī�޶� �������� ��/��/��/��)
        _vertical = Input.GetAxis("Vertical");
        _horizon = Input.GetAxis("Horizontal");

        // ���� �̵� ���� ��� (ī�޶� �������� ���� ����)
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

        if (collision.gameObject.tag.Contains("Coin")) // �����̶� �浹 �� ������ ������� �ϰ� ui ���� �� ����
        {
            Destroy(collision.gameObject);
            GameManager._Instance.AddCoin(); // ���� �� ����
        }
    }


    // �������� �޴� �޼���
    public void TakeDamage(int damage)
    {
        if (_isWaiting) return; // ��� ������ ���� ������ ����

        if (_isShieldActive)
        {
            _shieldHitCount++;
            Debug.Log($"��� ���·� {_shieldHitCount}��° ������ �޾ҽ��ϴ�.");
            _animator.SetTrigger("ShieldHit");
            if (_shieldHitCount >= 5)
            {
                StartCoroutine(ShieldBreak());
            }
            // ��� ���̹Ƿ� �������� ���� ����
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

            Debug.Log($"�÷��̾ {damage} �������� �޾ҽ��ϴ�. ���� ü��: {_health}");

            // ü�¹� ������Ʈ
            if (_healthBar != null)
            {
                _healthBar.TakeDamage(_health); // ü�¹��� TakeDamage�� ������ ������ ����
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
        _isWaiting = true; // ��� ���� ����
        _isShieldActive = false; // ��� ���� ����
        _animator.SetBool("Shield", false); // ��� �ִϸ��̼� ����
        _animator.SetTrigger("Hit"); // ShieldOf �ִϸ��̼� ����
        _shieldHitCount = 0; // ��� �� ���� ���� Ƚ�� �ʱ�ȭ

        Debug.Log("�� �������ϴ�! 2�� ���� ������ �� �����ϴ�.");

        yield return new WaitForSeconds(10.0f); // 2�� ���� ���

        _isWaiting = false; // ��� ���� ����
        Debug.Log("�ٽ� ������ �� �ֽ��ϴ�.");
    }

    // ������ ���¸� �����ϴ� �޼���
    private void ResetDamage()
    {
        _isDamaged = false;
    }

    // �÷��̾� ��� ó�� �޼���
    private void Die()
    {
        Debug.Log("�÷��̾ ����߽��ϴ�.");
        _animator.GetBool("Death");
        // ��� ó�� ���� (���� ����, ������ ��)
        // �̱����� ���� ScenesManager�� OnClickGameOver ȣ��
        if (ScenesManager._Instance != null)
        {
            ScenesManager._Instance.OnClickGameOver();
        }
        else
        {
            Debug.LogError("ScenesManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
    }

    private void OnTriggerExit(Collider collider)
    {

    }

    // ���� ���� �޼��� (���� ���ݰ� �߰� ���� ����)
    void Attack()
    {
        _isAttacking = true; // ���� �� ���� ����

        switch (_leftClickCount)
        {
            case 0: // ù ��° Ŭ�� �� ���� ����
                _leftClickCount = 1;
                _animator.SetTrigger("Attack"); // ���� ���� �ִϸ��̼� ����
                StartCoroutine(WaitForComboInput()); // ���� ���� �Է� ��� �ڷ�ƾ
                break;

            case 1: // �� ��° Ŭ�� �� ���� ����
                _leftClickCount = 2;
                _animator.SetTrigger("Attack2"); // ���� ���� �ִϸ��̼� ����
                StartCoroutine(WaitForComboInput()); // ���� ���� �Է� ��� �ڷ�ƾ
                break;

            case 2: // �� ��° Ŭ�� �� ���� ����
                _leftClickCount = 3;
                _animator.SetTrigger("Attack3"); // ���� ���� �ִϸ��̼� ����
                Invoke("EndComboAttack", 1.5f); // ���� ���� �ִϸ��̼��� ���� ��ġ�� �����ϰ� ���� �ʱ�ȭ
                break;
        }
    }

    // ���� ���� �Է� ��� �ڷ�ƾ
    IEnumerator WaitForComboInput()
    {
        yield return new WaitForSeconds(0.5f); // 0.5�� ���� ���� ���� �Է� ���
        if (_leftClickCount < 3) // Ŭ���� �����ϴٸ� ����
        {
            ResetAttackState(); // ���� ���� �ʱ�ȭ
        }
    }

    // ���� ������ ���� ��ġ�� �����ϰ� ���� ���¸� �ʱ�ȭ�ϴ� �޼���
    public void EndComboAttack()
    {
        ResetAttackState();
    }

    // ���� ���� �ʱ�ȭ �޼���
    private void ResetAttackState()
    {
        _isAttacking = false; // ���� ���� ����
        _leftClickCount = 0; // ����Ŭ�� ī��Ʈ �ʱ�ȭ
        _rightClickCount = 0; // ������Ŭ�� ī��Ʈ �ʱ�ȭ
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("Attack2");
        _animator.ResetTrigger("Attack3");
    }

    void BackJump()
    {
        _animator.SetTrigger("BackJump");
    }

    void Shield()
    {
        if (_isWaiting) return; // ��� ������ �� ��� �Ұ�
        _isShieldActive = true; // ��� ���� Ȱ��ȭ
        _animator.SetBool("Shield", true);
    }

    private void KeyEventProcess()
    {
        // Ű���� �Է� ó�� (�׻� ����)
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

        // �κ��丮 â ���� �ݱ�
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isInventoryOpen = !_isInventoryOpen; // �κ��丮 â ���
            _inventoryUI.SetActive(_isInventoryOpen);
            Time.timeScale = 0; // ���� �Ͻ�����
            Cursor.visible = true; // ���콺 Ŀ���� ǥ��
            Cursor.lockState = CursorLockMode.None; // ���콺 ����� ����

            if (!_isInventoryOpen) // �κ��丮 â�� ���� ���
            {
                _inventoryUI.SetActive(false); // �κ��丮 UI ��Ȱ��ȭ
                Time.timeScale = 1; // ���� �簳
                Cursor.visible = false; // ���콺 Ŀ�� ����
                Cursor.lockState = CursorLockMode.Locked; // ���콺 ���
            }
        }

        // �̵��ӵ� �ø���
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveSpeed = 14.0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed = 7.0f;
        }

        // OŰ�� �������� �ɼ�PopUp ����
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (GameManager._Instance != null)
            {
                GameManager._Instance.ShowOptionPopUp(); // GameManager���� ShowOptionPopUpȣ��
            }
        }

        // ���� �������� ��������  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0; // ���� �Ͻ�����
            Cursor.visible = true; // ���콺 Ŀ���� ǥ��
            Cursor.lockState = CursorLockMode.None; // ���콺����� ����
            if (GameManager._Instance != null)
            {
                GameManager._Instance.GameExitPopUp();
            }
            else
            {
                Debug.LogError("�ν��Ͻ��� ã�� �� �����ϴ�.");
            }
        }
    }

    private void MouseEventProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack(); // ���콺 ���� Ŭ������ ����
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

    void Update()
    {
        // ���� ���� ���� �̵� �� ȸ�� ����
        MouseEventProcess();
           
        if (!_isAttacking)
        {
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

    }
}