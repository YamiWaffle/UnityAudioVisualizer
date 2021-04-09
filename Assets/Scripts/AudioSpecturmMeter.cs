using UnityEngine;

public class AudioSpecturmMeter : MonoBehaviour
{
    [Range(10, 500)] 
    public float value = 100.0f;

    // Start is called before the first frame update
    private void Start()
    {
        spectrumDatas = new float[windowSize];
        spectrumDatasBuffer = new float[windowSize];

        specturnBandTexture = new Texture2D(windowSize, 1, TextureFormat.R8, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        Shader.SetGlobalTexture("_SpecturmDataTex", specturnBandTexture);
    }

    private void OnDestroy()
    {
        if (specturnBandTexture != null)
            Destroy(specturnBandTexture);
        specturnBandTexture = null;
    }

    // Update is called once per frame
    private void Update()
    {
        AudioListener.GetSpectrumData(spectrumDatas, 0, FFTWindow.Rectangular);

        for (int i = 0; i < windowSize; ++i)
        {
            float a = Mathf.Atan(i * 0.05f);

            if (spectrumDatas[i] > spectrumDatasBuffer[i])
            {
                spectrumDatasBuffer[i] = spectrumDatas[i];
            }
            else
            {
                spectrumDatasBuffer[i] *= 0.98f;
                if (spectrumDatasBuffer[i] < 0)
                    spectrumDatasBuffer[i] = 0;
            }

            specturnData.r = spectrumDatasBuffer[i] * value * a;
            specturnBandTexture.SetPixel(i, 0, specturnData);
        }
        specturnBandTexture.Apply();

        //for (int x = 0; x < 4; ++x)
        //{
        //    float f = 0f;
        //    for (int y = 0; y < 32; ++y)
        //        f += spectrumDatasBuffer[x * 32 + y] * value;

        //    specturnData.r = f / 32f;
        //    specturnBandTexture.SetPixel(x, 0, specturnData);
        //}
        //specturnBandTexture.Apply();

       
    }

    private int windowSize = 64;

    private Color specturnData;
    private Texture2D specturnBandTexture;
    private float[] spectrumDatas;
    private float[] spectrumDatasBuffer;
}
