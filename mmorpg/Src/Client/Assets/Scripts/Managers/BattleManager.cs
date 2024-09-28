using Battle;
using Entities;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class BattleManager : Singleton<BattleManager>
    {
        private Creature currentTarget;
        public Creature CurrentTarget
        {
            get { return currentTarget; }
            set
            {
                this.SetTarget(value);
            }
        }

        private NVector3 currentPosition;
        public NVector3 CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                this.SetPosition(value);
            }
        }

        public void Init()
        {

        }

        public void SetTarget(Creature target)
        {
            this.currentTarget = target;
            Debug.LogFormat("BattleManager.SetTarget[{0}:{1}]", target.entityId, target.Name);
        }

        public void SetPosition(NVector3 position)
        {
            this.currentPosition = position;
            Debug.LogFormat("BattleManger.SetPosition:[{0}]", position);
        }

        public void CastSkill(Skill skill)
        {
            int target = currentTarget != null ? currentTarget.entityId : 0;
            BattleService.Instance.SendSkillCast(skill.Define.ID, skill.Owner.entityId, target, currentPosition);
        }
    }
}
