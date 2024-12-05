using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public int id; // ī�� ID
    public string frontImage; // ī�� �ո� �̹���
    public string backImage; // ī�� �޸� �̹���
}

[System.Serializable]
public class CardDataList
{
    public List<CardData> cards; // CardData ��ü���� ����Ʈ
}
