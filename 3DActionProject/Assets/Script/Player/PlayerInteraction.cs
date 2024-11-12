using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Animator _animator;
    private bool _isDamaged = false; // 플레이어가 데미지를 받았는지 확인하는 변수
    private int _damageCount = 0; // 데미지 카운트를 세는 변수
    public int _health = 100; // 플레이어 체력
    [SerializeField] public HealthBar _healthBar; // HealthBar 참조 추가
    [SerializeField] private GameObject _gameOverUI; // 플레이어가 죽었을때 게임오버 표시하기 위한 변수

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // 데미지를 받는 메서드
    public void TakeDamage(int damage)
    {
        if (!_isDamaged) // 이미 데미지를 받은 상태가 아니면
        {
            _isDamaged = true; // 플레이어가 데미지를 받음
            _health -= damage; // 데미지를 체력에서 감소

            Debug.Log($"플레이어가 {damage} 데미지를 받았습니다. 남은 체력: {_health}");

            // 체력바 업데이트
            if (_healthBar != null)
            {
                _healthBar.TakeDamage(_health); // 체력바의 TakeDamage에 감소할 데미지 전달
            }

            if (_health <= 0)
            {
                Die();
            }

            // 일정 시간 후 데미지 상태 해제
            Invoke("ResetDamage", 3.0f); // 3초 후 데미지 상태를 해제
        }
    }

    // 데미지 상태를 리셋하는 메서드
    private void ResetDamage()
    {
        _isDamaged = false;
    }

    // 플레이어 사망 처리 메서드
    // 플레이어 사망 처리 메서드
    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다.");

        // 사망 애니메이션 실행
        if (_animator != null)
        {
            _animator.SetTrigger("Die");
        }

        // 애니메이션이 끝난 후 게임 오버 호출
        StartCoroutine(WaitForDeathAnimation());
    }

    // 사망 애니메이션 완료 후 게임 오버 호출
    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(2.0f); // 애니메이션 길이에 맞게 조정

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

    // 몬스터와의 충돌을 감지하는 메서드
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("EnemySword"))
        {
            Debug.Log("몬스터에게 공격당했습니다.");
            _damageCount++;
            // 데미지 애니메이션이 있다면 실행
            if (_animator != null && _damageCount == 3) // 3번맞았을때 1번 모션 재생
            {
                _animator.SetTrigger("Damaged");
                _damageCount = 0; // 데미지 카운트 초기화
            }
            TakeDamage(10); // 충돌 시 기본적으로 5의 데미지 처리
        }

        

    }

}
