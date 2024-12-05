using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image cardImage; // 카드의 이미지를 표시
    public CardData cardData; // 해당 카드의 데이터
    private bool isFlipped = false; // 카드가 뒤집혔는지 여부

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // 카드 데이터를 초기화하는 메서드
    public void Initialize(CardData data)
    {
        cardData = data; // CardData를 저장
        cardImage.sprite = Resources.Load<Sprite>(cardData.backImage); // 초기 상태로 카드의 뒷면 이미지를 설정
    }

    public void Flip()
    {
        if (isFlipped)
        {
            // 카드가 이미 뒤집혀 있다면 뒷면 이미지를 표시
            cardImage.sprite = Resources.Load<Sprite>(cardData.backImage);
        }
        else
        {
            // 카드가 뒤집혀 있지 않다면 앞면 이미지를 표시
            cardImage.sprite = Resources.Load<Sprite>(cardData.frontImage);
        }

        isFlipped = !isFlipped;
    }

    public bool IsFlipped()
    {
        return isFlipped;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
