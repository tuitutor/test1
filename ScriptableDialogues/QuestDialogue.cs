// this should probably be created at runtime for a quest etc.
// (via ScriptableObject.CreateInstance<QuestDialogue>())
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName="uRPG Dialogue/Quest Dialogue", order=999)]
public class QuestDialogue : ScriptableDialogue
{
    public ScriptableQuest quest;
    public string acceptText = "Accept";
    public string completeText = "Complete";
    public string rejectText = "Close";

    public override string GetText(GameObject player)
    {
        // find quest index in player quest list
        PlayerQuests playerQuests = player.GetComponent<PlayerQuests>();
        int questIndex = playerQuests.GetQuestIndexByName(quest.name);

        // running quest: shows description with current progress
        if (questIndex != -1)
        {
            Quest quest = playerQuests.quests[questIndex];
            return quest.ToolTip(player);
        }
        // new quest
        else
        {
            return new Quest(quest).ToolTip(player);
        }
    }

    public override List<DialogueChoice> GetChoices(GameObject player)
    {
        PlayerQuests playerQuests = player.GetComponent<PlayerQuests>();
        List<DialogueChoice> result = new List<DialogueChoice>();

        // accept button if we can accept it
        if (playerQuests.CanAccept(quest))
        {
            result.Add(new DialogueChoice(
                acceptText,
                true,
                (() => {
                    playerQuests.Accept(quest);
                    UINpcDialogue.singleton.Hide();
                })));
        }

        // complete button if we have this quest
        if (playerQuests.HasActive(quest.name))
        {
            result.Add(new DialogueChoice(
                completeText,
                playerQuests.CanComplete(quest.name),
                (() => {
                    playerQuests.Complete(quest);
                    UINpcDialogue.singleton.Hide();
                })));
        }

        // reject
        result.Add(new DialogueChoice(rejectText, true, UINpcDialogue.singleton.Hide));

        return result;
    }
}
