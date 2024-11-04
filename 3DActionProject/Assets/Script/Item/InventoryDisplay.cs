using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public InventoryLoader _inventoryLoader; // InventoryLoader 스크립트 참조해서 인벤토리 데이터를 가져오기

    // 아이템 정보를 표시할 UI 프리팹 (icon 및 수량)
    public GameObject _itemUIPrefab;

    // 아이템 정보를 표시할 UI 프리팹 (이름, 상세내용등) - 오른쪽 패널용
    public GameObject _ItemUIRightPrefab;

    // 아이템 UI를 배치할 부모 오브젝트, 일반적으로 스크롤뷰 안의 패널을 사용(icon 및 수량)
    public Transform _InventoryLeftPanel;

    // 아이템 정보를 표시할 오른쪽 패널 (상세 내용)
    public Transform _InventoryRightPanel;

    // 현재 오른쪽 패널에 표시되고 있는 아이템 정보 UI
    private GameObject _currentRightItemUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        DisplayInventory(); // 인벤토리 UI를 표시하는 함수 호출
        ClearRightPanel(); // 시작할 때 오른쪽 패널을 비움
    }

    // 인벤토리에 있는 아이템을 UI로 표시하는 함수
    void DisplayInventory()
    {
        foreach (var item in _inventoryLoader._inventoryItems)
        {
            // 인벤토리 패널에 아이템 UI 프리팹을 인스턴스화 (왼쪽 패널)
            GameObject newItemUI = Instantiate(_itemUIPrefab, _InventoryLeftPanel);

            // 왼쪽 패널에 텍스트 값 및 아이콘 할당
            Text countText = newItemUI.transform.Find("CountText")?.GetComponent<Text>();
            if (countText != null)
                countText.text = "개수: " + item._count;

            Transform iconTransform = newItemUI.transform.Find("ItemIcon");
            if (iconTransform != null)
            {
                Image itemIcon = iconTransform.GetComponent<Image>();
                Sprite iconSprite = Resources.Load<Sprite>(item._iconPath);
                if (iconSprite != null)
                    itemIcon.sprite = iconSprite;
            }

            // 아이템 클릭 이벤트 추가
            Button itemButton = newItemUI.GetComponent<Button>();
            if (itemButton != null)
            {
                InventoryLoader.Item currentItem = item; // 지역 변수로 저장하여 클로저 문제 해결
                itemButton.onClick.AddListener(() => DisplayItemDetails(currentItem)); // 클릭 이벤트 추가
                Debug.Log("버튼 클릭 이벤트가 추가: " + item._name); // 디버그 로그로 확인
            }

        }
    }

    // 오른쪽 패널에 아이템 상세 정보를 표시하는 함수
    public void DisplayItemDetails(InventoryLoader.Item item)
    {
        // 오른쪽 패널의 기존 내용을 지움 (항상 최신 아이템 정보만 표시)
        ClearRightPanel();

        // 새로운 프리팹을 인스턴스화하여 오른쪽 패널에 표시
        _currentRightItemUI = Instantiate(_ItemUIRightPrefab, _InventoryRightPanel);

        // 오른쪽 패널에 텍스트 값 할당
        Text nameText = _currentRightItemUI.transform.Find("NameText").GetComponent<Text>();
        Text typeText = _currentRightItemUI.transform.Find("TypeText").GetComponent<Text>();
        Text amountText = _currentRightItemUI.transform.Find("AmountText").GetComponent<Text>();
        Text durabilityText = _currentRightItemUI.transform.Find("DurabilityText").GetComponent<Text>();
        Text descriptionText = _currentRightItemUI.transform.Find("DescriptionText").GetComponent<Text>();

        if (nameText != null) nameText.text = "이름: " + item._name;
        if (typeText != null) typeText.text = "타입: " + item._type;
        if (amountText != null) amountText.text = "부여 속성: " + item._amount;
        if (durabilityText != null) durabilityText.text = "내구성: " + item._durability;
        if (descriptionText != null) descriptionText.text = "설명: " + item._description;
    }

    // 오른쪽 패널의 내용을 삭제하는 함수
    public void ClearRightPanel()
    {
        if (_currentRightItemUI != null)
        {
            Destroy(_currentRightItemUI); // 기존에 표시된 아이템 정보 UI를 삭제
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
