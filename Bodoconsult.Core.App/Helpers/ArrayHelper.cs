// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.Helpers;

/// <summary>
/// Tools for array handling
/// </summary>
public static class ArrayHelper
{
    /// <summary>
    /// Compare two allocation arrays
    /// </summary>
    /// <param name="array1">Allocation array 1</param>
    /// <param name="array2">Allocation array 2</param>
    /// <returns>True if both arrays have the same values, else false.</returns>
    public static bool ArrayCompare(int[,,] array1, int[,,] array2)
    {

        var stackLen = array1.GetLength(0);
        var rowLen = array1.GetLength(1);
        var subindexLen = array1.GetLength(2);

        if (stackLen != array2.GetLength(0))
        {
            return false;
        }

        if (rowLen != array2.GetLength(1))
        {
            return false;
        }

        if (subindexLen != array2.GetLength(2))
        {
            return false;
        }

        for (var stack = 0; stack < stackLen; stack++)
        {
            for (var row = 0; row < rowLen; row++)
            { 
                for (var subindex = 0; subindex < subindexLen; subindex++)
                {
                    if (array1[stack, row, subindex] != array2[stack, row, subindex])
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}