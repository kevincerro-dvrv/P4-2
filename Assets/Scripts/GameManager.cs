using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Player> players = new();
    public Player activePlayer;

    private Camera gameCamera;

    void Awake()
    {
        instance = this;
        gameCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = gameCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                activePlayer.SetDestination(hit.point);
            }
        }
    }
}
