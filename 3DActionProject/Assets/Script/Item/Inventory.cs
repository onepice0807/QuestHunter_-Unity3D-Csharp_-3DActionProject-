using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory _instance;
    public List<Item> _items = new List<Item>(); // 아이템 리스트

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // 아이템 추가 메서드
    public void AddItem(Item item)
    {
        _items.Add(item);
        Debug.Log($"{item._itemName}을(를) 인벤토리에 추가했습니다.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
