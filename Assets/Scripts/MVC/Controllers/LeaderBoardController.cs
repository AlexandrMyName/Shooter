using System.Collections.Generic;
using Core.ResourceLoader;
using MVC.Core.Factory;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Providers;
using MVC.Views;
using SavableData;
using UI;
using UnityEngine;

namespace MVC.Controllers
{
    public class LeaderBoardController : IInitialization, ICleanUp
    {
        private LeaderBoardView _leaderBoardView;
        private MainMenuView _mainMenuView;
        
        private PlayerScoreList _playerScoreList;
        private PlayerScoreList _globalScoreList;
        private GameObject _scorePanelPrefab;
        private RectTransform _leaderBoardContentLayout;
        
        private List<GameObject> _activeScorePanels;

        public LeaderBoardController(IViewProvider viewProvider)
        {
            _leaderBoardView = viewProvider.GetView<LeaderBoardView>();
            _mainMenuView = viewProvider.GetView<MainMenuView>();
        }
        
        public void Initialisation()
        {
            _leaderBoardView.BackButton.onClick.AddListener(BackButtonAction);
            _leaderBoardView.GlobalButton.onClick.AddListener(ShowGlobalScoreBoard);
            _leaderBoardView.SelfButton.onClick.AddListener(ShowSelfScoreBoard);
            _leaderBoardContentLayout = _leaderBoardView.ContentRectTransform;
            _activeScorePanels = new List<GameObject>();
            _playerScoreList = ResourceLoadManager.GetConfig<PlayerScoreList>("ScoreData");
            _globalScoreList = ResourceLoadManager.GetConfig<PlayerScoreList>("GlobalStatisticFake");
            _scorePanelPrefab = ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("ScorePanel");
            ShowGlobalScoreBoard();
        }

        public void Cleanup()
        {
            _leaderBoardView.BackButton.onClick.RemoveListener(BackButtonAction);
            _leaderBoardView.GlobalButton.onClick.RemoveListener(ShowGlobalScoreBoard);
            _leaderBoardView.SelfButton.onClick.RemoveListener(ShowSelfScoreBoard);
        }
        
        
        private void BackButtonAction()
        {
            HideActiveScorePanels();
            _mainMenuView.Show();
            _leaderBoardView.Hide();
        }

        private void ShowSelfScoreBoard()
        {
            HideActiveScorePanels();
            int i = 0;
            foreach (int score in _playerScoreList.ScoreList)
            {
                i++;
                GameObject scorePanel = GameObject.Instantiate(_scorePanelPrefab, _leaderBoardContentLayout);
                ScorePanelView scorePanelView = scorePanel.GetComponent<ScorePanelView>();
                scorePanelView.SetParameters(i, "Test", score);
                _activeScorePanels.Add(scorePanel);
            }
        }

        private void ShowGlobalScoreBoard()
        {
            HideActiveScorePanels();
            bool isPlayerScoreInBoard = false;
            int playerHighScore = 0;
            if (_playerScoreList.ScoreList.Count != 0)
            {
                playerHighScore = _playerScoreList.ScoreList[0];
            }
            else
            {
                isPlayerScoreInBoard = true;
            }
            int currentPlace = 1;
            
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

        private GameObject Instantiate(GameObject gameObject, RectTransform parent)
        {
            return GameObject.Instantiate(gameObject, parent);
        }

        private void HideActiveScorePanels()
        {
            if (_activeScorePanels.Count > 0)
            {
                foreach (GameObject scorePanel in _activeScorePanels)
                {
                    GameObject.Destroy(scorePanel);
                }
            }
            _activeScorePanels.Clear();
        }
    }
}