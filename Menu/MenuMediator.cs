using UnityEngine;
using UnityEngine.UI;

public class MenuMediator : MonoBehaviour
{
    [SerializeField] private GameObject[] _panels;
    private GameObject _currentPanel;

    private void Start()
    {
        for (int i = 0, length = _panels.Length; i < length; i++)
        {
            if (_panels[i].activeSelf)
            {
                _currentPanel = _panels[i];
            }
        }
    }
    public void ChangePanelByName(string name) => ChangePanel(GetPanelByName(name));
    public void ChangePanelByIndex(int index) => ChangePanel(_panels[index]);
    public void ExitApp() => Application.Quit();

    private void ChangePanel(GameObject newPanel)
    {
        _currentPanel?.SetActive(false);
        _currentPanel = newPanel;
        _currentPanel.SetActive(true);
    }

    private GameObject GetPanelByName(string name)
    {
        for (int i = 0, length = _panels.Length; i < length; i++)
        {
            if (_panels[i].name == name)
            {
                return _panels[i];
            }
        }

        throw new System.Exception("Cannot find panel with name: " + name);
    }
}
