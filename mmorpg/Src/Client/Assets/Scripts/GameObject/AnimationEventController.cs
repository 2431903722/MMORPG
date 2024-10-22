using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour 
{
    public EntityEffectManger EffectMgr;
    
    void PlayEffect(string name)
    {
        Debug.LogFormat("AnimationEventController:PlayEffect:{0} : {1}", this.name, name);
        EffectMgr.PlayEffect(name);
    }

    void PlaySound(string name)
    {
        Debug.LogFormat("AnimationEventController:PlaySound:{0} : {1}", this.name, name);        
    }

}
