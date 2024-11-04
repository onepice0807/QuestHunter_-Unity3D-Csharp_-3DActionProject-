using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _PlayerTarget = null; // 플레이어를 타겟으로 가져오기 위한 변수

    private float _distance = 6f;  // 플레이어와의 거리 (Z축 기준)
    private float _height = 3f;    // 카메라의 높이 (Y축 기준)
    private float _angle = 35f;    // 카메라의 내려다보는 각도

    public bool _isSideScrolling = false;  // 카메라가 횡스크롤 모드인지 여부를 나타내는 변수

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void LateUpdate()
    {
        if (_PlayerTarget == null) return; // 플레이어 타겟이 null이라면 반환

        if (_isSideScrolling)
        {
            // 횡스크롤 뷰 설정
            // 카메라의 회전 각도를 설정 (X축을 기준으로 0만큼 아래를 바라보도록)
            Quaternion rotation = Quaternion.Euler(0f, 0f, 0f); // 횡스크롤 시점

            // 플레이어의 위치를 기준으로 카메라 위치 계산
            Vector3 position = _PlayerTarget.position
                             - rotation * Vector3.left * _distance // 거리만큼 뒤로
                             + Vector3.up * 1; // 높이만큼 위로

            // 카메라의 위치와 회전 설정
            transform.position = position;
            transform.LookAt(_PlayerTarget.position + Vector3.up * _height / 2); // 캐릭터 중심을 바라보도록 설정
        }
        else
        {
            // 쿼터뷰 설정

            // 카메라의 회전 각도를 설정 (X축을 기준으로 _angle만큼 아래를 바라보도록)
            Quaternion rotation = Quaternion.Euler(_angle, 0f, 0f); // 35도 각도로 내려다보는 시점

            // 플레이어의 위치를 기준으로 카메라 위치 계산
            Vector3 position = _PlayerTarget.position
                             - rotation * Vector3.forward * _distance // 거리만큼 뒤로
                             + Vector3.up * _height; // 높이만큼 위로

            // 카메라의 위치와 회전 설정
            transform.position = position;
            transform.LookAt(_PlayerTarget.position + Vector3.up * _height / 2); // 캐릭터 중심을 바라보도록 설정
        }

        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
