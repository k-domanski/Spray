using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSampler : MonoBehaviour
{
    [SerializeField] private float _samplingInterval = 0.1f;
    [SerializeField] private LayerMask _raycastLayers;
    [SerializeField] private ColorsLib _colors;

    public Texture2D _samplingTexture;
    private Coroutine _coroutine;
    public Color rawColor;
    public ColorType colorType;

    public Action<ColorType, ColorType> onColorTypeChanged;

    void Awake()
    {
        _samplingTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false, true);
    }

    void OnEnable()
    {
        _coroutine = StartCoroutine(SampleCor());
    }

    void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    public void Sample()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out var hit, 1000.0f, _raycastLayers))
        {
            Paintable paintable = hit.transform.GetComponent<Paintable>();
            if (paintable)
            {
                MeshFilter filter = paintable.GetComponent<MeshFilter>();
                if (filter == null || paintable.colorTexture == null)
                {
                    return;
                }

                /* Texture Coordinates */
                float flip = SystemInfo.graphicsUVStartsAtTop ? -1 : 1;
                Vector3 local_pos = paintable.transform.InverseTransformPoint(hit.point);
                Vector3 size = filter.mesh.bounds.size;
                int x = Mathf.FloorToInt((local_pos.x / size.x + 0.5f) * paintable.textureSize);
                int y = Mathf.FloorToInt((flip * local_pos.z / size.z + 0.5f) * paintable.textureSize);

                /* Color Read */
                RenderTexture currentRT = RenderTexture.active;
                RenderTexture.active = paintable.colorTexture;
                _samplingTexture.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
                _samplingTexture.Apply();
                RenderTexture.active = currentRT;

                rawColor = _samplingTexture.GetPixel(0, 0);
                GetColorType(rawColor);
            }
        }
    }

    void GetColorType(Color color)
    {
        ColorType type = ColorType.None;
        float min_dist = float.MaxValue;
        Vector3 in_color = new Vector3(color.r, color.g, color.b);
        foreach (var pair in _colors.colors)
        {
            Vector3 list_color = new Vector3(pair.second.r, pair.second.g, pair.second.b);
            float dist = Vector3.Distance(in_color, list_color);
            if (dist < min_dist)
            {
                min_dist = dist;
                type = pair.firts;
            }
        }

        onColorTypeChanged?.Invoke(colorType, type);
        colorType = type;
    }

    private IEnumerator SampleCor()
    {
        while (true)
        {
            Sample();
            yield return new WaitForSeconds(_samplingInterval);
        }
    }
}
