using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Swipe : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight, swipeFail;
    private bool isDragging, isSwiping, isLoading;
    private Vector2 startTouch, swipeDelta, swipeDir;
    private Vector3 origin;

    //PlayerUI
    public Slider expSlider;
    public Image avatarUI;
    public Image hatUI;
    public AvatarsAndHats avatarHats;
    public Text level;

    public GameObject expErhalten;

    private float countTime;
    
    [SerializeField] [Range(1f, 700f)]
    private float deadzone = 180, dzPuffer = 100, swipeStrength = 480;

    [SerializeField] [Range(0.1f, 10f)] 
    private float lerpTime = 6;

    private List<string> nextTexts;

    public ServerConnect sc;
    public Text sentence;
    public Text food;
    private List<List<string>> valuesOfTable = new List<List<string>>();

    public BelohnungSystem belohnung;

    public bool Tap { get { return tap;}}
    public Vector2 SwipeDelta { get { return swipeDelta;}}
    public bool SwipeLeft { get { return swipeLeft;}}
    public bool SwipeRight { get { return swipeRight;}}

    void Start()
    {
        //SetUp PlayerUI
        expSlider.maxValue = LevelSystem.expForNextLevel;
        expSlider.value = GameManager.experience;

        (Sprite, Sprite) sprites = avatarHats.ReturnAvatarAndHatSprites(GameManager.avatarNum, GameManager.bestHatNum);

        avatarUI.sprite = sprites.Item1;
        hatUI.sprite = sprites.Item2;
        level.text = GameManager.playerLevel.ToString();

        countTime = 0;
        isLoading = true;
        StartCoroutine(SetUp());
        
        origin = transform.position;
        nextTexts = new List<string>();
        //string table = StartCoroutine(sc.GetFoodTable);
    }

    
    // Calls the Database for FoodInputsTable and sets the game up
    IEnumerator SetUp()
    {
        string getFoodURL = "https://krizzdesign.de/getFoodTable.php";

        string name = GameManager.name;

        List<IMultipartFormSection> getForm = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("name", name)
        };

        using (UnityWebRequest food_get = UnityWebRequest.Post(getFoodURL, getForm))
        {
            yield return food_get.SendWebRequest();

            if (food_get.isNetworkError || food_get.isHttpError)
            {
                print("There was an error getting the high score: " + food_get.error);
            }
            else
            {
                string table = food_get.downloadHandler.text;
                List<string> tab = new List<string>(table.Split('#'));
                foreach (string t in tab)
                {
                    List<string> parsedT = new List<string>(t.Split('~'));
                    valuesOfTable.Add(parsedT);
                }

                valuesOfTable.RemoveAt(valuesOfTable.Count - 1);
            }
        }

        UpdateText();
        isLoading = false;
    }

    // Update is called once per frame
    private void Update()
    {
        countTime += Time.deltaTime;

        tap = swipeLeft = swipeRight = false;

        if (isSwiping || isLoading)
            return;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Reset();
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDragging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Reset();
            }
        }
        #endregion

        // Calculate touch distance
        swipeDelta = swipeDir = Vector2.zero;
        if (isDragging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2) Input.mousePosition - startTouch;
            }
            Vector3 newPosition = new Vector3(origin.x + swipeDelta.x, origin.y + swipeDelta.y, origin.z);
            swipeDir = new Vector2(newPosition.x - transform.position.x, newPosition.y - transform.position.y);
            swipeDir.Normalize();
            transform.position = newPosition;
        }

        // Swipe if touch leaves the deadzone
        if (swipeDelta.magnitude > deadzone)
        {
            // Which direction is the swipe going
            float x = swipeDelta.x;

            if (x > dzPuffer)
            {
                swipeRight = true;
            }
            else if (x < -dzPuffer)
            {
                swipeLeft = true;
            }
            else
            {
                swipeFail = true;
            }
        }

        // Move Object out of the screen
        // Then Upload Data
        if (swipeRight)
        {
            isSwiping = true;
            StartCoroutine(SmoothSwipe(new Vector2(transform.position.x + swipeDir.x * swipeStrength, transform.position.y + swipeDir.y * swipeStrength), true));
        }
        else if (swipeLeft)
        {
            isSwiping = true;
            StartCoroutine(SmoothSwipe(new Vector2(transform.position.x + swipeDir.x * swipeStrength, transform.position.y + swipeDir.y * swipeStrength), false));
        }
        else if (swipeFail)
        {
            isSwiping = true;
            StartCoroutine(SmoothSwipe(new Vector2(transform.position.x + swipeDir.x * swipeStrength, transform.position.y + swipeDir.y * swipeStrength), false));
        }
    }

    // Swipes the text smoothly
    // After the Swipe is finished it starts @GetNextText()
    protected IEnumerator SmoothSwipe(Vector2 end, bool rating) 
    {
        int num = Random.Range(1, 4);
        FindObjectOfType<AudioManager>().Play("swipe" + num);

        while (Vector2.Distance(transform.position, end) > 0.05f) 
        {
            transform.position = Vector2.Lerp(transform.position, end, lerpTime * Time.deltaTime);
            yield return null;
        }

        if (!swipeFail)
        {
            float pitch = Random.Range(0.9f, 1.1f);
            FindObjectOfType<AudioManager>().ChangePitch("plateFalling", pitch);

            FindObjectOfType<AudioManager>().Play("plateFalling");

            sc.CallSendSwipeInput(rating, nextTexts[0]);
            //StartCoroutine(sc.SendSwipeInput(rating));
            yield return null;

            GameManager.foodRatings++;
            // Zeigt kurz wie viel EXP man erhält
            StartCoroutine(ShowExp());

            // Update Experience before
            if(LevelSystem.AddExp(80))
                belohnung.OpenLevelBelohnung(GameManager.playerLevel);

            if (!Collectables.avatarUnlocked[7] && GameManager.foodRatings >= 200)
                belohnung.OpenAvatarBelohnung(13);

            expSlider.maxValue = LevelSystem.expForNextLevel;
            expSlider.value = GameManager.experience;
            level.text = GameManager.playerLevel.ToString();

            UpdateText();
        }
        else
            FindObjectOfType<AudioManager>().Play("error");

        isSwiping = false;
        Reset();
    }

    IEnumerator ShowExp()
    {
        expErhalten.SetActive(true);
        yield return new WaitForSeconds(2);
        expErhalten.SetActive(false);
    }

    public void UpdateText()
    {
        if (valuesOfTable.Count == 0)
        {
            StartCoroutine(SetUp());
            return;
        }
        print(valuesOfTable.Count);
        nextTexts = valuesOfTable[0];

        food.text = nextTexts[1];
        sentence.text = nextTexts[2].Replace("_", "_____");

        valuesOfTable.RemoveAt(0);
    }

    private void Reset()
    {
        startTouch = swipeDelta = swipeDir = Vector2.zero;
        isDragging = false;
        swipeFail = false;
        transform.position = origin;
    }

    public void BackToMenu()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.timeTinder += countTime;
        GameManager.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
}
