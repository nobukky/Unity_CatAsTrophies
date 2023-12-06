using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndBattleUIManager : MonoBehaviour
{
    [Header("END TITLE")] 
    public GameObject endTitleParent;
    public TextMeshProUGUI endTitleTM;
    public Image endTitleLoadingImage;
    public float endTitleDuration;
    
    /// <summary>
    /// Shows end battle title screen
    /// </summary>
    /// <param name="doWin">TRUE display "victory" - FALSE display "defeat"</param>
    public async Task AnimateEndTitle(bool doWin)
    {
        endTitleParent.SetActive(true);

        endTitleTM.text = doWin ? "VICTORY" : "DEFEAT";

        endTitleLoadingImage.DOFillAmount(1, endTitleDuration);
        await Task.Delay(Mathf.RoundToInt(1000 * endTitleDuration));
        
        endTitleParent.SetActive(false);
    }
}