using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/popup_windows")]
public class PopupWindowConfig : ScriptableObject
{
    public List<GameObject> windowPrefabs;
    public GameObject notificationCashPrefab;
    public GameObject notificationSquadFullPrefab;
    public GameObject notificationNotEnoughCashPrefab;
}