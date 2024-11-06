using UnityEngine;

public class BossRoomPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _PlayerTarget = null; // 플레이어를 타겟으로 가져오기 위한 변수

    private float _distance = 3.5f;  // TPS 시점에서의 거리 (플레이어와 카메라 간의 거리)
    private float _height = 1.5f;  // 카메라의 높이 (플레이어의 어깨 높이 정도)
    private float _rotationSpeed = 5f; // 카메라 회전 속도

    private float _currentX = 0f;  // 마우스 X축 값 저장용
    private float _currentY = 0f;  // 마우스 Y축 값 저장용
    private float _minYAngle = -20f; // 카메라의 최소 높이 각도 (하향 제한)
    private float _maxYAngle = 20f;  // 카메라의 최대 높이 각도 (상향 제한)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 마우스 커서를 화면에 고정시켜서 시야 조작을 쉽게 합니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (_PlayerTarget == null) return; // 플레이어 타겟이 null이라면 반환

        // 마우스 입력을 받아 카메라 회전을 조정
        _currentX += Input.GetAxis("Mouse X") * _rotationSpeed;
        _currentY -= Input.GetAxis("Mouse Y") * _rotationSpeed;

        // Y축 회전 제한
        _currentY = Mathf.Clamp(_currentY, _minYAngle, _maxYAngle);

        // 카메라의 회전 각도를 설정 (TPS 시점)
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0f);

        // 플레이어의 위치를 기준으로 카메라 위치 계산
        Vector3 position = _PlayerTarget.position
                         - rotation * Vector3.forward * _distance // 플레이어 뒤쪽으로 일정 거리만큼
                         + Vector3.up * _height; // 높이만큼 위로

        // 카메라 위치 및 회전 설정
        transform.position = position;
        transform.LookAt(_PlayerTarget.position + Vector3.up * _height / 2); // 캐릭터 중심을 바라보도록 설정
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라 회전을 마우스 움직임으로 제어
        _currentX += Input.GetAxis("Mouse X") * _rotationSpeed;
        _currentY -= Input.GetAxis("Mouse Y") * _rotationSpeed;
    }
}
