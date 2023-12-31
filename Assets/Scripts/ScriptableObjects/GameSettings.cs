using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Config/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public string startingScene;
    public bool gameBattleDebugMode;
    public bool playerDeckDebugMode;
    public bool playLoadingScreen;

    [Header("ANIMATIONS")]
    public float delayBeforeAnimation;
    public float abilityAnimationDuration;
    public float delayAfterAnimation;
    public float verticalOffsetDuringDrag;
    public float verticalOffsetBlobShadow;

    [Header("GAMEPLAY")]
    public float holdingTimeMaxSingleClick;
    public float holdingTimeToTriggerAbility;
    public float dragingMinimumAmount;
    public Vector3[] stackedHandPointPosition = new Vector3[4];
    public Vector3 highlightedHandPointPosition;

    [Header("EFFECTS")]
    public int dotDamageAmount;
    public int hotHealAmount;
    [Range(0f, 1f)] public float damageResistanceModifier;
    public int antiHealAmout;
    public int buffArmorAmount;
    public int debuffArmorAmout;
    public int buffAttackAmout;
    public int debuffAttackAmout;

    [Header("BONFIRE SCENE")] 
    public int healAmount;
    
    [Header("SCROLLING TEXT EFFECT")]
    public float scrollingFeedbackLifetime;
    public float defaultFontSize;
    public float effectFontSize;
    public Color colorTextDamage;
    public Color colorTextHeal;
    public Color colorTextArmor;
    public Color colorTextEffect;
}