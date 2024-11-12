using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위한 네임스페이스 추가
using UnityEngine.UI; // UI Text를 사용하기 위한 네임스페이스 추가

public class DungeonSceneTrigger : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "DungeonScene"; // 로드할 씬의 이름
    [SerializeField] private float _delayBeforeLoad = 5.0f; // 씬 전환 전 대기 시간
    [SerializeField] private GameObject _popUp; // 플레이어의 해당맵 이동여부 판단하는 팝업창
    [SerializeField] private Text _popUpText; // 대기 시간 카운트다운 UI 텍스트

    private bool _playerInside = false; // 플레이어가 트리거 안에 있는지 여부 확인
    private bool _insideScene = false; // 해당 씬에 이동하는 여부 확인

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _popUp.SetActive(false);
    }

    // 플레이어가 트리거에 들어왔을 때 호출되는 함수
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) // 태그가 'Player'인 경우에만 실행
        {
            Time.timeScale = 0; // 게임 일시정지
            Cursor.visible = true; // 마우스 커서 표시
            Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
            _playerInside = true;
            _popUp.SetActive(true);
            _popUpText.text = "던전에 입장하시겠습니까?";
            _popUp.GetComponent<PopUp>()._callFunc = OnClickNextStage;
        }
    }
    public void OnClickNextStage()
    {
        Time.timeScale = 1; // 게임 재개
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        SceneManager.LoadScene(_sceneToLoad);
    }

    // Close버튼을 눌렀을때
    public void OnClickCloseButton()
    {
        _playerInside = false;
        _popUp.SetActive(false);
        Time.timeScale = 1; // 게임 재개
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
    }
}
