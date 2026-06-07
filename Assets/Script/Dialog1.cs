using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Dialog1 : MonoBehaviour
{

    public GameObject fadeScreenIn;
    public GameObject char1;
    public GameObject char2;
    public GameObject textBox;

    [SerializeField] AudioSource teteh1;
    [SerializeField] AudioSource nenek1;


    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;

    [SerializeField] GameObject nextButton;
    public int eventpos = 0;
    [SerializeField] GameObject charname;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        StartCoroutine(EventStarter());
    }

    IEnumerator EventStarter()
    {
        nextButton.SetActive(false);
        yield return new WaitForSeconds(2);
        fadeScreenIn.SetActive(false);
        char1.SetActive(true);
        yield return new WaitForSeconds(2);
        // this is where our text function will go in future tutorial
        mainTextObject.SetActive(true);
        textToSpeak = "Sampurasun, Assalamu`alaikum. Wilujeung sumping di Cianjur, wilayah ini pertama kali didirikan oleh Raden Aria Wira Tanu I ....juga dikenal sebagai Raden Djajasasana, putra dari Aria Wangsa Goparana yang merupakan keturunan Sunan Talaga";
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        charname.GetComponent<TMPro.TMP_Text>().text = "Mba Jamu";
        currentTextLength = textToSpeak.Length;
        nenek1.Play();
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        eventpos ++;
        nextButton.SetActive(true);
        textBox.SetActive(true);
    }

    IEnumerator EventStarter1()
    {
        nextButton.SetActive(false);
        char2.SetActive(true);
        yield return new WaitForSeconds(2);
        // this is where our text function will go in future tutorial
        mainTextObject.SetActive(true);
        textToSpeak = "Dengan demikian, pendiri Cianjur memiliki garis keturunan bangsawan yang terhormat, berasal dari keluarga besar penyebar agama Islam di Tatar Sunda.";
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        charname.GetComponent<TMPro.TMP_Text>().text = "Nenek ???";
        currentTextLength = textToSpeak.Length;
        nenek1.Play();
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        eventpos++;
        nextButton.SetActive(true);
        textBox.SetActive(true);
    }


    IEnumerator NextSxene()
    {
        nextButton.SetActive(false);
        mainTextObject.SetActive(false);
        fadeScreenIn.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }
    public void NextButtons()
    {
        if (eventpos == 1) {
            StartCoroutine  (EventStarter1());
        }
        else
        {
            StartCoroutine(NextSxene());
        }

    }
}
