using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    // tombol gerak atas
    public KeyCode upButton = KeyCode.W;

    // tombol gerak bawah
    public KeyCode downButton = KeyCode.S;

    // kecepatan gerak
    public float speed = 10.0f;

    // batas game scene
    public float yBoundary = 9.0f;

    // Rigidbody raket
    private Rigidbody2D rigidBody2D;

    //skor
    public int score;

    //untuk menampilkan variabel-variabel fisika terkait tumbukan tersebut
    private ContactPoint2D lastContactPoint;
    
    // trajectory
    private Vector2 trajectoryOrigin;
    
    // Untuk mengakses informasi titik kontak dari kelas lain
    public ContactPoint2D LastContactPoint
    {
        get { return lastContactPoint; }
    }

    // Ketika terjadi tumbukan dengan bola, rekam titik kontaknya.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            lastContactPoint = collision.GetContact(0);
        }
    }

    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    // Untuk mengakses informasi titik asal lintasan
    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }


    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        trajectoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // dapat kecepatan raket
        Vector2 velocity = rigidBody2D.velocity;

        // jika tekan tombol atas, beri kecepatan positif
        if (Input.GetKey(upButton))
        {
            velocity.y = speed;
        }

        // jika tekan tombol bawah, beri kecepatan negatif
        else if (Input.GetKey(downButton))
        {
            velocity.y = -speed;
        }

        // jika pemain tidak menekan tombol
        else
        {
            velocity.y = 0.0f;
        }

        rigidBody2D.velocity = velocity;


        // posisi raket
        Vector3 position = transform.position;

        // jika raket melewati batas atas, kembali ke batas tersebut
        if (position.y > yBoundary)
        {
            position.y = yBoundary;
        }

        // jika raket melewati batas bawah, kembali ke batas tersebut
        else if (position.y < -yBoundary)
        {
            position.y = -yBoundary;
        }

        transform.position = position;




}
    // naikkan skor 1 poin
    public void IncrementScore()
    {
        score++;
    }

    // kembali jd 0
    public void ResetScore()
    {
        score = 0;
    }

    // mendapat nilai score
    public int Score
    {
        get { return score; }
    }

}
