using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : MonoBehaviour
{
    [SerializeField] private Shader _initShader;
    [SerializeField] private Shader _maskShader;
    private CommandBuffer command;
    public Material initMaterial = null;
    public Material maskMaterial = null;

    private int _colorTexID = Shader.PropertyToID("_ColorTex");
    private int _colorID = Shader.PropertyToID("_Color");
    private int _brushPosID = Shader.PropertyToID("_BrushPos");
    private int _radiusID = Shader.PropertyToID("_Radius");
    private int _hardnessID = Shader.PropertyToID("_Hardness");
    private int _strengthID = Shader.PropertyToID("_Strength");
    private int _rotationID = Shader.PropertyToID("_BrushRotation");
    private int _brushID = Shader.PropertyToID("_BrushTex");
    private int _normalTexID = Shader.PropertyToID("_NormalTex");
    private int _brushNormalTexID = Shader.PropertyToID("_BrushNormalTex");
    private int _randomNormalID = Shader.PropertyToID("_RandomNormal");
    private int _normalStrengthID = Shader.PropertyToID("_NormalStrength");

    void Awake()
    {
        initMaterial = new Material(_initShader);
        maskMaterial = new Material(_maskShader);
        command = new CommandBuffer();
        command.name = $"Command Buffer - {gameObject.name}";
    }

    public void InitTextures(Paintable paintable)
    {
        command.Clear();

        command.SetRenderTarget(paintable.colorMaskTexture);
        command.ClearRenderTarget(RTClearFlags.All, Color.clear, 1.0f, 0);

        command.SetRenderTarget(paintable.colorTexture);
        command.ClearRenderTarget(RTClearFlags.All, Color.clear, 1.0f, 0);

        command.SetRenderTarget(paintable.normalMaskTexture);
        command.ClearRenderTarget(RTClearFlags.All, Color.clear, 1.0f, 0);

        command.SetRenderTarget(paintable.normalTexture);
        command.ClearRenderTarget(RTClearFlags.All, Color.clear, 1.0f, 0);

        command.SetRenderTarget(paintable.colorTexture);
        Graphics.ExecuteCommandBuffer(command);
    }

    public void Paint(Paintable paintable, Vector3 position, PaintData paintData)
    {
        Paint(paintable, position, paintData.radius, paintData.hardness, paintData.strength, paintData.rotation, paintData.color, paintData.brush, paintData.normalStrength, paintData.normal, paintData.randomNormals);
    }

    public void Paint(Paintable paintable, Vector3 position, float radius, float hardness, float strength, float rotation = 0, Color? color = null, Texture brush = null, float normalStrength = 1.0f, Texture normal = null, bool randomNormal = true)
    {
        command.Clear();

        maskMaterial.SetTexture(_colorTexID, paintable.colorTexture);
        maskMaterial.SetTexture(_normalTexID, paintable.normalTexture);
        maskMaterial.SetVector(_brushPosID, position);
        maskMaterial.SetFloat(_radiusID, radius);
        maskMaterial.SetFloat(_hardnessID, hardness);
        maskMaterial.SetFloat(_strengthID, strength);
        maskMaterial.SetFloat(_rotationID, rotation);
        maskMaterial.SetTexture(_brushNormalTexID, normal);
        maskMaterial.SetInt(_randomNormalID, randomNormal ? 1 : 0);
        maskMaterial.SetFloat(_normalStrengthID, normalStrength);

        if (color != null)
        {
            maskMaterial.SetColor(_colorID, color.Value);
        }

        maskMaterial.SetTexture(_brushID, brush);

        RenderTargetIdentifier[] targets = { paintable.colorMaskTexture, paintable.normalMaskTexture };
        command.SetRenderTarget(targets, paintable.colorMaskTexture.depthBuffer);
        command.DrawRenderer(paintable.renderer, maskMaterial, 0);

        command.SetRenderTarget(paintable.colorTexture);
        command.Blit(paintable.colorMaskTexture, paintable.colorTexture);

        command.SetRenderTarget(paintable.normalTexture);
        command.Blit(paintable.normalMaskTexture, paintable.normalTexture);

        Graphics.ExecuteCommandBuffer(command);
    }
}

public class PaintData
{
    public float radius = 1.0f;
    public float hardness = 1.0f;
    public float strength = 1.0f;
    public float rotation = 0.0f;
    public Color? color = null;
    public Texture brush = null;
    public Texture normal = null;
    public bool randomNormals = true;
    public float normalStrength = 1.0f;
}