
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Database : UdonSharpBehaviour
{
    public string[] names;
    public string[] ids;
    public string[] emails;

    public UserSubmit userSubmit;

    public Text[] texts;
    public Text[] pageTexts;

    public int size = 1000;
    public int idx = -1;
    public int page = 1;

    public void Start()
    {
        names = new string[size];
        ids = new string[size];
        emails = new string[size];
    }

    public void onSubmit()
    {
        Debug.Log("hey");
        idx++;
        names[idx] = userSubmit.username;
        ids[idx] = userSubmit.id;
        emails[idx] = userSubmit.email;

        onChangePage();
    }

    /*
        Pagination 관련 코드 모음
    */
    private void onChangePage()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            int textIdx = (page - 1) * texts.Length + i;
            Text text = texts[i];
            if (idx < textIdx)
                text.text = "";
            else
                text.text = ids[textIdx] + " / " + names[textIdx] + " / " + emails[textIdx];
        }
    }
    private void setPageTexts()
    {
        if (page <= 4)
        {
            for (int i = 0; i < pageTexts.Length; i++)
            {
                pageTexts[i].text = (i + 1).ToString();
            }
        }
        else
        {
            for (int i = 0; i < pageTexts.Length; i++)
            {
                pageTexts[i].text = (page - 3 + i).ToString();
            }
        }
    }

    public void onClick1()
    {
        page = int.Parse(pageTexts[0].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick2()
    {
        page = int.Parse(pageTexts[1].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick3()
    {
        page = int.Parse(pageTexts[2].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick4()
    {
        page = int.Parse(pageTexts[3].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick5()
    {
        page = int.Parse(pageTexts[4].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick6()
    {
        page = int.Parse(pageTexts[5].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick7()
    {
        page = int.Parse(pageTexts[6].text);
        setPageTexts();
        onChangePage();
    }
}
