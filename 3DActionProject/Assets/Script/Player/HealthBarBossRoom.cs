using UnityEngine;
using UnityEngine.UI;

public class HealthBarBossRoom : MonoBehaviour
{
    public Slider _healthSlider; // 플레이어 체력바 슬라이더
    public float _maxHealth = 100f; // 플레이어 최대 체력
    private float _currentHealth; // 플레이어 현재 체력
    public Text _healthText; // 플레이어 체력게이지 텍스트

    public Slider _bossHealthSlider; // 보스 체력바 슬라이더
    public float _bossMaxHealth = 100f; // 보스 최대 체력
    private float _bossCurrentHealth; // 보스 현재 체력
    public Text _bossHealthText; // 보스 체력게이지 텍스트

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 플레이어 체력상태
        _currentHealth = _maxHealth; // 초기 체력을 최대 체력으로 설정
        _healthSlider.value = _currentHealth;
        _healthText.text = _currentHealth.ToString();
        UpdateHealthBar();

        // 보스 체력상태
        _bossCurrentHealth = _bossMaxHealth; // 초기 체력을 최대 체력으로 설정
        _bossHealthSlider.value = _bossCurrentHealth;
        _bossHealthText.text = _bossCurrentHealth.ToString();
        UpdateBossHealthBar();
    }

    // 데미지를 입었을 때 체력 감소
    public void TakeDamage(float amount)
    {
        _currentHealth = amount;
        if (_currentHealth < 0)
        {
            _currentHealth = 0; // 체력이 0 이하로 떨어지지 않도록
        }

        UpdateHealthBar();
    }

    public void BossTakeDamage(float amount)
    {
        _bossCurrentHealth = amount;
        if (_bossCurrentHealth < 0)
        {
            _bossCurrentHealth = 0; // 체력이 0 이하로 떨어지지 않도록
        }

        UpdateBossHealthBar();
    }

    // 체력을 회복할 때 사용
    public void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth; // 체력이 최대 체력 이상으로 올라가지 않도록
        UpdateHealthBar();
    }

    // 체력바 업데이트
    private void UpdateHealthBar()
    {
        _healthSlider.value = _currentHealth; // 슬라이더 값을 현재 체력 비율에 맞게 설정
        _healthText.text = _currentHealth.ToString();
    }

    private void UpdateBossHealthBar()
    {
        _bossHealthSlider.value = _bossCurrentHealth; // 슬라이더 값을 현재 체력 비율에 맞게 설정
        _bossHealthText.text = _bossCurrentHealth.ToString();
    }
}
