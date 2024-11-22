using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위한 네임스페이스 추가
using UnityEngine.UI; // UI Text를 사용하기 위한 네임스페이스 추가

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager _Instance; // 싱글톤 패턴 사용

    // 선택 버튼 클릭 시 이동할 씬 이름
    [SerializeField] private string _sceneToLoad = "MainScene"; // 로드할 씬의 이름
    [SerializeField] private string _titleSceneToLoad = "TitleScene"; // 타이틀 화면으로 이동을 위한
    private string _selectSceneToLoad = "SelectionWindowScene"; // 보스 선택창으로 이동을 위한
    [SerializeField] private float _delayBeforeLoad = 5.0f; // 씬 전환 전 대기 시간
    [SerializeField] private GameObject _dungeonOut; // 대기 시간을 무시한 던전 나가기 UI 표시
    [SerializeField] private Text _countdownText; // 대기 시간 카운트다운 UI 텍스트
    [SerializeField] public GameObject _gameOver; // 플레이어가 죽었을때 던전 나가기 UI 표시
    [SerializeField] private Text _gameOverCountdownText; // 대기 시간 카운트다운 UI 텍스트
    [SerializeField] public GameObject _gameClear; // 몬스터가 죽었을때 클리어 UI 표시
    [SerializeField] private Text _gameClearCountdownText; // 대기 시간 카운트다운 UI 텍스트

    private bool _playerInside = false; // 플레이어가 트리거 안에 있는지 여부 확인

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dungeonOut.SetActive(false);
        _gameOver.SetActive(false);
        _gameClear.SetActive(false);
    }

    // 싱글톤 패턴을 구현하기 위해 Awake 메서드에서 인스턴스를 설정
    void Awake()
    {
        // _Instance가 null인 경우에만 현재 인스턴스를 할당
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 ScenesManager가 있으면 현재 인스턴스 파괴
        }
    }

    // 플레이어가 트리거에 들어왔을 때 호출되는 함수
    private void OnTriggerEnter(Collider collider)
    {
        // 충돌한 객체의 태그가 'Player'인 경우에만 실행
        if (collider.CompareTag("Player"))
        {
            _playerInside = true; // 플레이어가 트리거 안에 있는 것으로 표시
            StartCoroutine(LoadSceneAfterDelay()); // 씬 전환 대기 코루틴 시작
            _dungeonOut.SetActive(true); // 던전 나가기 UI 활성화
        }
    }

    // 일정 시간 대기 후 씬을 전환하는 코루틴
    IEnumerator LoadSceneAfterDelay()
    {
        float delay = _delayBeforeLoad; // 대기 시간을 로컬 변수로 설정
        while (delay > 0) // 대기 시간이 0보다 큰 동안 반복
        {
            _countdownText.text = "남은 시간: " + delay.ToString("F1") + " 초"; // UI 텍스트에 남은 시간 표시
            delay -= Time.deltaTime; // 매 프레임마다 남은 시간을 감소
            yield return null; // 다음 프레임까지 대기
        }

        // 대기 시간이 끝난 후, 플레이어가 트리거 안에 있을 경우 씬 전환
        if (_playerInside)
        {
            SceneManager.LoadScene(_sceneToLoad); // 지정된 씬으로 전환
        }
    }

    // 타이틀 씬을 로드하는 메서드
    public void LoadTitleScene()
    {
        SceneManager.LoadScene(_titleSceneToLoad);
        Cursor.visible = true; // 마우스 커서 표시
        Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
    }

    // 보스 선택씬을 로드하는 메서드
    public void LoadSelectScene()
    {
        SceneManager.LoadScene(_selectSceneToLoad);
        Cursor.visible = true; // 마우스 커서 표시
        Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
    }

    // 다음 스테이지로 이동할 때 호출되는 함수
    public void OnClickNextStage()
    {
        SceneManager.LoadScene(_sceneToLoad); // 지정된 씬으로 전환
        Time.timeScale = 1; // 게임 재개
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
    }

    public void OnStartButtionClick()
    {
        SceneManager.LoadScene(_sceneToLoad); // 시작버튼을 눌렀을때 지정된 씬으로 전환
        Time.timeScale = 1; // 게임 재개
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
    }

    // 플레이어가 사망 시 호출되는 함수
    public void OnClickGameOver()
    {
        StartCoroutine(GameOverSequence()); // 게임 오버 UI를 표시한 후 씬을 전환하는 코루틴 시작
    }

    // 게임 오버 UI 표시 후 씬을 전환하는 코루틴
    private IEnumerator GameOverSequence()
    {
        _gameOver.SetActive(true); // 게임 오버 UI 활성화
        Debug.Log("게임오버"); // 게임 오버 로그 출력

        float delay = 3.0f; // 대기 시간 설정
        while (delay > 0)
        {
            _gameOverCountdownText.text = delay.ToString("F1") + " 초 뒤에 이동합니다"; // 남은 시간 표시
            delay -= Time.deltaTime; // 대기 시간 감소
            yield return null; // 다음 프레임까지 대기
        }

        SceneManager.LoadScene(_sceneToLoad); // 지정된 씬으로 전환
    }

    public void OnClickGameClear()
    {
        StartCoroutine(GameClearSequence()); // 게임 오버 UI를 표시한 후 씬을 전환하는 코루틴 시작
    }

    // 게임 클리어 UI 표시 후 씬을 전환하는 코루틴
    private IEnumerator GameClearSequence()
    {
        _gameClear.SetActive(true); // 게임 오버 UI 활성화
        Debug.Log("게임 클리어"); // 게임 클리어 로그 출력

        float delay = 5.0f; // 대기 시간 설정
        while (delay > 0)
        {
            _gameClearCountdownText.text = delay.ToString("F1") + " 초 뒤에 홈으로 이동합니다"; // 남은 시간 표시
            delay -= Time.deltaTime; // 대기 시간 감소
            yield return null; // 다음 프레임까지 대기
        }

        SceneManager.LoadScene(_sceneToLoad); // 지정된 씬으로 전환
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
