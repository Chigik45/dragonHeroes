using UnityEngine;
public enum Modifier
{
    Increase,
    Decrease
}
public class Creature : MonoBehaviour
{
    public int healthPoints;
    public int maxHealthPoints;
    public int movementSpeed;
    public int maxMovementSpeed;
    public GameObject tile;

    public void Assign(GameObject newTile)
    {
        tile = newTile;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints < 0)
        {
            healthPoints = 0;
        }
    }

    public void SpendMovement(int amount, Modifier modifier)
    {   
        if (modifier == Modifier.Increase)
        {
            movementSpeed += amount;
        }
        else if (modifier == Modifier.Decrease)
        {
            movementSpeed -= amount;
            if (movementSpeed < 0)
            {
                movementSpeed = 0;
            }
        }
    }

    public void RestoreHealth(int amount, Modifier modifier)
    {
        if (modifier == Modifier.Increase)
        {
            healthPoints += amount;
        }
        else if (modifier == Modifier.Decrease)
        {
            healthPoints -= amount;
            if (healthPoints < 0)
            {
                healthPoints = 0;
            }
        }
    }

    public void RestoreMovement(int amount, Modifier modifier)
    {
        if (modifier == Modifier.Increase)
        {
            movementSpeed += amount;
        }
        else if (modifier == Modifier.Decrease)
        {
            movementSpeed -= amount;
            if (movementSpeed < 0)
            {
                movementSpeed = 0;
            }
        }
    }
    public void RestoreAll()
    {
        RestoreHealth(maxHealthPoints, Modifier.Increase);
        RestoreMovement(maxMovementSpeed, Modifier.Increase);
    }
}
