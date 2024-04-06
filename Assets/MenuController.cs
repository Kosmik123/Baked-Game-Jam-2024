using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    Sprite[] Cats;

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
        for(int i =0; i<Cats.Length; i++)
        {
            GameObject to_spawn = new GameObject();
            to_spawn.AddComponent<Image>().sprite = Cats[i];
            to_spawn.GetComponent<Image>().material = CatShader;
            to_spawn.transform.SetParent(MainScreen.transform);
            to_spawn.transform.position = CatsPosition.transform.position;
            to_spawn.transform.position = new Vector3(CatsPosition.transform.position.x + (i%3) * 100,
                CatsPosition.transform.position.y - (i/3)*100, 1);
            //Instantiate(to_spawn);
        }
    }

    private float tempTime;
    private float timeToLive = 0.1f;
    // Update is called once per frame
    void Update()
    {
        
        if (rolling)
        {
            tempTime += Time.deltaTime;
            if (tempTime >timeToLive )
            {
                timeToLive += 0.05f;
                catDropImageComponent.sprite = Cats[UnityEngine.Random.Range(0, Cats.Length -1)];        
            }
            if(timeToLive >= 3f)
            {
                rolling = false;
                timeToLive = 0.1f;
                tempTime = 0f;
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
        catChooseImageComponent.sprite = Cats[catIndex];
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
        CatDrop.SetActive(false);
    }

    public void OnPrevCatClick()
    {
        if(catIndex -1 < 0)
        {
            catIndex = Cats.Length - 1;
        } else
        {
            catIndex--;
        }
        catChooseImageComponent.sprite = Cats[catIndex];
    }

    public void OnNextCatClick()
    {
        if(catIndex +1 > Cats.Length - 1)
        {
            catIndex = 0;
        } else
        {
            catIndex++;
        }
        catChooseImageComponent.sprite = Cats[catIndex];
    }

}
