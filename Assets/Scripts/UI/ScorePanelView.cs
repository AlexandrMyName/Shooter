using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanelView : MonoBehaviour
{
    [SerializeField] private Image _panelBackground;
    [SerializeField] private TMP_Text _placeInLeaderBoard;
    [SerializeField] private TMP_Text _nickName;
    [SerializeField] private TMP_Text _score;

    public void SetParameters(int place, string nickname, int score)
    {
        _placeInLeaderBoard.text = place.ToString();
        _nickName.text = nickname;
        _score.text = score.ToString();
    }

    public void MarkPanel()
    {
        _panelBackground.color = Color.red;
    }
}
