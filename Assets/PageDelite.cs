using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを扱うために必要

public class PageController : MonoBehaviour
{
    // ページ（UIパネル）を参照するための変数
    public GameObject page;
    // ボタンを参照するための変数
    public Button hideButton;

    // Start is called before the first frame update
    void Start()
    {
        // ボタンがクリックされたときの処理を設定
        hideButton.onClick.AddListener(HidePage);
    }

    // ページを非表示にするメソッド
    void HidePage()
    {
        if (page != null)
        {
            // ページの表示状態をfalseにする
            page.SetActive(false);
        }
    }
}
