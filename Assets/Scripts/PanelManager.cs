using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;
    public List<GameObject> panels = new List<GameObject>();

    private GameObject activePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        //HideAllPanels();
    }

    public void ShowPanel(GameObject panelToShow)
    {
        HideAllPanels();
        //Debug.Log("Opening panel "+panelToShow.gameObject.name);
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
            Debug.Log("Opening panel " + panelToShow.gameObject.name);
            activePanel = panelToShow;
        }
    }

    private void HideAllPanels()
    {
        //foreach (GameObject panel in panels)
        //{
        //    panel.SetActive(false);
        //}
        //activePanel = null;
    }
}
