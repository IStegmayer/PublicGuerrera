using System;
using Cinemachine;
using Dzajna;
using UnityEngine;

public class GameManager : StaticInstance<GameManager>
{
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private Transform _enemySpawn;
    [SerializeField] private Transform _enemySpawn2;
    [SerializeField] private Transform _enemySpawn3;
    [SerializeField] private Transform _enemySpawn4;
    [SerializeField] private Transform _mainCamera;
    [SerializeField] private UIManager _uiManager;
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState CurrentGameState { get; private set; }

    // Kick the game off with the first state   
    void Start() => ChangeState(GameState.Starting);
    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        CurrentGameState = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawningHeroes:
                HandleSpawningHeroes();
                break;
            case GameState.SpawningEnemies:
                HandleSpawningEnemies();
                break;
            case GameState.HeroTurn:
                HandleHeroTurn();
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        // Debug.Log($"New state: {newState}");
    }


    private void HandleStarting() {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state
        
        ChangeState(GameState.SpawningHeroes);
    }

    private void HandleSpawningHeroes() {
        // Transform playerInstance = UnitManager.Instance.SpawnPlayer(_playerSpawn);
        // PlayerCamera.Instance.SetTarget(playerInstance.GetComponent<PlayerManager>());
        // _uiManager.CharacterInventoryManager = playerInstance.GetComponent<CharacterInventoryManager>();
        ChangeState(GameState.SpawningEnemies);
    }

    private void HandleWin() {
        AudioSystem.Instance.PlaySound("gameWin", Vector3.zero);
    }
    private void HandleLose() {
        AudioSystem.Instance.PlaySound("gameLose", Vector3.zero);
    }

    private void HandleSpawningEnemies() {
        UnitManager.Instance.SpawnEnemy(_enemySpawn, EnemyType.Shaman);
        UnitManager.Instance.SpawnEnemy(_enemySpawn2, EnemyType.Shaman);
        UnitManager.Instance.SpawnEnemy(_enemySpawn3, EnemyType.Goblin);
        // UnitManager.Instance.SpawnEnemy(_enemySpawn4, EnemyType.BossGoblin);
        
        ChangeState(GameState.HeroTurn);
    }

    private void HandleHeroTurn() {
        // If you're making a turn based game, this could show the turn menu, highlight available units etc
        
        // Keep track of how many units need to make a move, once they've all finished, change the state. This could
        // be monitored in the unit manager or the units themselves.
    }
}
    /// <summary>
    /// This is obviously an example and I have no idea what kind of game you're making.
    /// You can use a similar manager for controlling your menu states or dynamic-cinematics, etc
    /// </summary>
    [Serializable]
    public enum GameState
    {
        Starting = 0,
        SpawningHeroes = 1,
        SpawningEnemies = 2,
        HeroTurn = 3,
        Win = 4,
        Lose = 5,
    }
