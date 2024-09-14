using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private GameData _gameData;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    
    public static SaveManager instance;
    private List<ISaveManager> _saveManagers;
    private FileDataHandler _fileDataHandler;

    [ContextMenu("usun plik zapisu")]
    public void DeleteSaved()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        _fileDataHandler.DeleteData();
    }
    
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
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        
        _saveManagers = FindAllSaveManagers();
        
        LoadGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();
        Debug.Log("zaladowalem gre");
        
        if (this._gameData == null)
        {
            Debug.Log("nie ma zapisu");
            NewGame();
        }

        foreach (ISaveManager saveManager in _saveManagers)
        {
            saveManager.LoadData(_gameData);
        }
        
        
        Debug.Log("wczytalem currency " + _gameData.currency);
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in _saveManagers)
        {
            saveManager.SaveData(ref _gameData);
        }
        
        _fileDataHandler.Save(_gameData);
        //Debug.Log("gra zapisana " + _gameData.currency);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManager = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManager);
    }

    public bool HasSavedData()
    {
        if (_fileDataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }
}
