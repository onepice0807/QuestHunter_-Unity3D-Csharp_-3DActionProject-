using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance; // �̱��� ���� ���

    public Text _coinText; // ���� ���� ǥ���� Text
    private int _coinCount = 0; // ���� ������ �����ϴ� ����
    public Text _monsterText; // ���� ���� ǥ���� Text
    private int _monsterCount = 0; // ���� ó������ �����ϴ� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        // �̱��� �������� ���� �Ŵ��� �ν��Ͻ� ����
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        _coinCount++; // ���μ� ����
        UpdateUI();

    }

    public void AddMonster()
    {
        _monsterCount++; // ���� ó���� ����
        UpdateUI();

    }

    public void UpdateUI()
    {
        if (_coinText != null)
        {
            _coinText.text = "���� �� : " + _coinCount.ToString();
        }

        if (_monsterText != null)
        {
            _monsterText.text = "���� ó���� : " + _monsterCount.ToString();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
