using UnityEngine;

public class Coin : MonoBehaviour
{
    public float _rotationSpeed = 500.0f; // 코인의 회전 속도

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // 플레이어와 충돌 시 코인을 사라지게 하고 코인 수 증가

    // Update is called once per frame
    void Update()
    {
        // 코인을 일정 속도로 회전시킨다
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
}
