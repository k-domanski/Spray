using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USM = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    #region Public

    public event System.Action onSceneLoad;

    public AsyncOperation LoadSceneAsync(GameScene gameScene)
    {
        onSceneLoad?.Invoke();
        return USM.SceneManager.LoadSceneAsync((int)gameScene);
    }
    public void LoadScene(GameScene gameScene)
    {
        onSceneLoad?.Invoke();
        USM.SceneManager.LoadScene((int)gameScene);
    }
    #endregion
}
