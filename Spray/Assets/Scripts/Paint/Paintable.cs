using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Paintable : MonoBehaviour
{
    public int textureSize = 2048;
    new public Renderer renderer { get; private set; } = null;
    public RenderTexture colorMaskTexture = null;
    public RenderTexture colorTexture = null;
    public RenderTexture normalMaskTexture = null;
    public RenderTexture normalTexture = null;

    private int _colorTexID = Shader.PropertyToID("_SplatColorTex");
    private int _normalTexID = Shader.PropertyToID("_SplatNormalTex");
    void Start()
    {
        colorMaskTexture = new RenderTexture(textureSize, textureSize, 0);
        colorMaskTexture.filterMode = FilterMode.Bilinear;

        colorTexture = new RenderTexture(textureSize, textureSize, 0);
        colorTexture.filterMode = FilterMode.Bilinear;

        normalMaskTexture = new RenderTexture(textureSize, textureSize, 0);
        normalMaskTexture.filterMode = FilterMode.Bilinear;

        normalTexture = new RenderTexture(textureSize, textureSize, 0);
        normalTexture.filterMode = FilterMode.Bilinear;

        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture(_colorTexID, colorTexture);
        renderer.material.SetTexture(_normalTexID, normalTexture);

        Systems.paintManager.InitTextures(this);
    }

    void OnDestroy()
    {
        colorMaskTexture.Release();
        colorTexture.Release();
        normalMaskTexture.Release();
    }
}
