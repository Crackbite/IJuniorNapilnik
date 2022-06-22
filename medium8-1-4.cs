class Player
{
    public int Age { get; private set; }
    public Movement Movement { get; private set; }
    public string Name { get; private set; }
    public Weapon Weapon { get; private set; }

    public void Attack()
    {
        // Attack
    }

    public void Move()
    {
        // Do move
    }
}

class Weapon
{
    public float Cooldown { get; private set; }
    public int Damage { get; private set; }
    public float Speed { get; private set; }

    public bool IsReloading()
    {
        throw new NotImplementedException();
    }
}

class Movement
{
    public float DirectionX { get; private set; }
    public float DirectionY { get; private set; }
}