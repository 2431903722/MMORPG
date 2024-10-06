using Common.Battle;
using Common.Data;
using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class Skill
    {
        public NSkillInfo Info;
        public Creature Owner;
        public SkillDefine Define;
        private float cd= 0;

        public NDamageInfo Damage { get; private set; }

        private float castTime = 0;
        private float skllTime;
        private int Hit = 0;
        private SkillStatus Status;

        public bool IsCasting = false;

        Dictionary<int, List<NDamageInfo>> HitMap = new Dictionary<int, List<NDamageInfo>>();

        public float CD
        {
            get { return cd; }
        }

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.Owner.Define.Class][this.Info.Id];
            this.cd = 0;
        }

        public SkillResult CanCast(Creature target)
        {
            if (this.Define.CastTarget == TargetType.Target)
            {
                if (target == null || target == this.Owner)
                    return SkillResult.InvalidTaeget;

                int distance = this.Owner.Distance(target);
                if (distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }

            if (this.Define.CastTarget == TargetType.Position && BattleManager.Instance.CurrentPosition == null)
            {
                return SkillResult.InvalidTaeget;
            }

            if (this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.OutOfMp;
            }

            if (this.cd > 0)
            {
                return SkillResult.CoolDown;
            }

            return SkillResult.Ok;
        }

        public void BeginCast(NDamageInfo damage)
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.skllTime = 0;
            this.Hit = 0;
            this.cd = this.Define.CD;
            this.Damage = damage;
            this.Owner.PlayAnim(this.Define.SkillAnim);

            if (this.Define.CastTime > 0)
            {
                this.Status = SkillStatus.Casting;
            }
            else
            {
                this.Status = SkillStatus.Running;
            }
        }

        public void OnUpdate(float delta)
        {
            UpdateCD(delta);
            
            if (this.Status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }
            else if (this.Status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }
        }

        private void UpdateCasting()
        {
            if (this.castTime < this.Define.CastTime)
            {
                this.castTime += Time.deltaTime;
            }
            else
            {
                this.castTime = 0;
                this.Status = SkillStatus.Running;
                Debug.LogFormat("SKill[{0}.UpdateCasting Finish]", this.Define.Name);
            }
        }

        private void UpdateSkill()
        {
            this.skllTime += Time.deltaTime;
            // 技能持续中
            if (this.Define.Duration > 0)
            {
                // 技能伤害次数
                if (this.skllTime > this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }
                // 技能结束
                if (this.skllTime >= this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }
            // 瞬发技能
            else if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0)
            {
                // 技能伤害次数
                if (this.Hit < this.Define.HitTimes.Count)
                {
                    if (this.skllTime > this.Define.HitTimes[this.Hit])
                    {
                        this.DoHit();
                    }
                }
                // 技能结束
                else
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }
        }

        private void DoHit()
        {
            List<NDamageInfo> damages;
            if (this.HitMap.TryGetValue(this.Hit, out damages))
            {
                DoHitDamages(damages);
            }
            this.Hit++;
        }

        private void UpdateCD(float delta)
        {
            if (this.cd > 0)
            {
                this.cd -= delta;
            }

            if (cd < 0)
            {
                this.cd = 0;
            }
        }

        internal void DoHit(int hitId, List<NDamageInfo> damages)
        {
            if (hitId <= this.Hit)
                this.HitMap[hitId] = damages;
            else
                DoHitDamages(damages);
        }

        internal void DoHitDamages(List<NDamageInfo> damages)
        {
            foreach (var dmg in damages)
            {
                Creature target = EntityManager.Instance.GetEntity(dmg.entityId) as Creature;
                if (target == null) continue;
                target.DoDamage(dmg);
            }
        }
    }
}
