using UnityEngine;

namespace AudioVisualizer
{
    [RequireComponent(typeof(AudioSource))]
    public class SpectrumDataProvider : MonoBehaviour
    {
        [SerializeField]
        private int m_SampleSize = 512;

        public int SampleSize => m_SampleSize;
        public float[] SpectrumData => m_SpectrumData;

        private void Awake()
        {
            m_SpectrumData = new float[m_SampleSize];
        }
        private void Update()
        {
            _GetSpectrumDataFromAudioSource();
        }

        private void _GetSpectrumDataFromAudioSource()
        {
            AudioSource.GetSpectrumData(m_SpectrumData, 0, FFTWindow.BlackmanHarris);
        }

        private float[] m_SpectrumData;

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
}

