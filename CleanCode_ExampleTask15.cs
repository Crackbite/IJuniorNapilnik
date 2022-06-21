class Player { }
class Gun { }
class Target { }
class Unit
{
    public IReadOnlyCollection<Unit> UnitsToGet { get; private set; }
}