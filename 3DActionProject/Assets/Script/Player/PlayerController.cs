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
    private Rigidbody _rigidbody; // �÷��̾� ������ ���� Rigidbody

    public int playerDamage = 1; // �÷��̾ ������ ������ �ο�

    public GameObject inventoryUI; // �κ��丮 UI â (Ȱ��/��Ȱ��)
    private bool isInventoryOpen = false; // �κ��丮 â�� ���ȴ��� ����

    public bool _isSideScrolling = false;  // ī�޶� Ⱦ��ũ�� ������� ���θ� ��Ÿ���� ����

    private bool _isOnLadder = false;  // �÷��̾ ��ٸ��� �ִ��� ��Ÿ���� ����


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
            // Ⱦ��ũ�� ��忡���� �̵�
            float moveX = Input.GetAxisRaw("Horizontal"); // A/D Ű�� �¿� �̵�
            float moveY = 0.0f;

            if (_isOnLadder)
            {
                moveY = Input.GetAxisRaw("Vertical"); // W/S Ű�� ���� ���Ʒ� �̵� Ȱ��ȭ (��ٸ�������)
                _direct = new Vector3(0, moveY, moveX).normalized; // Y�� �̵��� ���
                this.transform.rotation = Quaternion.Euler(0, -90, 0); // ĳ���Ͱ� ��ٸ��� �ٶ󺸵��� ȸ�� ���� (��ٸ� ���⿡ �°� ���� �ʿ�)
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

        if (collision.gameObject.tag.Contains("Coin")) // �����̶� �浹 �� ������ ������� �ϰ� ui ���� �� ����
        {
            Destroy(collision.gameObject);
            GameManager._Instance.AddCoin(); // ���� �� ����
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            MonsterController enemy = collider.GetComponent<MonsterController>();
            if (enemy != null && _animator.GetBool("Attack")) // �÷��̾ ���� ������ ����
            {
                enemy.TakeDamage(playerDamage); // ������ ������ ����
            }
        }

    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
        // ���� �ִϸ��̼� ����� ���ÿ� ���� Ÿ���ϴ� ����
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
        // Ű���� �Է� ó�� (�׻� ����)
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

        // �κ��丮 â ���� �ݱ�
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;  // �κ��丮 â ���
            inventoryUI.SetActive(isInventoryOpen);
        }

        // �κ��丮 â �ݱ� (Esc)
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
