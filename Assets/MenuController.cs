using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject MainScreen;
    [SerializeField]
    GameObject CatChooser;
    [SerializeField]
    GameObject CatDrop;
    [SerializeField]
    List<SOCat> Cats;

    [SerializeField]
    List<SOCat> BlockedCats;
    [SerializeField]
    GameObject Cat_Name_Text;
    [SerializeField]
    GameObject Cat_Skill_Text;
    [SerializeField]
    GameObject Cat_Colddown_Text;
    [SerializeField]
    GameObject CatDropImage;
    [SerializeField]
    Material CatShader;

    [SerializeField]
    GameObject CatChooseImage;
    [SerializeField]
    GameObject CatsPosition;
    private bool rolling;
    private Image catDropImageComponent;
    private Image catChooseImageComponent;
    private int catIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        MainScreen.SetActive(true);
        CatChooser.SetActive(false);
        CatDrop.SetActive(false);
        UpdateCatList();
    }

    private void UpdateCatList()
    {
        for (int i = 0; i < Cats.Count; i++)
        {
            GameObject to_spawn = new GameObject();
            to_spawn.AddComponent<Image>();
            to_spawn.GetComponent<Image>().material = CatShader;
            Cats[i].SetMaterialColor(to_spawn.GetComponent<Image>());
            to_spawn.transform.SetParent(MainScreen.transform);
            to_spawn.transform.position = CatsPosition.transform.position;
            to_spawn.transform.position = new Vector3(CatsPosition.transform.position.x + (i % 3) * 100,
                CatsPosition.transform.position.y - (i / 3) * 100, 1);
        }
    }
    private float tempTime;
    private float timeToLive = 0.1f;
    private SOCat catToAdd;
    // Update is called once per frame
    void Update()
    {
        
        if (rolling)
        {
            tempTime += Time.deltaTime;
            if (tempTime >timeToLive )
            {
                timeToLive += 0.30f;
                catToAdd = BlockedCats[UnityEngine.Random.Range(0, BlockedCats.Count - 1)];
                catToAdd.SetMaterialColor(catDropImageComponent);        
            }
            if(timeToLive >= 6f)
            {
                rolling = false;
                timeToLive = 0.1f;
                tempTime = 0f;
                Cats.Add(catToAdd);
                BlockedCats.Remove(catToAdd);
            }
            
        }
    }

    public void OnStartGameClick()
    {
        Debug.Log("Start Game");
    }

    public void OnChooseCatClick()
    {
        MainScreen.SetActive(false);
        CatChooser.SetActive(true);
        catChooseImageComponent = CatChooseImage.GetComponent<Image>();
        Cats[catIndex].SetMaterialColor(catChooseImageComponent);
        SetCatText(Cats[catIndex]);
    }

    public void OnCatDropClick()
    {
        MainScreen.SetActive(false);
        CatDrop.SetActive(true);
        catDropImageComponent = CatDropImage.GetComponent<Image>();

    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void OnRollCatDrop()
    {
        catDropImageComponent.material = CatShader;
        rolling = true;
    }

    public void OnCancelDropCatClick()
    {
        MainScreen.SetActive(true);
        UpdateCatList();
        CatDrop.SetActive(false);
    }

    public void OnCancelChooseClick()
    {
        MainScreen.SetActive(true);
        UpdateCatList();
        CatChooser.SetActive(false);
    }

    public void OnCatChooseClick()
    {
        MainScreen.SetActive(true);
        //TODO: Zapisac wybranego kota w game Managerze
        CatChooser.SetActive(false);
    }
    public void OnPrevCatClick()
    {
        if(catIndex -1 < 0)
        {
            catIndex = Cats.Count -1;
        } else
        {
            catIndex--;
        }
        Cats[catIndex].SetMaterialColor(catChooseImageComponent);
        SetCatText(Cats[catIndex]);
    }

    public void OnNextCatClick()
    {
        if(catIndex +1 > Cats.Count - 1)
        {
            catIndex = 0;
        } else
        {
            catIndex++;
        }
        Cats[catIndex].SetMaterialColor(catChooseImageComponent);
        SetCatText(Cats[catIndex]);
    }


    private void SetCatText(SOCat cat)
    {
        var info = cat.GetDisplayInfo();
        Cat_Name_Text.GetComponent<TextMeshProUGUI>().SetText("Name: " + info.catName);
        Cat_Skill_Text.GetComponent<TextMeshProUGUI>().SetText("Breed: " + info.catBreed);
    }
}
