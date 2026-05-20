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
        public int FacingDirection { get; private set; }
        public IWeapon? EquippedWeapon { get; set; }

        public string LastActionMessage { get; private set; }

        public Player(string name) : base(name, 100, 2, 8)
        {
            Lives = 3;
            MapSymbol = 'P';
            SpriteKey = "player_sprite";
            currentState = new IdleState();
            LastActionMessage = "Player is idle.";
            FacingDirection = 1;
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
            FacingDirection = -1;
            LastActionMessage = $"{Name} moved left.";
            base.MoveLeft();
        }

        public void ExecuteMoveRightAction()
        {
            FacingDirection = 1;
            LastActionMessage = $"{Name} moved right.";
            base.MoveRight();
        }

        public void ExecuteJumpAction()
        {
            LastActionMessage = $"{Name} jumped.";
            Y--;
        }

        public void ExecuteAttackAction()
        {
            if (EquippedWeapon == null)
            {
                LastActionMessage = $"{Name} tried to attack, but no weapon is equipped.";
                Notify();
                return;
            }

            LastActionMessage = EquippedWeapon.Use();
            Notify();
        }

        protected override void UpdateMainBehavior()
        {
            HandleState();
        }

        protected override void UpdateSecondaryBehavior()
        {
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health < 0)
            {
                Health = 0;
            }

            LastActionMessage = $"{Name} took {damage} damage.";
            Notify();
        }
        public override string GetInfo()
        {
            string weaponInfo = EquippedWeapon == null ? "None" : EquippedWeapon.GetType().Name;
            return base.GetInfo() + $", Lives={Lives}, Weapon={weaponInfo}, State={playerState}";
        }
    }
}