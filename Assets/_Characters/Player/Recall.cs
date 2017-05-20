using System.Collections;
using UnityEngine;
using RPG.Characters;
using RPG.CameraUI;
public class Recall : MonoBehaviour
{

    [Range(0.0f, 4.0f)] [SerializeField] float speedUpGetPos = 3;//shorten the time Between Position Save
    [Range(1.0f, 8.0f)] [SerializeField] float speedUpRecall = 4;//speed up the Time for Recall
    enum Recalls { One, Two, Three, Four };
    [SerializeField] Recalls recalls;
    float endTime;


    float timer = -1;                                         //regular Timer     
    int timerFull = -1;                                              //Timer with full numbers
    bool startAbility = false;                                  //starts Ability
    bool getPosition = true;                                    //declats wich state of the abililty we are
    bool switchState = true;                                    //to call code one Timer at a specific moment
    [SerializeField] bool autoModeRecall = true;
    [SerializeField] float timeForInput;
    float inputEndTime = 6;
    [SerializeField] KeyCode InputKey;

    Vector3[] savedPosition = new Vector3[4]; //positions for recall
    Quaternion[] savedRotation = new Quaternion[4];
    [SerializeField] GameObject positionMark;                   //Place your Position Mark Here oder not
    GameObject[] markInArray = new GameObject[4];
    [SerializeField] ParticleSystem recallEffect;

    private void Start()
    {
        inputEndTime = timeForInput;
        switch (recalls)
        {
            case Recalls.One:
                endTime = 1;
                break;
            case Recalls.Two:
                endTime = 3;
                break;
            case Recalls.Three:
                endTime = 5;
                break;
            case Recalls.Four:
                endTime = 7;
                break;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(InputKey) && !startAbility) // Input
        {
            startAbility = true; // Start Ability 
        }

        if (startAbility && getPosition)
        {
            timer += speedUpGetPos * Time.deltaTime;            // Timer started
            timerFull = Mathf.FloorToInt(timer);                //whole number from Timer

            switch (timerFull)                                  //switch state 
            {
                case 0:                                         //in Case the timerFull is at 0 sec
                    if (switchState)
                    {
                        StartCoroutine(GetPositionRecall(0));   //Start Coroutine (MethodName (int is Number in Array)
                    }
                    break;
                case 2:                                         //in Case the timerFull is at 2 sec
                    if (!switchState)
                    {
                        StartCoroutine(GetPositionRecall(1));
                    }
                    break;
                case 4:                                         //in Case the timerFull is at 4 sec
                    if (switchState)
                    {
                        StartCoroutine(GetPositionRecall(2));
                    }
                    break;
                case 6:                                         //in Case the timerFull is at 6 sec
                    if (!switchState)
                    {
                        StartCoroutine(GetPositionRecall(3));
                    }

                    break;
            }
        }

        if (timer >= endTime)                                         //Player got 1 sec after last Position save
        {
            getPosition = false;
            startAbility = false;
        }

        if (!startAbility && !getPosition)
        {
            if (autoModeRecall)
            {
                timer -= speedUpRecall * Time.deltaTime;    //from now the Timer goes backward
                timerFull = Mathf.FloorToInt(timer);        //same
            }
            if (!autoModeRecall)
            {

                timeForInput -= Time.deltaTime;

                if (Input.GetKeyDown(InputKey))
                {
                    if (timerFull % 2 != 0)
                    {
                        timerFull += 1;
                    }
                    if (timerFull % 2 == 0)
                    {
                        timerFull -= 2;
                    }

                }
                if (timeForInput <= 0)
                {
                    getPosition = true; // set to true so u could start ability again
                    timer = timerFull = -1;
                    timeForInput = inputEndTime;
                    switchState = !switchState;
                    for (int i = 0; i < markInArray.Length; i++)
                    {
                        if (markInArray != null)
                        {
                            Destroy(markInArray[i]);
                        }
                    }
                }
            }



            switch (timerFull)
            {
                case 6:                                         //in Case the timerFull is at 6 sec
                    if (switchState)
                    {
                        StartCoroutine(LoadPositionRecall(3)); //Start Coroutine (MethodName (int is Number in Array, float delay for Recall Effect)
                    }
                    break;

                case 4:                                         //in Case the timerFull is at 4 sec
                    if (!switchState)
                    {
                        StartCoroutine(LoadPositionRecall(2));
                    }
                    break;

                case 2:                                         //in Case the timerFull is at 2 sec
                    if (switchState)
                    {
                        StartCoroutine(LoadPositionRecall(1));
                    }
                    break;
                case 0:                                         //in Case the timerFull is at 0 sec
                    if (!switchState)
                    {
                        StartCoroutine(LoadPositionRecall(0));

                        print("end Recall, end Ability");
                        getPosition = true; // set to true so u could start ability again
                        timer = timerFull = -1;
                        timeForInput = inputEndTime;
                    }
                    break;
            }
        }
    }
    IEnumerator GetPositionRecall(int numberInArray)
    {
        savedPosition[numberInArray] = this.transform.position;                                 //when its called can position an stor in array
        savedRotation[numberInArray] = this.transform.rotation;
        if (positionMark != null)                                                               //if u got an postionmark its spawns an well placed
        {
            GameObject mark = Instantiate(positionMark);
            markInArray[numberInArray] = mark;
            mark.transform.position = savedPosition[numberInArray] /*+ new Vector3(0, 1, 0)*/;
            mark.transform.rotation = savedRotation[numberInArray];
        }
        switchState = !switchState;
        yield return null;
    }
    IEnumerator LoadPositionRecall(int numberInArray)
    {
        Destroy(markInArray[numberInArray]);
        Debug.Log("Recall : " + numberInArray);

        if (recallEffect != null)
            recallEffect.Play();

        FindObjectOfType<PlayerMovement>().walkTarget.transform.position = savedPosition[numberInArray]; //if u use NavMeshAgent assign hier your Destination Target
        this.transform.position = savedPosition[numberInArray];                 //set Player pos to arrayPos
        this.transform.rotation = savedRotation[numberInArray];

        switchState = !switchState;
        yield return null;
    }
}
