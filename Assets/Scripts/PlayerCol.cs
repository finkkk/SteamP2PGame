using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerCol : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float boostMulitplier = 4f;
    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private TextMesh playerNameText;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private GameObject flipObjects;
    [SerializeField]
    private Transform groundCheck;

    private Rigidbody2D rb;
    private bool boost;
    private bool isGrounded;
    private Vector2 moveDircetion;
    private bool facingRight = true;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string playerName;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1.0f;

        playerNameText.color = Color.black;

        if(isLocalPlayer)
        {
            Camera.main.transform.SetParent(transform, false);
            Camera.main.transform.localPosition = new Vector3(0, 0, -10);
            // 获取steam用户名并且同步到服务器
            string steamName = SteamFriends.GetPersonaName();
            if (NetworkClient.ready)
            {
                CmdSetPlayerName(steamName);
            }
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            float moveX = Input.GetAxis("Horizontal");
            moveDircetion = new Vector2(moveX, 0).normalized;

            boost = Input.GetKey(KeyCode.LeftShift);

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }

            playerNameText.text = SteamFriends.GetPersonaName();

            if(moveX > 0 && !facingRight)
            {
                Flip();
            }
            else if(moveX < 0 && facingRight)
            {
                Flip();
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        }
    }
    private void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            float currentMoveSpeed = moveSpeed * (boost ? boostMulitplier : 1);
            rb.velocity = new Vector2(moveDircetion.x * currentMoveSpeed,rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = flipObjects.transform.localScale;
        theScale.x *= -1;
        flipObjects.transform.localScale = theScale;
    }

    private void OnNameChanged(string oldName,string newName)
    {
        playerNameText.text  = newName;
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
    }
}
