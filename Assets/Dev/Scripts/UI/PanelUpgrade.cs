using System.Collections.Generic;
using UnityEngine;

public class PanelUpgrade : MonoBehaviour
{
    [SerializeField] private List<UpgradeButton> upgradeButtons;

    // Common=100  Uncommon=60  Rare=30  Epic=15  Legendary=5
    private static readonly int[] RarityWeights = { 100, 60, 30, 15, 5 };

    // Pre-allocated — sahne boyunca yeniden kullanılır
    private readonly List<(UpgradeSO so, int weight)> _candidates = new List<(UpgradeSO, int)>(64);

    public void SetUpgradeButtons()
    {
        BuildCandidates();

        for (int b = 0; b < upgradeButtons.Count; b++)
        {
            if (_candidates.Count == 0) break;
            int idx = PickWeighted();
            upgradeButtons[b].SetUpgradeSO(_candidates[idx].so);
            _candidates.RemoveAt(idx);   // bir sonraki seçimde bu SO yok
        }
    }

    private void BuildCandidates()
    {
        _candidates.Clear();
        var upgrades = PlayerData.Instance.Upgrades;
        for (int i = 0; i < upgrades.Count; i++)
        {
            int w = RarityWeights[(int)upgrades[i].rarity];
            _candidates.Add((upgrades[i], w));
        }
    }

    private int PickWeighted()
    {
        int total = 0;
        for (int i = 0; i < _candidates.Count; i++)
            total += _candidates[i].weight;

        int roll       = Random.Range(0, total);
        int cumulative = 0;
        for (int i = 0; i < _candidates.Count; i++)
        {
            cumulative += _candidates[i].weight;
            if (roll < cumulative) return i;
        }
        return _candidates.Count - 1;
    }
}
