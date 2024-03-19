using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpotsMap : MonoBehaviour
{
    public int index;
    public bool star;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void setPlayerOnSpot()
    {

        //try
        //{
        GridLayoutGroup gridLayout=this.gameObject.GetComponent<GridLayoutGroup>();
        
        if (this.gameObject.GetComponent<GridLayoutGroup>() != null)
        {
            if (star == true && this.gameObject.name != "Blank")
            {
                if (this.gameObject.transform.childCount == 1)
                {
                    //Debug.Log("Setting grid layout for 1 child "+this.gameObject.name);
                    gridLayout = UIManager.Instance.gridLayoutfor1;
                }
                if (this.gameObject.transform.childCount == 2)
                {
                    gridLayout = UIManager.Instance.gridLayoutfor2;

                }
                if (this.gameObject.transform.childCount == 3 || this.gameObject.transform.childCount == 4)
                {
                    gridLayout = UIManager.Instance.gridLayoutfor3and4;

                }
                if (this.gameObject.transform.childCount > 4 && this.gameObject.transform.childCount < 8)
                {
                    gridLayout = UIManager.Instance.gridLayoutfor8;

                }
                if (this.gameObject.transform.childCount >8)
                {
                    gridLayout = UIManager.Instance.gridLayoutfor16;
                }
            }
            else
            {
                if (this.gameObject.transform.childCount == 1 && (this.gameObject.name != "Blank" && this.gameObject.name != "Inside"))
                {
                    //Debug.Log("Setting grid layout for 1 child "+this.gameObject.name);
                    gridLayout = UIManager.Instance.gridLayoutfor1;

                }
                if (this.gameObject.transform.childCount == 2 && (this.gameObject.name != "Blank" && this.gameObject.name != "Inside"))
                {
                    gridLayout = UIManager.Instance.gridLayoutfor2;

                }
                if ((this.gameObject.transform.childCount == 3 || this.gameObject.transform.childCount == 4) && (this.gameObject.name != "Blank" && this.gameObject.name != "Inside"))
                {
                    gridLayout = UIManager.Instance.gridLayoutfor3and4;

                }
                if ((this.gameObject.transform.childCount > 4 && this.gameObject.transform.childCount < 8) && (this.gameObject.name != "Blank" && this.gameObject.name != "Inside"))
                {
                    gridLayout = UIManager.Instance.gridLayoutfor8;

                }
                if ((this.gameObject.transform.childCount > 8) && (this.gameObject.name != "Blank" && this.gameObject.name != "Inside"))
                {
                    gridLayout = UIManager.Instance.gridLayoutfor16;

                }
            }
            if(this.gameObject.name != "Inside")
            {
                this.gameObject.GetComponent<GridLayoutGroup>().constraintCount = gridLayout.constraintCount;
                this.gameObject.GetComponent<GridLayoutGroup>().padding = gridLayout.padding;
                this.gameObject.GetComponent<GridLayoutGroup>().spacing = gridLayout.spacing;
                this.gameObject.GetComponent<GridLayoutGroup>().cellSize = gridLayout.cellSize;
                this.gameObject.GetComponent<GridLayoutGroup>().startCorner = gridLayout.startCorner;
                this.gameObject.GetComponent<GridLayoutGroup>().startAxis = gridLayout.startAxis;
                this.gameObject.GetComponent<GridLayoutGroup>().childAlignment = gridLayout.childAlignment;
                this.gameObject.GetComponent<GridLayoutGroup>().constraint = gridLayout.constraint;
            }
        }
        
    }
    IEnumerator checkTheWin()
    {
        //Debug.Log("Trying to Burst off Confetti");

        if (LudoManager.Instance.winningIndexesLocalPlayer.Contains(index))
        {
            // Debug.Log("Hurraaayyyyyyy");
            //Debug.Log("Burst off Confetti");
            UIManager.Instance.Confetti.gameObject.SetActive(true);
            //UIManager.Instance.Confetti.SetBool("Win", true);
            yield return new WaitForSeconds(3f);
            //UIManager.Instance.Confetti.SetBool("Win", false);
            UIManager.Instance.Confetti.gameObject.SetActive(false);
        }
    }
    public void CheckWin()
    {
      LudoManager.Instance.StartCoroutine(checkTheWin());
    }
}

