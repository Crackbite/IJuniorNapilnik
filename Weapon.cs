internal class Weapon
{
    private readonly int _damage;

    private int _bullets;

    public Weapon(int bullets, int damage)
    {
        if (bullets < 0)
        {
            throw new ArgumentException("Количество пуль не может быть отрицательным числом", nameof(bullets));
        }

        if (damage <= 0)
        {
            throw new ArgumentException("Некорректное количество урона", nameof(damage));
        }

        _bullets = bullets;
        _damage = damage;
    }

    public void Fire(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        if (_bullets <= 0 || player.IsDead)
        {
            return;
        }

        _bullets--;
        player.TakeDamage(_damage);
    }
}

internal class Player
{
    private int _health;

    public Player(int health)
    {
        if (health <= 0)
        {
            throw new ArgumentException("Некорректное количество здоровья", nameof(health));
        }

        _health = health;
    }

    public bool IsDead => _health <= 0;

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            throw new ArgumentException("Количество урона должно быть положительным числом", nameof(damage));
        }

        _health -= IsDead ? 0 : damage;
    }
}

internal class Bot
{
    private readonly Weapon _weapon;

    public Bot(Weapon weapon)
    {
        _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
    }

    public void OnSeePlayer(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        _weapon.Fire(player);
    }
}