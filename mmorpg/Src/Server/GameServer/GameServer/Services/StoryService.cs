using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class StoryService : Singleton<StoryService>
    {
        public StoryService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<StoryStartRequest>(this.OnStoryStart);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<StoryEndRequest>(this.OnStoryEnd);
        }

        public void Dispose()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<StoryStartRequest>(this.OnStoryStart);
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<StoryEndRequest>(this.OnStoryEnd);
        }

        public void Init()
        {
            StoryManager.Instance.Init();
        }

        private void OnStoryStart(NetConnection<NetSession> sender, StoryStartRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnStoryStart :: storyId: {0}", request.storyId);
            var story = StoryManager.Instance.NewStory(request.storyId, sender);
            sender.Session.Response.storyStart = new StoryStartResponse();
            sender.Session.Response.storyStart.storyId = story.StoryId;
            sender.Session.Response.storyStart.instanceId = story.InstanceId;
            sender.Session.Response.storyStart.Result = Result.Success;
            sender.Session.Response.storyStart.Errormsg = "";
            sender.SendResponse();
        }

        private void OnStoryEnd(NetConnection<NetSession> sender, StoryEndRequest request)
        {
            var story = StoryManager.Instance.GetStory(request.storyId, request.instanceId);
            story.End();
            sender.Session.Response.storyEnd = new StoryEndResponse();
            sender.Session.Response.storyEnd.storyId = story.StoryId;
            sender.Session.Response.storyEnd.instanceId = story.InstanceId;
            sender.Session.Response.storyEnd.Result = Result.Success;
            sender.SendResponse();
        }
    }
}
