namespace _17;

internal struct State : IEquatable<State>
{
    public State(int opcode, int operand, int[] registers)
    {
        Opcode = opcode;
        Operand = operand;
        RegisterA = registers[0];
        RegisterB = registers[1];
        RegisterC = registers[2];
    }

    public int RegisterA { get; init; }
    public int RegisterB { get; set; }
    public int RegisterC { get; set; }
    public int Opcode { get; set; }
    public int Operand { get; set; }

    public bool Equals(State other)
    {
        return RegisterA == other.RegisterA 
               && RegisterB == other.RegisterB 
               && RegisterC == other.RegisterC 
               && Opcode == other.Opcode 
               && Operand == other.Operand;
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RegisterA, RegisterB, RegisterC, Opcode, Operand);
    }
}