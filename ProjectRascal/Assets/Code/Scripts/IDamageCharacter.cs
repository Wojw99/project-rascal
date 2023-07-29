using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagaController {
    public void VisualizeDamage(Vector3 hitDirection, bool bloodSpill);
}