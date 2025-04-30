using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public static SceneManagment Instance;

    //[SerializeField] public GameObject? hallway;
    //[SerializeField] public GameObject? lobby;
    //[SerializeField] public GameObject? multiplayer;
    //[SerializeField] public GameObject? meetingRoom;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disconnect()
    {
        if (!SceneManager.GetSceneByBuildIndex(1).isLoaded) // Evita cargar duplicados
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive).completed += (operation) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1)); // Activar la nueva escena
            };
        }
    }

    public void GoToLobby()
    {
        if (!SceneManager.GetSceneByBuildIndex(0).isLoaded) // Evita cargar duplicados
        {
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single).completed += (operation) =>
            {
                var scenes = SceneManager.sceneCount;
                for (int i = 0; i < scenes; i++)
                {
                    if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0)); // Activar la nueva escena
            };
        }
    }

    public void GoToHallway()
    {
        if (!SceneManager.GetSceneByBuildIndex(2).isLoaded) // Evita cargar duplicados
        {
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Single).completed += (operation) =>
            {
                var scenes = SceneManager.sceneCount;
                for (int i = 0; i < scenes; i++)
                {
                    if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2)); // Activar la nueva escena
            };
        }
    }

    public void ChangeToClassRoom()
    {
        if (!SceneManager.GetSceneByBuildIndex(3).isLoaded) // Evita cargar duplicados
        {
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Single).completed += (operation) => 
            {
                var scenes = SceneManager.sceneCount;
                for (int i = 0; i < scenes; i++)
                {
                    if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(3)); // Activar la nueva escena
            };
        }
    }

    public void ChangeToMultiplayer()
    {
        if (!SceneManager.GetSceneByBuildIndex(4).isLoaded) // Evita cargar duplicados
        {
            SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive).completed += (operation) =>
            {
                var scenes = SceneManager.sceneCount;
                for (int i = 0; i < scenes; i++)
                {
                    if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(4)); // Activar la nueva escena
            };
        }
    }

    public void GoToPolitecnicoEstella()
    {
        if (!SceneManager.GetSceneByBuildIndex(1).isLoaded) // Evita cargar duplicados
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).completed += (operation) =>
            {
                var scenes = SceneManager.sceneCount;
                for (int i = 0; i < scenes; i++)
                {
                    if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1)); // Activar la nueva escena
            };
        }
    }

    public void GoToLinares()
    {
        if (!SceneManager.GetSceneByBuildIndex(2).isLoaded) // Evita cargar duplicados
        {
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Single).completed += (operation) =>
            {
                var scenes = SceneManager.sceneCount;
                for (int i = 0; i < scenes; i++)
                {
                    if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2)); // Activar la nueva escena
            };
        }
    }
}
