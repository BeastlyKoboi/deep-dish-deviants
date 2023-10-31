using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    public FoodItem[] playerInventory;
    private List<Icon> iconList;
    public float fireCoolDown;
    public bool fireCoolDownActive;
    public float kneedCoolDown;
    public bool kneedCoolDownActive;
    public float cutCoolDown;
    public bool cutCoolDownActive;

    [SerializeField] private List<Counter> counterScripts;

    // Particle stuff
    [SerializeField]
    private ParticleSystem fire;

    private Vector3 originalPositionFire;

    [SerializeField]
    private ParticleSystem cut;

    private Vector3 originalPositionCut;

    [SerializeField]
    private GameObject knead;

    private Vector3 originalPositionKnead;

    //Icon stuff
    [SerializeField]
    private Icon icon;

    private Icon icon2;
    private Icon icon3;
    private Icon icon4;
    private Icon icon5;
    private Icon icon6;
    private Icon icon7;
    // is true when player fires aka presses E or right click
    public bool isInteracting;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new FoodItem[1] { null };
        isInteracting = false;
        icon7 = Instantiate(icon);
        icon6 = Instantiate(icon);
        icon5 = Instantiate(icon);
        icon4 = Instantiate(icon);
        icon3 = Instantiate(icon);
        icon2 = Instantiate(icon);
        icon = Instantiate(icon);
        icon.transform.position = gameObject.transform.position;
        icon2.transform.position = gameObject.transform.position;
        icon3.transform.position = gameObject.transform.position;
        icon4.transform.position = gameObject.transform.position;
        icon5.transform.position = gameObject.transform.position;
        icon6.transform.position = gameObject.transform.position;
        icon7.transform.position = gameObject.transform.position;
        iconList = new List<Icon>();
        fireCoolDown = 0;
        fireCoolDownActive = false;
        kneedCoolDown = 0;
        kneedCoolDownActive = false;
        cutCoolDown = 0;
        cutCoolDownActive = false;

        // store particle's original off-screen position
        originalPositionFire = fire.transform.position;
        originalPositionCut = cut.transform.position;
        originalPositionKnead = knead.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //updates if time if cooldown is active
        if (fireCoolDown > 0 && fireCoolDownActive)
        {
            fireCoolDown -= Time.deltaTime;
        }
        // stops cooldown if it is below or at 0
        if (fireCoolDown <= 0)
        {
            fireCoolDown = 0;
            fireCoolDownActive = false;
        }
        //updates if time if cooldown is active
        if (kneedCoolDown > 0 && kneedCoolDownActive)
        {
            kneedCoolDown -= Time.deltaTime;
        }
        // stops cooldown if it is below or at 0
        if (kneedCoolDown <= 0)
        {
            kneedCoolDown = 0;
            kneedCoolDownActive = false;
        }
        //updates if time if cooldown is active
        if (cutCoolDown > 0 && cutCoolDownActive)
        {
            cutCoolDown -= Time.deltaTime;
        }
        // stops cooldown if it is below or at 0
        if (cutCoolDown <= 0)
        {
            cutCoolDown = 0;
            cutCoolDownActive = false;
        }

        //Set constructed icons to player position each frame
        icon.transform.position = gameObject.transform.position;
        icon2.transform.position = gameObject.transform.position;
        icon3.transform.position = gameObject.transform.position;
        icon4.transform.position = gameObject.transform.position;
        icon5.transform.position = gameObject.transform.position;
        icon6.transform.position = gameObject.transform.position;
        icon7.transform.position = gameObject.transform.position;

        if (iconList.Count == 0) //Must happen here or else original icon will not have time to instantiate, unfortunately means this is hardcoded until I find a better solution
        {
            iconList.Add(icon);
            icon2.GetComponent<SpriteRenderer>().sortingOrder = 2;
            iconList.Add(icon2);
            icon3.GetComponent<SpriteRenderer>().sortingOrder = 3;
            iconList.Add(icon3);
            icon4.GetComponent<SpriteRenderer>().sortingOrder = 4;
            iconList.Add(icon4);
            icon5.GetComponent<SpriteRenderer>().sortingOrder = 5;
            iconList.Add(icon5);
            icon6.GetComponent<SpriteRenderer>().sortingOrder = 6;
            iconList.Add(icon6);
            icon7.GetComponent<SpriteRenderer>().sortingOrder = 7;
            iconList.Add(icon7);
        }

        for (int i = 0; i < iconList.Count; i++)//Make all icons invisible so they can be refilled
        {
            iconList[i].Invisible();
        }

        if (playerInventory[0] == null)//Leave all icons invisible because there is nothing
        {
            //Leave invis
        }
        else if (playerInventory[0].id != FoodId.plate)//Only bottom icon is filled
        {
            iconList[0].SetIconType(playerInventory[0]);
        }
        else//Fill out plate icons
        {
            Plate tempPlate = (Plate)playerInventory[0];
            iconList[0].SetIconType(FoodId.plate);
            for (int i = 0; i < tempPlate.coreFoodlist.Count; i++)
            {
                if (i < iconList.Count)//Temp saftey net
                    iconList[i + 1].SetIconType(tempPlate.coreFoodlist[i]);
            }
        }

        GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Fire magic cooks the ingredient the player is holding
    /// </summary>
    public void FireMagic()
    {
        //check if cooldown is inactive
        if (!fireCoolDownActive)
        {
            // cycle through counters and check which one player is trying to interact with
            for (int i = 0; i < counterScripts.Count; i++)
            {
                if (counterScripts[i].isInteractable && isInteracting && counterScripts[i].inventory[0] != null && counterScripts[i].inventory[0].id == FoodId.plate)
                {
                    Vector3 targetPosition = counterScripts[i].transform.position;
                    float duration = 2.0f;

                    MoveParticleToLocation(targetPosition, duration, fire);

                    //cycle through plate inventory and place new food items in new plate with different cooked states
                    Plate tempPlate = (Plate)counterScripts[i].inventory[0];
                    foreach (FoodItem f in tempPlate.coreFoodlist)
                    {
                        // if raw, made cooked
                        if (f.foodState == CookState.raw)
                        {
                            f.foodState = CookState.cooked;
                        }
                        // if cooked, made burnt
                        else if (f.foodState == CookState.cooked)
                        {
                            f.foodState = CookState.burnt;
                        }
                        counterScripts[i].inventory[0] = tempPlate;
                        counterScripts[i].SetIcons();
                    }
                    fireCoolDown = 3;
                    fireCoolDownActive = true;
                }
            }
        }
    }

    /// <summary>
    /// Cuts toppings that have the cut function
    /// </summary>
    public void CutMagic()
    {
        if (!cutCoolDownActive)
        {
            for (int i = 0; i < counterScripts.Count; i++)
            {
                if (counterScripts[i].isInteractable && counterScripts[i].inventory[0] != null && isInteracting)
                {
                    Vector3 targetPosition = counterScripts[i].transform.position;
                    float duration = 2.0f;

                    MoveParticleToLocation(targetPosition, duration, cut);

                    // while I (liam) am pretty sure this is unnessassary I am going to leave it in for now
                    if (counterScripts[i].inventory[0].id == FoodId.mushroom || 
                        counterScripts[i].inventory[0].id == FoodId.onion || 
                        counterScripts[i].inventory[0].id == FoodId.olive || 
                        counterScripts[i].inventory[0].id == FoodId.pepper  ||
                        counterScripts[i].inventory[0].id == FoodId.pepperoni ||
                        counterScripts[i].inventory[0].id == FoodId.bacon ||
                        counterScripts[i].inventory[0].id == FoodId.pineapple)
                    {
                        // if uncut, cut
                        if (counterScripts[i].inventory[0].cutState == CutState.uncut)
                        {
                            counterScripts[i].inventory[0].cutState = CutState.cut;
                        }
                    }
                    counterScripts[i].SetIcons();
                    cutCoolDown = 3;
                    cutCoolDownActive = true;
                }
            }
        }
    }

    /// <summary>
    /// Kneeds dough and beef
    /// </summary>
    public void KneedMagic()
    {
        if (!kneedCoolDownActive)
        {
            //checks if player is interacting with a counter
            for (int i = 0; i < counterScripts.Count; i++)
            {
                if (counterScripts[i].isInteractable && counterScripts[i].inventory[0] != null && isInteracting)
                {
                    Vector3 targetPosition = counterScripts[i].transform.position;
                    float duration = 2.0f;

                    MoveParticleToLocation(targetPosition, duration, knead);

                    // checks if ID is beef or dough
                    if (counterScripts[i].inventory[0].id == FoodId.dough ||
                        counterScripts[i].inventory[0].id == FoodId.beef)
                    {
                        // if unkneeded, kneed
                        if (counterScripts[i].inventory[0].kneadState == KneadState.unkneaded)
                        {
                            counterScripts[i].inventory[0].kneadState = KneadState.kneaded;
                        }
                    }
                    counterScripts[i].SetIcons();
                    kneedCoolDown = 3;
                    kneedCoolDownActive = true;
                }
            }
        }
    }

    //helper methods for particle movement and activation
    /// <summary>
    /// Activates the particle system and moves it to target location
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="duration"></param>
    /// <param name="particleSystem"></param>
    public void MoveParticleToLocation(Vector3 targetPosition, float duration, ParticleSystem particleSystem)
    {
        // Activate the particle system
        particleSystem.Play();

        //Move the particle to the targe location over a specified duration
        StartCoroutine(MoveToLocation(targetPosition, duration, particleSystem));
    }

    //helper methods for particle movement and activation
    /// <summary>
    /// Activates the particle system and moves it to target location
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="duration"></param>
    /// <param name="gameObject"></param>
    public void MoveParticleToLocation(Vector3 targetPosition, float duration, GameObject gameObject)
    {
        //Move the particle to the targe location over a specified duration
        StartCoroutine(MoveToLocation(targetPosition, duration, gameObject));
    }

    /// <summary>
    /// Runs for the duration of time, then stops and moves off-screen
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="duration"></param>
    /// <param name="particleSystem"></param>
    /// <returns></returns>
    private IEnumerator MoveToLocation(Vector3 targetPosition, float duration, ParticleSystem particleSystem)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = particleSystem.transform.position;

        while (elapsedTime < duration)
        {
            particleSystem.transform.position = targetPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return the particle to its original position
        if (particleSystem == fire)
        {
            particleSystem.transform.position = originalPositionFire;
        }
        else if (particleSystem == cut)
        {
            particleSystem.transform.position = originalPositionCut;
        }

        // Disable the particle system
        particleSystem.Stop();
    }

    /// <summary>
    /// Runs for the duration of time, then stops and moves off-screen
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="duration"></param>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private IEnumerator MoveToLocation(Vector3 targetPosition, float duration, GameObject gameObject)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = gameObject.transform.position;

        while (elapsedTime < duration)
        {
            gameObject.transform.position = targetPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return the game object to its original position
        gameObject.transform.position = originalPositionKnead;
    }
}
