
using TMPro;
using UnityEngine;

public class ItemCollect : SaveComponent
{
    [SerializeField] private int gold;
    [SerializeField] private TextMeshProUGUI Gold_Text;

    public int Gold => gold;

    private void Start()
    {
        UpdateGoldUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Coin"))
            return;

        CoinSound sound = collision.GetComponent<CoinSound>();

        if (sound != null)
            sound.PlaySound();

        gold += 50;

        GameEvents.OnCoinsCollected.Invoke(50);

        UpdateGoldUI();

        Destroy(collision.gameObject, 0.2f);
    }

    public override void Save(GameState state)
    {
        state.gold = gold;
    }

    public override void Load(GameState state)
    {
        gold = state.gold;
        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        if (Gold_Text != null)
            Gold_Text.text = gold.ToString();
    }
}
