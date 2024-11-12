using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Player가 트리거를 밟으면 _regenPoint 위치로 이동
        if (collider.CompareTag("Player"))
        {
            // 싱글톤을 통해 ScenesManager의 OnClickGameClear 호출
            if (ScenesManager._Instance != null)
            {
                ScenesManager._Instance.OnClickGameClear();
            }
            else
            {
                Debug.LogError("ScenesManager 인스턴스를 찾을 수 없습니다.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
