using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Battle
{
    public enum SkillType
    {
        None = 0,
        Normal = 1,
        Skill = 2
    }

    public enum TargetType
    {
        None = 0,
        Self = 1,
        Target = 2,
        Position = 3
    }

    public enum BuffEffect
    {
        None = 0,
        Stun = 1
    }
}
