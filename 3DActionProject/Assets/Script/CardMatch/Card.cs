using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image cardImage; // ī���� �̹����� ǥ��
    public CardData cardData; // �ش� ī���� ������
    private bool isFlipped = false; // ī�尡 ���������� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // ī�� �����͸� �ʱ�ȭ�ϴ� �޼���
    public void Initialize(CardData data)
    {
        cardData = data; // CardData�� ����
        cardImage.sprite = Resources.Load<Sprite>(cardData.backImage); // �ʱ� ���·� ī���� �޸� �̹����� ����
    }

    public void Flip()
    {
        if (isFlipped)
        {
            // ī�尡 �̹� ������ �ִٸ� �޸� �̹����� ǥ��
            cardImage.sprite = Resources.Load<Sprite>(cardData.backImage);
        }
        else
        {
            // ī�尡 ������ ���� �ʴٸ� �ո� �̹����� ǥ��
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
