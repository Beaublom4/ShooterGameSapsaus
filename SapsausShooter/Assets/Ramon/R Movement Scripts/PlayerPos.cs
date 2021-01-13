using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerPos : MonoBehaviour
{
    public GameObject player;
    Checkpoint spawnLocation;
    HealthManager health;
    public Button respawnButton;

    public void Start()
    {
        Button button = respawnButton.GetComponent<Button>();
        button.onClick.AddListener(SpawnPlayer);
    }
    void SpawnPlayer()
    {
        Instantiate(player, spawnLocation.lastCheckPointPos);
    }
}
