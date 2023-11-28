using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePossible : MonoBehaviour
{
    public GameObject GameManagerRef;

    GameObject _reference = null;

    int _matrixX;
    int _matrixY;


    public bool Attack = false;
    private void Awake()
    {
        GameManagerRef = GameManager.Instance.gameObject;
    }
    private void Start()
    {
        if (Attack == true)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void OnMouseUp()
    {
        var chessPiece = _reference.GetComponent<ChessPiece>();
        if (Attack)
        {
            GameObject cp = GameManager.Instance.GetPosition(_matrixX, _matrixY);
            Destroy(cp);
        }

        GameManager.Instance.SetEmptyPosition(chessPiece.GetXBoard,
            chessPiece.GetYBoard);

        chessPiece.SetXBoard(_matrixX);
        chessPiece.SetYBoard(_matrixY);
        chessPiece.SetCoords();

        GameManager.Instance.SetPosition(_reference);

        //chessPiece.DestroyMovePossibles();



    }

    public void SetCoords(int x,int y)
    {
        _matrixX = x;
        _matrixY = y;
    }

    public void SetReference(GameObject go)
    {
        _reference = go;
    }

    public GameObject GetRef => _reference;

}
