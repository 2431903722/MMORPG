using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class QuestManager
    {
        Character Owner;

        public QuestManager(Character owner)
        {
            Owner = owner;
        }

        public void GetQuestInfos(List<NQuestInfo> list)
        {
            foreach(var quest in this.Owner.Data.Quests)
            {
                list.Add(GetQuestInfo(quest));
            }
        }

        public NQuestInfo GetQuestInfo(TCharacterQuest quest)
        {
            return new NQuestInfo()
            {
                QuestId = quest.QuestId,
                QuestGuid = quest.Id,
                Status = (QuestStatus)quest.Status,
                Targes = new int[3]
                {
                    quest.Target1,
                    quest.Target2,
                    quest.Target3
                }
            };
        }

        public Result AcceptQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;

            QuestDefine quest;
            if(DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                var dpquest = DBService.Instance.Entities.CharacterQuests.Create();
                dpquest.QuestId = quest.ID;
                if(quest.Target1 == QuestTarget.None)
                {   //没有完成目标的任务
                    dpquest.Status = (int)QuestStatus.Complated;
                }
                else
                {   //有目标的任务
                    dpquest.Status = (int)QuestStatus.InProgress;
                }
                sender.Session.Response.questAccept.Quest = this.GetQuestInfo(dpquest);
                character.Data.Quests.Add(dpquest);
                DBService.Instance.Save();
                return Result.Success;
            }
            else
            {
                sender.Session.Response.questAccept.Errormsg = "任务不存在";
                return Result.Failed;
            }
        }

        public Result SubmitQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;

            QuestDefine quest;
            if (DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                var dpquest = character.Data.Quests.Where(q => q.QuestId == questId).FirstOrDefault();
                if (dpquest != null)
                {
                    if (dpquest.Status != (int)QuestStatus.Complated)
                    {
                        //还不是完成状态
                        sender.Session.Response.questSubmit.Errormsg = "任务未完成";
                        return Result.Failed;
                    }
                    dpquest.Status = (int)QuestStatus.Finished;
                    sender.Session.Response.questSubmit.Quest = this.GetQuestInfo(dpquest);
                    DBService.Instance.Save();

                    //处理任务奖励
                    if (quest.RewardGold > 0)
                    {
                        character.Gold += quest.RewardGold;
                    }
                    if (quest.RewardExp > 0)
                    {
                        //character.AddExp(quest.RewardExp);
                    }
                    if (quest.RewardItem1 > 0)
                    {
                        character.ItemManager.AddItem(quest.RewardItem1, quest.RewardItem1Count);
                    }
                    if (quest.RewardItem2 > 0)
                    {
                        character.ItemManager.AddItem(quest.RewardItem2, quest.RewardItem2Count);
                    }
                    if (quest.RewardItem3 > 0)
                    {
                        character.ItemManager.AddItem(quest.RewardItem3, quest.RewardItem3Count);
                    }
                    DBService.Instance.Save();
                    return Result.Success;
                }
                sender.Session.Response.questSubmit.Errormsg = "任务不存在[2]";
                return Result.Failed;
            }
            else
            {
                sender.Session.Response.questSubmit.Errormsg = "任务不存在[1]";
                return Result.Failed;
            }
        }
    }
}
