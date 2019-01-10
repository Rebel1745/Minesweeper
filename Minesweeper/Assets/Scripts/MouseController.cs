using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public GraphicRaycaster m_Raycaster;
    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;

    public Tile tileUnderMouse;
    public bool isDoubleClicking = false;

    void Update()
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
                tileUnderMouse = result.gameObject.GetComponent<Tile>();
            }
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            isDoubleClicking = true;
        }
        else
        {
            if (!isDoubleClicking)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if(gm.isGameStarted)
                        StartCoroutine(tileUnderMouse.Reveal());
                    else
                    {
                        gm.StartGame(tileUnderMouse);
                    }
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    if (!tileUnderMouse.isRevealed)
                    {
                        tileUnderMouse.FlagTile();
                    }                    
                }                    
            }
            else
            {
                isDoubleClicking = false;
                tileUnderMouse.RevealNeighbours();
            }
            
        }
    }
}
