using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Battle
{
    public enum AttributeType
    {
        None = -1,
        /// <summary>
        /// 最大生命
        /// </summary>
        MaxHP = 0,
        /// <summary>
        /// 最大法力
        /// </summary>
        MaxMP = 1,
        /// <summary>
        /// 力量
        /// </summary>
        STR = 2,
        /// <summary>
        /// 智力
        /// </summary>
        INT = 3,
        /// <summary>
        /// 物理攻击
        /// </summary>
        DEX = 4,
        /// <summary>
        /// 物理攻击
        /// </summary>
        AD = 5,
        /// <summary>
        /// 法术攻击
        /// </summary>
        AP = 6,
        /// <summary>
        /// 物理防御
        /// </summary>
        DEF = 7,
        /// <summary>
        /// 法术防御
        /// </summary>
        MDEF = 8,
        /// <summary>
        /// 攻击速度
        /// </summary>
        SPD = 9,
        /// <summary>
        /// 暴击概率
        /// </summary>
        CRI = 10,

        MAX
    }

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

    public enum SkillResult
    {
        OK = 0,
        InvalidTarget,
        OutOfMP,
        Cooldown
    }
}
