using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance; // 싱글톤 패턴 사용

    public Text _coinText; // 코인 수를 표시할 Text
    private int _coinCount = 0; // 코인 개수를 저장하는 변수
    public Text _monsterText; // 몬스터 수를 표시할 Text
    private int _monsterCount = 0; // 몬스터 처리수를 저장하는 변수
    [SerializeField] private WarrningPopUp _WarrningPopUp; // 게임종료를 위한 팝업
    [SerializeField] GameObject _OptionPopUp; // 옵션(BGM)을 위한 팝업
    [SerializeField] private Sprite[] _BgmOnOffSprite; // Bgm 이미지를 위한 Splite
    [SerializeField] private Sprite[] _SoundEffectOnOffSprite; // 효과음의 이미지를 위한 Splite

    [SerializeField] private GameObject _sceneAButtonCheck; // 보스맵 선택창에서 왼쪽 버튼 클릭 시 활성화
    [SerializeField] private GameObject _sceneBButtonCheck; // 보스맵 선택창에서 오른쪽 버튼 클릭 시 버튼 텍스트
    [SerializeField] private Text _sceneAButtonText; // 보스맵 선택창에서 왼쪽 버튼 클릭 시 버튼 텍스트
    [SerializeField] private Text _sceneBButtonText; // 보스맵 선택창에서 오른쪽 버튼 클릭 시 버튼 텍스트
    private string _selectedScene = "";  // 현재 선택된 씬 이름 (초기값은 선택이 되기 전이기 때문에 빈 문자열)


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

    // 왼쪽 버튼을 클릭했을 때 호출되는 메서드
    public void SelectLeftBoss()
    {
        _selectedScene = "BossScene"; // 왼쪽 버튼에 할당된 씬 이름 저장
        _sceneAButtonCheck.SetActive(true); // 선택되었다는 체크이미지창 활성화
        _sceneBButtonCheck.SetActive(false);
        _sceneAButtonText.text = "선택됨";
        _sceneBButtonText.text = "선택"; 
        Debug.Log("왼쪽 씬 선택됨: " + _selectedScene); // 디버그 로그로 선택 확인
    }

    // 오른쪽 버튼을 클릭했을 때 호출되는 메서드
    public void SelectRightBoss()
    {
        _selectedScene = "Boss2Scene"; // 오른쪽 버튼에 할당된 씬 이름 저장
        _sceneAButtonText.text = "선택";
        _sceneBButtonText.text = "선택됨";
        _sceneBButtonCheck.SetActive(true); // 선택되었다는 체크이미지창 활성화
        _sceneAButtonCheck.SetActive(false);
        Debug.Log("오른쪽 씬 선택됨: " + _selectedScene); // 디버그 로그로 선택 확인

    }

    public void BossRoomStartButtonClick()
    {
        if (!string.IsNullOrEmpty(_selectedScene))
        {
            ScenesManager._Instance.LoadScene(_selectedScene);
            Debug.Log("시작 버튼 클릭, " + _selectedScene + "로 이동");
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Debug.LogWarning("씬이 선택되지 않았습니다!");
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

    public void SoundEffectOnOff(GameObject btn)
    {
        if (SoundManager.Instance.EffectOnOff)
        {
            SoundManager.Instance.EffectOnOff = false;
            btn.GetComponent<Image>().sprite = _SoundEffectOnOffSprite[0];
        }
        else
        {
            SoundManager.Instance.EffectOnOff = true;
            btn.GetComponent<Image>().sprite = _SoundEffectOnOffSprite[1];
        }
    }


    public void ShowOptionPopUp()
    {
        _OptionPopUp.gameObject.SetActive(true);
        Time.timeScale = 0; // 게임 일시정지
        Cursor.visible = true; // 마우스 커서 표시
        Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
    }

    public void CloseOptionPopUp()
    {
        _OptionPopUp.gameObject.SetActive(false);
        Time.timeScale = 1; // 게임 재개
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
    }

    public void CloseTitleOption()
    {
        _OptionPopUp.gameObject.SetActive(false);
    }

    public void GameExitPopUp()
    {
        if (_WarrningPopUp != null)
        {
            _WarrningPopUp.gameObject.SetActive(true);
            _WarrningPopUp.SetDescription("정말로 게임을 나가시겠습니까?");
            Time.timeScale = 0; // 게임 일시정지
            Cursor.visible = true; // 마우스 커서 표시
            Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
            _WarrningPopUp.SetOkButtonCallback(ExitGame); // OK 버튼에 ExitGame 메서드 연결
            _WarrningPopUp.SetCancelButtonCallback(CloseGameExitPopUp); // 취소 버튼에 팝업 닫기 연결
        }
    }

    public void CloseGameExitPopUp()
    {
        _WarrningPopUp.gameObject.SetActive(false);
        Time.timeScale = 1; // 게임 재개
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
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
