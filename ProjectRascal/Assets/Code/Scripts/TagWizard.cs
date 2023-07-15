using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagWizard : MonoBehaviour
{
    public static bool IsGround(Collider collider) {
        return collider.CompareTag("Ground");
    }
    public static bool IsEnemy(Collider collider) {
        return collider.CompareTag("Enemy");
    }
    public static bool IsPlayer(Collider collider) {
        return collider.CompareTag("Player");
    }
}
