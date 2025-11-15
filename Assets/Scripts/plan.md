# TDD Test Plan for Murim Card Game

## Test Strategy
This plan follows Kent Beck's Test-Driven Development approach:
- Write ONE failing test at a time
- Implement ONLY enough code to make that test pass
- Refactor when tests are green
- Separate structural changes from behavioral changes

## Test Marking Convention
- [ ] Unmarked - Not yet implemented
- [X] Completed - Test and implementation done
- [SKIP] Skipped - Intentionally skipped for now

---

## Phase 1: Core Data Structures

### StatusEffect Tests
- [ ] Test_StatusEffect_Constructor_SetsDataAndValue
- [ ] Test_StatusEffect_Value_CanBeModified

### StatusEffectBehavior Tests
- [ ] Test_PoisonBehavior_OnTurnStart_DealsDamageAndDecrementsValue
- [ ] Test_PoisonBehavior_IsFinished_WhenValueZero
- [ ] Test_VulnerableBehavior_OnDamageTaken_IncreasesBy50Percent
- [ ] Test_VulnerableBehavior_OnTurnEnd_DecrementsValue
- [ ] Test_WeakBehavior_OnDamageDealt_DecreasesBy25Percent
- [ ] Test_WeakBehavior_OnTurnEnd_DecrementsValue
- [ ] Test_StrengthBehavior_IsFinished_ReturnsFalse
- [ ] Test_ThornsBehavior_IsFinished_ReturnsFalse

### StatusEffectFactory Tests
- [ ] Test_StatusEffectFactory_Create_ReturnsCorrectBehaviorForPoison
- [ ] Test_StatusEffectFactory_Create_ReturnsCorrectBehaviorForVulnerable
- [ ] Test_StatusEffectFactory_Create_ReturnsCorrectBehaviorForWeak
- [ ] Test_StatusEffectFactory_Create_ReturnsCorrectBehaviorForStrength
- [ ] Test_StatusEffectFactory_Create_ReturnsDefaultBehaviorForUnknownType

---

## Phase 2: Player Basic Functionality

### Player Setup Tests
- [ ] Test_Player_Setup_InitializesHealthCorrectly
- [ ] Test_Player_Setup_InitializesGoldToZero
- [ ] Test_Player_Setup_InitializesDeckFromStartingDeck
- [ ] Test_Player_Setup_ShufflesDrawPile
- [ ] Test_Player_Setup_ClearsAllPiles

### Player Health Tests
- [ ] Test_Player_TakeDamage_ReducesCurrentHealth
- [ ] Test_Player_TakeDamage_WithDefense_ReducesDamageByDefenseAmount
- [ ] Test_Player_TakeDamage_WithVulnerable_IncreasesDamageBy50Percent
- [ ] Test_Player_TakeDamage_CannotGoBelowZero
- [ ] Test_Player_Heal_IncreasesCurrentHealth
- [ ] Test_Player_Heal_CannotExceedMaxHealth

### Player Defense Tests
- [ ] Test_Player_GainDefense_IncreasesDefenseValue
- [ ] Test_Player_StartTurn_ResetsDefenseToZero

### Player Gold Tests
- [ ] Test_Player_GainGold_IncreasesGoldAmount
- [ ] Test_Player_SpendGold_WithSufficientGold_DecreasesGoldAndReturnsTrue
- [ ] Test_Player_SpendGold_WithInsufficientGold_DoesNotChangeGoldAndReturnsFalse

---

## Phase 3: Card Management

### Draw System Tests
- [ ] Test_Player_DrawCards_MovesCardsFromDrawPileToHand
- [ ] Test_Player_DrawCards_WithEmptyDrawPile_ShufflesDiscardPileIntoDrawPile
- [ ] Test_Player_DrawCards_WithEmptyDrawAndDiscardPile_StopsDrawing

### Play Card Tests
- [ ] Test_Player_PlayCard_WithSufficientNaegong_DecreasesNaegong
- [ ] Test_Player_PlayCard_WithInsufficientNaegong_DoesNothing
- [ ] Test_Player_PlayCard_RemovesCardFromHand
- [ ] Test_Player_PlayCard_MovesCardToDiscardPile
- [ ] Test_Player_PlayCard_WithJeolchoCard_MovesToExhaustPile

### Card Upgrade Tests
- [ ] Test_Player_UpgradeCard_InDrawPile_ReplacesWithUpgradedVersion
- [ ] Test_Player_UpgradeCard_InHand_ReplacesWithUpgradedVersion
- [ ] Test_Player_UpgradeCard_InDiscardPile_ReplacesWithUpgradedVersion
- [ ] Test_Player_UpgradeCard_AlreadyUpgraded_ReturnsFalse
- [ ] Test_Player_UpgradeCard_NoUpgradedVersion_ReturnsFalse
- [ ] Test_Player_UpgradeRandomCard_UpgradesOneEligibleCard

### Card Removal Tests
- [ ] Test_Player_RemoveCardFromDeck_InHand_RemovesCard
- [ ] Test_Player_RemoveCardFromDeck_InDrawPile_RemovesCard
- [ ] Test_Player_RemoveCardFromDeck_InDiscardPile_RemovesCard
- [ ] Test_Player_RemoveCardFromDeck_NotInDeck_ReturnsFalse
- [ ] Test_Player_RemoveRandomCards_RemovesSpecifiedNumber
- [ ] Test_Player_RemoveAllBasicCards_RemovesAllStrikeAndDefendCards
- [ ] Test_Player_RemoveAllCurseCards_RemovesAllCurseCards

---

## Phase 4: Status Effects

### Apply Status Effect Tests
- [ ] Test_Player_ApplyStatusEffect_NewEffect_AddsToList
- [ ] Test_Player_ApplyStatusEffect_ExistingEffect_StacksValue
- [ ] Test_Player_GetStatusEffectValue_ExistingEffect_ReturnsValue
- [ ] Test_Player_GetStatusEffectValue_NonExistingEffect_ReturnsZero

### Process Status Effects Tests
- [ ] Test_Player_ProcessStatusEffectsOnTurnStart_CallsOnTurnStartOnAllEffects
- [ ] Test_Player_ProcessStatusEffectsOnTurnStart_RemovesFinishedEffects
- [ ] Test_Player_ProcessStatusEffectsOnTurnEnd_CallsOnTurnEndOnAllEffects
- [ ] Test_Player_ProcessStatusEffectsOnTurnEnd_RemovesFinishedEffects

---

## Phase 5: Enemy Basic Functionality

### Enemy Setup Tests
- [ ] Test_Enemy_Setup_InitializesHealthFromEnemyData
- [ ] Test_Enemy_Setup_InitializesActionPatternFromEnemyData
- [ ] Test_Enemy_Setup_PlansFirstAction

### Enemy Combat Tests
- [ ] Test_Enemy_TakeDamage_ReducesCurrentHealth
- [ ] Test_Enemy_TakeDamage_WithDefense_ReducesDamageByDefenseAmount
- [ ] Test_Enemy_TakeDamage_WithVulnerable_IncreasesDamageBy50Percent
- [ ] Test_Enemy_TakeDamage_WithStrength_AppliesStrengthBonus
- [ ] Test_Enemy_TakeDirectDamage_IgnoresDefense
- [ ] Test_Enemy_TakeDamage_AtZeroHealth_CallsDie

### Enemy Action Tests
- [ ] Test_Enemy_TakeTurn_Attack_DamagesPlayer
- [ ] Test_Enemy_TakeTurn_Defend_GainsDefense
- [ ] Test_Enemy_TakeTurn_Buff_AppliesStatusEffectToSelf
- [ ] Test_Enemy_TakeTurn_Debuff_AppliesStatusEffectToPlayer
- [ ] Test_Enemy_TakeTurn_AdvancesTurnIndex

### Enemy Phase System Tests
- [ ] Test_Enemy_SwitchToPhase2_AtThreshold_ChangesActionPattern
- [ ] Test_Enemy_SwitchToPhase2_RemovesDebuffs
- [ ] Test_Enemy_SwitchToPhase2_AppliesStrengthBuff

---

## Phase 6: EffectProcessor

### Target Resolution Tests
- [ ] Test_EffectProcessor_Process_TargetSelf_AppliesEffectToCaster
- [ ] Test_EffectProcessor_Process_TargetSingleEnemy_AppliesEffectToPrimaryTarget
- [ ] Test_EffectProcessor_Process_TargetAllEnemies_AppliesEffectToAllEnemies
- [ ] Test_EffectProcessor_Process_TargetRandomEnemy_AppliesEffectToOneRandomEnemy
- [ ] Test_EffectProcessor_Process_TargetPlayer_AppliesEffectToPlayer

### Effect Type Tests
- [ ] Test_EffectProcessor_Damage_WithoutModifiers_DealsBaseDamage
- [ ] Test_EffectProcessor_Damage_WithStrength_IncreasedByStrength
- [ ] Test_EffectProcessor_Damage_WithWeak_ReducedBy25Percent
- [ ] Test_EffectProcessor_Damage_WithDefense_ReducedByDefense
- [ ] Test_EffectProcessor_Block_GainsDefense
- [ ] Test_EffectProcessor_DrawCard_DrawsCards
- [ ] Test_EffectProcessor_Heal_HealsPlayer
- [ ] Test_EffectProcessor_GainGold_IncreasesGold
- [ ] Test_EffectProcessor_ApplyStatus_AppliesStatusEffect
- [ ] Test_EffectProcessor_UpgradeRandomCard_UpgradesCard

---

## Phase 7: Relic System

### Relic Application Tests
- [ ] Test_Player_AddRelic_AddsToRelicList
- [ ] Test_Player_AddRelic_OnPickupTrigger_ProcessesEffectsImmediately
- [ ] Test_Player_GetRelicEffectValue_SingleRelic_ReturnsValue
- [ ] Test_Player_GetRelicEffectValue_MultipleRelics_ReturnsSummedValue
- [ ] Test_Player_GetRelicEffectValue_NoMatchingRelics_ReturnsZero

### Relic Trigger Tests
- [ ] Test_Player_ApplyCombatStartRelicEffects_ProcessesAllCombatStartRelics
- [ ] Test_Player_StartTurn_AppliesTurnStartRelicEffects

---

## Phase 8: BattleManager

### Battle Flow Tests
- [ ] Test_BattleManager_StartBattle_InitializesEnemies
- [ ] Test_BattleManager_StartBattle_CallsPlayerCombatStartEffects
- [ ] Test_BattleManager_StartPlayerTurn_ProcessesStatusEffects
- [ ] Test_BattleManager_StartPlayerTurn_CallsPlayerStartTurn
- [ ] Test_BattleManager_EndPlayerTurn_ProcessesPlayerStatusEffects
- [ ] Test_BattleManager_EndPlayerTurn_StartsEnemyTurn

### Enemy Turn Tests
- [ ] Test_BattleManager_EnemyTurn_AllEnemiesAct
- [ ] Test_BattleManager_EnemyTurn_ProcessesEnemyStatusEffects
- [ ] Test_BattleManager_EnemyTurn_ReturnsToPlayerTurn

### Card Processing Tests
- [ ] Test_BattleManager_ProcessCardEffect_CallsEffectProcessorForEachEffect
- [ ] Test_BattleManager_ProcessCardEffect_PassesCorrectPrimaryTarget

### Victory Condition Tests
- [ ] Test_BattleManager_OnEnemyDied_RemovesEnemyFromList
- [ ] Test_BattleManager_OnEnemyDied_WithNoEnemiesRemaining_EndsBattle

---

## Phase 9: GameManager State Management

### State Transition Tests
- [ ] Test_GameManager_ChangeState_UpdatesCurrentState
- [ ] Test_GameManager_ChangeState_ActivatesCorrectUI
- [ ] Test_GameManager_ChangeState_DeactivatesOtherUIs

### Game Flow Tests
- [ ] Test_GameManager_StartNewGame_InitializesPlayer
- [ ] Test_GameManager_StartNewGame_GeneratesMap
- [ ] Test_GameManager_StartBattle_ChangesToBattleState
- [ ] Test_GameManager_EndBattle_Victory_ShowsCardReward
- [ ] Test_GameManager_EndBattle_Victory_BossFight_AdvancesFloor
- [ ] Test_GameManager_EndBattle_Defeat_ShowsGameOver

### Reward System Tests
- [ ] Test_GameManager_ShowCardRewardScreen_GeneratesThreeCards
- [ ] Test_GameManager_SelectCardReward_AddsCardToDeck
- [ ] Test_GameManager_ShowRelicRewardScreen_GeneratesThreeRelics
- [ ] Test_GameManager_SelectRelicReward_AddsRelicToPlayer

---

## Phase 10: Save/Load System

### Save Tests
- [ ] Test_GameManager_SaveCurrentRun_SerializesPlayerState
- [ ] Test_GameManager_SaveCurrentRun_SerializesFloorNumber
- [ ] Test_GameManager_SaveCurrentRun_SerializesDecks
- [ ] Test_GameManager_SaveCurrentRun_SerializesRelics

### Load Tests
- [ ] Test_Player_RestoreState_RestoresHealth
- [ ] Test_Player_RestoreState_RestoresGold
- [ ] Test_Player_RestoreState_RestoresDecks
- [ ] Test_Player_RestoreState_RestoresRelics
- [ ] Test_Player_RestoreState_RestoresRealmProgress

### SaveLoadManager Tests
- [ ] Test_SaveLoadManager_SaveRun_WritesFile
- [ ] Test_SaveLoadManager_LoadRun_ReadsFile
- [ ] Test_SaveLoadManager_DoesSaveFileExist_ReturnsTrueWhenExists
- [ ] Test_SaveLoadManager_DeleteSaveFile_RemovesFile

---

## Phase 11: Meta Progression

### MetaProgress Tests
- [ ] Test_MetaManager_AddEnlightenmentPoints_IncreasesPoints
- [ ] Test_MetaManager_GetUpgradeCost_BonusHealth_ReturnsCorrectCost
- [ ] Test_MetaManager_GetUpgradeCost_StartingGold_ReturnsCorrectCost
- [ ] Test_MetaManager_TryPurchaseUpgrade_WithSufficientPoints_IncreasesLevel
- [ ] Test_MetaManager_TryPurchaseUpgrade_WithInsufficientPoints_ReturnsFalse
- [ ] Test_MetaManager_SaveProgress_PersistsData
- [ ] Test_MetaManager_LoadProgress_RestoresData

---

## Phase 12: Map System

### MapManager Tests
- [ ] Test_MapManager_GenerateMap_CreatesCorrectNumberOfLayers
- [ ] Test_MapManager_GenerateMap_FinalFloor_CreatesFinalBossNode
- [ ] Test_MapManager_GenerateMap_ConnectsNodesWithPaths
- [ ] Test_MapManager_GenerateMap_ActivatesFirstLayer
- [ ] Test_MapManager_OnNodeSelected_DeactivatesCurrentLayer
- [ ] Test_MapManager_OnNodeSelected_ActivatesConnectedNodes

### MapNode Tests
- [ ] Test_MapNode_OnNodeClicked_Combat_StartsBattle
- [ ] Test_MapNode_OnNodeClicked_Event_ShowsEvent
- [ ] Test_MapNode_OnNodeClicked_Shop_OpensShop
- [ ] Test_MapNode_OnNodeClicked_RestSite_OpensRestSite
- [ ] Test_MapNode_SetClickable_UpdatesButtonInteractable

---

## Phase 13: Shop System

### ShopManager Tests
- [ ] Test_ShopManager_OpenShop_GeneratesCardStock
- [ ] Test_ShopManager_TryBuyCard_WithSufficientGold_PurchasesCard
- [ ] Test_ShopManager_TryBuyCard_WithInsufficientGold_DoesNotPurchase
- [ ] Test_ShopManager_GenerateCardStock_CreatesCorrectNumberOfCards
- [ ] Test_ShopManager_OnRemoveServiceButtonClicked_WithSufficientGold_ShowsDeckView

---

## Phase 14: Event System

### EventManager Tests
- [ ] Test_EventManager_ShowEvent_DisplaysEventData
- [ ] Test_EventManager_ShowEvent_CreatesCorrectNumberOfChoices
- [ ] Test_EventManager_OnChoiceSelected_SuccessRoll_AppliesSuccessEffects
- [ ] Test_EventManager_OnChoiceSelected_FailureRoll_AppliesFailureEffects
- [ ] Test_EventManager_OnChoiceSelected_WithoutCombat_ReturnsToMap

---

## Phase 15: Realm/XP System

### XP Tests
- [ ] Test_Player_GainXp_IncreasesCurrentXp
- [ ] Test_Player_GainXp_AtMaxRealm_IncreasesCurrentXp
- [ ] Test_Player_GainXp_ReachesThreshold_SetsReadyToAscend

### Sword Realm Tests
- [ ] Test_Player_RecordEvent_CardPlayed_IncrementsCounters
- [ ] Test_Player_RecordEvent_ZeroDamageTurn_IncrementsCounter
- [ ] Test_Player_CheckForSwordRealmAscension_MeetsCondition_AscendsRealm
- [ ] Test_Player_AscendSwordRealm_UpdatesCurrentSwordRealm
- [ ] Test_Player_ResetCombatRecords_ClearsAllCounters

---

## Phase 16: UI Tests (Integration)

### PlayerStatsUI Tests
- [ ] Test_PlayerStatsUI_OnStatsChanged_UpdatesHealthText
- [ ] Test_PlayerStatsUI_OnStatsChanged_UpdatesDefenseVisibility
- [ ] Test_PlayerStatsUI_OnStatsChanged_UpdatesNaegongText

### HandUI Tests
- [ ] Test_HandUI_OnHandChanged_CreatesCorrectNumberOfCardObjects
- [ ] Test_HandUI_OnHandChanged_ClearsPreviousCards

### EnemyUI Tests
- [ ] Test_EnemyUI_Setup_SubscribesToEnemyEvents
- [ ] Test_EnemyUI_UpdateUI_UpdatesHealthSlider
- [ ] Test_EnemyUI_UpdateUI_UpdatesIntentIcon

---

## Phase 17: Edge Cases and Integration

### Complex Battle Scenarios
- [ ] Test_Battle_MultipleEnemies_AllReceiveDamageFromAllEnemiesEffect
- [ ] Test_Battle_StatusEffectStacking_CorrectlyAccumulates
- [ ] Test_Battle_CombinedModifiers_StrengthAndWeak_CalculatesCorrectly

### Data Validation
- [ ] Test_CardData_MissingUpgradedVersion_UpgradeReturnsGracefully
- [ ] Test_GameEffect_MissingStatusEffectData_HandlesGracefully
- [ ] Test_ResourceManager_InvalidID_ReturnsNull

### Boundary Conditions
- [ ] Test_Player_NegativeHealth_ClampsToZero
- [ ] Test_Player_OverMaxHealth_ClampsToMax
- [ ] Test_Player_NegativeGold_PreventsTransaction

---

## Notes

### Testing Approach
1. Each test should be independent and not rely on other tests
2. Use minimal setup - only create what's needed for the test
3. Follow Arrange-Act-Assert pattern
4. Test one behavior per test

### Unity Testing Specifics
- Tests will be in EditMode tests where possible
- PlayMode tests only when absolutely necessary (UI integration)
- Mock Unity components (MonoBehaviour) when testing logic
- Use NUnit framework

### When to Refactor
- After getting tests to pass (Green phase)
- Extract common setup code into helper methods
- Remove duplication between tests and production code
- Improve naming and structure
- Always keep tests passing during refactoring

### Commit Strategy
- Commit after each Red-Green-Refactor cycle
- Mark structural changes clearly in commit messages
- Keep commits small and focused
- Never commit with failing tests
