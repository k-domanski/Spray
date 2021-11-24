using System.Collections.Generic;
using UnityEngine;

public class EffectPanelUI : MonoBehaviour
{
    [SerializeField] private BloodEffectController _bloodEffectController;
    [SerializeField] private GameObject _buffPrefab;
    [SerializeField] private GameObject _debuffPrefab;
    private List<EffectIconUI> _effectIcons = new List<EffectIconUI>();

    void OnEnable()
    {
        _bloodEffectController.onEffectAdded += AddIcon;
        _bloodEffectController.onEffectRemoved += RemoveIcon;
    }

    private void RemoveIcon(BloodEffectBase obj)
    {
        foreach (var icon in _effectIcons)
        {
            if (obj.icon == icon.icon.sprite)
            {
                obj.OnCurrentDuration -= icon.SetDuration;
                icon.gameObject.SetActive(false);
                _effectIcons.Remove(icon);
                Destroy(icon.gameObject);
                break;
            }
        }
    }

    private void AddIcon(BloodEffectBase obj)
    {
        var icon = Instantiate(obj.type == EffectType.Buff ? _buffPrefab : _debuffPrefab).GetComponent<EffectIconUI>();
        icon.transform.SetParent(transform);
        icon.icon.sprite = obj.icon;
        _effectIcons.Add(icon);
        obj.OnCurrentDuration += icon.SetDuration;

    }
}
