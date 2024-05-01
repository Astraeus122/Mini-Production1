using UnityEngine;

public class InputInstance : MonoBehaviour
{
    private static Controls controls;
    public static Controls Controls
    {
        get
        {
            // a better singleton pattern i made after last GDV110 assessment. this one doesnt require on a instance in the scene (meaning can only start game from that scene for it to not break)
            if (controls == null)
            {
                controls = new Controls();
            }

            controls.Enable();

            return controls;
        }
    }
}
