using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour {
    [SerializeField] private Button m_tileButton;
    [SerializeField] private GameObject[] m_slots;

    private List<Button> m_buttonArray = new List<Button>();
    private int m_size;
    private int m_numOfBlocks;

    private int[] m_gameArray;
    private int m_clickedButtonSlotIndex;
    private int m_blankPosition;

    private bool m_isButtonClicked = false;
    private bool m_winCheck = false;
    private int[] m_winArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0 };
 
    void Start() {
        m_size = 4;
        m_numOfBlocks = (m_size * m_size) - 1;
        m_gameArray = new int[m_size * m_size];
    }

    void Update() {
        Move();
    }

    private void Move() {

        if (m_isButtonClicked) {

            int buttonIndex = (m_clickedButtonSlotIndex > m_blankPosition) ? m_clickedButtonSlotIndex - 1 :
                m_clickedButtonSlotIndex;

            Button clickedButton = m_buttonArray[m_gameArray[m_clickedButtonSlotIndex]-1];
            GameObject slot = m_slots[m_blankPosition];
            
            Vector2 startPosition = clickedButton.transform.position;
            Vector2 endPosition = slot.transform.position;
            
            clickedButton.transform.position = Vector2.Lerp(startPosition, endPosition, 2.0f);


            m_gameArray[m_blankPosition] = m_gameArray[m_clickedButtonSlotIndex];
            m_gameArray[m_clickedButtonSlotIndex] = 0;

            m_blankPosition = m_clickedButtonSlotIndex;
            m_isButtonClicked = false;

            checkWin();
        }
    }


    private void checkWin() {
        bool flag = true;
        for(int i=0; i<m_gameArray.Length; i++) {
            if(m_gameArray[i] != m_winArray[i]) {
                flag = false;
                break;
            }
        }
        if (flag)
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    // TODO: this can seriously use some refactor, come back later
    public void setUpGame() {

        newGame();
        
        for (int i = 0; i < m_gameArray.Length; i++) {
        
            Vector2 position = m_slots[i].transform.position;

            if (m_gameArray[i] != 0) {
            
                Button gameButton = Instantiate(m_tileButton, position, Quaternion.identity, this.transform);
                gameButton.GetComponentInChildren<Text>().text = m_gameArray[i].ToString();
                
                m_buttonArray.Add(gameButton);
                int buttonValue = Int16.Parse(gameButton.GetComponentInChildren<Text>().text);
                gameButton.onClick.AddListener(() => onButtonClicked(buttonValue));
            }
            else {
            
                m_blankPosition = i;
            }
        }
    }

    private void onButtonClicked(int buttonIndex) {

        m_clickedButtonSlotIndex = Array.IndexOf(m_gameArray, buttonIndex);

        if (isClickable(m_clickedButtonSlotIndex)) 
            m_isButtonClicked = true;

    }


    private bool isClickable(int buttonIndex) {
        int top = (buttonIndex < m_size) ? -1 : buttonIndex - m_size;
        int bottom = (buttonIndex + m_size > m_numOfBlocks) ? -1 : buttonIndex + m_size;
        int left = (buttonIndex % m_size == 0) ? -1 : buttonIndex - 1;
        int right = (buttonIndex % m_size == (m_size - 1)) ? -1 : buttonIndex + 1;

        print("Blank : "+ m_blankPosition);
        print("Indexes : " + (top + " " + bottom + " " + left + " " + right));
        print("Current button : " + buttonIndex);

        if(top == m_blankPosition 
            || bottom == m_blankPosition
            || left == m_blankPosition
            || right == m_blankPosition) 
        return true;
        
        return false;
    }




    // Array generation

    private void newGame() {
        do {
            reset();
            shuffle();
        } while (!isSolvable());

        string result = string.Join(",", m_gameArray);
        print(result);
    }

    private void reset() {
        for (int i = 0; i < m_gameArray.Length; i++)
            m_gameArray[i] = (i + 1) % m_gameArray.Length;
        m_blankPosition = m_gameArray.Length - 1;
    }

    private void shuffle() {
        int n = m_numOfBlocks;

        while (n > 1) {
            int r = UnityEngine.Random.Range(0, 15);
            int temp = m_gameArray[r];
            m_gameArray[r] = m_gameArray[n];
            m_gameArray[n] = temp;
            n--;
        }
    }

    private bool isSolvable() {
        int countNoOfInversions = 0;

        for (int i = 0; i < m_numOfBlocks; i++)
            for (int j = 0; j < i; j++)
                if (m_gameArray[j] > m_gameArray[i])
                    countNoOfInversions++;

        return countNoOfInversions % 2 == 0;
    }
}
