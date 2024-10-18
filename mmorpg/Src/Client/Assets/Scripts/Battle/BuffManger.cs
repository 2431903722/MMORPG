using Common.Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle
{
    public class BuffManger
    {
        private Creature Owner;

        public Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();

        public BuffManger(Creature owner)
        {
            this.Owner = owner;
        }

        internal Buff AddBuff(int buffId, int buffType, int casterId)
        {
            BuffDefine define;
            if (DataManager.Instance.Buffs.TryGetValue(buffType, out define))
            {
                Buff buff = new Buff(this.Owner, buffId, define, casterId);
                this.Buffs[buffId] = buff;
                return buff;
            }
            return null;
        }

        internal Buff RemoveBuff(int buffId)
        {
            Buff buff;
            if (this.Buffs.TryGetValue(buffId, out buff))
            {
                buff.OnRemove();
                this.Buffs.Remove(buffId);
                return buff;
            }
            return null;
        }

        internal void OnUpdate(float delta)
        {
            List<int> needRemove = new List<int>();

            foreach (var kv in this.Buffs)
            {
                kv.Value.OnUpdate(delta);
                if (kv.Value.Stoped)
                {
                    needRemove.Add(kv.Key);
                }
            }

            foreach (var key in needRemove)
            {
                this.Owner.RemoveBuff(key);
            }
        }
    }
}
