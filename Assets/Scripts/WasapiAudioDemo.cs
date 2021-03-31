using Assets.WasapiAudio.Scripts.Unity;
using Assets.WasapiAudio.Scripts.Unity.Transformers;
using UnityEngine;

namespace AudioVisualizer
{
    public class WasapiAudioDemo : AudioVisualizationEffect
    {
        [SerializeField]
        private GameObject m_CubePrefab;

        [SerializeField]
        private float m_MaxScale;

        public override void Awake()
        {
            base.Awake();

            m_SmpleCubes = new GameObject[SpectrumSize];
        }

        private void Start()
        {
            Transformers.Clear();
            Transformers.Add(new SmoothSpectrumTransformer());

            for (int i = 0; i < SpectrumSize; ++i)
            {
                var inst = Instantiate(m_CubePrefab, this.transform, false);
                inst.name = "Sample_" + i;

                Quaternion angle = Quaternion.Euler(0f, 360f / SpectrumSize * i, 0f);
                inst.transform.position = this.transform.position + angle * Vector3.forward * 100;
                inst.transform.rotation = angle;

                m_SmpleCubes[i] = inst;
            }
        }

        private void Update()
        {
            var spectrumData = GetSpectrumData();

            for (int i = 0; i < SpectrumSize; ++i)
            {
                if (m_SmpleCubes[i] == null) continue;

                m_SmpleCubes[i].transform.localScale = new Vector3(1, (spectrumData[i] * m_MaxScale) + 1, 1);
            }
        }

        private GameObject[] m_SmpleCubes;
    }
}

