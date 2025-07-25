using System;
using System.Collections;
using System.Collections.Generic;
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
    private TextAsset _throwCsvFile;

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
        _throwCsvFile = (TextAsset)EditorGUILayout.ObjectField("Throw CSV", _throwCsvFile, typeof(TextAsset), false);
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
        
        if (GUILayout.Button("Generate ETCItem"))
        {
            if (_etcCsvFile != null)
            {
                CreateEtcItemFromCSV();
            }
            else
            {
                Debug.LogWarning("ETCItem CSV 파일이 지정되지 않았습니다.");
            }
        }
    }

    private void CreateEtcItemFromCSV()
    {
        if (_etcCsvFile == null) return;
        string[] lines = _etcCsvFile.text.Split('\n');

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
            
            EtcItem etcItem = ScriptableObject.CreateInstance<EtcItem>();

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            string itemId = parts[0];
            etcItem.ItemID = int.Parse(itemId);
            string name = parts[1];
            etcItem.Name = name;
            etcItem.EtcType = parts[2];
            etcItem.MaxStackSize = int.Parse(parts[3]);
            etcItem.SoundResource = FileFinder.FindSFXByName(parts[4]);
            etcItem.StrParam = parts[5];
            if(int.TryParse(parts[6], out int param))
            {
                etcItem.IntParam = param;
            }
            
            string assetPath = $"{folderPath}/Item_{itemId}_{name}.Asset";
            AssetDatabase.CreateAsset(etcItem, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
            string description = parts[2].Replace("\\n", "\n");
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
                case ItemType.Etc:
                    if (_etcCsvFile != null)
                    {
                        item = CreateEtcItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("ETC CSV파일이 등록되지 않았습니다.");
                    }
                    break;
                case ItemType.Throw:
                    if (_throwCsvFile != null)
                    {
                        item = CreateThrowItem(itemId);
                    }
                    else
                    {
                        Debug.LogWarning("Throw CSV파일이 등록되지 않았습니다.");
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
            item.Sprite = FileFinder.FindSpriteByName(icon);
            
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
            meleeItem.MaxDurability = int.Parse(weaponParts[2]);
            meleeItem.AtkDamage = int.Parse(weaponParts[3]);
            meleeItem.AtkSpeed = int.Parse(weaponParts[4]);
            meleeItem.AtkSoundResources = FileFinder.FindSFXByName(weaponParts[5]);
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
            gunItem.AtkDamage = int.Parse(gunDataParts[2]);
            gunItem.Rof = int.Parse(gunDataParts[3]);
            gunItem.BulletPerShot = int.Parse(gunDataParts[4]);
            gunItem.Range = float.Parse(gunDataParts[5]);
            gunItem.AmmoID = int.Parse(gunDataParts[6]);
            gunItem.AmmoCapacity = int.Parse(gunDataParts[7]);
            gunItem.ShotSoundResource = FileFinder.FindSFXByName(gunDataParts[8]);
            gunItem.ReloadSoundResource = FileFinder.FindSFXByName(gunDataParts[9]);
            if (float.TryParse(gunDataParts[10], out float noiseLevel))
            {
                gunItem.NoiseLevel = noiseLevel;
            }
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
            shieldItem.MaxDurability = int.Parse(shieldDataParts[2]);
            shieldItem.DefenseAmount = int.Parse(shieldDataParts[3]);
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
            specialItem.MaxDurability = int.Parse(specialDataParts[2]);
            specialItem.DurabilitySec = int.Parse(specialDataParts[3]);
            specialItem.SoundResource = FileFinder.FindSFXByName(specialDataParts[4]);
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
            consumableItem.HpAmount = int.Parse(consumableDataParts[2]);
            consumableItem.HungerAmount = int.Parse(consumableDataParts[3]);
            consumableItem.MoistureAmount = int.Parse(consumableDataParts[4]);
            consumableItem.StaminaAmount = int.Parse(consumableDataParts[5]);
            consumableItem.SoundResource = consumableDataParts[6];
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
            etcItem.EtcType = etcDataParts[2];
            etcItem.MaxStackSize = int.Parse(etcDataParts[3]);
            etcItem.SoundResource = FileFinder.FindSFXByName(etcDataParts[4]);
            etcItem.StrParam = etcDataParts[5];
            if (int.TryParse(etcDataParts[6], out int param))
            {
                etcItem.IntParam = param;
            }
        }
        
        return etcItem;
    }

    private ThrowItem CreateThrowItem(string itemId)
    {
        Dictionary<int, string[]> throwData = LoadData(_throwCsvFile);
        ThrowItem throwItem = ScriptableObject.CreateInstance<ThrowItem>();
        if (throwData.ContainsKey(int.Parse(itemId)))
        {
            string[] throwDataParts = throwData[int.Parse(itemId)];
            throwItem.MaxStack = int.Parse(throwDataParts[2]);
            throwItem.AtkDamage = float.Parse(throwDataParts[3]);
            throwItem.AtkSpeed = float.Parse(throwDataParts[4]);
            throwItem.MinSpeed = float.Parse(throwDataParts[5]);
            throwItem.MaxSpeed = float.Parse(throwDataParts[6]);
            throwItem.MaxChargeTime = float.Parse(throwDataParts[7]);
            throwItem.MinRotateValue = float.Parse(throwDataParts[8]);
            throwItem.MaxRotateValue = float.Parse(throwDataParts[9]); 
            throwItem.ThrowSoundResource = FileFinder.FindSFXByName(throwDataParts[10]);
        }
        return throwItem;
    }
}
