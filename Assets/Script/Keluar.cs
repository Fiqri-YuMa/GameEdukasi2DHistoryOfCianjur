using UnityEngine;
using UnityEngine.SceneManagement;

public class Keluar : MonoBehaviour
{
    public void exit()
    {
        SceneManager.LoadScene(0);
    }
}
