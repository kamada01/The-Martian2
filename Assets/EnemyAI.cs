using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Public variable that contains the speed of the enemy

    public static int score = 0;
    public static int life = 5;
    [SerializeField] private int movSpd = 1;
    [SerializeField] private int bounceForce = 1;
    private float elapsedTime = 0.0f;

    GameObject player;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        /*Vector2 v = rb.velocity;
        v.x = 0;
        v.y = speed;
        rb.velocity = v;
        rb.angularVelocity = Random.Range(-200, 200);*/
        //Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        elapsedTime += Time.deltaTime;
        FollowPlayer();
    }

    void FollowPlayer() {

        /*transform.up =  - transform.position;

        rb.AddForce(transform.up * movSpd);*/
        Vector3 aimDirection = (player.transform.position - transform.position).normalized;

        // Set the bullet's direction
        rb.GetComponent<Rigidbody2D>().velocity = aimDirection * movSpd;

        if (GlobalVariables.deadLabel == 1){
            rb.GetComponent<Rigidbody2D>().velocity = aimDirection * 0;
        }

    }


    //function called when the enemy collides with another object
    void OnTriggerEnter2D(Collider2D obj)
    {
        string name = obj.gameObject.name;

        // if collided with bullet
        if (name == "Bullet(Clone)")
        {
            
            score += 1;
            //Instantiate(fireEffect, transform.position, Quaternion.identity);
            //AudioSource.PlayClipAtPoint(myClip, transform.position);
            // Destroy itself (the enemy)
            Destroy(gameObject);

            // And destroy the bullet
            Destroy(obj.gameObject);
        }

        // if collided with the spaceship
        if (name == "Astronaut")
        {
            life -= 1;
            //Instantiate(fireEffect, transform.position, Quaternion.identity);
            //AudioSource.PlayClipAtPoint(myClip, transform.position);
            // destroy itself (the enemy) to keep things simple
            Destroy(gameObject);
        }

    }
    void OnGUI()
    {
        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(100,0,400,400), "Kill Count: " + score);
        GUI.Label(new Rect(100,10,400,400), "Life: " + life);
        DrawLabelWithBackground(new Rect(Screen.width / 2 - 50, 10, 100, 20), "Time: " + formattedTime);

        if (life <= 0){
            GUI.Label(new Rect(Screen.width/2-100,Screen.height/2,500,100), "Game Over!");
            GlobalVariables.deadLabel = 1;
        }
    }

    void DrawLabelWithBackground(Rect position, string text)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.normal.background = Texture2D.whiteTexture;
        labelStyle.normal.textColor = Color.black;
        GUI.Label(position, text, labelStyle);
    }


}
