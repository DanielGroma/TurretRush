using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    public void ShowWin()
    {
        _winPanel.SetActive(true);
    }

    public void ShowLose()
    {
        _losePanel.SetActive(true);
    }

    public void Hide()
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
    }
}