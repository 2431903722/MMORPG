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
        public Creature Target;

        public NVector3 TargetPosition { get; private set; }

        public SkillDefine Define;
        private float cd= 0;
        private float castTime = 0;
        private float skllTime = 0;
        public int Hit = 0;
        private SkillStatus Status;

        public bool IsCasting = false;

        Dictionary<int, List<NDamageInfo>> HitMap = new Dictionary<int, List<NDamageInfo>>();

        List<Bullet> Bullets = new List<Bullet>();

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
                Debug.LogFormat("TargetPosition:[{0}]", target.position);
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

        public void BeginCast(Creature target, NVector3 pos)
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.skllTime = 0;
            this.Hit = 0;
            this.cd = this.Define.CD;
            this.Target = target;
            this.TargetPosition = pos;
            this.Owner.PlayAnim(this.Define.SkillAnim);
            this.Bullets.Clear();
            this.HitMap.Clear();

            if (this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                this.Owner.FaceTo(this.TargetPosition.ToVector3Int());
            }
            else if (this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                this.Owner.FaceTo(this.Target.position);
            }

            if (this.Define.CastTime > 0)
            {
                this.Status = SkillStatus.Casting;
            }
            else
            {
                this.StartSkill();
            }
        }

        /// <summary>
        /// 技能执行开始
        /// </summary>
        void StartSkill()
        {
            this.Status = SkillStatus.Running;
            if (!string.IsNullOrEmpty(this.Define.AOEEffect))
            {
                if (this.Define.CastTarget == TargetType.Position)
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.TargetPosition);
                else if (this.Define.CastTarget == TargetType.Target)
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.Target);
                else if (this.Define.CastTarget == TargetType.Self)
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.Owner);
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
                this.StartSkill();
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
                    if (!this.Define.Bullet)
                    {
                        this.Status = SkillStatus.None;
                        this.IsCasting = false;
                        Debug.LogFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                    }
                }
            }

            if (this.Define.Bullet)
            {
                bool finish = true;
                foreach (Bullet bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stoped)
                    {
                        finish = false;
                    }
                }

                if (finish && this.Hit >= this.Define.HitTimes.Count)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }
        }

        private void DoHit()
        {
            if (this.Define.Bullet)
            {
                this.CastBullet();
            }
            else
                this.DoHitDamages(this.Hit);
            this.Hit++;
        }

        public void DoHitDamages(int hit)
        {
            List<NDamageInfo> damages;
            if (this.HitMap.TryGetValue(hit, out damages))
            {
                DoHitDamages(damages);
            }
        }

        private void CastBullet()
        {
            Bullet bullet = new Bullet(this);
            Debug.LogFormat("Skill[{0}].CastBullet[{1}] Target:{2}", this.Define.Name, this.Define.Bullet, this.Target.Name);
            this.Bullets.Add(bullet);
            this.Owner.PlayEffect(EffectType.Bullet, this.Define.BulletResource, this.Target, bullet.duration);
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

        internal void DoHit(NSkillHitInfo hit)
        {
            if (hit.isBullet || !this.Define.Bullet)
            {
                this.DoHit(hit.hitId, hit.Damages);
            }
        }

        internal void DoHit(int hitId, List<NDamageInfo> damages)
        {
            if (hitId >= this.Hit)
            {
                this.HitMap[hitId] = damages;
            }
            else
            {
                DoHitDamages(damages);
            }
        }

        internal void DoHitDamages(List<NDamageInfo> damages)
        {
            foreach (var dmg in damages)
            {
                Creature target = (Creature)EntityManager.Instance.GetEntity(dmg.entityId);
                if (target == null) continue;
                target.DoDamage(dmg, true);
                if (this.Define.HitEffect != null)
                {
                    target.PlayEffect(EffectType.Hit, this.Define.HitEffect, target);
                }
            }
        }
    }
}
