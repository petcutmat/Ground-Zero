using UnityEngine;

public class Info : MonoBehaviour
{
    public GameObject diceCamera;
    public GameObject infoCamera;
    public GameObject mainCamera;
    public GameObject textObjective;
    public GameObject textPwUp;
    public GameObject textShop;
    public GameObject textMinigame;
    public GameObject videoObjective;
    public GameObject videoPwUp;
    public GameObject videoShop;
    public GameObject videoMinigame;

    public void ShowInfo()
    {
        Time.timeScale = 0;
        mainCamera.SetActive(false);
        diceCamera.SetActive(false);
        infoCamera.SetActive(true);
    }
    public void HideInfo()
    {
        Time.timeScale = 1;
        infoCamera.SetActive(false);
        diceCamera.SetActive(true);
        mainCamera.SetActive(true);
    }

    public void SelectMinigameText()
    {
        textMinigame.SetActive(true);
        textPwUp.SetActive(false);
        textObjective.SetActive(false);
        textShop.SetActive(false);
        videoObjective.SetActive(false);
        videoMinigame.SetActive(true);
        videoPwUp.SetActive(false);
        videoShop.SetActive(false);
    }
    public void SelectShopText()
    {
        textMinigame.SetActive(false);
        textPwUp.SetActive(false);
        textObjective.SetActive(false);
        textShop.SetActive(true);
        videoObjective.SetActive(false);
        videoMinigame.SetActive(false);
        videoPwUp.SetActive(false);
        videoShop.SetActive(true);
    }
    public void SelectObjectiveText()
    {
        textMinigame.SetActive(false);
        textPwUp.SetActive(false);
        textObjective.SetActive(true);
        textShop.SetActive(false);
        videoObjective.SetActive(true);
        videoMinigame.SetActive(false);
        videoPwUp.SetActive(false);
        videoShop.SetActive(false);
    }
    public void SelectPwUpText()
    {
        textMinigame.SetActive(false);
        textPwUp.SetActive(true);
        textObjective.SetActive(false);
        textShop.SetActive(false);
        videoObjective.SetActive(false);
        videoMinigame.SetActive(false);
        videoPwUp.SetActive(true);
        videoShop.SetActive(false);
    }

}
