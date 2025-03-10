using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    #region  pubilc
    // Singleton
    public static GameManager Instance { get; private set; }
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public UIDocument UIDoc;


    // property
    public TurnManager turnManager { get; private set; }
    #endregion

    #region private
    private const int START_FOOD_AMOUNT = 30;
    private int m_FoodAmount = 3;
    private Label m_FoodLabel;
    private int m_CurrentLevel;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    private AudioSource audioSource;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = $"Food :  {m_FoodAmount}";

        turnManager = new TurnManager();
        turnManager.OnTick += OnTurnHappen;

        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");

        m_GameOverPanel.style.visibility = Visibility.Hidden;

        StartNewGame();
    }


    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        m_CurrentLevel = 0;
        m_FoodAmount = START_FOOD_AMOUNT;
        m_FoodLabel.text = $"Food : {m_FoodAmount}";

        PlayerController.Init();

        /*
        BoardManager.Clean();
        BoardManager.Init();       
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
        */
        NewLevel();
    }


    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

        m_CurrentLevel++;
    }

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = $"Food :  {m_FoodAmount:000}";

        if (m_FoodAmount <= 0)
        {
            PlayerController.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = $"Game Over! \n\nYou traveled\n through {m_CurrentLevel} Levels\n\n(Press Enter to\n NEW Game) ";
        }
    }

    // Update is called once per frame
    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
