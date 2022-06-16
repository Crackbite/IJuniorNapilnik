public static int MakeValidNumber(int a, int b, int c)
{
    if (a < b)
        return b;
    else if (a > c)
        return c;
    else
        return a;
}