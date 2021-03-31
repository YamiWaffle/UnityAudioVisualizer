using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpectrumDataProvider : MonoBehaviour
{
    public const int Samples = 512;

    public float[] SpectrumData => m_SpectrumData;

    private void Update()
    {
        _GetSpectrumDataFromAudioSource();
    }

    private void _GetSpectrumDataFromAudioSource()
    {
        AudioSource?.GetSpectrumData(m_SpectrumData, 0, FFTWindow.BlackmanHarris);
    }

    private float[] m_SpectrumData = new float[Samples];

    private AudioSource m_AudioSource;
    private AudioSource AudioSource
    {
        get
        {
            if (m_AudioSource == null)
                m_AudioSource = GetComponent<AudioSource>();

            return m_AudioSource;
        }
    }
}
