using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    private GameObject _player;
    
    public static GameManager instance;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;


    [Header("Currency info")] 
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyXPosition;
    [SerializeField] private float lostCurrencyYPosition;
    
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        _player = PlayerManager.instance.player.gameObject;
    }


    public void RestartScene()
    {
        Debug.Log("zapisuje gre przed restartem");
        SaveManager.instance.SaveGame();
        
        Scene scene = SceneManager.GetActiveScene();
        
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData data)
    {
        StartCoroutine(LoadWithDelay(data));
    }

    private void LoadCheckpoints(GameData data)
    {
        foreach (var pair in data.checkpoints)
        {
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData data)
    {
        lostCurrencyAmount = data.lostCurrencyAmount;
        lostCurrencyXPosition = data.lostCurrencyX;
        lostCurrencyYPosition = data.lostCurrencyY + 1f;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab,
                new Vector3(lostCurrencyXPosition, lostCurrencyYPosition), quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData data)
    {
        yield return new WaitForSeconds(0.3f);
        
        LoadCheckpoints(data);
        LoadLostCurrency(data);
        PlacePlayerAtClosestCheckpoint(data);
    }

    private void PlacePlayerAtClosestCheckpoint(GameData data)
    {
        if (data.closestCheckpointId == null)
        {
            return;
        }
        closestCheckpointId = data.closestCheckpointId;
        
        foreach (var checkpoint in checkpoints)
        {
            //Debug.Log("sprawdzam pozycje " + " pozycja gracza " + _player.transform.position);
            if (closestCheckpointId == checkpoint.id)
            {
                var vector3 = PlayerManager.instance.player.transform.position;
                vector3.x = checkpoint.transform.position.x;
                vector3.y = checkpoint.transform.position.y;
                _player.transform.position = vector3;
                //Debug.Log("zmienilem pozycje " + checkpoint.transform.position + "nowa pozycja gracza " + _player.transform.position);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.lostCurrencyAmount = lostCurrencyAmount;
        data.lostCurrencyX = _player.transform.position.x;
        data.lostCurrencyY = _player.transform.position.y;

        if (FindClosestCheckpoint() != null)
        {
            data.closestCheckpointId = FindClosestCheckpoint().id;
        }
        data.checkpoints.Clear();
        
        foreach (Checkpoint checkpoint in checkpoints)
        {
            data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;
        
        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(_player.transform.position,
                checkpoint.transform.position);
            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
