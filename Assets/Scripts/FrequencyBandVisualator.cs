using UnityEngine;

public class FrequencyBandVisualator : MonoBehaviour
{
    public const int Bands = 8;

    [SerializeField]
    private SpectrumDataProvider m_Provider;

    [SerializeField]
    private GameObject m_CubePrefab;

    [SerializeField]
    private float m_MaxScale;

    [SerializeField]
    private bool m_UseBuffer = true;

    [SerializeField]
    private float m_BaseDecrease = 0.00001f;

    [SerializeField]
    private float m_DecreaseMultiple = 1.1f;

    private void Start()
    {
        if (m_CubePrefab == null) return;

        for (int i = 0; i < Bands; ++i)
        {
            var inst = Instantiate(m_CubePrefab, this.transform, false);
            inst.name = "Band_" + i;

            inst.transform.position = this.transform.position + Vector3.right * 2 * i;
            m_SmpleCubes[i] = inst;
        }
    }

    private void Update()
    {
        if (m_Provider == null) return;

        _MakeFrequencyBands();

        for (int i = 0; i < Bands; ++i)
        {
            if (m_SmpleCubes[i] == null) continue;

            if (m_UseBuffer)
            {
                if (m_FrequencyBands[i] > m_BandBuffers[i])
                {
                    m_BandBuffers[i] = m_FrequencyBands[i];
                    m_BandDecreases[i] = m_BaseDecrease;
                }
                else if (m_FrequencyBands[i] < m_BandBuffers[i])
                {
                    m_BandBuffers[i] -= m_BandDecreases[i];
                    m_BandDecreases[i] *= m_DecreaseMultiple;

                    if (m_BandBuffers[i] < 0f)
                        m_BandBuffers[i] = 0f;
                }

                m_SmpleCubes[i].transform.localScale = new Vector3(1, (m_BandBuffers[i] * m_MaxScale) + 1, 1);
            }
            else
            {
                m_SmpleCubes[i].transform.localScale = new Vector3(1, (m_FrequencyBands[i] * m_MaxScale) + 1, 1);
            }
        }
    }

    private void _MakeFrequencyBands()
    {
        // Unity可以通過靜態成員 AudioSettings.outputSampleRate 告訴我們混音器的赫茲（Hz）音頻採樣率。
        // 這將爲我們提供Unity播放音頻的採樣率，通常爲48000或44100。
        // 我們還可以使用AudioClip.frequency獲取單個AudioClip的採樣率。
        // FFT的最大支持頻率，這將是採樣率的一半，
        // 此時，我們可以除以我們的頻譜長度，以瞭解每個bin（索引）代表的頻率。

        /*
         * EX: 
         * Audio clip of frequency is 44100,
         * so the FFT max support frequency is 22050(44100/2).
         * And the sample is 512,
         * so the hertz per sample is 43(22050/512).
         * 
         * Separate to 8 bands
         *  Sub Bass        20 -  60 Hz
         *  Bass            60 - 250 Hz
         *  Low Midrange   250 - 500 Hz
         *  Midrange       500 -  2k Hz
         *  Upper Midrange  2k -  4k Hz
         *  Persence        4k -  6k Hz
         *  Brilliance      6k - 20k Hz
         *  
         *  [0] - 2   =    86 Hz cover(    0 ~    86)
         *  [1] - 4   =   172 Hz cover(   87 ~   258)
         *  [2] - 8   =   334 Hz cover(  259 ~   602)
         *  [3] - 16  =   688 Hz cover(  603 ~  1290)
         *  [4] - 32  =  1376 Hz cover( 1291 ~  2666)
         *  [5] - 64  =  2752 Hz cover( 2667 ~  5418)
         *  [6] - 128 =  5504 Hz cover( 5419 ~ 10922)
         *  [7] - 256 = 11008 Hz cover(10923 ~ 21930)
         *  total cover the 510 samples
         */

        if (m_Provider == null) return;

        int sampleCount = 0;
        for (int i = 1; i <= Bands; ++i)
        {
            int coverSampleCount = (int)Mathf.Pow(2, i);

            // Add last 2 samples
            if (i == Bands)
                coverSampleCount += 2;

            float average = 0;
            for (int j = 0; j < coverSampleCount; ++j)
            {
                // Why does he multiply each sample by (Count+1)
                // I think this increases the values of high frequencies. If you delete this, it will be too low(as in the 3rd part of the tutorial)
                average += m_Provider.SpectrumData[sampleCount];// * (sampleCount +1);
                ++sampleCount;
            }

            average /= coverSampleCount;
            m_FrequencyBands[i - 1] = average;
        }
    }

    private GameObject[] m_SmpleCubes = new GameObject[Bands];
    private float[] m_FrequencyBands = new float[Bands];
    private float[] m_BandBuffers = new float[Bands];
    private float[] m_BandDecreases = new float[Bands];
}

