using UnityEngine;
using System;
using UnityEngine.UI;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class Bloom : MonoBehaviour
{
    // Start is called before the first frame update
    const int BoxDownPrefilterPass = 0;
    const int BoxDownPass = 1;
    const int BoxUpPass = 2;
    const int ApplyBloomPass = 3;
    const int DebugBloomPass = 4;


    public Shader bloomShader;
    [SerializeField] private Toggle _button;


    [Range(1, 16)] [SerializeField] private int iterations = 4;

    [Range(0, 10)] [SerializeField] private float threshold = 1;

    [Range(0, 10)] [SerializeField] private float intensity = 1;


    private RenderTexture[] textures = new RenderTexture[16];

    private Material bloom;
    private static readonly int SourceTex = Shader.PropertyToID("_SourceTex");
    private static readonly int Intensity = Shader.PropertyToID("_Intensity");
    private static readonly int Threshold = Shader.PropertyToID("_Threshold");

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (bloom == null)
        {
            bloom = new Material(bloomShader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
        }


        int width = source.width / 2;
        int height = source.height / 2;

        RenderTextureFormat format = source.format;
        RenderTexture currentDestination = textures[0] = RenderTexture.GetTemporary(width, height, 0, format);
        RenderTexture currentSource = currentDestination;
        if (_button.isOn)
        {
            bloom.SetFloat(Threshold, threshold);
            bloom.SetFloat(Intensity, Mathf.GammaToLinearSpace(intensity));
            Graphics.Blit(source, currentDestination, bloom, BoxDownPrefilterPass);

            
            int i = 1;
            for (; i < iterations; i++)
            {
                width /= 2;
                height /= 2;
                if (height < 2)
                {
                    break;
                }

                currentDestination = textures[i] =
                    RenderTexture.GetTemporary(width, height, 0, format);

                Graphics.Blit(currentSource, currentDestination, bloom, BoxDownPass);
                currentSource = currentDestination;
            }

            for (i -= 2; i >= 0; i--)
            {
                currentDestination = textures[i];
                textures[i] = null;
                Graphics.Blit(currentSource, currentDestination, bloom, BoxUpPass);
                RenderTexture.ReleaseTemporary(currentSource);
                currentSource = currentDestination;
            }

            bloom.SetTexture(SourceTex, currentSource);
            Graphics.Blit(source, destination, bloom, ApplyBloomPass);
        }

        else Graphics.Blit(source, destination);



        RenderTexture.ReleaseTemporary(currentSource);
    }
}