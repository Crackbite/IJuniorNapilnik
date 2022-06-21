public static void CreateObjectOnMap()
{
    //Создание объекта на карте
}

public static void SetChance()
{
    _chance = Random.Range(0, 100);
}

public static int GetSalary(int hoursWorked)
{
    return _hourlyRate * hoursWorked;
}