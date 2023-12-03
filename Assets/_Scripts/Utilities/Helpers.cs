using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Set of functions accessible from any class
/// </summary>
public static class Helpers
{
    public static void DestroyChildren(this UnityEngine.Transform t)
    {
        foreach (UnityEngine.Transform child in t) Object.Destroy(child.gameObject);
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}