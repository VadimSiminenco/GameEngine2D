using GameEngine2D.Biomes;
using GameEngine2D.Bosses;
using GameEngine2D.Commands;
using GameEngine2D.Construction;
using GameEngine2D.Core.Systems;
using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;
using GameEngine2D.Observers;
using GameEngine2D.Spawning;
using GameEngine2D.Weapons;

namespace GameEngine2D.Core.Scene
{
    public class EngineScene
    {
        public string CurrentBiomeName { get; private set; }
        public string CurrentPlatformStyleName { get; private set; }
        public string CurrentDecorationStyleName { get; private set; }
        public BiomeType CurrentBiomeType { get; private set; }
        public string CurrentBackgroundVisualKey { get; private set; }
        public IReadOnlyList<BiomeDecoration> CurrentBiomeDecorations { get; private set; }
        public Player Player { get; private set; }
        public List<Npc> Npcs { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public List<Structure> Structures { get; private set; }
        public IWeapon PlayerWeapon { get; private set; }
        public IBoss Boss { get; private set; }
        public int BossX { get; private set; }
        public int BossY { get; private set; }
        public bool IsBossSpawned { get; private set; }
        public string LastBossMessage { get; private set; }
        public string LastPlayerMessage { get; private set; }
        public bool IsBossAttacking { get; private set; }
        public bool IsPlayerAttackVisible { get; private set; }
        public bool IsPlayerHitVisible { get; private set; }
        public bool IsPlayerOnTrap { get; private set; }
        public bool IsTrapActivated { get; private set; }
        public string LastEditorMessage { get; private set; }
        private int enemyCounter;
        private int structureCounter;
        public bool IsPlayerDefeated { get; private set; }
        public bool IsLevelRestarting { get; private set; }
        private readonly CommandInvoker commandInvoker;
        private readonly PhysicsSystem physicsSystem;
        private readonly BossBehaviorSystem bossBehaviorSystem;
        private readonly CombatSystem combatSystem;
        private readonly InteractionSystem interactionSystem;
        private float playerAttackVisualTimer;
        private const float PlayerAttackVisualDuration = 0.18f;
        private float playerHitVisualTimer;
        private float restartTimer;
        private const float LevelRestartDelay = 2.0f;
        private const float PlayerHitVisualDuration = 0.25f;
        public EngineScene()
        {
            CurrentBiomeName = string.Empty;
            CurrentBackgroundVisualKey = "forest";
            CurrentPlatformStyleName = string.Empty;
            CurrentDecorationStyleName = string.Empty;
            CurrentBiomeType = BiomeType.Forest;
            CurrentBiomeDecorations = new List<BiomeDecoration>();

            Player = null!;
            Npcs = new List<Npc>();
            Enemies = new List<Enemy>();
            Structures = new List<Structure>();

            PlayerWeapon = null!;

            Boss = null!;
            BossX = 34;
            BossY = PhysicsSystem.CharacterGroundY;
            IsBossSpawned = false;
            LastBossMessage = "Boss is hidden behind proxy.";
            LastPlayerMessage = "Player is ready.";
            IsBossAttacking = false;
            IsPlayerAttackVisible = false;
            IsPlayerHitVisible = false;
            IsPlayerOnTrap = false;
            IsTrapActivated = false;
            LastEditorMessage = "World editor ready";
            IsPlayerDefeated = false;
            IsLevelRestarting = false;
            restartTimer = 0f;
            enemyCounter = 2;
            structureCounter = 0;

            commandInvoker = new CommandInvoker();
            physicsSystem = new PhysicsSystem();
            bossBehaviorSystem = new BossBehaviorSystem();
            combatSystem = new CombatSystem();
            interactionSystem = new InteractionSystem();

            playerAttackVisualTimer = 0f;
            playerHitVisualTimer = 0f;

            BuildScene();
        }

        private void BuildScene()
        {
            CreateBiome();
            CreateCharacters();
            CreateStructures();
            CreateWeapon();
            CreateBoss();
        }
        private void CreateBiome()
        {
            SetForestBiome();
        }

        public void SetForestBiome()
        {
            SetBiome(BiomeType.Forest, new ForestBiomeFactory());
        }

        public void SetDesertBiome()
        {
            SetBiome(BiomeType.Desert, new DesertBiomeFactory());
        }

        public void SetCastleBiome()
        {
            SetBiome(BiomeType.Castle, new CastleBiomeFactory());
        }

        private void SetBiome(BiomeType biomeType, IBiomeFactory biomeFactory)
        {
            IBackground background = biomeFactory.CreateBackground();
            IPlatformStyle platformStyle = biomeFactory.CreatePlatformStyle();
            IDecorationStyle decorationStyle = biomeFactory.CreateDecorationStyle();

            CurrentBiomeType = biomeType;
            CurrentBiomeName = background.Name;
            CurrentBackgroundVisualKey = background.VisualKey;
            CurrentPlatformStyleName = platformStyle.Name;
            CurrentDecorationStyleName = decorationStyle.Name;
            CurrentBiomeDecorations = decorationStyle.Decorations;

            LastEditorMessage = $"{CurrentBiomeName} biome applied";
        }
        private void CreateCharacters()
        {
            CharacterFactory playerFactory = new PlayerFactory();
            CharacterFactory npcFactory = new NpcFactory();
            CharacterFactory enemyFactory = new EnemyFactory();

            Player = (Player)playerFactory.SpawnCharacter("Player");
            Player.X = 2;
            Player.Y = PhysicsSystem.CharacterGroundY;

            Npc guide = (Npc)npcFactory.SpawnCharacter("Guide NPC");
            guide.X = 6;
            guide.Y = PhysicsSystem.CharacterGroundY;
            Npcs.Add(guide);

            Enemy enemy1 = (Enemy)enemyFactory.SpawnCharacter("Enemy 1");
            enemy1.X = 15;
            enemy1.Y = PhysicsSystem.CharacterGroundY;
            Enemies.Add(enemy1);

            Enemy enemy2 = (Enemy)enemyFactory.SpawnCharacter("Enemy 2");
            enemy2.X = 28;
            enemy2.Y = PhysicsSystem.CharacterGroundY;
            Enemies.Add(enemy2);
        }
        public void SpawnEnemy()
        {
            enemyCounter++;

            CharacterFactory enemyFactory = new EnemyFactory();
            Enemy enemy = (Enemy)enemyFactory.SpawnCharacter($"Enemy {enemyCounter}");

            enemy.X = 12 + enemyCounter * 2;
            enemy.Y = PhysicsSystem.CharacterGroundY;

            Enemies.Add(enemy);
        }
        public void SpawnEnemyAt(int x, int y)
        {
            enemyCounter++;

            CharacterFactory enemyFactory = new EnemyFactory();
            Enemy enemy = (Enemy)enemyFactory.SpawnCharacter($"Enemy {enemyCounter}");

            enemy.X = x;
            enemy.Y = y;

            Enemies.Add(enemy);

            LastEditorMessage = "Enemy spawned";
        }

        public void SpawnNpcAt(int x, int y)
        {
            CharacterFactory npcFactory = new NpcFactory();
            Npc npc = (Npc)npcFactory.SpawnCharacter($"NPC {Npcs.Count + 1}");

            npc.X = x;
            npc.Y = y;

            Npcs.Add(npc);

            LastEditorMessage = "NPC placed";
        }

        private void CreateStructures()
        {
            StructureDirector director = new StructureDirector();

            HouseBuilder houseBuilder = new HouseBuilder();
            director.Construct(houseBuilder);
            Structure house = houseBuilder.GetResult();
            house.X = 8;
            house.Y = physicsSystem.GetStructureGroundY(house);
            Structures.Add(house);

            BridgeBuilder bridgeBuilder = new BridgeBuilder();
            director.Construct(bridgeBuilder);
            Structure bridge = bridgeBuilder.GetResult();
            bridge.X = 20;
            bridge.Y = physicsSystem.GetStructureGroundY(bridge);
            Structures.Add(bridge);

            TrapBuilder trapBuilder = new TrapBuilder();
            director.Construct(trapBuilder);
            Structure trap = trapBuilder.GetResult();
            trap.X = 25;
            trap.Y = physicsSystem.GetStructureGroundY(trap);
            Structures.Add(trap);
        }

        public void BuildHouse()
        {
            HouseBuilder builder = new HouseBuilder();
            BuildStructure(builder);
        }

        public void BuildBridge()
        {
            BridgeBuilder builder = new BridgeBuilder();
            BuildStructure(builder);
        }

        public void BuildTrap()
        {
            TrapBuilder builder = new TrapBuilder();
            BuildStructure(builder);
        }

        private void BuildStructure(IStructureBuilder builder)
        {
            structureCounter++;

            StructureDirector director = new StructureDirector();
            director.Construct(builder);

            Structure structure = builder.GetResult();

            structure.X = 5 + structureCounter * 3;
            structure.Y = physicsSystem.GetStructureGroundY(structure);

            Structures.Add(structure);
        }

        public void BuildHouseAt(int x)
        {
            HouseBuilder builder = new HouseBuilder();
            BuildStructureAt(builder, x);

            LastEditorMessage = "House created";
        }

        public void BuildBridgeAt(int x)
        {
            BridgeBuilder builder = new BridgeBuilder();
            BuildStructureAt(builder, x);

            LastEditorMessage = "Bridge created";
        }

        public void BuildTrapAt(int x)
        {
            TrapBuilder builder = new TrapBuilder();
            BuildStructureAt(builder, x);

            LastEditorMessage = "Trap placed";
        }

        private void BuildStructureAt(IStructureBuilder builder, int x)
        {
            StructureDirector director = new StructureDirector();
            director.Construct(builder);

            Structure structure = builder.GetResult();

            structure.X = x;
            structure.Y = physicsSystem.GetStructureGroundY(structure);

            Structures.Add(structure);
        }
        private void CreateWeapon()
        {
            IAttackImplementor attackImplementor = new MeleeAttackImplementor();

            PlayerWeapon = new Sword(20, attackImplementor);
            Player.EquippedWeapon = PlayerWeapon;
        }
        public void EquipBasicSword()
        {
            IAttackImplementor attackImplementor = new MeleeAttackImplementor();

            PlayerWeapon = new Sword(20, attackImplementor);
            Player.EquippedWeapon = PlayerWeapon;

            LastEditorMessage = "Basic sword equipped";
        }

        public void ApplyFireEffectToSword()
        {
            IAttackImplementor attackImplementor = new MeleeAttackImplementor();

            IWeapon sword = new Sword(20, attackImplementor);
            PlayerWeapon = new FireWeapon(sword, 5);
            Player.EquippedWeapon = PlayerWeapon;

            LastEditorMessage = "Fire effect applied";
        }

        public void ApplyIceEffectToSword()
        {
            IAttackImplementor attackImplementor = new MeleeAttackImplementor();

            IWeapon sword = new Sword(20, attackImplementor);
            PlayerWeapon = new IceWeapon(sword);
            Player.EquippedWeapon = PlayerWeapon;

            LastEditorMessage = "Ice effect applied";
        }
        private void CreateBoss()
        {
            Boss = new BossProxy("Ancient Guardian", 400, 25);
        }

        public void SpawnBoss()
        {
            LastBossMessage = Boss.Spawn();
            IsBossSpawned = true;

            bossBehaviorSystem.Reset();
            IsBossAttacking = bossBehaviorSystem.IsBossAttacking;

            LastEditorMessage = "Boss awakened";
        }

        public void Update(float elapsedSeconds)
        {
            if (IsLevelRestarting)
            {
                UpdateLevelRestart(elapsedSeconds);
                return;
            }

            if (Player.Health <= 0)
            {
                StartLevelRestart();
                return;
            }
            physicsSystem.UpdatePlayerGravity(Player, Structures, elapsedSeconds);

            UpdatePlayerAttackVisual(elapsedSeconds);

            string? bossMessage = bossBehaviorSystem.Update(
                Boss,
                IsBossSpawned,
                Player,
                BossX,
                BossY,
                elapsedSeconds,
                out bool playerWasHitByBoss);

            if (bossMessage != null)
            {
                LastBossMessage = bossMessage;
            }

            if (playerWasHitByBoss)
            {
                IsPlayerHitVisible = true;
                playerHitVisualTimer = PlayerHitVisualDuration;
            }

            IsBossAttacking = bossBehaviorSystem.IsBossAttacking;

            UpdatePlayerInteractions();
            UpdatePlayerHitVisual(elapsedSeconds);
            UpdateEnemyAttacks(elapsedSeconds);
        }

        private void StartLevelRestart()
        {
            IsPlayerDefeated = true;
            IsLevelRestarting = true;
            restartTimer = 0f;

            LastPlayerMessage = "Player was defeated. Restarting level...";
        }

        private void UpdateLevelRestart(float elapsedSeconds)
        {
            restartTimer += elapsedSeconds;

            if (restartTimer < LevelRestartDelay)
            {
                return;
            }

            RestartLevel();
        }
        private void RestartLevel()
        {
            IsPlayerDefeated = false;
            IsLevelRestarting = false;
            restartTimer = 0f;

            LastPlayerMessage = "Level restarted.";
            LastBossMessage = "Boss is hidden behind proxy.";
            LastEditorMessage = "World editor ready";

            IsPlayerAttackVisible = false;
            IsPlayerHitVisible = false;
            IsPlayerOnTrap = false;
            IsTrapActivated = false;

            IsBossSpawned = false;
            IsBossAttacking = false;

            playerAttackVisualTimer = 0f;
            playerHitVisualTimer = 0f;

            enemyCounter = 2;
            structureCounter = 0;

            Enemies.Clear();
            Npcs.Clear();
            Structures.Clear();

            CreateCharacters();
            CreateStructures();
            CreateWeapon();
            CreateBoss();

            bossBehaviorSystem.Reset();

            BossX = 34;
            BossY = PhysicsSystem.CharacterGroundY;
        }
        private void UpdatePlayerAttackVisual(float elapsedSeconds)
        {
            if (!IsPlayerAttackVisible)
            {
                return;
            }

            playerAttackVisualTimer -= elapsedSeconds;

            if (playerAttackVisualTimer <= 0f)
            {
                IsPlayerAttackVisible = false;
                playerAttackVisualTimer = 0f;
            }
        }
        private void UpdatePlayerHitVisual(float elapsedSeconds)
        {
            if (!IsPlayerHitVisible)
            {
                return;
            }

            playerHitVisualTimer -= elapsedSeconds;

            if (playerHitVisualTimer <= 0f)
            {
                IsPlayerHitVisible = false;
                playerHitVisualTimer = 0f;
            }
        }

        private void UpdateEnemyAttacks(float elapsedSeconds)
        {
            bool playerWasHit = combatSystem.UpdateEnemyAttacks(Player, Enemies, elapsedSeconds);

            if (!playerWasHit)
            {
                return;
            }

            IsPlayerHitVisible = true;
            playerHitVisualTimer = PlayerHitVisualDuration;
        }
        private void UpdatePlayerInteractions()
        {
            IsPlayerOnTrap = interactionSystem.IsPlayerOnTrap(Player, Structures);
            IsTrapActivated = interactionSystem.TryActivateTrap(Player, Structures, 10);
        }
        public void AttachPlayerObserver(IPlayerObserver observer)
        {
            Player.Attach(observer);
        }
        public void MovePlayerLeft()
        {
            ICommand command = new MoveLeftCommand(Player);
            ExecutePlayerCommand(command);
        }

        public void MovePlayerRight()
        {
            ICommand command = new MoveRightCommand(Player);
            ExecutePlayerCommand(command);
        }

        public void MakePlayerJump()
        {
            if (!physicsSystem.IsPlayerOnGroundOrPlatform(Player, Structures))
            {
                return;
            }

            ICommand command = new JumpCommand(Player);
            ExecutePlayerCommand(command);
        }

        public void MakePlayerAttack()
        {
            ICommand command = new AttackCommand(Player);
            ExecutePlayerCommand(command);

            IsPlayerAttackVisible = true;
            playerAttackVisualTimer = PlayerAttackVisualDuration;

            bool enemyWasHit = combatSystem.TryAttackEnemy(Player, Enemies);

            if (enemyWasHit)
            {
                return;
            }

            bool bossWasHit = combatSystem.TryAttackBoss(
                Player,
                Boss,
                IsBossSpawned,
                BossX,
                BossY,
                out string bossMessage);

            if (bossWasHit)
            {
                LastBossMessage = bossMessage;
            }
        }

        private void ExecutePlayerCommand(ICommand command)
        {
            commandInvoker.SetCommand(command);
            commandInvoker.ExecuteCommand();

            Player.HandleState();

            physicsSystem.ClampCharacterToWorld(Player);
        }
        public bool HasWeaponEffect(WeaponEffectType effectType)
        {
            if (Player.EquippedWeapon == null)
            {
                return false;
            }

            return Player.EquippedWeapon.GetEffects().Contains(effectType);
        }

        public bool HasFireWeaponEffect()
        {
            return HasWeaponEffect(WeaponEffectType.Fire);
        }

        public bool HasIceWeaponEffect()
        {
            return HasWeaponEffect(WeaponEffectType.Ice);
        }
    }
}