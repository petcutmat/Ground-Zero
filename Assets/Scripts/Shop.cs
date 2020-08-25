using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;
    public List<Item> itemList = new List<Item>();
    public GameObject master;
    public GameObject flowerImage;
    public GameObject floorImage;
    public GameObject smokeImage;
    public GameObject[] inventory = new GameObject[2];
    public Sprite placeHolderSprite;
    public AudioClip flowerSFX;
    public AudioClip floorSFX;
    public AudioClip smokeSFX;
    public AudioClip errorSFX;

    public struct Item {
        public GameObject image;
        public string description;
        public int points;

        public Item(GameObject image, string description, int points) {
            this.image = image;
            this.description = description;
            this.points = points;
        }
    }

    void Start(){
        inventory[0] = transform.GetChild(1).gameObject;
        inventory[1] = transform.GetChild(2).gameObject;

        Item i1 = new Item(flowerImage, "Loto", 900);
        itemList.Add(i1);
        Item i2 = new Item(floorImage, "Teleregreso", 600);
        itemList.Add(i2);
        Item i3 = new Item(smokeImage, "Bomba de humo", 150);
        itemList.Add(i3);
        Reshuffle();
    }

    public void Reshuffle(){
        List<Item> provisionalList = new List<Item>();
        foreach(Item i in itemList) provisionalList.Add(i);

        Item i1 = provisionalList[Random.Range(0, provisionalList.Count)];
        shopPanel.transform.GetChild(2).GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = i1.image.GetComponent<SpriteRenderer>().sprite;
        shopPanel.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = i1.points.ToString() +"p";
        shopPanel.transform.GetChild(2).GetChild(3).GetComponent<Text>().text = i1.description;

        provisionalList.Remove(i1);

        Item i2 = provisionalList[Random.Range(0, provisionalList.Count)];
        shopPanel.transform.GetChild(3).GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = i2.image.GetComponent<SpriteRenderer>().sprite;
        shopPanel.transform.GetChild(3).GetChild(2).GetComponent<Text>().text = i2.points.ToString() + "p";
        shopPanel.transform.GetChild(3).GetChild(3).GetComponent<Text>().text = i2.description;
    }

    public void ToggleShop() {
        if (shopPanel.activeSelf == true) shopPanel.SetActive(false);
        else shopPanel.SetActive(true);
        shopPanel.transform.GetChild(1).GetComponentInChildren<Text>().text = "Objetos de gran calidad!";
        shopPanel.transform.GetChild(1).GetComponentInChildren<Text>().color = Color.white;
    }

    public void BuyItem(GameObject item){
        int invItems = 0;
        foreach (GameObject inv in inventory) {
            if (inv.transform.GetChild(0).GetComponent<Image>().sprite != placeHolderSprite)  invItems++;
        }
        if (invItems < 2){
            Transform currentPlayer = master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1);
            string srPoints = removeLastChar(item.transform.GetChild(2).GetComponent<Text>().text); //eliminamos la p
            if (currentPlayer.GetComponent<Points>().points >= int.Parse(srPoints)) {
                currentPlayer.GetComponent<Points>().points -= int.Parse(srPoints); //pagar objeto
                foreach (GameObject inv in inventory){
                    if (inv.transform.GetChild(0).GetComponent<Image>().sprite == placeHolderSprite){
                        GetComponent<AudioSource>().Play();
                        inv.transform.GetChild(0).GetComponent<Image>().sprite = item.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite;
                        if (item.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite == 
                            flowerImage.GetComponent<SpriteRenderer>().sprite) inv.GetComponent<Button>().onClick.AddListener(() => ActivateLotus(inv) );
                        if (item.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite == 
                            floorImage.GetComponent<SpriteRenderer>().sprite) inv.GetComponent<Button>().onClick.AddListener(() => ActivateTpFloor(inv) );
                        if (item.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite == 
                            smokeImage.GetComponent<SpriteRenderer>().sprite) inv.GetComponent<Button>().onClick.AddListener(() => ActivateSmoke(inv) );
                    break;
                    }
                }
                Reshuffle();
            } else {
                GetComponent<AudioSource>().PlayOneShot(errorSFX);
                shopPanel.transform.GetChild(1).GetComponentInChildren<Text>().text = "No tienes el dinero necesario";
                shopPanel.transform.GetChild(1).GetComponentInChildren<Text>().color = Color.yellow;
            }
        } else{
            GetComponent<AudioSource>().PlayOneShot(errorSFX);
            shopPanel.transform.GetChild(1).GetComponentInChildren<Text>().text = "Tienes el maximo de 2 objetos";
            shopPanel.transform.GetChild(1).GetComponentInChildren<Text>().color = Color.yellow;
        }
       
    }
    private static string removeLastChar(string str){
        return str.Substring(0, str.Length - 1);
    }

    void ActivateLotus(GameObject inv) {
        if (master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn-1).GetComponent<Movement>().steps == 0){
            inv.transform.GetChild(0).GetComponent<Image>().sprite = placeHolderSprite;
            if (master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Health>().healthPoints < 3)
                master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Health>().healthPoints += 1;
            master.GetComponent<MasterScript>().UpdateHealthBar();
            inv.GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<AudioSource>().PlayOneShot(flowerSFX);
        }
        
    }
    void ActivateSmoke(GameObject inv) {
        if (master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Movement>().steps == 0) {
            inv.transform.GetChild(0).GetComponent<Image>().sprite = placeHolderSprite;
            master.GetComponent<MiniGame>().character.GetComponent<CharacterController>().charges += 1;
            inv.GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<AudioSource>().PlayOneShot(smokeSFX);
        }
    }
    void ActivateTpFloor(GameObject inv) {
        if (master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Movement>().steps == 0) {
            int[] choices = new int[master.GetComponent<MasterScript>().maxPlayers];
            for (int i = 0; i < master.GetComponent<MasterScript>().maxPlayers; i++){
                choices[i] = i;
            }
            int randPlayer = Random.Range(0, choices.Length);

            inv.transform.GetChild(0).GetComponent<Image>().sprite = placeHolderSprite;
            StartCoroutine(master.GetComponent<MasterScript>().players.transform.GetChild(randPlayer).GetComponent<Movement>().TpBack(5, randPlayer + 1));
            inv.GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<AudioSource>().PlayOneShot(floorSFX);
        }
    }
}
