using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPagination : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject upInstruction;
    public GameObject downInstruction;
    public Transform content;
    [Range(8, 30f)]
    public int itemsPerPage;

    public int currentPage = 0;
    private bool reachedTop = false;
    private bool reachedBottom = false;

    private ScrollRect scrollRect;
    public float normalizePoint;
    private bool loading;
    public TextMeshProUGUI pageNo;
    [SerializeField]
    private int maximumItems;
    void Start()
    {
        LoadPage(currentPage);

        scrollRect = GetComponent<ScrollRect>();

        scrollRect.onValueChanged.AddListener(OnScroll);
    }
    private void Update()
    {
        normalizePoint = scrollRect.normalizedPosition.y;
    }
    void LoadPage(int page)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        pageNo.text=(page+1).ToString();
        int startIndex = page * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, /*YourDataList.Count*/maximumItems);
        if(currentPage>0)
        {
            GameObject newItem = Instantiate(upInstruction, content);
        }
        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, content);
        }
        if (currentPage >= 0 && currentPage<9)
        {
            GameObject newItem = Instantiate(downInstruction, content);
        }
    }


    public void ScrollUp()
    {
        if(loading==false)
        {
            StartCoroutine(reachedTopScroll());
        }
    }
    IEnumerator reachedTopScroll()
    {
        bool loaded = false;
        if (currentPage > 0 && !reachedTop)
        {
            scrollRect.vertical = false;
            loading = true;

            currentPage--;
            LoadPage(currentPage);
            reachedTop = true;
            reachedBottom = false;
            scrollRect.normalizedPosition = new Vector2(0, 0f);
            Debug.Log("Scrolling up to page: " + currentPage);
            loaded = true;
        }
        yield return new WaitForSeconds(0.6f);
        if (/*currentPage > 0 && !reachedTop*/loaded)
        {
            loading = false;
            scrollRect.vertical = true;
        }
    }

    public void ScrollDown()
    {
        if (loading == false)
        {
            StartCoroutine(reachedBottomScroll());
        }
    }
    IEnumerator reachedBottomScroll()
    {
        bool loaded = false;
        if (currentPage < Mathf.CeilToInt((float)/*YourDataList.Count*/maximumItems / itemsPerPage) - 1 && !reachedBottom)
        {
            currentPage++;
            LoadPage(currentPage);
            reachedBottom = true;
            reachedTop = false;
            scrollRect.normalizedPosition = new Vector2(0, 1f);
            scrollRect.vertical = false;
            Debug.Log("Scrolling down to page: " + currentPage);
            loaded = true;
        }
        yield return new WaitForSeconds(0.6f);
        if (/*currentPage > 0 && !reachedTop*/loaded)
        {
            loading = false;
            scrollRect.vertical = true;
        }
    }

    void OnScroll(Vector2 scrollPos)
    {
        if (scrollPos.y <= -0.1 && !reachedBottom)
        {
            ScrollDown();
        }
        else if (scrollPos.y >= 1.1 && !reachedTop)
        {
            ScrollUp();
        }
        else
        {
            reachedTop = false;
            reachedBottom = false;
        }
    }
}
