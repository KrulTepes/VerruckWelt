using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environments
{
    public class MovingPlatform : MonoBehaviour
    {
        public enum Axis
        {
            Horizontal,
            Vertical
        }
        
        [Tooltip("First element always start position")]
        public List<Vector3> pointList;
        public float speed;
        public Axis axis;
        public float timeDelay;

        protected float _localTimeDelay;
        protected int _currentPoint = 0;

        protected Transform _oldPlayerParent; 

        private void Awake()
        {
            if (pointList == null)
                pointList = new List<Vector3>();

            if (pointList.Count == 0)
                pointList.Add(this.transform.position);
            else
                pointList[0] = transform.position;
        }

        private void FixedUpdate()
        {
            if (pointList.Count == 1)
                return;

            if (_localTimeDelay > 0f)
            {
                _localTimeDelay -= Time.fixedDeltaTime;
                return;
            }
        
            if (transform.position == pointList[_currentPoint])
            {
                _localTimeDelay = timeDelay;
            
                if (_currentPoint + 1 < pointList.Count)
                    _currentPoint++;
                else
                    _currentPoint = 0;
            }
            
            transform.position = MovingAxisFunction(transform.position, pointList[_currentPoint], axis, speed * Time.fixedDeltaTime);
        }

        private Vector3 MovingAxisFunction(Vector3 pointA, Vector3 pointB, Axis axis, float speed)
        {
            if (axis == Axis.Horizontal)
            {
                if (pointA.x == pointB.x)
                    return pointA;
                
                int sign = Math.Sign(pointB.x - pointA.x);
                pointA.x += sign * speed;
                if (Math.Sign(pointB.x - pointA.x) != sign)
                {
                    pointA.x = pointB.x;
                }
            }
            else if (axis == Axis.Vertical)
            {
                if (pointA.y == pointB.y)
                    return pointA;
                
                int sign = Math.Sign(pointB.y - pointA.y);
                pointA.y += sign * speed;
                if (Math.Sign(pointB.y - pointA.y) != sign)
                {
                    pointA.y = pointB.y;
                }
            }

            return pointA;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _oldPlayerParent = other.transform.parent;
                other.transform.parent = this.transform;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.parent = _oldPlayerParent;
            }
        }
    }
}
