namespace _09;

internal static class Program
{
    private static int _mapHeight;

    internal static void Main()
    {
        var input = File.ReadAllBytes("input.txt");
        
        _mapHeight = input.Length;

        List<int> files = [];
        List<int> space = [];
        
        for (var i = 0; i < _mapHeight; i++)
        {
            if (i % 2 == 0)
            {
                files.Add(input[i] - '0');
                continue;
            }

            space.Add(input[i] - '0');
        }
        
        Console.WriteLine($"Part 1: {PartOne(files, space)}");
        Console.WriteLine($"Part 2: {PartTwo(files, space)}");
    }

    private static long PartOne(List<int> files, List<int> space)
    {
        long tally = 0;

        var totalFileSize = files.Sum();
        
        var index = 0;
        var forwardIndex = 0;
        var lastFileIndex = files.Count - 1;
        var spaceIndex = 0;
        var currentEndFileSize = files[lastFileIndex];
        
        var isForward = true;
        while (index < totalFileSize)
        {
            if (isForward)
            {
                for (var j = 0; j < files[forwardIndex]; j++)
                {
                    if (index < totalFileSize)
                    {
                        tally += index++ * forwardIndex;
                    }
                }

                forwardIndex++;
                isForward = false;
            }
            else
            {
                var currentSpace = space[spaceIndex++];
                
                while (currentSpace-- > 0 && index < totalFileSize)
                {
                    tally += index++ * lastFileIndex;
                    
                    if (--currentEndFileSize <= 0)
                    {
                        currentEndFileSize = files[--lastFileIndex];
                    }
                }

                isForward = true;
            }
        }
        
        return tally;
    }
    
    private static long PartTwo(List<int> files, List<int> space)
    {
        List<(int size, int id)> filesInfo = [];
        filesInfo.AddRange(files.Select((size, id) => (size, id)));
        space.Add(0);

        var endIndex = filesInfo.Count - 1;
        while (endIndex > 0)
        {
            var hasUpdated = false;
            var info = filesInfo[endIndex];
            for (var j = 0; j < endIndex; j++)
            {
                if (space[j] >= info.size)
                {
                    space[endIndex - 1] += info.size + space[endIndex];
                    space.RemoveAt(endIndex);
                    space[j] -= info.size;
                    space.Insert(j,  0);
                    
                    filesInfo.RemoveAt(endIndex);
                    filesInfo.Insert(j + 1, info);
                    hasUpdated = true;
                    break;
                }
            }

            if (!hasUpdated)
                endIndex--;
        }

        return CalcChecksum(filesInfo, space);
    }
    
    private static long CalcChecksum(List<(int size, int id)> filesInfo, List<int> space)
    {
        var index = 0;
        long tally = 0;
        for (var i = 0; i < filesInfo.Count; i++)
        {
            for (var j = 0; j < filesInfo[i].size; j++)
            {
                tally += filesInfo[i].id * index++;
            }

            index += space[i];
        }

        return tally;
    }
}