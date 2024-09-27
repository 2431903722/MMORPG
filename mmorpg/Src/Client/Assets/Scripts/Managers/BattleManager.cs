using Entities;
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
        public Creature Target { get; set; }
        public Vector3 Position { get; set; }
    }
}
