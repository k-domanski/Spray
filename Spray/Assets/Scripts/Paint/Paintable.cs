using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Paintable : MonoBehaviour
{
    private const int TEXTURE_SIZE = 1024;
    new public Renderer renderer { get; private set; } = null;
    public RenderTexture maskTexture = null;
    public RenderTexture swapTexture = null;
    public RenderTexture normalTexture = null;

    private int _maskTexID = Shader.PropertyToID("_MaskTex");
    void Start()
    {
        maskTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        maskTexture.filterMode = FilterMode.Bilinear;

        swapTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        swapTexture.filterMode = FilterMode.Bilinear;

        normalTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0, RenderTextureFormat.Default);

        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture(_maskTexID, swapTexture);

        Systems.paintManager.InitTextures(this);
    }

    void OnDestroy()
    {
        maskTexture.Release();
        swapTexture.Release();
        normalTexture.Release();
    }
}
