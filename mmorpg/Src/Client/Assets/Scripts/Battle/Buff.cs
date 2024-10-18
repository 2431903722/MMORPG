using Common.Battle;
using Common.Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class Buff
    {
        private Creature Owner;
        public int BuffId;
        public BuffDefine Define;
        private int CasterId;
        public float time;
        internal bool Stoped;        

        public Buff(Creature owner, int buffId, BuffDefine define, int casterId)
        {
            this.Owner = owner;
            this.BuffId = buffId;
            this.Define = define;
            this.CasterId = casterId;
            this.OnAdd();
        }

        private void OnAdd()
        {
            Debug.LogFormat("Buff[{0}:{1}] OnAdd", this.BuffId, this.Define.Name);
            if (this.Define.Effect != BuffEffect.None)
            {
                this.Owner.AddBuffEffect(this.Define.Effect);
            }
            AddAttr();
        }

        internal void OnRemove()
        {
            Debug.LogFormat("Buff[{0}:{1}] OnRemove", this.BuffId, this.Define.Name);
            RemoveAttr();
            Stoped = true;
            if (this.Define.Effect != BuffEffect.None)
            {
                this.Owner.RemoveBuffEffect(this.Define.Effect);
            }
        }

        private void AddAttr()
        {
            if (this.Define.DEFRatio != 0)
            {
                this.Owner.Attributes.Buff.DEF += this.Owner.Attributes.Basic.DEF * this.Define.DEFRatio;
            }
            this.Owner.Attributes.InitFinalAttributes();
        }

        private void RemoveAttr()
        {
            if (this.Define.DEFRatio != 0)
            {
                this.Owner.Attributes.Buff.DEF -= this.Owner.Attributes.Basic.DEF * this.Define.DEFRatio;
            }
            this.Owner.Attributes.InitFinalAttributes();
        }

        internal void OnUpdate(float delta)
        {
            if (Stoped) return;
            this.time += delta;
            if (time > this.Define.Duration)
            {
                this.OnRemove();
            }
        }
    }
}
