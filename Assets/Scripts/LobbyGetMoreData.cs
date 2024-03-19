using UnityEngine;
using UnityEngine.UI;

public class LobbyGetMoreData : MonoBehaviour
{
    // Reference to the ScrollRect component
    public ScrollRect scrollRect;
    public bool bottom;
    public GameObject gb;
    public RectTransform contentContainer;
    private bool hit=false;
    public float scroll;
    [SerializeField]
    private int pageNo;

    private void Start()
    {

    }

    private void Update()
    {
        if(scrollRect.verticalNormalizedPosition<=0.1f /*&& !bottom*/ /*&& hit==false*/)
        {
            bottom = true;
            hit = true;
            PrintBottom();
        }
        scroll= scrollRect.verticalNormalizedPosition; 
    }
    
    // Print "bottom" when reaching the bottom
    private void PrintBottom()
    {
        for(int i=0;i<10;i++)
        {
            GameObject instantiatedObject = Instantiate(gb, contentContainer);
        }
        bottom = true;
        pageNo = pageNo + 1;
        Debug.Log("bottom");
    }
}
