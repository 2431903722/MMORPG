using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class ArenaService : Singleton<ArenaService>, IDisposable
    {
        public void Init()
        {
            //ArenaManager.Instance.Init();
        }

        public ArenaService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ArenaChallengRequest>(this.OnArenaChallengeRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ArenaChallengeResponse>(this.OnArenaChallengeResponse);
        }

        public void Dispose()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<ArenaChallengRequest>(this.OnArenaChallengeRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<ArenaChallengeResponse>(this.OnArenaChallengeResponse);
        }

        private void OnArenaChallengeRequest(NetConnection<NetSession> sender, ArenaChallengRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnArenaChallengeRequest: RedID:{0} RedName:{1} |  BlueID:{2} BlueName:{3}", request.ArenaInfo.Red.EntityId, request.ArenaInfo.Red.Name, request.ArenaInfo.Blue.EntityId, request.ArenaInfo.Blue.Name);
            NetConnection<NetSession> blue = null;
            if (request.ArenaInfo.Blue.EntityId > 0)
            {
                blue = SessionManager.Instance.GetSession(request.ArenaInfo.Blue.EntityId);
            }
            if (blue == null)
            {
                sender.Session.Response.arenaChallengeRes = new ArenaChallengeResponse();
                sender.Session.Response.arenaChallengeRes.Result = Result.Failed;
                sender.Session.Response.arenaChallengeRes.Errormsg = "对方不在线";
                sender.SendResponse();
                return;
            }
            Log.InfoFormat("ForwardArenaChallengeRequest: RedID:{0} RedName:{1} |  BlueID:{2} BlueName:{3}", request.ArenaInfo.Red.EntityId, request.ArenaInfo.Red.Name, request.ArenaInfo.Blue.EntityId, request.ArenaInfo.Blue.Name);
            blue.Session.Response.arenaChallengeReq = request;
            blue.SendResponse();
        }

        private void OnArenaChallengeResponse(NetConnection<NetSession> sender, ArenaChallengeResponse response)
        {
            Character character = sender.Session.Character;                    
            Log.InfoFormat("OnArenaChallengeResponse:: character:{0} Result:{1} FromId:{2} ToID:{3}", character.Id, response.Result, sender.Session.Character.Id, response.ArenaInfo.Blue.EntityId);
            var requester = SessionManager.Instance.GetSession(response.ArenaInfo.Red.EntityId);
            if (requester == null)
            {
                sender.Session.Response.arenaChallengeRes.Result = Result.Failed;
                sender.Session.Response.arenaChallengeRes.Errormsg = "对方不在线";
                sender.SendResponse();
                return;
            }
            if (response.Result == Result.Failed)
            {
                requester.Session.Response.arenaChallengeRes = response;
                requester.Session.Response.arenaChallengeRes.Result = Result.Failed;
                requester.SendResponse();
                return;
            }

            //var arena = ArenaManager.Instance.NewArena(response.ArenaInfo, requester, sender);
            this.SendArenaBegin(requester, sender);
        }

        public void SendArenaBegin(NetConnection<NetSession> Red, NetConnection<NetSession> Blue)
        {
            var arenaBegin = new ArenaBeginResponse();
            arenaBegin.Result = Result.Failed;
            arenaBegin.Errormsg = "对方不在线";
            //arenaBegin.ArenaInfo = arena.ArenaInfo;
            Red.Session.Response.arenaBegin = arenaBegin;
            Red.SendResponse();
            Blue.Session.Response.arenaBegin = arenaBegin;
            Blue.SendResponse();
        }
    }
}
