using UnityEngine;

public enum SkillTreeCanvases
{
    Player,
    Base
}

public class SkillTreeUImanager : MonoBehaviour
{
    [SerializeField] private GameObject _playerSkillTreeUI;
    [SerializeField] private GameObject _baseSkillTreeUI;

    private SkillTreeCanvases _currentCanvas;

    private void Start()
    {
        _currentCanvas = SkillTreeCanvases.Base;
        _playerSkillTreeUI.SetActive(false);
        _baseSkillTreeUI.SetActive(true);
    }

    private void CloseCanvas()
    {
        switch (_currentCanvas)
        {
            case SkillTreeCanvases.Player:
                _playerSkillTreeUI.SetActive(false);
                break;
            case SkillTreeCanvases.Base:
                _baseSkillTreeUI.SetActive(false);
                break;
        }
    }

    #region buttons

    public void OnPlayerSkillTreeButtonClicked()
    {
        CloseCanvas();
        _currentCanvas = SkillTreeCanvases.Player;
        _playerSkillTreeUI.SetActive(true);
    }
    public void OnBaseSkillTreeButtonClicked()
    {
        CloseCanvas();
        _currentCanvas = SkillTreeCanvases.Base;
        _baseSkillTreeUI.SetActive(true);
    }

    #endregion
}