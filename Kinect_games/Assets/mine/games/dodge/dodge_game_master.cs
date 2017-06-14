using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class dodge_game_master : MonoBehaviour
{
    [System.Serializable]
    public class Bullet
    {
        public int bulletType;
        public GameObject mesh;
    }

    public Vector3 centerPosition_person;
    public float distanceToPerson;
    public float sideMargin;
    public float bulletSpeed;
    public int timeBetweenShots;
    public int numberOfLifes;
    public int oldBulletLimit;
    public int gameState = 0;

    public myKinectManager myKinManager;
    public Text instruction;
    public GameObject playerHead;
    public GameObject shooterObject;
    public GameObject bullet;
    public GameObject backWall;
    public Bullet[] bullets;
    public List<Bullet> currentBullets;
    public int collidedBullets;
    public int currentLifes;
    public int score;
    public List<GameObject> objsToDestroy;

    public GameObject LifeTemplate;
    public GameObject bulletFather;
    public List<GameObject> Lifes;
    public float spaceBetweenHearts;

    public bool workFlag;
    public bool waiting = false;
    public int secsToWait;
    public int secsWaited;
    public float startWaitingTime;

    // Use this for initialization
    void Start()
    {

    }

    void removeLife()
    {
        currentLifes--;
        Destroy(Lifes[Lifes.Count - 1]);
        Lifes.RemoveAt(Lifes.Count - 1);
    }

    void removeOldBullet()
    {
        //Destroy(objsToDestroy[0]);
        //objsToDestroy.RemoveAt(0);
        foreach(GameObject child in objsToDestroy)
        {
            GameObject.Destroy(child);
        }
        objsToDestroy = new List<GameObject>();
    }

    void waitFor(int seconds)
    {
        secsToWait = seconds;
        waiting = true;
        startWaitingTime = Time.time;
    }

    void waitButDontStop(int seconds)
    {
        secsToWait = seconds;
        startWaitingTime = Time.time;
    }

    bool doneWaiting()
    {
        bool result = false;

        float currentTime = Time.time;
        secsWaited = (int)(currentTime - startWaitingTime);
        if (secsWaited >= secsToWait)
        {
            result = true;
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (workFlag == true)
        {

            if (myKinManager == null)
            {
                myKinManager = myKinectManager.instance;
            }

            if (waiting)
            {
                float currentTime = Time.time;
                secsWaited = (int)(currentTime - startWaitingTime);
                if (secsWaited >= secsToWait)
                {
                    waiting = false;
                }
            }
            else
            {
                if (gameState == 0)
                {
                    instruction.text = "Welcome";
                    gameState = 1;
                    currentLifes = numberOfLifes;

                    Lifes.Add(LifeTemplate);

                    for (int i = 1; i < currentLifes; i++)
                    {
                        GameObject newLife = GameObject.Instantiate(LifeTemplate, Camera.main.transform);
                        newLife.transform.Translate(spaceBetweenHearts * i, 0, 0);
                        
                        Lifes.Add(newLife);
                    }

                    waitFor(2);
                }
                else if (gameState == 1)
                {
                    Debug.Log("placing things");

                    Vector3 headPosition = playerHead.transform.position;

                    shooterObject.transform.position = new Vector3(headPosition.x, headPosition.y, headPosition.z + distanceToPerson);
                    currentBullets = new List<Bullet>();
                    objsToDestroy = new List<GameObject>();

                    gameState = 2;
                    waitFor(1);
                }
                else if (gameState == 2)
                {
                    //check for collided bullets

                    instruction.text = "score: " + score;

                    for (int i = currentBullets.Count-1; i >-1; i--)
                    {
                        bullet bulletScript = currentBullets[i].mesh.GetComponent<bullet>();
                        if (bulletScript.collided == true || currentBullets[i].mesh.transform.position.z < playerHead.transform.position.z)
                        {
                            if (bulletScript.collided == false && currentBullets[i].mesh.transform.position.z < playerHead.transform.position.z)
                            {
                                score++;
                            }
                            else if (bulletScript.collisionObjectName != backWall.name && bulletScript.collisionObject.transform.parent.name != bulletFather.name)
                            {
                                Debug.Log("collision detected: bullet " + currentBullets[i].mesh.name + " with " + bulletScript.collisionObjectName + " or " + bulletScript.collisionObject.transform.parent.name);
                                removeLife();
                            }
                            objsToDestroy.Add(currentBullets[i].mesh);
                            currentBullets.RemoveAt(i);
                            //break;
                        }
                    }

                    if (currentLifes <= 0)
                    {
                        gameState = 99;
                    }
                    else
                    {
                        if (doneWaiting() == true)
                        {
                            int chosenBullet = Random.Range(0, bullets.Length);

                            currentBullets.Add(bullets[chosenBullet]);

                            GameObject newBullet = GameObject.Instantiate(currentBullets[currentBullets.Count - 1].mesh, bulletFather.transform);

                            currentBullets[currentBullets.Count - 1].mesh = newBullet;
                            currentBullets[currentBullets.Count - 1].mesh.transform.position = new Vector3(shooterObject.transform.position.x, shooterObject.transform.position.y, shooterObject.transform.position.z - 1f);
                            currentBullets[currentBullets.Count - 1].mesh.GetComponent<Rigidbody>().velocity = (playerHead.transform.position - currentBullets[currentBullets.Count - 1].mesh.transform.position).normalized * bulletSpeed;


                            //bullet.transform.position = new Vector3(shooterObject.transform.position.x, shooterObject.transform.position.y, shooterObject.transform.position.z - 0.75f);
                            //bullet.GetComponent<Rigidbody>().velocity = (playerHead.transform.position - bullet.transform.position).normalized * bulletSpeed;


                            waitButDontStop(timeBetweenShots);
                        }
                    }

                }
                else if (gameState == 99)
                {
                    instruction.text = "game over. Score: " + score;
                    workFlag = false;
                }
            }
        }
    }
}
