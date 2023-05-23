using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public GameObject LocationPrefab;
    private int GridWidth = 5;
    private int GridHeight = 5;
    private GameObject[,] Locations;

    // Start is called before the first frame update
    void Start()
    {
        Renderer locationRenderer = LocationPrefab.GetComponent<Renderer>();
        float renderedLocationWidth = locationRenderer.bounds.size.x; // * locationRenderer.transform.localScale.x;
        float renderedLocationHeight = locationRenderer.bounds.size.y; // * locationRenderer.transform.localScale.y;
        float renderedGridWidth = renderedLocationWidth * GridWidth;
        float renderedGridHeight = renderedLocationHeight * GridHeight;

        float xPosShift = renderedLocationWidth / 2 - renderedGridWidth / 2;
        float yPosShift = renderedLocationHeight / 2 - renderedGridHeight / 2;

        Locations = new GameObject[GridWidth, GridHeight];
        for (int i = 0; i < GridWidth * GridHeight; i++)
        {
            int row = i / 5; // calcul de la ligne
            int col = i % 5; // calcul de la colonne

            // calcul de la position réelle en multipliant les valeurs de ligne et de colonne par la taille d'un sprite
            float xPos = col * renderedLocationWidth;
            float yPos = row * renderedLocationHeight;
            Vector3 position =
                new Vector3(xPos + xPosShift, yPos + yPosShift, 0) + transform.position;

            GameObject newLocation = Instantiate(LocationPrefab, position, Quaternion.identity);
            newLocation.transform.parent = transform;
            newLocation.name = "Location " + (i + 1);
            Locations[row, col] = newLocation;
        }
    }

    // Update is called once per frame
    void Update() { }
}
