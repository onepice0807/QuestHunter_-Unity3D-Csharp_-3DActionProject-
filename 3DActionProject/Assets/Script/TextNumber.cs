using UnityEngine;

public class TextNumber : MonoBehaviour
{
    [SerializeField] private GameObject _NumberTextPrefab;

    float _lapTime = 0.5f;
    float _spendTime = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void CreateNumberText(int number)
    {
        var numberObj = Instantiate(_NumberTextPrefab);
        numberObj.transform.position = this.transform.position;
        numberObj.GetComponent<NumberText>().SetNumber(number);  
    }

    // Update is called once per frame
    void Update()
    {
        _spendTime += Time.deltaTime;
        if (_spendTime >= _lapTime)
        {
            _spendTime = 0.0f;

            CreateNumberText(200);
        }

    }
}
