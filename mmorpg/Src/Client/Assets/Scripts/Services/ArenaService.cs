using Models;
using Network;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    class ArenaService : Singleton<ArenaService>, IDisposable
    {
        public void Init()
        {

        }

        public ArenaService()
        {
            MessageDistributer.Instance.Subscribe<ArenaChallengRequest>(this.OnArenaChallengeRequest); 
            MessageDistributer.Instance.Subscribe<ArenaChallengeResponse>(this.OnArenaChallengeResponse);
            MessageDistributer.Instance.Subscribe<ArenaBeginResponse>(this.OnArenaBegin);
            MessageDistributer.Instance.Subscribe<ArenaEndResponse>(this.OnArenaEnd);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ArenaChallengRequest>(this.OnArenaChallengeRequest);
            MessageDistributer.Instance.Unsubscribe<ArenaChallengeResponse>(this.OnArenaChallengeResponse);
            MessageDistributer.Instance.Unsubscribe<ArenaBeginResponse>(this.OnArenaBegin);
            MessageDistributer.Instance.Unsubscribe<ArenaEndResponse>(this.OnArenaEnd);
        }

        /// <summary>
        /// 发送挑战请求
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="targetName"></param>
        public void SendArenaChallengeRequest(int targetId, string targetName)
        {
            Debug.Log("SendArenaChallengeRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.arenaChallengeReq = new ArenaChallengRequest();
            message.Request.arenaChallengeReq.ArenaInfo = new ArenaInfo();
            message.Request.arenaChallengeReq.ArenaInfo.Red = new ArenaPlayer()
            {
                EntityId = User.Instance.CurrentCharacterInfo.Id,
                Name = User.Instance.CurrentCharacterInfo.Name,
            };
            message.Request.arenaChallengeReq.ArenaInfo.Blue = new ArenaPlayer()
            {
                EntityId = targetId,
                Name = targetName,
            };
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 发送挑战响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SendArenaChallengResponse(bool accept, ArenaChallengRequest request)
        {
            Debug.Log("SendArenaChallengeResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.arenaChallengeRes = new ArenaChallengeResponse();
            message.Request.arenaChallengeRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.arenaChallengeRes.Errormsg = accept ? "" : "对方拒绝了您的竞技场挑战请求";
            message.Request.arenaChallengeRes.ArenaInfo = request.ArenaInfo;
            NetClient.Instance.SendMessage(message);
        }

        private void OnArenaChallengeRequest(object sender, ArenaChallengRequest request)
        {
            Debug.Log("OnArenaChallengeRequest");
            var confirm = MessageBox.Show(string.Format("{0}请求挑战您", request.ArenaInfo.Red.Name), "竞技场挑战", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () => { this.SendArenaChallengResponse(true, request); };
            confirm.OnNo = () => { this.SendArenaChallengResponse(false, request); };
        }

        private void OnArenaChallengeResponse(object sender, ArenaChallengeResponse message)
        {
            Debug.Log("OnArenaChallengeResponse");
            if (message.Result != Result.Success)
                MessageBox.Show(message.Errormsg, "对方拒绝挑战");
        }

        private void OnArenaBegin(object sender, ArenaBeginResponse message)
        {
            Debug.Log("OnArenaBegin");
            //ArenaManager.Instance.EnterArena(message.ArenaInfo);
        }

        private void OnArenaEnd(object sender, ArenaEndResponse message)
        {
            Debug.Log("OnArenaEnd");
            //ArenaManager.Instance.ExitArena(message.ArenaInfo);
        }
    }
}
