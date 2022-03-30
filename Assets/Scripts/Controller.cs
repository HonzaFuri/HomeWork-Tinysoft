using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    public enum Bet { None, One = 1, Two = 2, Tree = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10 }
    public Bet bet;
    public GameObject wheel;
    public Button spinTrigger;
    public Text CoinsRewardsText;
    public Text CurrentCoinsText;
    public Text betH;
    public Image noCredits;
    public Image selectAmount;
    public Text percentageText;
    public Slider slider;
    public int creditAmount = 10000;
    public int PreviousCoinsAmount;
    float[] winningAngles;
    float rotationTime;
    float speed = 10f;
    float randomValue;
    int priceNumber;
    int bettingAmount;
    public int keepbetting;
    bool wheelIsSpining;
    public int[] multipleChance;

    void Awake() 
    {
        PreviousCoinsAmount = creditAmount;
        CurrentCoinsText.text = creditAmount.ToString();
        winningAngles = new float[] { 36, 72, 108, 144, 180, 216, 252, 288, 324, 360};
        //multipleChance = new int[] {0, 1, 10, 100};
        betH.GetComponent<Text>();
        noCredits.GetComponent<Image>();
        selectAmount.GetComponent<Image>();
        spinTrigger.GetComponent<Button>();
        percentageText.GetComponent<Text>();
        slider.GetComponent<Slider>();
        //int customSeed = 1234;
        //Random.InitState(customSeed);
    }
    public void BetSwitch(int betSize)
    {
        Bet bet = (Bet)betSize;
            switch(bet)
            {
                case Bet.One:
                    BetUpdate(100);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Two:
                    BetUpdate(200);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Tree:
                    BetUpdate(300);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Four:
                    BetUpdate(400);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Five:
                    BetUpdate(500);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Six:
                    BetUpdate(1000);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Seven:
                    BetUpdate(2000);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Eight:
                    BetUpdate(3000);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Nine:
                    BetUpdate(4000);
                    SetNoEnoughtCoinsToFalse();
                    break;
                case Bet.Ten:
                    BetUpdate(5000);
                    SetNoEnoughtCoinsToFalse();
                    break;
                // Number 0 is set on Spin button
                case Bet.None:
                    if(creditAmount >= keepbetting && creditAmount > 0 && keepbetting > 0)
                    {
                        RunFirstChanceForSpin();
                        SpinWheel();
                        wheelIsSpining = true;
                        BetUpdate(keepbetting);
                        SetNoEnoughtCoinsToFalse();
                    }
                    else
                    {
                        noCredits.gameObject.SetActive(true);
                    }
                    break;
                default:
                    break;
            } 
    }
    void RunFirstChanceForSpin()
    {
        ChanceForFirstSpin();
        Debug.Log("Spinning wheel number: " + priceNumber);
    }
    public int BetUpdate(int bettingAmount)
    {
        if (creditAmount > 0 && creditAmount >= bettingAmount)
        {
            if (wheelIsSpining)
            {
                PreviousCoinsAmount = creditAmount;
                creditAmount -= bettingAmount;
                CurrentCoinsText.text = "-" + creditAmount;
    	        CurrentCoinsText.gameObject.SetActive (true);
    	        StartCoroutine(UpdateCoinsAmount());
                StartCoroutine(HideWiningChance());
                wheelIsSpining = false;
            }
        }
        keepbetting = bettingAmount;
        betH.text = "Bet: " + bettingAmount.ToString();
        return keepbetting;
    }

    public void PercentageUpdate(float value)
    {   
        percentageText.text = Mathf.RoundToInt(value) + "%";
        value *= creditAmount / 100;
        betH.text = "Bet: " + value.ToString();
        keepbetting = Mathf.RoundToInt(value);
        StartCoroutine(UpdateCoinsAmount());
        if (creditAmount >= keepbetting)
        {
            noCredits.gameObject.SetActive(false);
        }
    }
    void SetNoEnoughtCoinsToFalse()
    {
        noCredits.gameObject.SetActive(false);
    }
    public void SpinWheel()
    {
        StartCoroutine(StartSpin());
    }
    IEnumerator StartSpin()
    {
        spinTrigger.interactable = false;
        yield return new WaitForSeconds(0.2f);
        rotationTime = 0;
        while (rotationTime < 3)
        {
            wheel.transform.Rotate(0, 0, 70f * speed * Time.deltaTime);
            rotationTime += Time.deltaTime;
            yield return null;
        }
        RewardAngle();
        ChanceForWiningSpin();
        spinTrigger.interactable = true;
    }
    void RewardAngle()
    {
        switch(priceNumber)
        {
            case 10:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[0]);
            break;
            case 4:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[1]);
            break;
            case 7:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[2]);
            break;
            case 6:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[3]);
            break;
            case 3:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[4]);
            break;
            case 9:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[5]);
            break;
            case 8: 
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[6]);
            break;
            case 2:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[7]);
            break;
            case 5:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[8]);
            break;
            case 1:
            wheel.transform.eulerAngles = new Vector3 (0, 0, winningAngles[9]);
            break;
            default:
            break;
        }
    }
    void RewardCoins(int awardReward)
    {
        creditAmount += keepbetting * awardReward;
        CoinsRewardsText.text = "x" + awardReward;

        CoinsRewardsText.gameObject.SetActive (true);
    	CurrentCoinsText.text = creditAmount.ToString();
        StartCoroutine(UpdateCoinsAmount());
    }
    public int ChanceForWiningSpin()
    {
        int randomValueForPrice = Random.Range(0, 101);
        if(randomValueForPrice <= 80)
        {
              RewardCoins(multipleChance[0]);
              Debug.Log("80% chance to multiple, number: " + randomValueForPrice);
              return multipleChance[0];
        }
        else if(randomValueForPrice > 80 && randomValueForPrice <= 88)
        {
              RewardCoins(multipleChance[1]);
              Debug.Log("8% chance to multiple, number:" + randomValueForPrice);
              return multipleChance[1];
        }
        else if(randomValueForPrice > 88 && randomValueForPrice <= 93)
        {
              RewardCoins(multipleChance[2]);
              Debug.Log("5% chance to multiple, number: " + randomValueForPrice);
              return multipleChance[2];
        }
        else if (randomValueForPrice > 93 && randomValueForPrice <= 96)
        {
             RewardCoins(multipleChance[3]);
             Debug.Log("3% chance to multiple, number: " + randomValueForPrice);
             return multipleChance[3];
        }
        else if (randomValueForPrice > 96 && randomValueForPrice <= 99)
        {
            RewardCoins(multipleChance[4]);
            Debug.Log("3% chance to multiple, number: " + randomValueForPrice);
            return multipleChance[4];
        }
        else
        {
            RewardCoins(multipleChance[5]);
            Debug.Log("1% chance to multiple, number: " + randomValueForPrice);
            return multipleChance[5];
        }
    }
    /*void RewardPrice()
    {
          switch(priceNumber)
        {
            case 10:
            RewardCoins(100);
            break;
            case 4:
            RewardCoins(200);
            break;
            case 7:
            RewardCoins(300);
            break;
            case 6:
            RewardCoins(400);
            break;
            case 3:
            RewardCoins(500);
            break;
            case 9:
            RewardCoins(1000);
            break;
            case 8: 
            RewardCoins(2000);
            break;
            case 2:
            RewardCoins(3000);
            break;
            case 5:
            RewardCoins(4000);
            break;
            case 1:
            RewardCoins(5000);
            break;
        }
    }
    */
    int ChanceForFirstSpin()
    { 
        randomValue = Random.Range(0, 101);
        if(randomValue > 0 && randomValue <= 25)
        {
            priceNumber = 10;
            Debug.Log("25% for number: 10, actual number: " + randomValue);
        }
        else if (randomValue > 25 && randomValue <= 47)
        {
            priceNumber = 9;
            Debug.Log("22% for number: 9, actual number: " + randomValue);
        }
        else if (randomValue > 47 && randomValue <= 62)
        {
            priceNumber = 8;
            Debug.Log("15% for number: 8, actual number: " + randomValue);
        }
        else if (randomValue > 62 && randomValue <= 73)
        {
            priceNumber = 7;
            Debug.Log("11% for number: 7, actual number: " + randomValue);
        }
        else if (randomValue > 73 && randomValue <= 82)
        {
            priceNumber = 6;
            Debug.Log("9% for number: 6, actual number: " + randomValue);
        }
        else if (randomValue > 82 && randomValue <= 89)
        {
            priceNumber = 5;
            Debug.Log("7% for number: 5, actual number: " + randomValue);
        }
        else if (randomValue > 89 && randomValue <= 94)
        {
            priceNumber = 4;
            Debug.Log("5% for number: 4, actual number: " + randomValue);
        }
        else if (randomValue > 94 && randomValue <= 97)
        {
            priceNumber = 3;
            Debug.Log("3% for number: 3, actual number: " + randomValue);
        }
        else if (randomValue > 97 && randomValue <= 99)
        {
            priceNumber = 2;
            Debug.Log("2% for number: 2, actual number: " + randomValue);
        }
        else
        {
            priceNumber = 1;
            Debug.Log("1% for number: 1, actual number: " + randomValue);
        }
        return priceNumber;
    }
    private IEnumerator HideWiningChance ()
    {
        yield return new WaitForSeconds(0.01f);
	    CoinsRewardsText.gameObject.SetActive (false);
    }
    private IEnumerator UpdateCoinsAmount()
    {
    	const float seconds = 0.5f;
    	float elapsedTime = 0;
    
    	while (elapsedTime < seconds) {
    	    CurrentCoinsText.text = Mathf.Floor(Mathf.Lerp (PreviousCoinsAmount, creditAmount, (elapsedTime / seconds))).ToString ();
    	    elapsedTime += Time.deltaTime;
    
    	    yield return new WaitForEndOfFrame ();
        }
    	PreviousCoinsAmount = creditAmount;
    	CurrentCoinsText.text = creditAmount.ToString();
    } 
}  
