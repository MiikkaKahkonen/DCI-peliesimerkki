using UnityEngine;
using System.Collections;

public class VLevelSelectionMono : MonoBehaviour {
    public int maxLevels = 7;
    int currentLevels = 0;
    int selectedItem = -1;
    GameObject level;
    GameObject[] golevels;
    GameObject selected;
    bool flag = false;
    float timer = 2f;
	// Use this for initialization
	void Start () {
        foreach(Transform child in transform)
        {
            float z = 0;
            Vector3 pos = child.transform.position;
            z = pos.z;
            
            switch (child.name)
            {
                case "Title":   pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f, Screen.height*0.9f));
                                pos.z = z;
                                child.transform.position = pos;
                                break;
                case "Up":
                                pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.8f, Screen.height * 0.6f));
                                pos.z = z;
                                child.transform.position = pos;
                                break;
                case "Down":
                                pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.8f, Screen.height * 0.3f));
                                pos.z = z;
                                child.transform.position = pos;
                                break;
                case "Load":
                                pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.6f, Screen.height * 0.1f));
                                pos.z = z;
                                child.transform.position = pos;
                                break;
                case "Quit":
                                pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.4f, Screen.height * 0.1f));
                                pos.z = z;
                                child.transform.position = pos;
                                break;
                case "Level":
                                level = child.gameObject;
                                pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height * 0.6f));
                                pos.z = z;
                                child.transform.position = pos;
                                level.renderer.enabled = false;
                                golevels = new GameObject[maxLevels];
                                for (int i = 0; i < maxLevels; i++)
                                {
                                    GameObject temp = Instantiate(level) as GameObject;
                                    golevels[i] = temp;
                                    temp.renderer.enabled = false;
                                    temp.GetComponent<TextMesh>().text = "uninit";
                                    pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height * (0.6f - (0.05f * i))));
                                    pos.z = z;
                                    temp.transform.position = pos;
                                    temp.transform.parent = this.transform;
                                    temp.name = "level" + i;
                                }
                                level.collider.enabled = false;
                                break;
            }
        }

        

    }
	
	// Update is called once per frame
	void Update () {
    }

    public void RenderLevels(string[] levels)
    {
        if (selected != null)
        {
            selected.GetComponent<TextMesh>().color = Color.white;
            selected = null;
            selectedItem = -1;
        }

        if (levels == null)
            return;

        if (selected != null)
            selected = null;
        
        currentLevels = levels.Length;

        for(int i = 0; i < maxLevels; i++)
        {
            if(i < currentLevels)
            {
                golevels[i].GetComponent<TextMesh>().text = levels[i];
                golevels[i].renderer.enabled = true;
            }
            else
            {
                golevels[i].renderer.enabled = false;
            }
        }
    }

   public void SelectItem(int i)
    {
        
        if (i > currentLevels - 1)
            return;

        if(selected != null)
        {
            selected.GetComponent<TextMesh>().color = Color.white;
            selectedItem = -1;
        }
        selectedItem = i;
        selected = golevels[i];

        selected.GetComponent<TextMesh>().color = Color.green;

    }

    public int GetSelected()
    {
        return selectedItem;
    }
}
