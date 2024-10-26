using Common.Battle;
using GameServer.Battle;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.AI
{
    class AIBase
    {
        private Monster owner;
        private Creature target;
        Skill normalSkill;

        public AIBase(Monster owner)
        {
            this.owner = owner;
            normalSkill = this.owner.SkillMgr.NormalSkill;
        }

        internal void Update()
        {
            if (this.owner.BattleState == Common.Battle.BattleState.InBattle)
            {
                this.UpdateBattle();
            }
        }

        private void UpdateBattle()
        {
            if (this.target == null)
            {
                this.owner.BattleState = Common.Battle.BattleState.Idle;
                return;
            }

            if (!TryCastSkill())
            {
                if (!TryCastNormal())
                {
                    FollowTarget();
                }
            }
        }

        private bool TryCastSkill()
        {
            if (this.target != null)
            {
                BattleContext context = new BattleContext(this.owner.Map.Battle)
                {
                    Target = this.target,
                    Caster = this.owner,
                };

                Skill skill = this.owner.FindSkill(context, SkillType.Skill);

                if (skill != null)
                {
                    this.owner.CastSkill(context, skill.Define.ID);
                    return true;
                }    
            }
            return false;
        }

        private bool TryCastNormal()
        {
            if (this.target != null)
            {
                BattleContext context = new BattleContext(this.owner.Map.Battle)
                {
                    Target = this.target,
                    Caster = this.owner,
                };

                var result =  normalSkill.CanCast(context);

                if (result == SkillResult.Ok)
                {
                    this.owner.CastSkill(context, normalSkill.Define.ID);
                }

                if (result == SkillResult.OutOfRange)
                {
                    return false;
                }
            }
            return true;
        }

        private void FollowTarget()
        {
            int distance = this.owner.Distance(this.target);
            if (distance > normalSkill.Define.CastRange - 50)
            {
                this.owner.MoveTo(this.target.Position);
            }
            else
            {
                this.owner.StopMove();
            }
        }

        internal void OnDamage(NDamageInfo damage, Creature source)
        {
            this.target = source;
        }
    }
}
