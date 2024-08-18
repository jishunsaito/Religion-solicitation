using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//スクリプトでUI(テキストなど)扱うときはこれ必須！！

public class test : MonoBehaviour
{
    public RectTransform Glove;//RectTransform型の変数aを宣言　作成したテキストオブジェクトをアタッチしておく

    //スタート関数
    void Start()
    {
    }

    //アップデート関数
    void Update()
    {
        Glove.position += new Vector3(0.1f, 0, 0);//毎フレームx座標を0.1ずつプラス        
    }
}
