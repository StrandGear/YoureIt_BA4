using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] public GameObject LayerUI { get; private set; }
    [field: SerializeField] public GameObject GameUI { get; private set; }
    [field: SerializeField] public Animator LayerUI_AnimatorUnfolding { get; private set; }
    [field: SerializeField] public Animator LayerUI_AnimatorFolding { get; private set; }

    //Timers
    //timer for unfold animation
    float timePassed_u = 0f, timer_u = 0.3f;
    bool startTimer_u = false;

    //timer for fold animation
    float timePassed_f = 0f, timer_f = 0.3f;
    bool startTimer_f = false;

    // SINGLETON
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    instance = go.AddComponent<UIManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        Debug.Log("Awake called");
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //LayerUI_AnimatorUnfolding.gameObject.SetActive(false);
        //LayerUI_AnimatorFolding.gameObject.SetActive(false);
    }

    private void Update()
    {
        //timers
        //folding timer
        //timePassed_f += Time.deltaTime;
        if (startTimer_f)
        {
            timePassed_f += Time.deltaTime;
            if (timePassed_f >= timer_f)
            {
                timePassed_f = 0f;
                GameUI.SetActive(true);
                startTimer_f = false;
            }
        }

        //unfolding timer 
        
        if (startTimer_u)
        {
            timePassed_u += Time.deltaTime;
            if (timePassed_u >= timer_u)
            {
                timePassed_u = 0f;
                LayerUI.SetActive(true);
                startTimer_u = false;
            }
        }
    }

    public void OpenLayerUI()
    {
        // Deactivate GameUI
        GameUI.SetActive(false);

        LayerUI_AnimatorUnfolding.SetTrigger("StartAnim");
        //StartCoroutine(OpenLayerUIAfterDelay()); 
        startTimer_u = true;

        print("OPen layer ui");
    }

    public void CloseLayerUI()
    {
        LayerUI.SetActive(false);
        // Trigger folding animation
        LayerUI_AnimatorFolding.SetTrigger("StartAnim");
        
        startTimer_f = true;
    }

    /*private void CloseObjectAfterDelay(GameObject gameObject)
    {

    }

    private IEnumerator CloseLayerUIAfterDelay()
    {
        // Trigger folding animation
        //LayerUI_Animator.SetTrigger("Close");

        // Wait for additional 29 seconds before deactivating LayerUI
        yield return new WaitForSeconds(1.5f);

        // Deactivate LayerUI and activate GameUI
        //LayerUI.SetActive(false);
        GameUI.SetActive(true);

        StopCoroutine(CloseLayerUIAfterDelay());
        print("Close Enumerator layer ui");
    }

    private IEnumerator OpenLayerUIAfterDelay()
    {
        

        // Wait for the unfolding animation to complete (assuming it's 1 second)
        yield return new WaitForSeconds(1.5f);

        LayerUI.SetActive(true);

        StopCoroutine(OpenLayerUIAfterDelay());
    }*/
}
