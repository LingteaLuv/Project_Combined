using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CreateSO : EditorWindow
{
    private TextAsset _baseCsvFile;
    private TextAsset _meleeCsvFile;
    private TextAsset _gunCsvFile;
    private TextAsset _shieldCsvFile;
    private TextAsset _specialCsvFile;
    private TextAsset _consumableCsvFile;
    private TextAsset _etcCsvFile;

    private TextAsset _recipeCsvFile;

    [MenuItem("Tools/CSV to ScriptableObjects")]
    public static void ShowWindow()
    {
        GetWindow<CreateSO>("CSV to SO Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV To ScriptableObject Generator", EditorStyles.boldLabel);
        _baseCsvFile = (TextAsset)EditorGUILayout.ObjectField("Base CSV", _baseCsvFile, typeof(TextAsset), false);
        _meleeCsvFile = (TextAsset)EditorGUILayout.ObjectField("Melee CSV", _meleeCsvFile, typeof(TextAsset), false);
        _gunCsvFile = (TextAsset)EditorGUILayout.ObjectField("Gun CSV", _gunCsvFile, typeof(TextAsset), false);
        _shieldCsvFile = (TextAsset)EditorGUILayout.ObjectField("Shield CSV", _shieldCsvFile, typeof(TextAsset), false);
        _specialCsvFile = (TextAsset)EditorGUILayout.ObjectField("Special CSV", _specialCsvFile, typeof(TextAsset), false);
        _consumableCsvFile = (TextAsset)EditorGUILayout.ObjectField("Consumable CSV", _consumableCsvFile, typeof(TextAsset), false);
        _etcCsvFile = (TextAsset)EditorGUILayout.ObjectField("ETC CSV", _etcCsvFile, typeof(TextAsset), false);
        _recipeCsvFile = (TextAsset)EditorGUILayout.ObjectField("Recipe CSV", _recipeCsvFile, typeof(TextAsset), false);
        
        if (GUILayout.Button("Generate ScriptableObjects"))
        {
            if (_baseCsvFile != null)
            {
                CreateSOFromCSV();
            }
            else
            {
                Debug.LogWarning("공용 CSV 파일이 지정되지 않았습니다.");
            }
        }

        if (GUILayout.Button("Generate Recipe"))
        {
            if (_recipeCsvFile != null)
            {
                CreateRecipeFromCSV();
            }
            else
            {
                Debug.LogWarning("레시피 CSV 파일이 지정되지 않았습니다.");
            }
        }
    }

     private void CreateSOFromCSV()
    {
        if (_baseCsvFile == null) return;
        string[] lines = _baseCsvFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Items";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","Items");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            string itemId = parts[0];
            string name = parts[1];
            string description = parts[2];
            string itemType = parts[3];
            string icon = parts[4];

            ItemBase item = null;
            
            switch ((ItemType)Enum.Parse(typeof(ItemType),itemType))
            {
                case ItemType.Melee:
                    if (_meleeCsvFile != null)
                    {
                        item = CreateMeleeItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("Melee CSV파일이 등록되지 않았습니다.");
                    }
                    break;
                case ItemType.Gun:
                    if (_gunCsvFile != null)
                    {
                        item = CreateGunItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("Gun CSV파일이 등록되지 않았습니다.");
                    }
                    break;
                case ItemType.Shield:
                    if (_shieldCsvFile != null)
                    {
                        item = CreateShieldItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("Shield CSV파일이 등록되지 않았습니다.");
                    }
                    break;
                case ItemType.Special:
                    if (_specialCsvFile != null)
                    {
                        item = CreateSpecialItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("Special CSV파일이 등록되지 않았습니다.");
                    }
                    break;
                case ItemType.Consumable:
                    if (_consumableCsvFile != null)
                    {
                        item = CreateConsumableItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("Consumable CSV파일이 등록되지 않았습니다.");
                    }
                    break;
                case ItemType.ETC:
                    if (_etcCsvFile != null)
                    {
                        item = CreateEtcItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("ETC CSV파일이 등록되지 않았습니다.");
                    }
                    break;
                default:
                    break;
            }

            if (item == null)
            {
                item = ScriptableObject.CreateInstance<ItemBase>();
            }

            item.ItemID = int.Parse(itemId);
            item.Name = name;
            item.Description = description;
            item.Type = (ItemType)Enum.Parse(typeof(ItemType),itemType);
            item.Sprite = SpriteFinder.FindSpriteByName(icon);
            
            string assetPath = $"{folderPath}/Item_{itemId}_{name}.Asset";
            AssetDatabase.CreateAsset(item, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void CreateRecipeFromCSV()
    {
        if (_recipeCsvFile == null) return;
        string[] lines = _recipeCsvFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Recipes";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Recipes");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();
            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;
            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            int[] array = new int[parts.Length];
            for (int j = 0; j < parts.Length; j++)
            {
                if(int.TryParse(parts[j] , out int k))
                {
                    array[j] = k;
                }
                else
                {
                    array[j] = -1;
                }
            }
            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.Init(array);
            
            string assetPath = $"{folderPath}/Recipe_{parts[0]}.Asset";
            AssetDatabase.CreateAsset(recipe, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private Dictionary<int, string[]> LoadData(TextAsset csvFile)
    {
        if (csvFile != null)
        {
            Dictionary<int, string[]> data = new Dictionary<int, string[]>();
            string[] Lines = csvFile.text.Split('\n');
            for (int i = 1; i < Lines.Length; i++)
            {
                string line = Lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split(',');
                int id = int.Parse(parts[0]);
                data[id] = parts;
            }
            return data;
        }

        return null;
    }
    
    private MeleeItem CreateMeleeItem(string itemId)
    {
        Dictionary<int, string[]> meleeData = LoadData(_meleeCsvFile);
        MeleeItem meleeItem = ScriptableObject.CreateInstance<MeleeItem>();
        if (meleeData.ContainsKey(int.Parse(itemId)))
        {
            string[] weaponParts = meleeData[int.Parse(itemId)];
            meleeItem.MaxDurability = int.Parse(weaponParts[1]);
            meleeItem.AtkDamage = int.Parse(weaponParts[2]);
        }
        
        return meleeItem;
    }

    private GunItem CreateGunItem(string itemId)
    {
        Dictionary<int, string[]> gunData = LoadData(_gunCsvFile);
        GunItem gunItem = ScriptableObject.CreateInstance<GunItem>();
        int id = int.Parse(itemId);
        if (gunData.ContainsKey(id))
        {
            string[] gunDataParts = gunData[id];
            gunItem.AtkDamage = int.Parse(gunDataParts[1]);
            gunItem.Rof = int.Parse(gunDataParts[2]);
            gunItem.BulletPerShot = int.Parse(gunDataParts[3]);
            gunItem.Range = float.Parse(gunDataParts[4]);
            gunItem.AmmoID = int.Parse(gunDataParts[5]);
            gunItem.AmmoCapacity = int.Parse(gunDataParts[6]);
            gunItem.ShotSoundResource = gunDataParts[7];
            gunItem.ReloadSoundResource = gunDataParts[8];
        }

        return gunItem;
    }

    private ShieldItem CreateShieldItem(string itemId)
    {
        Dictionary<int, string[]> shieldData = LoadData(_shieldCsvFile);
        ShieldItem shieldItem = ScriptableObject.CreateInstance<ShieldItem>();
        if (shieldData.ContainsKey(int.Parse(itemId)))
        {
            string[] shieldDataParts = shieldData[int.Parse(itemId)];
            shieldItem.MaxDurability = int.Parse(shieldDataParts[1]);
            shieldItem.DefenseAmount = int.Parse(shieldDataParts[2]);
        }
        
        return shieldItem;
    }
    private SpecialItem CreateSpecialItem(string itemId)
    {
        Dictionary<int, string[]> specialData = LoadData(_specialCsvFile);
        SpecialItem specialItem = ScriptableObject.CreateInstance<SpecialItem>();
        if (specialData.ContainsKey(int.Parse(itemId)))
        {
            string[] specialDataParts = specialData[int.Parse(itemId)];
            specialItem.MaxDurability = int.Parse(specialDataParts[1]);
            specialItem.ConDurability = int.Parse(specialDataParts[2]);
            specialItem.ConDurabilitySec = int.Parse(specialDataParts[3]);
            specialItem.SoundResource = specialDataParts[4];
        }
        
        return specialItem;
    }
    private ConsumableItem CreateConsumableItem(string itemId)
    {
        Dictionary<int, string[]> consumableData = LoadData(_consumableCsvFile);
        ConsumableItem consumableItem = ScriptableObject.CreateInstance<ConsumableItem>();
        if (consumableData.ContainsKey(int.Parse(itemId)))
        {
            string[] consumableDataParts = consumableData[int.Parse(itemId)];
            consumableItem.HpAmount = int.Parse(consumableDataParts[1]);
            consumableItem.MoistureAmount = int.Parse(consumableDataParts[2]);
            consumableItem.HungerAmount = int.Parse(consumableDataParts[3]);
            consumableItem.SoundResource = consumableDataParts[4];
        }
        
        return consumableItem;
    }
    private EtcItem CreateEtcItem(string itemId)
    {
        Dictionary<int, string[]> etcData = LoadData(_etcCsvFile);
        EtcItem etcItem = ScriptableObject.CreateInstance<EtcItem>();
        if (etcData.ContainsKey(int.Parse(itemId)))
        {
            string[] etcDataParts = etcData[int.Parse(itemId)];
            etcItem.EtcType = etcDataParts[1];
            etcItem.MaxStackSize = int.Parse(etcDataParts[2]);
            etcItem.SoundResource = etcDataParts[3];
            etcItem.StrParam = etcDataParts[4];
            etcItem.IntParam = int.Parse(etcDataParts[5]);
        }
        
        return etcItem;
    }
}
