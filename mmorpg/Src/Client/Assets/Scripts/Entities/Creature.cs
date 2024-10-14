using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using Common.Battle;
using Managers;
using Common.Data;
using Battle;

namespace Entities
{
    public class Creature : Entity
    {
        public NCharacterInfo Info;
        public Common.Data.CharacterDefine Define;
        public Attributes Attributes;
        public SkillManager SkillMgr;

        bool battleState = false;
        public bool BattleStats
        {
            get { return battleState; }
            set
            {
                if (battleState != value)
                {
                    battleState = value;
                    this.SetStandby(value);
                }
            }
        }

        public int Id
        {
            get { return this.Info.Id; }
        }

        public Skill CastringSkill = null;

        public string Name
        {
            get
            {
                if (this.Info.Type == CharacterType.Player)
                    return this.Info.Name;
                else
                    return this.Define.Name;
            }
        }

        // 判断是否为玩家
        public bool IsPlayer
        {
            get { return this.Info.Type == CharacterType.Player; }
        }

        public bool IsCurrentPlayer
        {
            get
            {
                if (!IsPlayer)
                    return false;
                return this.Info.Id == Models.User.Instance.CurrentCharacterInfo.Id;
            }
        }

        public Creature(NCharacterInfo info) : base(info.Entity)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Characters[info.ConfigId];
            this.Attributes = new Attributes();
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr = new SkillManager(this);
        }

        public void UpdateInfo(NCharacterInfo info)
        {
            this.SetEntityData(info.Entity);
            this.Info = info;
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr.UpdateSkills();
        }

        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }

        public void MoveForward()
        {
            //Debug.LogFormat("MoveForward");
            this.speed = this.Define.Speed;
        }

        public void MoveBack()
        {
            //Debug.LogFormat("MoveBack");
            this.speed = -this.Define.Speed;
        }

        public void Stop()
        {
            //Debug.LogFormat("Stop");
            this.speed = 0;
        }

        public void SetDirection(Vector3Int direction)
        {
            //Debug.LogFormat("SetDirection:{0}", direction);
            this.direction = direction;
        }

        public void SetPosition(Vector3Int position)
        {
            //Debug.LogFormat("SetPosition:{0}", position);
            this.position = position;
        }

        public void CastSkill(int skillId, Creature target, NVector3 pos, NDamageInfo damage)
        {
            this.SetStandby(true);
            var skill = this.SkillMgr.GetSkill(skillId);
            skill.BeginCast(target);
        }

        internal void DoSkillHit(int skillId, int hitId, List<NDamageInfo> damages)
        {
            var skill = this.SkillMgr.GetSkill(skillId);
            skill.DoHit(hitId, damages);
        }

        public void PlayAnim(string name)
        {
            if (this.Controller != null)
                this.Controller.PlayAnim(name);
        }

        public void SetStandby(bool standby)
        {
            if (this.Controller != null)
                this.Controller.SetStandby(standby);
        }

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            this.SkillMgr.OnUpdate(delta);
        }

        public void DoDamage(NDamageInfo damage)
        {
            Debug.LogFormat("DoDamage:{0} DMG:{1} CRIT:{2}", this.Name, damage.Damage, damage.Crit);
            this.Attributes.HP -= damage.Damage;
            this.PlayAnim("Hurt");
        }

        internal void DoSkillHit(NSkillHitInfo hit)
        {
            Debug.LogFormat("DoSkillhit: Caster:{0} Skill:{1} Hit:{2} IsBullet:{3}", hit.casterId, hit.skillId, hit.hitId, hit.isBullet);
            var skill = this.SkillMgr.GetSkill(hit.skillId);
            skill.DoHit(hit);
        }

        internal int Distance(Creature target)
        {
            return (int)Vector3Int.Distance(this.position, target.position);
        }
    }
}
