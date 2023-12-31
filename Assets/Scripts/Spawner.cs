using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {
    //SCRIPT CONNECTED TO PLAYER

    // not using currently - mia
    // public Transform spawnPoint;

    //GameObjects to be spawned: enemies and fieldsword
    public GameObject Stalker;
    public GameObject pumpking;
    public GameObject Shadev2;
    public GameObject fieldsword;
    public SaveManager SaveInfo;

    //Enemy Max Spawn

    //reference to PlayerWeapon to help determine collisions with sword and pickUp
    GameObject playerWeapon;

 
    //first monster spawns after
    public int startSpawnTime = 5;
    //monsters spawn every <spawnTime> seconds
    public int spawnTime = 10;

    // predertermined spawn points (due to having to take into account height)
    public Vector3 swordSpawn0 = new Vector3(-110.72f,-2.00f,104.97f);
    public Vector3 swordSpawn1 = new Vector3(35.54f,14.30f,90.26f);
    public Vector3 swordSpawn2 = new Vector3(-34.95f,0.01f,-47.64f);
    public Vector3 swordSpawn3 = new Vector3(3.31f,8.00f,-180.60f);
    public Vector3 swordSpawn4 = new Vector3(93.18f,0.00f,57.52f);

    // TODO probably will need to add random spawn locations for enemies too so they don't randomly fall off the map/spawn in inacessible areas

    //Max number of enemies (stalker and pumpking) allowed on field at a time
    public const int MAX_ENEMY = 5;
    //enemy count (stalker and pumpking) reference
    int enemyCount = 0;
    //shade enemy count, only 1!
    int shadeCount = 0;

    // health. How many times they can run into a standard enemy before death
    public int playerHealth = 3;
    public GameObject healthbar1;
    public GameObject healthbar2;
    public GameObject healthbar3;
    
    public AudioSource playerSound;
    public AudioClip hurt;

    public bool invuln = false;

    void Start () {
        //hides player sword until obtained
        playerWeapon = GameObject.FindWithTag("HoldWeap");
        
        if(SaveManager.shouldLoad)
        {
            playerHealth = SaveInfo.savedHealth;
        }
        else
        {
            SaveInfo.savedHealth = playerHealth;
        }

        if(!(SaveInfo.hasSword))
        {
            playerWeapon.SetActive(false);
            
            //spawns a sword from a randomly chosen coordinate out of 5 predetermined locations if no sword yet
            List<Vector3> swordSpawns = new List<Vector3>();
            swordSpawns.Add(swordSpawn0);
            swordSpawns.Add(swordSpawn1);
            swordSpawns.Add(swordSpawn2);
            swordSpawns.Add(swordSpawn3);
            swordSpawns.Add(swordSpawn4);
            
            int randomNum;
            if(SaveManager.shouldLoad)
            {
                randomNum = SaveInfo.swordIndex;
            }
            else
            {
                randomNum = Random.Range(0,5); //Random.Range(inclusive, exclusive)
                SaveInfo.swordIndex = randomNum;
            }
            
            
            // Debug.Log("Spawn Area: "+randomNum);
            Vector3 randomSpawnPoint = swordSpawns[randomNum];//index begins at 0
            Instantiate(this.fieldsword, randomSpawnPoint, Quaternion.Euler(180, 0, 0)); // Quat.identity = no rotation
            Debug.Log("Added a Sword, Spawn Area:"+randomNum);
        }

        // sets Health HUD
        healthbar1 = GameObject.FindWithTag("Health 1");
        Debug.Log("health bar: " + (healthbar1 != null));
        healthbar2 = GameObject.FindWithTag("Health 2");
        Debug.Log("health bar: " + (healthbar2 != null));
        healthbar3 = GameObject.FindWithTag("Health 3");
        Debug.Log("health bar: " + (healthbar3 != null));
        ShowHealthBar();
        
        //Spawning enemy in timed intervals
        InvokeRepeating ("Spawn", startSpawnTime, spawnTime);
    }
 
    void Update () {
     
    }
 
    private void Spawn () {
        Debug.Log("inside spawn");
        if (shadeCount == 0) {
            //spawns a singular shade, default spawn coordinates are coordinates in prefab
            Instantiate(this.Shadev2);
            shadeCount++;
        }
        if (enemyCount < MAX_ENEMY) {
            //Randomly chooses enemy to spawn 1 = stalker, 2 pumpking)
            //spawns an enemy, default spawn coordinates are coordinates in prefab, may need to specify spawn points
            int rndNum = Random.Range(1,3);//Random.Range(inclusive, exclusive)
            // Debug.Log("Random number: " + rndNum);
            if (rndNum == 1){
            Instantiate(this.Stalker);
            }
            else {
            Instantiate(this.pumpking);
            }
            enemyCount++;
            Debug.Log("spawned");
        }
    }

    private void OnTriggerEnter(Collider other){
        // running into sword
        if (other.gameObject.CompareTag("FieldWeap")) {
            Debug.Log("if field weap");
            Destroy(other.gameObject);
            Debug.Log("destroyed");
            playerWeapon.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(!invuln)
        {
            if (other.gameObject.CompareTag("Stalker") || other.gameObject.CompareTag("Pumpking"))
            {
                Debug.Log("Ran into an enemy took -1 damage! Health:"+playerHealth+" - 1 = " + (playerHealth-1));
                playerHealth--;
                SaveInfo.savedHealth--;

                playerSound.PlayOneShot(hurt);

                ShowHealthBar();
                if (playerHealth <= 0){
                    LoseScreen();
                }

                invuln = true;
                StartCoroutine(iFrames());
            }
            //running into shade GAME OVER?
            else if (other.gameObject.CompareTag("Shade")){
                Debug.Log("Ran into Shade. You Died!!!");
                 playerHealth = playerHealth-3;
                ShowHealthBar();
                LoseScreen();
            }
        }
    }

    private void LoseScreen(){
        // Ends game
        SceneManager.LoadSceneAsync("LoseScreen");
    }

    private void ShowHealthBar(){
        Debug.Log("In Show Health");
        if (playerHealth == 3) {
           healthbar1.SetActive(false);
           healthbar2.SetActive(false);
           healthbar3.SetActive(true);
        }
        else if (playerHealth == 2) {
            healthbar1.SetActive(false);
            healthbar3.SetActive(false);
            healthbar2.SetActive(true);
        }
        else if (playerHealth == 1){
            healthbar2.SetActive(false);
            healthbar3.SetActive(false);
            healthbar1.SetActive(true);
        }
        else if (playerHealth == 0) {
            healthbar1.SetActive(false);
            healthbar2.SetActive(false);
            healthbar3.SetActive(false);
        }
    }

    private IEnumerator iFrames()
    {
        yield return new WaitForSeconds(0.7f);

        invuln = false;
    }
 
}