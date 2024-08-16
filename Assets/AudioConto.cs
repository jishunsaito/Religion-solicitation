using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceController : MonoBehaviour
{
    public AudioMixer mixer; // Unity�G�f�B�^��AudioMixer���h���b�O���h���b�v�Őݒ肵�܂�
    public AudioMixerGroup mixerGroup; // AudioMixerGroup���h���b�O���h���b�v�Őݒ肵�܂�
    public AudioSource audioSource; // ���ʂ�K�p����AudioSource

    private string parameterName; // AudioMixer�̃p�����[�^��

    private void Start()
    {
        // AudioMixerGroup����p�����[�^��������
        if (mixerGroup != null)
        {
            parameterName = mixerGroup.name;
            UpdateAudioSourceVolume();
        }
        else
        {
            Debug.LogWarning("AudioMixerGroup is not assigned.");
        }
    }

    private void Update()
    {
        // ����I�ɉ��ʂ��X�V����
        UpdateAudioSourceVolume();
    }

    public void UpdateAudioSourceVolume()
    {
        if (mixer != null && !string.IsNullOrEmpty(parameterName))
        {
            float mixerVolume;
            if (mixer.GetFloat(parameterName, out mixerVolume))
            {
                // AudioMixer�̉��ʃp�����[�^�̓f�V�x���P�ʂȂ̂ŁA���j�A���ʂɕϊ�����K�v������܂�
                float linearVolume = Mathf.Pow(10, mixerVolume / 20);
                audioSource.volume = linearVolume;
            }
            else
            {
                Debug.LogWarning($"Parameter '{parameterName}' not found in AudioMixer.");
            }
        }
        else
        {
            Debug.LogWarning("Mixer or parameter name is not set.");
        }
    }
}
