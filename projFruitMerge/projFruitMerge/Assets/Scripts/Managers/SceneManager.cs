using UnityEngine.SceneManagement;

static class SceneLoaderManager
{
    public static void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
