using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� ���ӽ����̽� �߰�
using UnityEngine.UI; // UI Text�� ����ϱ� ���� ���ӽ����̽� �߰�

public class SideScrollingSceneTrigger : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "SideScrollingScene"; // �ε��� ���� �̸�
    [SerializeField] private float _delayBeforeLoad = 5.0f; // �� ��ȯ �� ��� �ð�
    [SerializeField] private GameObject _popUp; // �÷��̾��� �ش�� �̵����� �Ǵ��ϴ� �˾�â
    [SerializeField] private Text _popUpText; // ��� �ð� ī��Ʈ�ٿ� UI �ؽ�Ʈ

    private bool _playerInside = false; // �÷��̾ Ʈ���� �ȿ� �ִ��� ���� Ȯ��
    private bool _insideScene = false; // �ش� ���� �̵��ϴ� ���� Ȯ��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _popUp.SetActive(false);
    }

    // �÷��̾ Ʈ���ſ� ������ �� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) // �±װ� 'Player'�� ��쿡�� ����
        {
            _playerInside = true;
            _popUp.SetActive(true);
            _popUpText.text = "Ⱦ��ũ�� ������ �����Ͻðڽ��ϱ�?";
        }
    }
    public void OnClickNextStage()
    {
        SceneManager.LoadScene(_sceneToLoad);

    }

    public void OnClickCloseButton()
    {
        _playerInside = false;
        _popUp.SetActive(false);
    }
}
