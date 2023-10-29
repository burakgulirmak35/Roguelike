using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Player player;

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>(true);
    }

    void Start()
    {
        Enviroment.Instance.CreateCity();
        player.StartGame();
    }
}
