using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private GameBoard m_gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        m_gameBoard = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GameBoard>();
        m_gameBoard.setUpGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
   