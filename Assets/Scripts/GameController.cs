using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class GameController : MonoBehaviour
{
    public MapController mc;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckClick();
    }

    // Return the gameobject that was clicked on.
    private void CheckClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                mc.Build(hit.transform.position, "Turret");
            }
        }
    }
}