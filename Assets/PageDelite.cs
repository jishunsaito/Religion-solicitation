using UnityEngine;
using UnityEngine.UI; // UI�R���|�[�l���g���������߂ɕK�v

public class PageController : MonoBehaviour
{
    // �y�[�W�iUI�p�l���j���Q�Ƃ��邽�߂̕ϐ�
    public GameObject page;
    // �{�^�����Q�Ƃ��邽�߂̕ϐ�
    public Button hideButton;

    // Start is called before the first frame update
    void Start()
    {
        // �{�^�����N���b�N���ꂽ�Ƃ��̏�����ݒ�
        hideButton.onClick.AddListener(HidePage);
    }

    // �y�[�W���\���ɂ��郁�\�b�h
    void HidePage()
    {
        if (page != null)
        {
            // �y�[�W�̕\����Ԃ�false�ɂ���
            page.SetActive(false);
        }
    }
}
