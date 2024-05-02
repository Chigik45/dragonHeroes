using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHolder : MonoBehaviour
{
    [SerializeField] TextMesh hptext;
    public Entity entity;
    public float speed = 0.1f;
    BoardGlobalHolder boardGlobalHolder;

    private void Start()
    {
        boardGlobalHolder = FindObjectOfType<BoardGlobalHolder>();
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("entityPlaceholders/" + entity.placeholderSprite);
    }
    void Update()
    {
        if (entity == null)
            return;
        hptext.text = entity.healthPoints.ToString();
        if (entity.healthPoints <= 0)
        {
            Destroy(gameObject);
        }
        if (entity.GetCurrTile() != null)
        {
            transform.position = Vector2.Lerp(transform.position, boardGlobalHolder.tileHolders[entity.GetCurrTile()].gameObject.transform.position, speed * Time.deltaTime);
        }
    }
    private void OnMouseDown()
    {
        boardGlobalHolder.tileHolders[entity.GetCurrTile()].SimulateClick();
    }
}
