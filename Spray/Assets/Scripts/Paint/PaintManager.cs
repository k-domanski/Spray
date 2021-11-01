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

    private int _mainTexID = Shader.PropertyToID("_MainTex");
    private int _colorID = Shader.PropertyToID("_Color");
    private int _brushPosID = Shader.PropertyToID("_BrushPos");
    private int _radiusID = Shader.PropertyToID("_Radius");
    private int _hardnessID = Shader.PropertyToID("_Hardness");
    private int _strengthID = Shader.PropertyToID("_Strength");
    private int _rotationID = Shader.PropertyToID("_BrushRotation");

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

        command.SetRenderTarget(paintable.maskTexture);
        command.ClearRenderTarget(RTClearFlags.All, Color.clear, 1.0f, 0);

        command.SetRenderTarget(paintable.swapTexture);
        // command.Blit(paintable.renderer.material.mainTexture, paintable.swapTexture);

        // command.DrawRenderer(paintable.renderer, initMaterial);
        Graphics.ExecuteCommandBuffer(command);
    }

    public void Paint(Paintable paintable, Vector3 position, float radius, float hardness, float strength, float rotation = 0, Color? color = null)
    {
        command.Clear();

        maskMaterial.SetTexture(_mainTexID, paintable.swapTexture);
        maskMaterial.SetVector(_brushPosID, position);
        maskMaterial.SetFloat(_radiusID, radius);
        maskMaterial.SetFloat(_hardnessID, hardness);
        maskMaterial.SetFloat(_strengthID, strength);
        maskMaterial.SetFloat(_rotationID, rotation);

        if(color != null)
        {
            maskMaterial.SetColor(_colorID, color.Value);
        }

        command.SetRenderTarget(paintable.maskTexture);
        command.DrawRenderer(paintable.renderer, maskMaterial, 0);

        command.SetRenderTarget(paintable.swapTexture);
        command.Blit(paintable.maskTexture, paintable.swapTexture);

        Graphics.ExecuteCommandBuffer(command);
    }
}
