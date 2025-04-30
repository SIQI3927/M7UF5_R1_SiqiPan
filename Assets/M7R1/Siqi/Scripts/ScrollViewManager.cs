using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject listItemPrefab; // Asigna el prefab en el Inspector
    public Transform content; // Asigna el Content del Scroll View en el Inspector

    private List<string> data = new List<string> { "Jugador 1", "Jugador 2", "Jugador 3", "Jugador 4" };

    void Start()
    {
        PopulateList();
    }

    void PopulateList()
    {
        foreach (string item in data)
        {
            GameObject newItem = Instantiate(listItemPrefab, content);
            newItem.GetComponentInChildren<TMP_Text>().text = item; // Si usas TextMeshPro
        }
    }
}
