using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
    public Transform _regenPoint; // Regen1 위치를 참조할 변수

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Player가 트리거를 밟으면 _regenPoint 위치로 이동
        if (collider.CompareTag("Player"))
        {
            collider.transform.position = _regenPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
