using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FXManager : MonoSingleton<FXManager>
{
    public GameObject[] prefabs;

    private Dictionary<string, GameObject> Effects = new Dictionary<string, GameObject>();

    protected override void OnStart()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabs[i].SetActive(false);
            this.Effects[prefabs[i].name] = prefabs[i];
        }
    }

    EffectController CreateEffect(string name, Vector3 pos)
    {
        GameObject prefab;
        if (this.Effects.TryGetValue(name, out prefab))
        {
            GameObject go = Instantiate(prefab, FXManager.Instance.transform, true);
            go.transform.position = pos;
            return go.GetComponent<EffectController>();
        }
        return null;
    }

    internal void PlayEffect(EffectType type, string name, Transform target, Vector3 pos, float duration)
    {
        EffectController effect = FXManager.Instance.CreateEffect(name, pos);
        if (effect == null)
        {
            Debug.LogErrorFormat("Effect not found: {0}", name);
            return;
        }
        effect.Init(type, this.transform, target, pos, duration);
        effect.gameObject.SetActive(true);
    }
}

