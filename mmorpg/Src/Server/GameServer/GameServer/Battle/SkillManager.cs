using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class SkillManager
    {
        private Creature Owner;

        public List<Skill> Skills { get; private set; }
        public List<NSkillInfo> Infos { get; private set; }

        public SkillManager(Creature owner)
        {
            this.Owner = owner;
            this.Skills = new List<Skill>();
            this.Infos = new List<NSkillInfo>();
            this.InitSkills();
        }

        void InitSkills()
        {
            this.Skills.Clear();
            this.Infos.Clear();

            // 这里应该从数据库中读取技能信息，作为拓展内容待完成

            if (!DataManager.Instance.Skills.ContainsKey(this.Owner.Define.TID))
                return;

            foreach (var define in DataManager.Instance.Skills[this.Owner.Define.TID])
            {
                NSkillInfo info = new NSkillInfo();
                info.Id = define.Key;
                if (this.Owner.Info.Level >= define.Value.UnlockLevel)
                {
                    info.Level = 5;
                }
                else
                {
                    info.Level = 1;
                }
                this.Infos.Add(info);
                Skill skill = new Skill(info, this.Owner);
                this.AddSkill(skill);
            }
        }

        public void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }
    }
}
