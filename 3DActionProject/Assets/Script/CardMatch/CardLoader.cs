using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class CardLoader : MonoBehaviour
{
    public TextAsset jsonFile; // JSON 파일
    public GameObject cardPrefab; // 카드 프리팹
    public Transform cardContainer; // 카드 배치 부모 객체
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
            Debug.Log("JSON 파일 로드 성공!");
            Debug.Log($"JSON 내용: {jsonFile.text}");

            // JSON 데이터를 CardDataList 객체로 변환
            CardDataList cardDataList = JsonUtility.FromJson<CardDataList>(jsonFile.text);

            foreach (var data in cardDataList.cards)
            {
                Debug.Log($"카드 데이터 로드: ID={data.id}, Front={data.frontImage}, Back={data.backImage}");

                // 카드 오브젝트 생성 및 초기화
                GameObject cardObject = Instantiate(cardPrefab, cardContainer);
                Card card = cardObject.GetComponent<Card>();

                // Resources에서 이미지 로드
                Sprite frontImage = Resources.Load<Sprite>(data.frontImage);
                Sprite backImage = Resources.Load<Sprite>(data.backImage);

                if (frontImage == null)
                {
                    Debug.LogError($"Front 이미지 로드 실패: {data.frontImage}");
                    continue;
                }

                if (backImage == null)
                {
                    Debug.LogError($"Back 이미지 로드 실패: {data.backImage}");
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
            Debug.LogError("JSON 파일이 설정되지 않았습니다!");
        }
    }

    void ShuffleCards()
    {
        cards.Sort((a, b) => Random.Range(-1, 2)); // 카드 셔플
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
