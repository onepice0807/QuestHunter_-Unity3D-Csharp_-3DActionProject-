using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance; // �̱��� ���� ���

    public Text _coinText; // ���� ���� ǥ���� Text
    private int _coinCount = 0; // ���� ������ �����ϴ� ����
    public Text _monsterText; // ���� ���� ǥ���� Text
    private int _monsterCount = 0; // ���� ó������ �����ϴ� ����
    [SerializeField] private WarrningPopUp _WarrningPopUp; // �������Ḧ ���� �˾�
    [SerializeField] private Sprite[] _BgmOnOffSprite; // Bgm�� ����ϱ� ���� ����
    [SerializeField] GameObject _OptionPopUp; // �ɼ�(BGM)�� ���� �˾�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _OptionPopUp.gameObject.SetActive(false);
        SoundManager.Instance.Play_BackgroundMusic();
    }

    void Awake()
    {
        // �̱��� �������� ���� �Ŵ��� �ν��Ͻ� ����
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        _coinCount++; // ���μ� ����
        UpdateUI();

    }

    public void AddMonster()
    {
        _monsterCount++; // ���� ó���� ����
        UpdateUI();

    }

    public void UpdateUI()
    {
        if (_coinText != null)
        {
            _coinText.text = "���� �� : " + _coinCount.ToString();
        }

        if (_monsterText != null)
        {
            _monsterText.text = "���� ó���� : " + _monsterCount.ToString();
        }
    }

    public void BgmOnOff(GameObject btn)
    {
        if (SoundManager.Instance.MusicOnOff)
        {
            SoundManager.Instance.MusicOnOff = false;
            btn.GetComponent<Image>().sprite = _BgmOnOffSprite[0];
        }
        else
        {
            SoundManager.Instance.MusicOnOff = true;
            btn.GetComponent<Image>().sprite = _BgmOnOffSprite[1];
        }
    }

    public void ShowOptionPopUp()
    {
        _OptionPopUp.gameObject.SetActive(true);
        Time.timeScale = 0; // ���� �Ͻ�����
    }

    public void CloseOptionPopUp()
    {
        _OptionPopUp.gameObject.SetActive(false);
        Time.timeScale = 1; // ���� �簳
    }

    public void GameExitPopUp()
    {
        if (_WarrningPopUp != null)
        {
            _WarrningPopUp.gameObject.SetActive(true);
            _WarrningPopUp.SetDescription("������ ������ �����ðڽ��ϱ�?");
            _WarrningPopUp.SetOkButtonCallback(ExitGame); // OK ��ư�� ExitGame �޼��� ����
            _WarrningPopUp.SetCancelButtonCallback(CloseGameExitPopUp); // ��� ��ư�� �˾� �ݱ� ����
            Time.timeScale = 0; // ���� �Ͻ�����
        }
    }

    public void CloseGameExitPopUp()
    {
        _WarrningPopUp.gameObject.SetActive(false);
        Time.timeScale = 1; // ���� �簳
    }

    public void ShowExitPopUp()
    {
        _WarrningPopUp.gameObject.SetActive(true);
        _WarrningPopUp.SetDescription("������ �����ðڽ��ϱ�?");
        _WarrningPopUp._oKCallFunc = ExitGame;
        Time.timeScale = 0; // ���� �Ͻ�����
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
