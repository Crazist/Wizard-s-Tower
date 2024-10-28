using UnityEngine;
using Zenject;
using Factory;

public class Bootstrapper : MonoBehaviour
{
    private UIFactory _uiFactory;
   
    [Inject]
    public void Construct(UIFactory uiFactory)
    {
        _uiFactory = uiFactory;
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        _uiFactory.CreateUI();
        _uiFactory.CreateJoystick();
        _uiFactory.CreateSkillsWindow();
        _uiFactory.CreateStatsWindow();
    }
}