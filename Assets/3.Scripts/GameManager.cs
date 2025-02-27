using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public UIDocument UIDoc;

    // property
    public TurnManager turnManager { get; private set; }

    private int m_FoodAmount = 100;
    private Label m_FoodLabel;

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
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = $"Food :  {m_FoodAmount}";
        
        turnManager = new TurnManager();
        turnManager.OnTick += OnTurnHappen;

        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }

    void OnTurnHappen()
    {
        m_FoodAmount -= 1;
        m_FoodLabel.text = $"Food :  {m_FoodAmount:000}";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
