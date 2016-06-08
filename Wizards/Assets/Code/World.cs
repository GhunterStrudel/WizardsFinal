using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Code
{
    class World : MonoBehaviour
    {
        //Get list of rooms
        private List<GameObject> rooms;

        public GameObject activeRoom;

        public GameObject player;

        public CameraFollow cameraScript;
        public MusicManager mm;
        void Start()
        {
            rooms = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                rooms.Add(transform.GetChild(i).gameObject);
            }
            //Debug.Log(transform.childCount);
            NewActiveRoom(activeRoom);
        }

        public void ChangeRoom(Enter enter)
        {
            GameObject roomObject = enter.GetRoom();
            NewActiveRoom(roomObject);
            mm.ChangeMusic(activeRoom.GetComponent<Room>().musicID);
            //foreach (var room in rooms)
            //{
            //    if(room == roomObject)
            //    {
            //        newActiveRoom(room);
            //        break;
            //    }
            //}
            Transform trans = enter.transform;
            player.transform.position = new Vector3(trans.position.x, trans.position.y, trans.position.z);
            switch (enter.direction)
            {
                case Direction.Left:
                    player.transform.Translate(Vector3.left * trans.localScale.x);
                    break;
                case Direction.Right:
                    player.transform.Translate(Vector3.right * trans.localScale.x);
                    break;
                case Direction.Up:
                    player.transform.Translate(Vector3.up * trans.localScale.y);
                    break;
                case Direction.Down:
                    player.transform.Translate(Vector3.down * trans.localScale.y);
                    break;
            }
            //Debug.Log(trans.position);
        }

        private void NewActiveRoom(GameObject room)
        {
            activeRoom.SetActive(false);
            activeRoom = room;
            Room roomScript = activeRoom.GetComponent<Room>();
            roomScript.isVisisted = true;

            cameraScript.changePosition(roomScript.leftBound, roomScript.rightBound, roomScript.upperBound, roomScript.lowerBound);
            activeRoom.SetActive(true);

            //Maybe new idea for camera locking
            //BoxCollider boundries = activeRoom.GetComponent<BoxCollider>();
            //if (boundries != null)
            //{
            //    Debug.Log(boundries.bounds.min);
            //    Debug.Log(boundries.bounds.max);
            //    Vector3 boundPoint1 = boundries.bounds.min;
            //    Vector3 boundPoint2 = boundries.bounds.max;
            //    Vector3 boundPoint3 = new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z);
            //    Vector3 boundPoint4 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z);
            //    Vector3 boundPoint5 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z);
            //    Vector3 boundPoint6 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z);
            //    Vector3 boundPoint7 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z);
            //    Vector3 boundPoint8 = new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z);
            //    cameraScript.changePosition(boundPoint1.x, boundPoint2.x, boundPoint2.y, boundPoint1.y);

            //    Color lineColor = Color.green;

            //    Debug.DrawLine(boundPoint6, boundPoint2, lineColor);
            //    Debug.DrawLine(boundPoint2, boundPoint8, lineColor);
            //    Debug.DrawLine(boundPoint8, boundPoint4, lineColor);
            //    Debug.DrawLine(boundPoint4, boundPoint6, lineColor);

            //    // bottom of rectangular cuboid (3-7-5-1)
            //    Debug.DrawLine(boundPoint3, boundPoint7, lineColor);
            //    Debug.DrawLine(boundPoint7, boundPoint5, lineColor);
            //    Debug.DrawLine(boundPoint5, boundPoint1, lineColor);
            //    Debug.DrawLine(boundPoint1, boundPoint3, lineColor);

            //    // legs (6-3, 2-7, 8-5, 4-1)
            //    Debug.DrawLine(boundPoint6, boundPoint3, lineColor);
            //    Debug.DrawLine(boundPoint2, boundPoint7, lineColor);
            //    Debug.DrawLine(boundPoint8, boundPoint5, lineColor);
            //    Debug.DrawLine(boundPoint4, boundPoint1, lineColor);
            //}
            //else
            //{
            //cameraScript.changePosition(roomScript.leftBound, roomScript.rightBound, roomScript.upperBound, roomScript.lowerBound);
            //}
        }
    }
}
