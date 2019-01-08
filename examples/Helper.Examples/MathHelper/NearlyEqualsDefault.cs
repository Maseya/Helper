using System;
using Maseya.Helper;

public static class Example
{
    public static void Main()
    {
        var x = 0f;
        for (var i = 1; i <= 10; i++)
        {
            // Repeated floating point addition can lead to rounding errors.
            x += 0.01f;
            PrintEquality(x, i / 100f);
        }

        PrintEquality(0, 0);
        PrintEquality(0, 1);
        PrintEquality(0, MathHelper.DefaultTolerance);
        PrintEquality(
            -MathHelper.DefaultTolerance,
            +MathHelper.DefaultTolerance);
    }

    // Determine whether two values are precisely equal, nearly equal within
    // some range, or are unequal by more than that range.
    public static void PrintEquality(float x, float y)
    {
        if (MathHelper.NearlyEquals(x, y))
        {
            if (x == y)
            {
                Console.WriteLine($"{x} and {y} are equal.");
            }
            else
            {
                Console.WriteLine(
                    $"{x} and {y} are approximately equal within the range " +
                    $"of {MathHelper.DefaultTolerance}.");
            }
        }
        else
        {
            Console.WriteLine(
                $"{x} and {y} differ in value by more than " +
                $"{MathHelper.DefaultTolerance}.");
        }
    }
}

/* The example displays the following output:
0.01 and 0.01 are equal.
0.02 and 0.02 are equal.
0.03 and 0.03 are equal.
0.04 and 0.04 are equal.
0.05 and 0.05 are approximately equal to within 1E-07.
0.05999999 and 0.06 are approximately equal to within 1E-07.
0.06999999 and 0.07 are approximately equal to within 1E-07.
0.07999999 and 0.08 are approximately equal to within 1E-07.
0.08999999 and 0.09 are approximately equal to within 1E-07.
0.09999999 and 0.1 are approximately equal to within 1E-07.
0 and 0 are equal.
0 and 1 differ by more than 1E-07.
0 and 1E-07 are approximately equal to within 1E-07.
-1E-07 and 1E-07 differ by more than 1E-07.
*/
