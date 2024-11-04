using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public GameObject sword; // 검 오브젝트를 직접 참조하는 값
    public float moveSpeed = 0.0f; // 이동하는 속도
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == sword)
        {
            Debug.Log("Hit");

            // Vector3 monsterPosition = transform.position + Vector3.up * 3.0f + Vector3.left * 3.0f;

            Vector3 monsterPosition = (transform.position + Vector3.up * 3.0f + Vector3.left * 3.0f - transform.position).normalized;
            transform.position += monsterPosition * moveSpeed * Time.deltaTime;

            gameObject.transform.position = monsterPosition;

            // 몬스터를 일정 시간 후에 제거
            Destroy(gameObject, 2f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
