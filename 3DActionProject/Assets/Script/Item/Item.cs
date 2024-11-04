using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    public string _itemName; // 아이템 이름
    public Sprite _icon; // 아이템 이미지
    public string _description; // 상세 설명
    public int _quantity;  // 개수
 
}

public class ItemPickup : MonoBehaviour
{
    public Item _item; // 획득할 아이템

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌했을 때
        {
            // 인벤토리에 아이템 추가
            Inventory._instance.AddItem(_item);

            // 아이템 오브젝트 제거
            Destroy(gameObject);
        }
    }
}
