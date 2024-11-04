using UnityEngine;
using TMPro;

public class NumberText : MonoBehaviour
{   
    [SerializeField] private TextMeshPro _NumberText;
    [SerializeField] private Vector3 _StartScale;
    [SerializeField] private Vector3 _EndScale;
    [SerializeField] private float _Ypos;

    private Camera _MainCamera;
    private Vector3 _movePos = new Vector3(0.0f, 0.0f, 0.0f); 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.localScale = _StartScale;
        StartEffect();
    }

    public void SetNumber(int number)
    {
        _NumberText.text = number.ToString();
    }

    void StartEffect()
    {
        _movePos = this.transform.localPosition;
        _movePos.y += _Ypos;
        LeanTween.scale(this.gameObject, _EndScale, 0.8f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(this.gameObject, _movePos, 0.8f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(EffectEnd);
    }

    void EffectEnd()
    {
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        this.transform.LookAt(Camera.main.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
