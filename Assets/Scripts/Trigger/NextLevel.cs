﻿using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : Triggerable
{
    public GameObject Camera;
    public GameObject Player;
    public GameObject GameManager;

    public Transform cameraTransform;
    public float MoveSpeed;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Trigger(TriggerAction action)
    {
        if (action != TriggerAction.Activate) return;
        GameManager.GetComponent<GameManager>().enabled = false;

        gameObject.SetActive(true);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        StartCoroutine(RunCatscene(1.5f));
    }

    IEnumerator RunCatscene(float time)
    {
        yield return new WaitForSeconds(time);
        
        Player.GetComponent<CharacterMovement>().enabled = false;
        Camera.GetComponent<MouseLook>().enabled = false;

        Camera.transform.parent = null;
        Camera.transform.rotation = cameraTransform.rotation;
        yield return MoveToTarget();
    }

    IEnumerator MoveToTarget()
    {
        while (true)
        {
            Vector3 offset = cameraTransform.position - Camera.transform.position;
            float distance = offset.magnitude;
            float moveDistance = (MoveSpeed / distance) * Time.deltaTime;
            if (moveDistance < distance)
            {
                Vector3 move = offset.normalized * moveDistance;
                Camera.transform.position = Camera.transform.position + move;
                // cameraRegidBody.MovePosition(transform.position + move);
                yield return null;
            }
            else
            {
                break;
            }
        }

        Camera.transform.position = cameraTransform.position;
        // cameraRegidBody.MovePosition(cameraTransform.position);
    }
}
