using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceController : MonoBehaviour
{
    public AudioMixer mixer; // UnityエディタでAudioMixerをドラッグ＆ドロップで設定します
    public AudioMixerGroup mixerGroup; // AudioMixerGroupをドラッグ＆ドロップで設定します
    public AudioSource audioSource; // 音量を適用するAudioSource

    private string parameterName; // AudioMixerのパラメータ名

    private void Start()
    {
        // AudioMixerGroupからパラメータ名を決定
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
        // 定期的に音量を更新する
        UpdateAudioSourceVolume();
    }

    public void UpdateAudioSourceVolume()
    {
        if (mixer != null && !string.IsNullOrEmpty(parameterName))
        {
            float mixerVolume;
            if (mixer.GetFloat(parameterName, out mixerVolume))
            {
                // AudioMixerの音量パラメータはデシベル単位なので、リニア音量に変換する必要があります
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
