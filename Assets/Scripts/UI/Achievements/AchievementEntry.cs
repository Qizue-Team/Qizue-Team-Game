using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementEntry : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private TextMeshProUGUI tierText;
    [SerializeField]
    private TextMeshProUGUI completitionText;
    [SerializeField]
    private TextMeshProUGUI rewardText;

    public void SetEntry(Achievement achievement)
    {
        descriptionText.text = achievement.Data.Description;
        tierText.text = "Tier " + achievement.Data.CurrentTier;
        completitionText.text = achievement.Data.CurrentValue.ToString()+"/"+ achievement.Data.GetMaxValue().ToString();
        if (achievement.Data.GetReward().SkinReward != null && achievement.Data.GetReward().GearReward <= 0)
            rewardText.text = "Reward: " + achievement.Data.GetReward().SkinReward.SkinName+" Skin";
        if (achievement.Data.GetReward().SkinReward == null)
            rewardText.text = "Reward: " + achievement.Data.GetReward().GearReward.ToString() + " Gears";
        if(achievement.Data.GetReward().SkinReward != null && achievement.Data.GetReward().GearReward > 0)
            rewardText.text = "Reward: "+ achievement.Data.GetReward().SkinReward.ToString() + " Skin and " + achievement.Data.GetReward().GearReward.ToString() + " Gears";
    }
}