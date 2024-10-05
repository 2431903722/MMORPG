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
        public bool IsCasting = false;
        private int hit;

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

                int distance = (int)Vector3.Distance(this.Owner.position, target.position);
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
            this.hit = 0;
            this.cd = this.Define.CD;
            this.Damage = damage;
            this.Owner.PlayAnim(this.Define.SkillAnim);
        }

        public void OnUpdate(float delta)
        {
            if (this.IsCasting)
            {
                this.skllTime += delta;
                if (this.skllTime > 0.5f && this.hit == 0)
                {
                    this.DoHit();
                }
                if (this.skllTime >= this.Define.CD)
                {
                    this.skllTime = 0;
                }
            }

            UpdateCD(delta);
        }

        private void DoHit()
        {
            if (this.Damage != null)
            {
                var cha = CharacterManager.Instance.GetCharacter(this.Damage.entityId);
                cha.DoDamage(this.Damage);
            }
            this.hit++;
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
    }
}
