using UnityEngine;

public class SaveComponent : MonoBehaviour
{
    public virtual void Save(GameState state) { }

    public virtual void Load(GameState state) { }
}