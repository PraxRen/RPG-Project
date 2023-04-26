using RPG.Core.UI.Tooltips;
using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestsTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus status = GetComponent<QuestItemUI>().QuestStatus;
            QuestTooltipUI questTooltipUI = tooltip.GetComponent<QuestTooltipUI>();
            questTooltipUI.Setup(status);
        }
    }
}
