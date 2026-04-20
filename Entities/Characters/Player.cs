using GameEngine2D.Observers;
using GameEngine2D.States;
using GameEngine2D.Weapons;

namespace GameEngine2D.Entities.Characters
{
    public class Player : Character, IPlayerSubject
    {
        private readonly List<IPlayerObserver> observers = new();

        private string playerState = "Idle";
        private IPlayerState currentState;

        public int Lives { get; }
        public IWeapon? EquippedWeapon { get; set; }

        public Player(string name) : base(name, 100, 2, 8)
        {
            Lives = 3;
            MapSymbol = 'P';
            SpriteKey = "player_sprite";
            currentState = new IdleState();
        }

        public void Attach(IPlayerObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IPlayerObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IPlayerObserver observer in observers)
            {
                observer.Update();
            }
        }

        public string GetState()
        {
            return playerState;
        }

        public void SetState(IPlayerState state, bool notify = true)
        {
            currentState = state;
            playerState = state.Name;

            if (notify)
            {
                Notify();
            }
        }

        public void HandleState()
        {
            currentState.Handle(this);
        }

        public override void MoveLeft()
        {
            SetState(new MovingLeftState());
        }

        public override void MoveRight()
        {
            SetState(new MovingRightState());
        }

        public void Jump()
        {
            SetState(new JumpingState());
        }

        public void Attack()
        {
            SetState(new AttackingState());
        }

        public void ExecuteMoveLeftAction()
        {
            base.MoveLeft();
        }

        public void ExecuteMoveRightAction()
        {
            base.MoveRight();
        }

        public void ExecuteJumpAction()
        {
            Y++;
        }

        public void ExecuteAttackAction()
        {
            if (EquippedWeapon == null)
            {
                Console.WriteLine($"{Name} tries to attack, but no weapon is equipped.");
                return;
            }

            Console.WriteLine($"{Name} attacks:");
            EquippedWeapon.Use();
        }

        protected override void UpdateMainBehavior()
        {
            HandleState();
        }

        protected override void UpdateSecondaryBehavior()
        {
        }

        public override string GetInfo()
        {
            string weaponInfo = EquippedWeapon == null ? "None" : EquippedWeapon.GetType().Name;
            return base.GetInfo() + $", Lives={Lives}, Weapon={weaponInfo}, State={playerState}";
        }
    }
}