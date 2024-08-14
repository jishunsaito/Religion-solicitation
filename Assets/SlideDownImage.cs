using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlideDownImage : MonoBehaviour
{
    public RectTransform uiImage; // 動かしたいImageのRectTransform
    public Vector2 initialPosition = new Vector2(0f, -361f); // 初期位置
    public float initialSpeed = 200f; // 初速度
    public float acceleration = 50f; // 加速度
    public float stopPositionY = 800f; // 移動を止めるY座標
    public float decelerationDistance = 100f; // 減速を開始する距離

    private float currentSpeed;
    private bool isDecelerating = false;

    private void Start()
    {

        // 初期位置を設定
        uiImage.anchoredPosition = initialPosition;

        // 初速度を設定
        currentSpeed = initialSpeed;
    }

    private void Update()
    {
        // 現在の位置を取得
        Vector3 position = uiImage.anchoredPosition;

        // 所定の位置までの距離を計算
        float distanceToStop = stopPositionY - position.y;

        // 減速を開始する距離内に入ったら減速する
        if (distanceToStop < decelerationDistance)
        {
            isDecelerating = true;
        }

        // 減速状態の場合
        if (isDecelerating)
        {
            // 減速する処理
            float deceleration = currentSpeed * Time.deltaTime;
            currentSpeed = Mathf.Max(0, currentSpeed - deceleration);
        }
        else
        {
            // 加速度を適用して速度を増加
            currentSpeed += acceleration * Time.deltaTime;
        }

        // 移動量を決定
        float moveAmount = currentSpeed * Time.deltaTime;

        // 新しい位置を計算
        position.y -= moveAmount;

        // 位置が所定の位置を超えたら停止
        if (position.y >= stopPositionY)
        {
            position.y = stopPositionY;
            currentSpeed = 0; // 停止時に速度も0にする
            isDecelerating = false; // 減速フラグもリセット
        }

        // 新しい位置を設定
        uiImage.anchoredPosition = position;
    }
}
