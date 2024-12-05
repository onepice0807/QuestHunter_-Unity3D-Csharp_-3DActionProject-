using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class CardLoader : MonoBehaviour
{
    public TextAsset jsonFile; // JSON ����
    public GameObject cardPrefab; // ī�� ������
    public Transform cardContainer; // ī�� ��ġ �θ� ��ü
    private List<Card> cards = new List<Card>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCards();
        ShuffleCards();
        InstantiateCards();
    }

    void LoadCards()
    {
        if (jsonFile != null)
        {
            Debug.Log("JSON ���� �ε� ����!");
            Debug.Log($"JSON ����: {jsonFile.text}");

            // JSON �����͸� CardDataList ��ü�� ��ȯ
            CardDataList cardDataList = JsonUtility.FromJson<CardDataList>(jsonFile.text);

            foreach (var data in cardDataList.cards)
            {
                Debug.Log($"ī�� ������ �ε�: ID={data.id}, Front={data.frontImage}, Back={data.backImage}");

                // ī�� ������Ʈ ���� �� �ʱ�ȭ
                GameObject cardObject = Instantiate(cardPrefab, cardContainer);
                Card card = cardObject.GetComponent<Card>();

                // Resources���� �̹��� �ε�
                Sprite frontImage = Resources.Load<Sprite>(data.frontImage);
                Sprite backImage = Resources.Load<Sprite>(data.backImage);

                if (frontImage == null)
                {
                    Debug.LogError($"Front �̹��� �ε� ����: {data.frontImage}");
                    continue;
                }

                if (backImage == null)
                {
                    Debug.LogError($"Back �̹��� �ε� ����: {data.backImage}");
                    continue;
                }

                card.Initialize(new CardData
                {
                    id = data.id,
                    frontImage = data.frontImage,
                    backImage = data.backImage
                });

                cards.Add(card);
            }
        }
        else
        {
            Debug.LogError("JSON ������ �������� �ʾҽ��ϴ�!");
        }
    }

    void ShuffleCards()
    {
        cards.Sort((a, b) => Random.Range(-1, 2)); // ī�� ����
    }

    void InstantiateCards()
    {
        foreach (var card in cards)
        {
            card.transform.SetParent(cardContainer, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
