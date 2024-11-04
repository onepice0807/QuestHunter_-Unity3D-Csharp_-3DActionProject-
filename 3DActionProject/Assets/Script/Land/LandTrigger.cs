using UnityEngine;

public class LandTrigger : MonoBehaviour
{
    public PlayerCamera _cameraController;  // 카메라 제어 스크립트
    public PlayerController _playerController; // player 제어 스크립트
    public MonsterController _monsterController; // 적 제어 스크립트

    [SerializeField] private bool _isLand2 = false;  // 현재 구역이 Land2인지 여부


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Land2에 들어가면 카메라 모드를 횡스크롤 뷰로 전환
            if (_isLand2)
            {
                _cameraController._isSideScrolling = true;
                _playerController._isSideScrolling = true;
                _monsterController._isSideScrolling = true;

            }
            // Land에 돌아오면 다시 쿼터뷰로 전환
            else
            {
                _cameraController._isSideScrolling = false;
                _playerController._isSideScrolling = false;
                _monsterController._isSideScrolling = false;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
