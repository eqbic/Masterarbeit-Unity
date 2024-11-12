using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
    [SerializeField] private UserData _currentUser;
    [SerializeField] private TMP_InputField _userIdInput;
    [SerializeField] private Button _submitButton;
    [SerializeField] private TMP_Dropdown _sceneSelection;
    [SerializeField] private Canvas _menu;

    private string _currentScene = null;
    
    private void Start()
    {
        _userIdInput.text = _currentUser.UserId.ToString();
    }

    private void OnEnable()
    {
        _submitButton.onClick.AddListener(StartApp);
    }

    private void OnDisable()
    {
        _submitButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menu.enabled = !_menu.enabled;
        }
    }

    private void StartApp()
    {
        _currentUser.UserId = int.TryParse(_userIdInput.text, out var userId) ? userId : -1;
        var selectionValues = _sceneSelection.options[_sceneSelection.value].text.Split("-");
        var sceneName = selectionValues[0].Trim();
        var inputType = selectionValues[1].Trim();
        switch (sceneName)
        {
            case "Touch":
                _currentUser.TouchControl = Enum.Parse<TouchControl>(inputType);
                break;
            case "TUI":
                _currentUser.TuioControl = Enum.Parse<TuiControl>(inputType);
                break;
        }
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        if (_currentScene != null)
        {
            var asyncUnload = SceneManager.UnloadSceneAsync(_currentScene);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            _currentScene = null;
        }
        
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _currentScene = sceneName;
        _menu.enabled = !_menu.enabled;
    }
}