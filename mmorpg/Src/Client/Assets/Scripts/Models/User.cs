using Common.Data;
using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        NUserInfo userInfo;

        public NUserInfo Info
        {
            get { return userInfo; }
        }

        public void SetupUserInfo(NUserInfo info)
        {
            this.userInfo = info;
        }

        public MapDefine CurrentMapData { get; set; }
        public NCharacterInfo CurrentCharacterInfo { get; set; }
        public PlayerInputController CurrentCharacterObject { get; set; }        
        public NTeamInfo TeamInfo { get; set; }
        public Character CurrentCharacter { get; set; }

        public void AddGold(int gold)
        {
            this.CurrentCharacterInfo.Gold += gold;
        }

        public int CurrentRide = 0;

        internal void Ride(int id)
        {
            if(CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
            }
            else
            {
                CurrentRide = 0;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, 0);
            }
        }

        public delegate void CharacterInithandle();
        public event CharacterInithandle OnCharacterInit;

        internal void CharacterInited()
        {
            if (OnCharacterInit != null)
                OnCharacterInit();
        }
    }
}
