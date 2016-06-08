using UnityEngine;

namespace Assets.Code
{
    class Exit : MonoBehaviour
    {
        public Enter enter;
        public World world;
        
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("entered");
                if (world != null)
                    world.ChangeRoom(enter);
            }
        }
    }
}
