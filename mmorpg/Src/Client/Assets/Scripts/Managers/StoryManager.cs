using Common.Data;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class StoryManager : Singleton<StoryManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(NpcFunction.InvokeStory, OnOpenStory);
        }

        private bool OnOpenStory(NpcDefine npc)
        {
            this.ShowStoryUI(npc.Param);
            return true;
        }

        public void ShowStoryUI(int storyId)
        {
            StoryDefine story;
            if (DataManager.Instance.Storys.TryGetValue(storyId, out story))
            {
                UIStory uIStory = UIManager.Instance.Show<UIStory>();
                if (uIStory != null)
                {
                    uIStory.SetStory(story);
                }
            }
        }

        public bool StartStory(int storyId)
        {
            StoryService.Instance.SendStartStory(storyId);
            return true;
        }

        internal void OnStoryStart(int storyId)
        {
            
        }
    }
}
