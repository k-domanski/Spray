using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USM = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    #region Public
    public AsyncOperation LoadSceneAsync(GameScene gameScene)
    {
        return USM.SceneManager.LoadSceneAsync((int)gameScene);
    }
    public void LoadScene(GameScene gameScene)
    {
        USM.SceneManager.LoadScene((int)gameScene);
    }
    #endregion
}
