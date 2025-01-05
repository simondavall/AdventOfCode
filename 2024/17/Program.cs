using System.Diagnostics;

namespace _17;

internal static class Program
{
    private static long _registerA;
    private static long _registerB;
    private static long _registerC;
    private static int _instrPtr;
    
    internal static void Main()
    {
        var input = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        var registers = input[0].Split('\n');
        var initialRegisters = new long[3];
        for (var i = 0; i < registers.Length; i++)
        {
            initialRegisters[i] = long.Parse(registers[i].Split(":")[1]);
        }

        var program = input[1].Split(' ')[1].Split(',').ToIntArray();
        
        Console.WriteLine($"Part 1: {PartOne(program, initialRegisters)}");
        Console.WriteLine($"Part 2: {PartTwo(program)}");
    }

    private static string PartOne(int[] program, long[] initReg)
    {
        SetRegisters(a: initReg[0], b: initReg[1], c: initReg[2]);
        var result = Execute(program);
        return string.Join(',', result);
    }
    
    private static string PartTwo(int[] program)
    {
        SetRegisters(a: 1, b: 0, c: 0);

        var result = ReverseExecute([..program], 0);
        
        return string.Join(',', result);
    }
    
    private static int[] Execute(int[] program)
    {
        List<int> output = [];
        while (_instrPtr < program.Length)
        {
            var opcode = program[_instrPtr++];
            var operand = program[_instrPtr++];
            
            var result = ExecuteOpcode(opcode, operand);
            
            if (result > -1)
                output.Add(result);
        }
        
        return output.ToArray();
    }
    
    private static long ReverseExecute(int[] program, long output)
    {
        if (program.Length == 0)
            return output;
        
        var lastIndex = program.Length - 1;
        
        foreach (var t in Enumerable.Range(0, 8))
        {
            _registerA = (output << 3) + t;
            _registerB = _registerA % 8;
            _registerB ^= 2;
            _registerC = _registerA >> (int)_registerB;
            _registerB ^= _registerC;
            _registerB ^= 3;

            if (_registerB % 8 != program[lastIndex]) 
                continue;
            
            var reducedProgram = program.Take(lastIndex).ToArray();
            var sub = ReverseExecute(reducedProgram, _registerA);
            if (sub == 0)
                continue;
            
            return sub;
        }

        return 0;
    }
    
    private static int ExecuteOpcode(int opcode, int operand)
    {
        var output = -1;
        
        switch (opcode)
        {
            case 0: // adv
                _registerA >>= (int)Combo(operand);
                break;
            case 1: // bxl
                _registerB ^= operand;
                break;
            case 2: // bst
                _registerB = Combo(operand) % 8;
                break;
            case 3: // jnz
                _instrPtr = _registerA == 0 ? _instrPtr : operand;
                break;
            case 4: // bxc
                _registerB ^= _registerC;
                break;
            case 5: // out
                output = (int)(Combo(operand) % 8);
                break;
            case 6: // bdv
                _registerB = _registerA >> (int)Combo(operand);
                break;
            case 7: // cdv
                _registerC = _registerA >> (int)Combo(operand);
                break;
            default:
                throw new UnreachableException($"Invalid opcode: {opcode}");
        }

        return output;
    }
    
    private static void SetRegisters(long a = 0, long b = 0, long c = 0)
    {
        _instrPtr = 0;
        _registerA = a;
        _registerB = b;
        _registerC = c;
    }
    
    private static long Combo(int i)
    {
        return i switch
        {
            0 or 1 or 2 or 3 => i,
            4 => _registerA,
            5 => _registerB,
            6 => _registerC,
            _ => throw new UnreachableException($"Invalid operand: {i}")
        };
    }
    
    private static int[] ToIntArray(this string[] array)
    {
        var intArray = new int[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            intArray[i] = int.Parse(array[i]);
        }

        return intArray;
    }
}