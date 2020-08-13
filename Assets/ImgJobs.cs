using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ImgJobs : MonoBehaviour
{
    public TMP_Text totalrc;
    float total = 0;
    public Texture2D sourceTex;
    public NativeArray<Color> img_pix;
    [SerializeField]
    public List<FormaImagen> division = new List<FormaImagen>();
    public GameObject[] marcos;
    float x = 0.0f, y = 0.0f;
    FramesImgJob img_Job;
    JobHandle m_JobHandle;
    // Start is called before the first frame update
    void Start()
    {


        CrearSegmentos();
    }
    void CrearSegmentos()
    {
        for (int i = 0; i < 2; i++)
        {
            x = i > 0 ? (sourceTex.width / 2) : 0;

            for (int j = 0; j < 2; j++)
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
    public void colorPixel()
    {
        int i = 0;
        foreach (FormaImagen item in division)
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
            // this persistent memory setup assumes our vertex count will not expand
            img_pix = new NativeArray<Color>(pix, Allocator.TempJob);

            img_Job = new FramesImgJob()
            {
                pix = img_pix,
                total = total
            };

            m_JobHandle = img_Job.Schedule(pix.Length, 1);
            m_JobHandle.Complete();
            img_pix.Dispose();
            

        }
    }
    struct FramesImgJob : IJobParallelFor
    {
        public NativeArray<Color> pix;
        public float total;
        public void Execute(int i)
        {
            var miColor = pix[i];
            total += miColor.gamma.r;
            if (miColor.gamma.r > 0 || miColor.gamma.r <= 1)
            {
                total++;
            }
            Debug.Log("Pixeles R Channel en la imagen es :"+total);
        }

    }
        // Update is called once per frame
    void Update()
    {


        
    }
}
