using UnityEngine;

public class SpectrumDataVisualator : MonoBehaviour
{
    [SerializeField]
    private SpectrumDataProvider m_Provider;

    [SerializeField]
    private GameObject m_CubePrefab;

    [SerializeField]
    private float m_MaxScale;

    private void Start()
    {
        if (m_CubePrefab == null) return;

        for (int i = 0; i < SpectrumDataProvider.Samples; ++i)
        {
            var inst = Instantiate(m_CubePrefab, this.transform, false);
            inst.name = "Sample_" + i;

            Quaternion angle = Quaternion.Euler(0f, 360f / 512f * i, 0f);
            inst.transform.position = this.transform.position + angle * Vector3.forward * 100;
            inst.transform.rotation = angle;

            m_SmpleCubes[i] = inst;

            //this.transform.eulerAngles = new Vector3(0f, 360f/512f * i, 0f);
            //inst.transform.position = this.transform.forward * 100;
        }
    }

    private void Update()
    {
        if (m_Provider == null) return;

        for (int i = 0; i < SpectrumDataProvider.Samples; ++i)
        {
            if (m_SmpleCubes[i] == null) continue;

            m_SmpleCubes[i].transform.localScale = new Vector3(1, (m_Provider.SpectrumData[i] * m_MaxScale) + 1, 1);
        }
    }

    private GameObject[] m_SmpleCubes = new GameObject[SpectrumDataProvider.Samples];
}
