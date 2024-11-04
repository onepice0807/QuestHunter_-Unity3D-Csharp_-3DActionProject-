using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterTrigger : MonoBehaviour
{
    [SerializeField] private Transform[] _RegenTrigger;
    [SerializeField] private MonsterController _TriggerMonster;
    [SerializeField] private GameObject _MoveTrigger; // MoveTrigger를 활성화하기 위한 변수
    private int _maxMonsterCount = 5; // 최대 소환할 몬스터 수
    private int _currentMonsterCount = 0; // 현재 소환된 몬스터 수
    private List<GameObject> _triggerMonstersList = new List<GameObject>(); // 소환된 몬스터를 추적하는 리스트
    [SerializeField] private List<Transform> _defaultMonstersList = new List<Transform>(); // 기존의 DefaltMonster 추적 리스트
    [SerializeField] private Text _clearDungeonText; // 각 던전을 클리어 할때다마 표시해주는 Text
    [SerializeField] private GameObject _clearDungeonUI; // 던전을 클리어Text를 활성화하기 위한 변수

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // MoveTrigger는 처음에 비활성화
        if (_MoveTrigger != null)
        {
            _MoveTrigger.SetActive(false);
            _clearDungeonUI.SetActive(false);
        }

        // 기본 몬스터들이 존재하면, 그 몬스터가 죽었을 때 리스트에서 제거하는 로직 연결
        foreach (var monster in _defaultMonstersList)
        {
            var monsterGameObject = monster.gameObject;
            var monsterController = monsterGameObject.GetComponent<MonsterController>();

            if (monsterController != null)
            {
                monsterController.OnMonsterDeath += DefaultMonsterDied;
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    { 
        // 태그가 'Player' 상태에서 소환된 몬스터가 6마리 미만일 때만 실행
        if (collider.CompareTag("Player") && _currentMonsterCount < _maxMonsterCount)
        {
            StartCoroutine(CreateMonster(collider));
        }
    }

    IEnumerator CreateMonster(Collider collider)
    {

        foreach (var trigger in _RegenTrigger)
        {
            if (_currentMonsterCount >= _maxMonsterCount)
                yield break; // 최대 소환 수에 도달하면 중단

            var enemy = Instantiate(_TriggerMonster.gameObject, trigger.position, trigger.rotation);
            enemy.GetComponent<MonsterController>().setTarget(collider.gameObject.transform);
            yield return new WaitForSecondsRealtime(0.5f);

            // 소환된 몬스터를 리스트에 추가하고 카운터 증가
            _triggerMonstersList.Add(enemy);
            _currentMonsterCount++;

            // 몬스터가 죽었을 때 리스트에서 제거하는 로직 연결
            enemy.GetComponent<MonsterController>().OnMonsterDeath += MonsterDied;

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    // 소환된 몬스터가 죽었을 때 호출되는 함수
    private void MonsterDied(GameObject monster)
    {
        _triggerMonstersList.Remove(monster); // 소환된 몬스터 리스트에서 제거
        CheckIfAllMonstersAreDead(); // 모든 몬스터가 사망했는지 확인
    }

    // 기본 몬스터가 죽었을 때 호출되는 함수
    private void DefaultMonsterDied(GameObject monster)
    {
        _defaultMonstersList.Remove(monster.transform); // 기본 몬스터 리스트에서 제거
        CheckIfAllMonstersAreDead(); // 모든 몬스터가 사망했는지 확인
    }

    // 모든 몬스터(소환된 몬스터와 기본 몬스터)가 사망했는지 확인
    private void CheckIfAllMonstersAreDead()
    {
        if (_triggerMonstersList.Count == 0 && _defaultMonstersList.Count == 0)
        {
            // 모든 몬스터가 사라지면 3초 뒤 MoveTrigger 활성화
            if (_MoveTrigger != null)
            {
                _clearDungeonUI.SetActive(true);
                Invoke("ActivateMoveTrigger", 3f); // 3초 후에 ActivateMoveTrigger 호출
            }
        }
    }

    // MoveTrigger 활성화할때 클리어 Text는 비활성화
    private void ActivateMoveTrigger()
    {
        _clearDungeonUI.SetActive(false);
        _MoveTrigger.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {

    }
}


