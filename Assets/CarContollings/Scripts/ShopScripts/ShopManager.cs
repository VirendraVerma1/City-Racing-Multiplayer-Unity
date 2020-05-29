using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject ShopPannel;
    public Image CarImage1;
    public Image CarImage2;
    public Image CarImage3;
    public Text StatusText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCloseButton()
    {
        ShopPannel.SetActive(false);
    }

    public void Car1Select()
    {
        
        CarImage1.transform.localScale = new Vector2(1.2f, 1.2f);
        CarImage2.transform.localScale = new Vector2(1f, 1f);
        CarImage3.transform.localScale = new Vector2(1f, 1f);
        StatusText.text = "Car1 selected";
        saveload.carid = 1;
        saveload.Save();
    }
    public void Car2Select()
    {

        CarImage1.transform.localScale = new Vector2(1f, 1f);
        CarImage2.transform.localScale = new Vector2(1.2f, 1.2f);
        CarImage3.transform.localScale = new Vector2(1f, 1f);
        StatusText.text = "Car2 selected";
        saveload.carid = 2;
        saveload.Save();
    }
    public void Car3Select()
    {

        CarImage1.transform.localScale = new Vector2(1f, 1f);
        CarImage2.transform.localScale = new Vector2(1f, 1f);
        CarImage3.transform.localScale = new Vector2(1.2f, 1.2f);
        StatusText.text = "Car3 selected";
        saveload.carid = 3;
        saveload.Save();
    }

    
}
