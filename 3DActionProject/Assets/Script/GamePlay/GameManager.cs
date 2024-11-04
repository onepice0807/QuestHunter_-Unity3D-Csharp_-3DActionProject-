using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance; // 싱글톤 패턴 사용

    public Text _coinText; // 코인 수를 표시할 Text
    private int _coinCount = 0; // 코인 개수를 저장하는 변수
    public Text _monsterText; // 몬스터 수를 표시할 Text
    private int _monsterCount = 0; // 몬스터 처리수를 저장하는 변수

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        // 싱글톤 패턴으로 게임 매니저 인스턴스 관리
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
        _coinCount++; // 코인수 증가
        UpdateUI();

    }

    public void AddMonster()
    {
        _monsterCount++; // 몬스터 처리수 증가
        UpdateUI();

    }

    public void UpdateUI()
    {
        if (_coinText != null)
        {
            _coinText.text = "코인 수 : " + _coinCount.ToString();
        }

        if (_monsterText != null)
        {
            _monsterText.text = "몬스터 처리수 : " + _monsterCount.ToString();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
