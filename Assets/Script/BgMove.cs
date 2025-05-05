using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f;
    private float spriteHeight;
    private Vector3 startPosition;

    // Background references assigned in the Inspector
    public Transform background1;
    public Transform background2;

    void Start()
    {
        // Ensure both backgrounds are assigned
        if (background1 == null || background2 == null)
        {
            Debug.LogError("Backgrounds are not assigned in the Inspector!");
            return;
        }
        else
        {
            Debug.Log("Background1 assigned: " + background1.name);
            Debug.Log("Background2 assigned: " + background2.name);
        }

        // Get the height of the sprite to know when to reset position
        SpriteRenderer sr = background1.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer not found on background1!");
            return;
        }

        spriteHeight = sr.bounds.size.y;

        // Store the start position of both backgrounds
        startPosition = background1.position;
    }

    void Update()
    {
        // Ensure backgrounds are assigned
        if (background1 == null || background2 == null)
        {
            Debug.LogError("Backgrounds are not properly assigned!");
            return;
        }

        // Move both backgrounds downward
        background1.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        background2.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Reset the first background when it goes off-screen
        if (background1.position.y < startPosition.y - spriteHeight)
        {
            // Reset background1 to be immediately below background2 to avoid a gap
            background1.position = new Vector3(background1.position.x, background2.position.y + spriteHeight, background1.position.z);
        }

        // Reset the second background when it goes off-screen
        if (background2.position.y < startPosition.y - spriteHeight)
        {
            // Reset background2 to be immediately below background1 to avoid a gap
            background2.position = new Vector3(background2.position.x, background1.position.y + spriteHeight, background2.position.z);
        }
    }
}
