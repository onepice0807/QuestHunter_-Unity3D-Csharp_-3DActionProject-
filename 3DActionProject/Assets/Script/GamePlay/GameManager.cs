using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance; // 싱글톤 패턴 사용

    public Text _coinText; // 코인 수를 표시할 Text
    private int _coinCount = 0; // 코인 개수를 저장하는 변수
    public Text _monsterText; // 몬스터 수를 표시할 Text
    private int _monsterCount = 0; // 몬스터 처리수를 저장하는 변수
    [SerializeField] private WarrningPopUp _WarrningPopUp; // 게임종료를 위한 팝업
    [SerializeField] private Sprite[] _BgmOnOffSprite; // Bgm을 사용하기 위한 변수
    [SerializeField] GameObject _OptionPopUp; // 옵션(BGM)을 위한 팝업

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _OptionPopUp.gameObject.SetActive(false);
        SoundManager.Instance.Play_BackgroundMusic();
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

    public void BgmOnOff(GameObject btn)
    {
        if (SoundManager.Instance.MusicOnOff)
        {
            SoundManager.Instance.MusicOnOff = false;
            btn.GetComponent<Image>().sprite = _BgmOnOffSprite[0];
        }
        else
        {
            SoundManager.Instance.MusicOnOff = true;
            btn.GetComponent<Image>().sprite = _BgmOnOffSprite[1];
        }
    }

    public void ShowOptionPopUp()
    {
        _OptionPopUp.gameObject.SetActive(true);
        Time.timeScale = 0; // 게임 일시정지
    }

    public void CloseOptionPopUp()
    {
        _OptionPopUp.gameObject.SetActive(false);
        Time.timeScale = 1; // 게임 재개
    }

    public void GameExitPopUp()
    {
        if (_WarrningPopUp != null)
        {
            _WarrningPopUp.gameObject.SetActive(true);
            _WarrningPopUp.SetDescription("정말로 게임을 나가시겠습니까?");
            _WarrningPopUp.SetOkButtonCallback(ExitGame); // OK 버튼에 ExitGame 메서드 연결
            _WarrningPopUp.SetCancelButtonCallback(CloseGameExitPopUp); // 취소 버튼에 팝업 닫기 연결
            Time.timeScale = 0; // 게임 일시정지
        }
    }

    public void CloseGameExitPopUp()
    {
        _WarrningPopUp.gameObject.SetActive(false);
        Time.timeScale = 1; // 게임 재개
    }

    public void ShowExitPopUp()
    {
        _WarrningPopUp.gameObject.SetActive(true);
        _WarrningPopUp.SetDescription("정말로 나가시겠습니까?");
        _WarrningPopUp._oKCallFunc = ExitGame;
        Time.timeScale = 0; // 게임 일시정지
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
