using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlideDownImage : MonoBehaviour
{
    public RectTransform uiImage; // ����������Image��RectTransform
    public Vector2 initialPosition = new Vector2(0f, -361f); // �����ʒu
    public float initialSpeed = 200f; // �����x
    public float acceleration = 50f; // �����x
    public float stopPositionY = 800f; // �ړ����~�߂�Y���W
    public float decelerationDistance = 100f; // �������J�n���鋗��

    private float currentSpeed;
    private bool isDecelerating = false;

    private void Start()
    {

        // �����ʒu��ݒ�
        uiImage.anchoredPosition = initialPosition;

        // �����x��ݒ�
        currentSpeed = initialSpeed;
    }

    private void Update()
    {
        // ���݂̈ʒu���擾
        Vector3 position = uiImage.anchoredPosition;

        // ����̈ʒu�܂ł̋������v�Z
        float distanceToStop = stopPositionY - position.y;

        // �������J�n���鋗�����ɓ������猸������
        if (distanceToStop < decelerationDistance)
        {
            isDecelerating = true;
        }

        // ������Ԃ̏ꍇ
        if (isDecelerating)
        {
            // �������鏈��
            float deceleration = currentSpeed * Time.deltaTime;
            currentSpeed = Mathf.Max(0, currentSpeed - deceleration);
        }
        else
        {
            // �����x��K�p���đ��x�𑝉�
            currentSpeed += acceleration * Time.deltaTime;
        }

        // �ړ��ʂ�����
        float moveAmount = currentSpeed * Time.deltaTime;

        // �V�����ʒu���v�Z
        position.y -= moveAmount;

        // �ʒu������̈ʒu�𒴂������~
        if (position.y >= stopPositionY)
        {
            position.y = stopPositionY;
            currentSpeed = 0; // ��~���ɑ��x��0�ɂ���
            isDecelerating = false; // �����t���O�����Z�b�g
        }

        // �V�����ʒu��ݒ�
        uiImage.anchoredPosition = position;
    }
}
