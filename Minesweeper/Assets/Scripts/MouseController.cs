using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

    public GraphicRaycaster m_Raycaster;
    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Tile"))
                {
                    if (Input.GetMouseButtonDown(0))
                        StartCoroutine(result.gameObject.GetComponent<Tile>().Reveal());
                    else if (Input.GetMouseButtonDown(1))
                        result.gameObject.GetComponent<Tile>().FlagTile();
                }
            }
        }
    }

    /*
	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    GameManager gameManager;
    public Tile tileUnderMouse;
    public Tile tileLastUnderMouse;

    public LayerMask LayerIDForTiles;

    // Update is called once per frame
    void Update () {
        tileUnderMouse = MouseToTile();

        HighlightTile();
        
        tileLastUnderMouse = tileUnderMouse;
    }

    Tile MouseToTile()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (mouseRay.direction.z <= 0)
        {
            Debug.LogError("Why is mouse pointing up?");
        }

        int layerMask = LayerIDForTiles.value;

        if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, layerMask))
        {
            // something got hit
            GameObject tileGO = hitInfo.rigidbody.gameObject;

            return gameManager.GetTileFromGameObject(tileGO);
        }

        //Debug.Log("Found Nothing");
        return null;
    }

    void HighlightTile()
    {
        if ((tileUnderMouse != null || tileLastUnderMouse != null) && tileUnderMouse != tileLastUnderMouse)
        {
            SpriteRenderer currentTileRenderer, lastTileRenderer;

            if (tileUnderMouse != null)
            {
                currentTileRenderer = tileUnderMouse.GetComponentInChildren<SpriteRenderer>();
                currentTileRenderer.color = tileUnderMouse.HoverColor;
            }

            if (tileLastUnderMouse != null)
            {
                lastTileRenderer = tileLastUnderMouse.GetComponentInChildren<SpriteRenderer>();
                lastTileRenderer.color = tileUnderMouse.OriginalColour;
            }
        }
    }*/
}
