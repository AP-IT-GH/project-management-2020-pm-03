﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyAI : MonoBehaviour
{
    // Variables
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;
    public Vector2 force;
    public Animator animator;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;


    void Start()
    {
        // Search for the desired components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // Repeatfunction for updated pathfinding
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Pathfinding
    private void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);

    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }



    void FixedUpdate()
    {
        // Follow path
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    
    void Update()
    {
        // Face the right direction (animation)
        if (force.x >= 0.01f)
        {
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("LastMove", 1);

        }
        else if (force.x < 0.01f)
        {
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("LastMove", -1);
        }
    }
}
