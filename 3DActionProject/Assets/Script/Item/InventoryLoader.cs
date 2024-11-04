using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryLoader : MonoBehaviour
{
    // 아이템 정보를 담을 클래스를 정의하는부분. [System.Serializable]는 Unity Inspector에 노출하기 위해 사용
    [System.Serializable]
    public class Item
    {
        public int _id; // 인벤토리 ID번호
        public string _name; // 이름
        public string _type; // 타입
        public int _count; // 개수
        public int _amount; // 부여값 (ex: 체력회복등)
        public int _durability; // 무기 내구성
        public string _description; // 인벤토리 아이템 설명
        public string _iconPath; // 아이콘 이미지 파일 경로
    }

    // 아이템 리스트. 모든 인벤토리 아이템을 저장하는 리스트
    public List<Item> _inventoryItems = new List<Item>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadItem();
    }

    // 아이템을 불러오는 변수
    void LoadItem()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("inventoryItems");
        // CSV 파일의 텍스트를 읽기 위해 StringReader 사용
        StringReader stringReader = new StringReader(csvFile.text);

        bool firstLine = true; // 첫 번째 라인은 헤더이므로 생략하는 변수
        while (true)
        {
            string line = stringReader.ReadLine(); // 파일의 한 줄을 읽음

            if (line == null) break; // 읽어올 내용이 없으면 반환

            if (firstLine) // 첫 번째 줄 즉 헤더 부분은 생략
            {
                firstLine = false;
                continue; // 다음 줄로 넘어가기
            }

            // 각 라인의 데이터값을 ,(쉼표)로 구분해서 배열에 저장
            string[] values = line.Split(':');

            // 아이템 객체 생성 및 배열 데이터를 사용하여 초기화
            Item newItem = new Item
            {
                _id = int.Parse(values[0]),
                _name = values[1],
                _type = values[2],
                _count = int.Parse(values[3]),
                _amount = int.Parse(values[4]),
                _durability = int.Parse(values[5]),
                _description = values[6],
                _iconPath = values[7] // CSV에서 아이콘 경로를 읽어와 할당
            };
            // 새 아이템을 인벤토리 리스트에 추가
            _inventoryItems.Add(newItem);
        }

    }

    // 인벤토리에 새로운 아이템을 추가하는 함수
    public void AddItem(int id, string name, string type, int count, int amount, int durability, string description, string iconPath)
    {
        Item newItem = new Item
        {
            _id = id,
            _name = name,
            _type = type,
            _count = count,
            _amount = amount,
            _durability = durability,
            _description = description,
            _iconPath = iconPath
        };

        _inventoryItems.Add(newItem); // 인벤토리 리스트에 아이템 추가
        //UpdateCSV(); // CSV 파일 업데이트
    }

    // 인벤토리에서 아이템을 삭제하는 함수
    public void RemoveItem(int id)
    {
        Item itemToRemove = _inventoryItems.Find(item => item._id == id);
        if (itemToRemove != null)
        {
            _inventoryItems.Remove(itemToRemove); // 아이템 리스트에서 삭제
            UpdateCSV(); // CSV 파일 업데이트
        }
    }

    // CSV 파일을 업데이트하는 함수
    void UpdateCSV()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/inventoryItems.csv");

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("_id,_name,_type,_count,_amount,_durability,_description,_iconPath");

            foreach (var item in _inventoryItems)
            {
                writer.WriteLine($"{item._id},{item._name},{item._type},{item._count},{item._amount},{item._durability},{item._description},{item._iconPath}");
            }
        }

        Debug.Log("Inventory CSV Updated!"); // 디버그 메시지
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
