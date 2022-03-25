using Photon.Pun;
using UnityEngine;


public class AvatorController : MonoBehaviourPunCallbacks
{
    [SerializeField] Rigidbody2D rb2d;
    private void FixedUpdate()
    {
        // ���g�����������I�u�W�F�N�g�����Ɉړ��������s��
        if (photonView.IsMine)
        {
            var input = new Vector3(/*Input.GetAxis("Horizontal")*/0f, Input.GetAxisRaw("Vertical"), 0f);
            transform.Translate(6f * Time.deltaTime * input.normalized);
            //var input = Input.GetAxisRaw("Vertical");
            //rb2d.AddForce(new Vector2(0,input));
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            rb2d.velocity = Vector3.zero;
        }
    }
}