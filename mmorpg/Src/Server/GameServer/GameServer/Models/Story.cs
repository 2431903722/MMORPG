using Common.Data;
using GameServer.Managers;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Story
    {
        public Map Map;
        public int StoryId;
        public int InstanceId;
        public NetConnection<NetSession> Player;

        Map SourceMapRed;

        int startPoint = 12;

        public Story(Map map, int storyId, int instance, NetConnection<NetSession> owner)
        {
            this.StoryId = storyId;
            this.Player = owner;
            this.Map = map;
        }

        internal void PlayerEnter()
        {
            this.SourceMapRed = PlayerLeaveMap(this.Player);            
            this.PlayerEnterArena();
        }

        private Map PlayerLeaveMap(NetConnection<NetSession> player)
        {
            var currentMap = MapManager.Instance[player.Session.Character.Info.mapId];
            currentMap.CharacterLeave(player.Session.Character);
            EntityManager.Instance.RemoveMapEntity(currentMap.ID, currentMap.InstanceId, player.Session.Character);
            return currentMap;
        }

        private void PlayerEnterArena()
        {
            TeleporterDefine startPoint = DataManager.Instance.Teleporters[this.startPoint];
            this.Player.Session.Character.Position = startPoint.Position;
            this.Player.Session.Character.Direction = startPoint.Direction;
            this.Map.AddCharacter(this.Player, this.Player.Session.Character);
            this.Map.CharacterEnter(this.Player, this.Player.Session.Character);
            EntityManager.Instance.AddMapEntity(this.Map.ID, this.Map.InstanceId, this.Player.Session.Character);
        }

        internal void Update()
        {

        }

        public void End()
        {

        }
    }
}
