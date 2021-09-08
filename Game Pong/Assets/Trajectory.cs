using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    // skrip, collider, dan rigidbody bola
    public BallControl ball;
    CircleCollider2D ballCollider;
    Rigidbody2D ballRigidbody;

    // bola "bayangan" yang ditampilkan
    public GameObject ballAtCollision;

    // Inisialisasi rigidbody dan collider
    void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // inisiasi status pantulan lintasan, yg ditampilkan jika lintasan bertumbukan dengan object tertentu
        bool drawBallAtCollision = false;

        // titik tumbukan digeser, untuk gambar ballatcoll
        Vector2 offsetHitPoint = new Vector2();

        // tentukan titik tumbuk dengan deteksi gerak lingkaran
        RaycastHit2D[] circleCastHit2DArray = 
        Physics2D.CircleCastAll(ballRigidbody.position, ballCollider.radius, ballRigidbody.velocity.normalized);

        // for each tumbukan
        foreach (RaycastHit2D circleCastHit2D in circleCastHit2DArray)
        {
            // if bertumbuk, dan tidak dengan bola
            // karena garis lintasan digambar dari titik tengan bola

            if (circleCastHit2D.collider != null &&
                circleCastHit2D.collider.GetComponent<BallControl>() == null)
            {
                // tentukan titik tumbukan
                Vector2 hitPoint = circleCastHit2D.point;

                // tentukan normal di titik tumbukan
                Vector2 hitNormal = circleCastHit2D.normal;

                //tentukan offset hitpoint, yaitu titik tengan bola
                offsetHitPoint = hitPoint + hitNormal * ballCollider.radius;

                // gambar garis lintasan dari titik tengah bola saat ini ke titik tengah bola bertumbukan
                DottedLine.DottedLine.Instance.DrawDottedLine(ball.transform.position, offsetHitPoint);


                // Kalau bukan sidewall, gambar pantulannya
                if (circleCastHit2D.collider.GetComponent<SideWall>() == null)
                {
                    // Hitung vektor datang
                    Vector2 inVector = (offsetHitPoint - ball.TrajectoryOrigin).normalized;

                    // Hitung vektor keluar
                    Vector2 outVector = Vector2.Reflect(inVector, hitNormal);

                    // Hitung dot product dari outVector dan hitNormal. Digunakan supaya garis lintasan ketika 
                    // terjadi tumbukan tidak digambar.
                    float outDot = Vector2.Dot(outVector, hitNormal);
                    if (outDot > -1.0f && outDot < 1.0)
                    {
                        // Gambar lintasan pantulannya
                        DottedLine.DottedLine.Instance.DrawDottedLine(
                            offsetHitPoint,
                            offsetHitPoint + outVector * 10.0f);

                        // Untuk menggambar bola "bayangan" di prediksi titik tumbukan
                        drawBallAtCollision = true;
                    }
                }

                // Hanya gambar lintasan untuk satu titik tumbukan, jadi keluar dari loop
                break;
            }
            // Jika true, ...
            if (drawBallAtCollision)
            {
                // Gambar bola "bayangan" di prediksi titik tumbukan
                ballAtCollision.transform.position = offsetHitPoint;
                ballAtCollision.SetActive(true);
            }
            else
            {
                // Sembunyikan bola "bayangan"
                ballAtCollision.SetActive(false);
            }


        }





    }
}
