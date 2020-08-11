using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class FormaImagen
{
    public Rect pos;
}
public class Imagen : MonoBehaviour
{
    public TMP_Text totalrc;
    // Source texture and the rectangular area we want to extract.
    public Texture2D sourceTex;
    [SerializeField]
    public List<FormaImagen> division=new List<FormaImagen>();
    public GameObject[] marcos;
    float x = 0.0f, y = 0.0f;
    public Texture2D imagenR(Texture2D src,Rect area)
    {
        return new Texture2D(Mathf.FloorToInt(area.width), Mathf.FloorToInt(area.height));
    }
    public void colorPixel()
    {
        int i = 0;
        foreach(FormaImagen item in division)
        {
            Color[] pix = sourceTex.GetPixels(Mathf.FloorToInt(item.pos.x),
                Mathf.FloorToInt(item.pos.y), Mathf.FloorToInt(item.pos.width),
                Mathf.FloorToInt(item.pos.height));
            Texture2D destTex = new Texture2D(Mathf.FloorToInt(item.pos.width),
                Mathf.FloorToInt(item.pos.height));
            destTex.SetPixels(pix);
            destTex.Apply();

            marcos[i].GetComponent<Renderer>().material.mainTexture = destTex;
            i++;
        }        
    }
    public void contarColor(int pos)
    {
        float total = 0;
        int totalPixR = 0;
        Color[] pix = sourceTex.GetPixels(Mathf.FloorToInt(division[pos].pos.x),
                Mathf.FloorToInt(division[pos].pos.y), Mathf.FloorToInt(division[pos].pos.width),
                Mathf.FloorToInt(division[pos].pos.height));
        foreach (Color col in pix)
        {
            total +=col.gamma.r;
            if(col.gamma.r>0 || col.gamma.r<=1)
            {
                totalPixR++;
            }
           /* Debug.Log("g" + Mathf.Floor(col.gamma.g * 256.0f));
            Debug.Log("b" + Mathf.Floor(col.gamma.b * 256.0f));*/
        }
        totalrc.text = total.ToString();
        Debug.Log("Suma canal :"+total+" Total Pix"+totalPixR);
    }
    void CrearSegmentos()
    {
        for(int i=0;i<2;i++)
        {
            x = i > 0 ? (sourceTex.width / 2) : 0;

            for(int j=0;j<2;j++)
            {
                y = j > 0 ? (sourceTex.height / 2) : 0;
                FormaImagen imgRect = new FormaImagen();
                imgRect.pos.x = x;
                imgRect.pos.y = y;
                imgRect.pos.width = sourceTex.width / 2;
                imgRect.pos.height = sourceTex.height / 2;
                division.Add(imgRect);
            }

        }
        colorPixel();
    }
    void Start()
    {
        CrearSegmentos();
    }
}
