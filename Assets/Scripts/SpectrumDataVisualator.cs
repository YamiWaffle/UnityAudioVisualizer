using UnityEngine;

namespace AudioVisualizer
{
    public class SpectrumDataVisualator : MonoBehaviour
    {
        [SerializeField]
        private SpectrumDataProvider m_Provider;

        [SerializeField]
        private GameObject m_CubePrefab;

        [SerializeField]
        private float m_MaxScale;

        private void Awake()
        {
            if (m_Provider != null)
                m_SmpleCubes = new GameObject[m_Provider.SampleSize];
        }

        private void Start()
        {
            if (m_Provider == null || m_CubePrefab == null) return;

            for (int i = 0; i < m_Provider.SampleSize; ++i)
            {
                var inst = Instantiate(m_CubePrefab, this.transform, false);
                inst.name = "Sample_" + i;

                Quaternion angle = Quaternion.Euler(0f, 360f / 512f * i, 0f);
                inst.transform.position = this.transform.position + angle * Vector3.forward * 100;
                inst.transform.rotation = angle;

                m_SmpleCubes[i] = inst;
            }
        }

        private void Update()
        {
            if (m_Provider == null) return;

            for (int i = 0; i < m_Provider.SampleSize; ++i)
            {
                if (m_SmpleCubes[i] == null) continue;

                m_SmpleCubes[i].transform.localScale = new Vector3(1, (m_Provider.SpectrumData[i] * m_MaxScale) + 1, 1);
            }
        }

        private GameObject[] m_SmpleCubes;
    }
}
