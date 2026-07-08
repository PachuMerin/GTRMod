using BepInEx.Configuration;
using EntityStates;
using GTRMod.Survivors.GTRaptor.SkillStates;
using GTRMod.Survivors.GTRaptor;
using GTRMod.Modules;
using GTRMod.Modules.Characters;
using GTRMod.Survivors.GTRaptor.Components;
using RoR2;
using RoR2.Skills;
using RoR2BepInExPack.GameAssetPathsBetter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor
{
    public class GTRaptorSurvivor : SurvivorBase<GTRaptorSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "gtrassets"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "GTRaptorBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string mastername => "GTRaptorMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlGTRaptor";
        public override string displayPrefabName => "GTRaptorDisplay";

        public const string GTRaptor_PREFIX = GTRaptorPlugin.DEVELOPER_PREFIX + "_GTRAPTOR_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => GTRaptor_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = GTRaptor_PREFIX + "NAME",
            subtitleNameToken = GTRaptor_PREFIX + "SUBTITLE",

            characterPortrait = gtrassets.LoadAsset<Texture>("texGTRIcon"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 340f,
            healthRegen = 2.5f,
            armor = 25f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "GTK_Head",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTK_Visor",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTK_Upper_Torso",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Omni_Ball",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Shoulder.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Shoulder.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Thigh.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Thigh.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_UpperArm.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_UpperArm.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Blaster.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Blaster_Cannon.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Blaster.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Blaster_Cannon.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Eyes",
                    material = gtrassets.LoadMaterial("matBlueEyes"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Guage",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Tail",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Torso",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_WheelMount.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_WheelMount.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Radar_Sphere",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTR_Barrier",
                    material = gtrassets.LoadMaterial("matBarrier"),
                }
        };

        public override UnlockableDef characterUnlockableDef => GTRaptorUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => new GTRaptorItemDisplays();

        //set in base classes
        public override AssetBundle gtrassets { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }

        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            // ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "GTRaptor");

            // if (!characterEnabled.Value)
            //return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            GTRaptorUnlockables.Init();

            base.InitializeCharacter();

            GTRaptorConfig.Init();
            GTRaptorStates.Init();
            GTRaptorTokens.Init();

            GTRaptorAssets.Init(gtrassets);
            GTRaptorBuffs.Init();

            DamageHooks.Init();

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AddHooks();
        }
        public override void InitializeEntityStateMachines()
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your GTRaptorStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Scanner");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtilitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = GTRaptor_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_AGILE",
                icon = gtrassets.LoadAsset<Sprite>("texPassiveIcon"),
            };
            //Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
            #region activeskillPassive
            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            //GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            //SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //skillName = "GTRaptorPassive",
            //skillNameToken = GTRaptor_PREFIX + "PASSIVE_NAME",
            //skillDescriptionToken = GTRaptor_PREFIX + "PASSIVE_DESCRIPTION",
            //keywordTokens = new string[] { "KEYWORD_AGILE" },
            //skillIcon = gtrassets.LoadAsset<Sprite>("texPassiveIcon"),

            //unless you're somehow activating your passive like a skill, none of the following is needed.
            //but that's just me saying things. the tools are here at your disposal to do whatever you like with

            //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
            //activationStateMachineName = "Weapon1",
            //interruptPriority = EntityStates.InterruptPriority.Skill,

            //baseRechargeInterval = 1f,
            //baseMaxStock = 1,

            //rechargeStock = 1,
            //requiredStock = 1,
            //stockToConsume = 1,

            //resetCooldownTimerOnUse = false,
            //fullRestockOnAssign = true,
            //dontAllowPastMaxStocks = false,
            //mustKeyPress = false,
            //beginSkillCooldownOnSkillEnd = false,

            //isCombatSkill = true,
            //canceledFromSprinting = false,
            //cancelSprintingOnActivation = false,
            //forceSprintDuringState = false,

            //});
            //Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
            #endregion
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            SkillDef primarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Blaze Vulcans",
                skillNameToken = GTRaptor_PREFIX + "PRIMARY_SHOT_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "PRIMARY_SHOT_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = gtrassets.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Vulcans)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,
                baseRechargeInterval = 0.5f,
                baseMaxStock = 1,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });
            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Blaze ScatterShot",
                skillNameToken = GTRaptor_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = gtrassets.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Blasting)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 3f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }

        private void AddUtilitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GT Dash",
                skillNameToken = GTRaptor_PREFIX + "UTILITY_DASH_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "UTILITY_DASH_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = gtrassets.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Dash)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Expidition Radar",
                skillNameToken = GTRaptor_PREFIX + "SPECIAL_RADAR_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "SPECIAL_RADAR_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = gtrassets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ActivateScanner)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Scanner",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 20f,

                isCombatSkill = false,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills

        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = R2API.Skins.CreateNewSkinDef(new R2API.SkinDefParamsInfo
            {
                RootObject = prefabCharacterModel.gameObject,
                Name = GTRaptor_PREFIX + "DEFAULT_SKIN",
                NameToken = GTRaptor_PREFIX + "DEFAULT_SKIN",
                Icon = gtrassets.LoadAsset<Sprite>("texMainSkin"),
                UnlockableDef = null,
                RendererInfos = [.. defaultRendererinfos],
                MeshReplacements = [],
                GameObjectActivations = []
            });
            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            SkinDef GTRSkinYellow = R2API.Skins.CreateNewSkinDef(new R2API.SkinDefParamsInfo
            {
                RootObject = prefabCharacterModel.gameObject,
                Name = GTRaptor_PREFIX + "DEFAULT_SKINYELLOW",
                NameToken = GTRaptor_PREFIX + "DEFAULT_SKINYELLOW",
                Icon = gtrassets.LoadAsset<Sprite>("texGTYELLOW"),
                UnlockableDef = null,
                RendererInfos = [.. defaultRendererinfos],
                MeshReplacements = [],
                GameObjectActivations = []
            });
            skins.Add(GTRSkinYellow);
            SkinDef GTRSkinTAC = R2API.Skins.CreateNewSkinDef(new R2API.SkinDefParamsInfo
            {
                RootObject = prefabCharacterModel.gameObject,
                Name = GTRaptor_PREFIX + "DEFAULT_SKINTAC",
                NameToken = GTRaptor_PREFIX + "DEFAULT_SKINTAC",
                Icon = gtrassets.LoadAsset<Sprite>("texGTRTACskin"),
                UnlockableDef = null,
                RendererInfos = [.. defaultRendererinfos],
                MeshReplacements = [],
                GameObjectActivations = []
            });
            skins.Add(GTRSkinTAC);
            #endregion
            #region MasterySkin
            SkinDef masterySkin = R2API.Skins.CreateNewSkinDef(new R2API.SkinDefParamsInfo
            {
                RootObject = prefabCharacterModel.gameObject,
                Name = GTRaptor_PREFIX + "MASTERY_SKIN_NAME",
                NameToken = GTRaptor_PREFIX + "MASTERY_SKIN_NAME",
                Icon = gtrassets.LoadAsset<Sprite>("texGTRAchiveIcon"),
                UnlockableDef = GTRaptorUnlockables.masterySkinUnlockableDef,
                RendererInfos = [.. defaultRendererinfos],
                MeshReplacements = [],
                GameObjectActivations = []
            });
            skins.Add(masterySkin);
            #endregion
            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            GTRaptorAI.Init(bodyPrefab, mastername);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(GTRaptorBuffs.armorBuff))
            {
                args.armorAdd += 300;
            }
        }
    }
}