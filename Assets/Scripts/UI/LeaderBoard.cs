using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private UIMainMenuScreen _uiMainMenuScreen;
    [SerializeField] private GameObject _scorePanelPrefab;
    [SerializeField] private PlayerScoreList _playerScoreList;
    [SerializeField] private PlayerScoreList _globalScoreList;
    [SerializeField] private RectTransform _leaderBoardContentLayout;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _globalButton;
    [SerializeField] private Button _selfButton;

    private List<GameObject> _activeScorePanels;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(BackButtonAction);
        _globalButton.onClick.AddListener(ShowGlobalScoreBoard);
        _selfButton.onClick.AddListener(ShowSelfScoreBoard);
        _activeScorePanels = new List<GameObject>();
        ShowSelfScoreBoard();
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(BackButtonAction);
        _globalButton.onClick.RemoveListener(ShowGlobalScoreBoard);
        _selfButton.onClick.RemoveListener(ShowSelfScoreBoard);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void BackButtonAction()
    {
        HideActiveScorePanels();
        _uiMainMenuScreen.Show();
        Hide();
    }

    private void ShowSelfScoreBoard()
    {
        HideActiveScorePanels();
        int i = 0;
        foreach (int score in _playerScoreList.ScoreList)
        {
            i++;
            GameObject scorePanel = Instantiate(_scorePanelPrefab, _leaderBoardContentLayout);
            ScorePanelView scorePanelView = scorePanel.GetComponent<ScorePanelView>();
            scorePanelView.SetParameters(i, "Test", score);
            _activeScorePanels.Add(scorePanel);
        }
    }

    private void ShowGlobalScoreBoard()
    {
        HideActiveScorePanels();

        int playerHighScore = _playerScoreList.ScoreList[0];
        int currentPlace = 1;
        bool isPlayerScoreInBoard = false;
        for (int i = 0; i < _globalScoreList.MaxListCapacity; i++)
        {
            int score = _globalScoreList.ScoreList[i];
            GameObject scorePanel = Instantiate(_scorePanelPrefab, _leaderBoardContentLayout);
            ScorePanelView scorePanelView = scorePanel.GetComponent<ScorePanelView>();
            if (playerHighScore > score && !isPlayerScoreInBoard)
            {
                scorePanelView.SetParameters(currentPlace, "Test", playerHighScore);
                scorePanelView.MarkPanel();
                currentPlace++;
                _activeScorePanels.Add(scorePanel);
                isPlayerScoreInBoard = true;
                scorePanel = Instantiate(_scorePanelPrefab, _leaderBoardContentLayout);
                scorePanelView = scorePanel.GetComponent<ScorePanelView>();
                scorePanelView.SetParameters(currentPlace, "Test", score);
                currentPlace++;
            }
            else
            {
                scorePanelView.SetParameters(currentPlace, "Test", score);
                currentPlace++;
            }
            _activeScorePanels.Add(scorePanel);
        }

        if (!isPlayerScoreInBoard)
        {
            for (int n = _globalScoreList.MaxListCapacity; n < _globalScoreList.ScoreList.Count; n++)
            {
                int score = _globalScoreList.ScoreList[n];
                if (playerHighScore > score && !isPlayerScoreInBoard)
                {
                    isPlayerScoreInBoard = true;
                    GameObject scorePanel = Instantiate(_scorePanelPrefab, _leaderBoardContentLayout);
                    ScorePanelView scorePanelView = scorePanel.GetComponent<ScorePanelView>();
                    scorePanelView.MarkPanel();
                    scorePanelView.SetParameters(currentPlace, "Test", playerHighScore);
                    _activeScorePanels.Add(scorePanel);
                    break;
                }
                else if (n == _globalScoreList.ScoreList.Count - 1)
                {
                    isPlayerScoreInBoard = true;
                    GameObject scorePanel = Instantiate(_scorePanelPrefab, _leaderBoardContentLayout);
                    ScorePanelView scorePanelView = scorePanel.GetComponent<ScorePanelView>();
                    scorePanelView.MarkPanel();
                    scorePanelView.SetParameters(currentPlace, "Test", playerHighScore);
                    _activeScorePanels.Add(scorePanel);
                }
                currentPlace++;
            }
        }
    }
    
    private void HideActiveScorePanels()
    {
        if (_activeScorePanels.Count > 0)
        {
            foreach (GameObject scorePanel in _activeScorePanels)
            {
                Destroy(scorePanel);
            }
        }
        _activeScorePanels.Clear();
    }
}
