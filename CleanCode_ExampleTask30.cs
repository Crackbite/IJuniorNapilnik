public void SetEnable()
{
    _enable = true;
    _effects.StartEnableAnimation();
}

public void SetDisable()
{
    _enable = false;
    _pool.Free(this);
}