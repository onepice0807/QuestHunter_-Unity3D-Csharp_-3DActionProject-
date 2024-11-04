using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider _healthSlider; // 체력바 슬라이더
    public float _maxHealth = 100f; // 최대 체력
    private float _currentHealth; // 현재 체력
    public Text _healthText; // 체력게이지 텍스트

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth; // 초기 체력을 최대 체력으로 설정
        _healthSlider.value = _currentHealth;
        _healthText.text = _currentHealth.ToString();
        UpdateHealthBar();
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

}