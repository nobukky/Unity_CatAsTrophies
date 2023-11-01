using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EntityIntegration : EditorWindow
{
    #region INITIALIZATION
    // lists of entities
    private List<EntityConfig> enemies = new List<EntityConfig>();
    private List<EntityConfig> cats = new List<EntityConfig>();

    // entities attributes
    private EntityConfig currentEntity;

    private string entityName;
    private float health;
    private Ability ability;
    private List<Ability> autoAttacks = new List<Ability>();
    private GameObject basePrefab, rightHandAddon, leftHandAddon, headAddon;
    
    // window editor component
    private int id;
    private Vector2 sideScrollPos, detailsScrollPos;
    private bool canDisplayDetails = true;
    private ReorderableList reorderableList;
    #endregion
    
    [MenuItem("Tool/Entity Integration")]
    static void InitializeWindow()
    {
        EntityIntegration window = GetWindow<EntityIntegration>();
        window.titleContent = new GUIContent("Entity Integration");
        window.Show();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(150), GUILayout.ExpandHeight(true));
            {
                DisplaySideLists();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            {
                if (canDisplayDetails)
                {
                    DisplayDetails();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnEnable()
    {
        cats.Clear();
        enemies.Clear();
        
        // load the data from the entities config
        LoadData();
        
        if (cats.Count > 0)
        {
            UpdateDetails(cats[0]);
        }
        else if (enemies.Count > 0)
        {
            UpdateDetails(enemies[0]);
        }
        else
        {
            canDisplayDetails = false;
        }
    }
    
    private void OnDisable()
    {
        UpdateEntitiesConfig();
    }
    
    private void DisplaySideLists()
    {
        GUILayout.BeginVertical("HelpBox");
        {
            sideScrollPos = EditorGUILayout.BeginScrollView(sideScrollPos);
            #region CATS SIDE LIST
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("CATS", EditorStyles.boldLabel);
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newInstance = CreateInstance<EntityConfig>();
                    newInstance.isCat = true;
                    var date = DateTime.Now;
                    newInstance.id = "Cat_" + date.ToString("yyyyMMdd_HHmmss_fff");
                    
                    cats.Add(newInstance);
                    UpdateDetails(newInstance);
                    canDisplayDetails = true;
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Space(5);
        
            // LIST OF CATS
            foreach (EntityConfig cat in cats)
            {
                string buttonName = cat.entityName == "" ? "New cat" : cat.entityName;
                if (GUILayout.Button($"{buttonName}", GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                {
                    UpdateDetails(cat);
                }
            }
            #endregion
            GUILayout.Space(15);
            #region ENEMIES SIDE LIST
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("ENEMIES", EditorStyles.boldLabel);
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newInstance = CreateInstance<EntityConfig>();
                    newInstance.isCat = false;
                    var date = DateTime.Now;
                    newInstance.id = "Enemy_" + date.ToString("yyyyMMdd_HHmmss_fff");
                    
                    enemies.Add(newInstance);
                    UpdateDetails(newInstance);
                    canDisplayDetails = true;
                }
            }
            GUILayout.EndHorizontal();
        
            GUILayout.Space(5);
            
            // LIST OF ENEMIES
            foreach (EntityConfig enemy in enemies)
            {
                string buttonName = enemy.entityName == "" ? "New enemy" : enemy.entityName;
                if (GUILayout.Button($"{buttonName}", GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                {
                    UpdateDetails(enemy);
                }
            }
            #endregion
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }
    
    private void DisplayDetails()
    {
        detailsScrollPos = EditorGUILayout.BeginScrollView(detailsScrollPos);
        {
            // INFORMATIONS
            GUILayout.Label("INFORMATION", EditorStyles.boldLabel);
            entityName = EditorGUILayout.TextField("Name", entityName);
            health = EditorGUILayout.FloatField("Health", health);
        
            
            // ABILITY
            GUILayout.Space(10);
            GUILayout.Label("ABILITY", EditorStyles.boldLabel);
            DisplayInstructionList(ability.instructions);
            

            // AUTO ATTACK
            GUILayout.Space(10);
            GUILayout.Label("AUTO ATTACK", EditorStyles.boldLabel);
            DisplayAbilityList(autoAttacks);
            

            // GRAPHICS
            GUILayout.Space(10);
            GUILayout.Label("GRAPHICS", EditorStyles.boldLabel);
            basePrefab = (GameObject)EditorGUILayout.ObjectField("Base mesh prefab", basePrefab, typeof(GameObject), true);
            rightHandAddon = (GameObject)EditorGUILayout.ObjectField("Right hand addon", rightHandAddon, typeof(GameObject), true);
            leftHandAddon = (GameObject)EditorGUILayout.ObjectField("Left hand addon", leftHandAddon, typeof(GameObject), true);
            headAddon = (GameObject)EditorGUILayout.ObjectField("Head addon", headAddon, typeof(GameObject), true);

            
            // DATA MANAGEMENT
            // save button
            GUILayout.Space(20);
            if (GUILayout.Button("SAVE", GUILayout.ExpandWidth(true)))
            {
                SaveData();
            }
        
            // delete button
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("DELETE", GUILayout.ExpandWidth(true)))
            {
                if (EditorUtility.DisplayDialog("Delete Entity", "Do you really want to permanently delete this entity?", "Yes", "No"))
                {
                    DeleteData();
                }
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndScrollView();
    }

    private void DisplayAbilityList(List<Ability> _autoAttack)
    {
        GUILayout.BeginVertical("HelpBox");
        {
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Abilities");
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newAutoAttackAbility = new Ability();
                    newAutoAttackAbility.instructions.Add(new Instruction());
                    _autoAttack.Add(newAutoAttackAbility);
                }
            }
            GUILayout.EndHorizontal();

            // LIST
            foreach (Ability autoAttackAbility in _autoAttack)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        DisplayInstructionList(autoAttackAbility.instructions);
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    {
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete Ability", "Do you really want to permanently delete this Ability?", "Yes", "No"))
                            {
                                _autoAttack.Remove(autoAttackAbility);
                                return;
                            }
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }

    private void DisplayInstructionList(List<Instruction> _instructions)
    {
        GUILayout.BeginVertical("HelpBox");
        {
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Instructions");
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newInstruction = new Instruction();
                    _instructions.Add(newInstruction);
                }
            }
            GUILayout.EndHorizontal();

            // LIST
            if (_instructions != null)
            {
                foreach (Instruction instruction in _instructions)
                {
                    GUILayout.BeginHorizontal("HelpBox");
                    {
                        GUILayout.BeginVertical();
                        {
                            instruction.type = (InstructionType)EditorGUILayout.EnumPopup("Type", instruction.type);
                            instruction.target = (TargetType)EditorGUILayout.EnumPopup("Target", instruction.target);
                        }
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                        {
                            instruction.value = EditorGUILayout.IntField("Value", instruction.value);
                            instruction.us = EditorGUILayout.Toggle("Us", instruction.us);
                        }
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                        {
                            GUI.backgroundColor = Color.red;
                            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                if (EditorUtility.DisplayDialog("Delete Instruction", "Do you really want to permanently delete this instruction?", "Yes", "No"))
                                {
                                    _instructions.Remove(instruction);
                                    return;
                                }
                            }
                            GUI.backgroundColor = Color.white;
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
        GUILayout.EndVertical();
    }
    
    private void SaveData()
    {
        // exceptions
        if (entityName == "") return;
            
        // update data into current entity
        currentEntity.entityName = entityName;
        currentEntity.health = health;
        currentEntity.ability = ability;
        currentEntity.autoAttack = autoAttacks;
        currentEntity.basePrefab = basePrefab;
        currentEntity.rightHandAddon = rightHandAddon;
        currentEntity.leftHandAddon = leftHandAddon;
        currentEntity.headAddon = headAddon;

        // get the path
        string path = $"Assets/Configs/Entities/{currentEntity.id}.asset";

        // if the asset does not already exists then create a new one 
        if (!AssetDatabase.LoadAssetAtPath<EntityConfig>(path))
        {
            AssetDatabase.CreateAsset(currentEntity, path);
        }
        
        // save changes
        AssetDatabase.SaveAssets();
    }

    private void DeleteData()
    {
        // remove from lists depending on the entity
        if (currentEntity.isCat)
        {
            cats.Remove(currentEntity);
        }
        else
        {
            enemies.Remove(currentEntity);
        }

        // deleting the asset
        AssetDatabase.DeleteAsset($"Assets/Configs/Entities/{currentEntity.id}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void LoadData()
    {
        // get all files with type "EntityConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(EntityConfig));

        // fullfil cats and enemies with EntityConfigs 
        foreach (string fileGuid in fileGuidsArray)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuid);
            EntityConfig entityConfig = AssetDatabase.LoadAssetAtPath<EntityConfig>(assetPath);

            if (entityConfig.isCat)
            {
                cats.Add(entityConfig);
            }
            else
            {
                enemies.Add(entityConfig);
            }
        }
    }
    
    private void UpdateDetails(EntityConfig _entityConfig)
    {
        currentEntity = _entityConfig;
        
        // INFORMATIONS
        entityName = currentEntity.entityName;
        health = currentEntity.health;

        // ABILITY
        ability = currentEntity.ability;
        autoAttacks = currentEntity.autoAttack;

        // GRAPHICS
        basePrefab = currentEntity.basePrefab;
        rightHandAddon = currentEntity.rightHandAddon;
        leftHandAddon = currentEntity.leftHandAddon;
        headAddon = currentEntity.headAddon;
    }
    
    private static EntitiesConfig FindEntitiesConfig()
    {
        // get all files with type "EntitiesConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(EntitiesConfig));
        
        if (fileGuidsArray.Length > 0)
        {
            // if file exists, get first EntitiesConfig and return it
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArray[0]);
            return AssetDatabase.LoadAssetAtPath<EntitiesConfig>(assetPath);
        }
        else
        {
            // if file does not exist, create a new EntitiesConfig and save it into a dedicated path
            EntitiesConfig entitiesConfig = CreateInstance<EntitiesConfig>();
            AssetDatabase.CreateAsset(entitiesConfig, "Assets/Configs/EntitiesConfig.asset");
            AssetDatabase.SaveAssets();
            return entitiesConfig;
        }
    }
    
    /// <summary>
    /// Update cats and enemies list in the EntitiesConfig
    /// </summary>
    private void UpdateEntitiesConfig()
    {
        FindEntitiesConfig().cats.Clear();
        FindEntitiesConfig().cats = cats;
        FindEntitiesConfig().enemies.Clear();
        FindEntitiesConfig().enemies = enemies;
    }
}