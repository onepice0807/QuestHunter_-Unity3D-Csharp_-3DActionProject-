using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public int id; // 카드 ID
    public string frontImage; // 카드 앞면 이미지
    public string backImage; // 카드 뒷면 이미지
}

[System.Serializable]
public class CardDataList
{
    public List<CardData> cards; // CardData 객체들의 리스트
}
