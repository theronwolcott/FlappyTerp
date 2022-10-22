using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 10f;
    private const float PIPE_DESTROY_X = -100f;
    float pipeHeadY;
    float pipeBodyY;

    private List<Pipe> pipes;

    private void Awake() {
        pipes = new List<Pipe>();
    }

    private void Start() {
        //CreatePipe(40f, 20f, true);
        //CreatePipe(40f, 20f, false);
        CreateGaps(50f, 20f, 20f);
        //CreatePipe(30f, 40f, true);
        //CreatePipe(20f, 60f, false);
    }

    private void Update() {
        HandlePipeMovement();
    }

    private void HandlePipeMovement() {
        for (int i = 0; i < pipes.Count; i++) {
            // move pipe across the screen
            Pipe pipe = pipes[i];
            pipe.Move();
            // destroy pipes after they pass by
            if (pipe.GetX() < PIPE_DESTROY_X) {
                pipes.Remove(pipe);
                pipe.DestroyPipe();
                // need to iterate same index after removing
                i--;
            }
        }
        /*foreach (Pipe pipe in pipes) {
            pipe.Move();
            // destroy pipes after they pass by
            if (pipe.GetX() < PIPE_DESTROY_X) {
                pipe.DestroyPipe();
            }
        } */
    }
    
    private void CreateGaps(float y, float size, float xpos) {
        // bottom pipe
        CreatePipe(y - (size / 2f), xpos, true);
        // top pipe
        CreatePipe(100 - y - (size / 2), xpos, false);
    }

    private void CreatePipe(float height, float xpos, bool bottom) {

        // make head at x and top of body, height/2 makes sure head doesn't stick above body 
        Transform pipeHead = Instantiate(GameAssets.GetInstance().PipeHeadpf);
        
        if (bottom) {
            pipeHeadY = height - (PIPE_HEAD_HEIGHT / 2f) - 50;
        } else {
            pipeHeadY = (PIPE_HEAD_HEIGHT / 2f) - height + 50;
        }
        pipeHead.position = new Vector3(xpos, pipeHeadY);

        // make body at x and bottom of screen (-50 is bottom)
        Transform pipeBody = Instantiate(GameAssets.GetInstance().PipeBodypf);

        if (bottom) {
            pipeBodyY = -50;
        } else {
            pipeBodyY = 50;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xpos, pipeBodyY);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyCollider.offset = new Vector2(0f, (height / 2f));

        // make a Pipe object and add to list
        Pipe pipe = new Pipe(pipeHead, pipeBody);
        pipes.Add(pipe);
    }

    private class Pipe {

        private Transform headTrans;
        private Transform bodyTrans;

        public Pipe(Transform headTrans, Transform bodyTrans) {
            this.headTrans = headTrans;
            this.bodyTrans = bodyTrans;
        }

        public void Move() {
            headTrans.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            bodyTrans.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetX() {
            return headTrans.position.x;
        }

        public void DestroyPipe() {
            Destroy(headTrans.gameObject);
            Destroy(bodyTrans.gameObject);
        }
    }
}
