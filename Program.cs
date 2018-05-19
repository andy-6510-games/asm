using System;

namespace _6510
{
    class Program
    {
        public static string[,] instLookup = new string[16,16]
        {
        {"BRK impl","ORA X,ind","???",  "???", "???",       "ORA zpg",   "ASL zpg",   "???", "PHP impl", "ORA #",     "ASL A",     "???", "???",       "ORA abs",   "ASL abs",   "???"},
        {"BPL rel", "ORA ind,Y","???",  "???", "???",       "ORA zpg,X", "ASL zpg,X", "???", "CLC impl", "ORA abs,Y", "???",       "???", "???",       "ORA abs,X", "ASL abs,X", "???"},
        {"JSR abs", "AND X,ind","???",  "???", "BIT zpg",   "AND zpg",   "ROL zpg",   "???", "PLP impl", "AND #",     "ROL A",     "???", "BIT abs",   "AND abs",   "ROL abs",   "???"},
        {"BMI rel", "AND ind,Y","???",  "???", "???",       "AND zpg,X", "ROL zpg,X", "???", "SEC impl", "AND abs,Y", "???",       "???", "???",       "AND abs,X", "ROL abs,X", "???"},
        {"RTI impl","EOR X,ind","???",  "???", "???",       "EOR zpg",   "LSR zpg",   "???", "PHA impl", "EOR #",     "LSR A",     "???", "JMP abs",   "EOR abs",   "LSR abs",   "???"},
        {"BVC rel", "EOR ind,Y","???",  "???", "???",       "EOR zpg,X", "LSR zpg,X", "???", "CLI impl", "EOR abs,Y", "???",       "???", "???",       "EOR abs,X", "LSR abs,X", "???"},
        {"RTS impl","ADC X,ind","???",  "???", "???",       "ADC zpg",   "ROR zpg",   "???", "PLA impl", "ADC #",     "ROR A",     "???", "JMP ind",   "ADC abs",   "ROR abs",   "???"},
        {"BVS rel", "ADC ind,Y","???",  "???", "???",       "ADC zpg,X", "ROR zpg,X", "???", "SEI impl", "ADC abs,Y", "???",       "???", "???",       "ADC abs,X", "ROR abs,X", "???"},
        {"???",     "STA X,ind","???",  "???", "STY zpg",   "STA zpg",   "STX zpg",   "???", "DEY impl", "???",       "TXA impl",  "???", "STY abs",   "STA abs",   "STX abs",   "???"},
        {"BCC rel", "STA ind,Y","???",  "???", "STY zpg,X", "STA zpg,X", "STX zpg,Y", "???", "TYA impl", "STA abs,Y", "TXS impl",  "???", "???",       "STA abs,X", "???",       "???"},
        {"LDY #",   "LDA X,ind","LDX #","???", "LDY zpg",   "LDA zpg",   "LDX zpg",   "???", "TAY impl", "LDA #",     "TAX impl",  "???", "LDY abs",   "LDA abs",   "LDX abs",   "???"},
        {"BCS rel", "LDA ind,Y","???",  "???", "LDY zpg,X", "LDA zpg,X", "LDX zpg,Y", "???", "CLV impl", "LDA abs,Y", "TSX impl",  "???", "LDY abs,X", "LDA abs,X", "LDX abs,Y", "???"},
        {"CPY #",   "CMP X,ind","???",  "???", "CPY zpg",   "CMP zpg",   "DEC zpg",   "???", "INY impl", "CMP #",     "DEX impl",  "???", "CPY abs",   "CMP abs",   "DEC abs",   "???"},
        {"BNE rel", "CMP ind,Y","???",  "???", "???",       "CMP zpg,X", "DEC zpg,X", "???", "CLD impl", "CMP abs,Y", "???",       "???", "???",       "CMP abs,X", "DEC abs,X", "???"},
        {"CPX #",   "SBC X,ind","???",  "???", "CPX zpg",   "SBC zpg",   "INC zpg",   "???", "INX impl", "SBC #",     "NOP impl",  "???", "CPX abs",   "SBC abs",   "INC abs",   "???"},
        {"BEQ rel", "SBC ind,Y","???",  "???", "???",       "SBC zpg,X", "INC zpg,X", "???", "SED impl", "SBC abs,Y", "???",       "???", "???",       "SBC abs,X", "INC abs,X", "???"},
        };

        static void Main(string[] args)
        {
            Random rnd = new Random();

            string value1 = string.Empty;
            string value2 = string.Empty;

            int byteCount = 1;

            int i = 0;

            do
            {
                value1 = "   ";
                value2 = "   ";
                byteCount = 1;

                var x = rnd.Next(0, 16);
                var y = rnd.Next(0, 16);

                var address = (i + 4096).ToString("X4");
                var opcode = x.ToString("X") + y.ToString("X");
                var inst = instLookup[x, y];

                if (inst.Contains("???"))
                    continue;

                if (inst.Contains("zpg"))
                {
                    var data1 = rnd.Next(0, 255).ToString("X2");

                    inst = inst.Replace("zpg", data1);

                    value1 = " " + data1;
                    byteCount++;
                }

                if (inst.Contains("#"))
                {
                    var data1 = rnd.Next(0, 255).ToString("X2");

                    inst = inst.Replace("#", "#" + data1);

                    value1 = " " + data1;
                    byteCount++;
                }

                if (inst.Contains("ind"))
                {
                    var data1 = rnd.Next(0, 255).ToString("X2");
                    var data2 = rnd.Next(0, 255).ToString("X2");

                    inst = inst.Replace("ind", "(" + data2 + data1) + ")";

                    value1 = " " + data1;
                    value2 = " " + data2;
                    byteCount += 2;
                }

                if (inst.Contains("rel"))
                {
                    var data1 = rnd.Next(0, 255).ToString("X2");

                    inst = inst.Replace("rel", data1);

                    value1 = " " + data1;
                    byteCount++;
                }

                if (inst.Contains("abs"))
                {
                    var data1 = rnd.Next(0, 255).ToString("X2");
                    var data2 = rnd.Next(0, 255).ToString("X2");

                    inst = inst.Replace("abs", data2 + data1);

                    value1 = " " + data1;
                    value2 = " " + data2;
                    byteCount += 2;
                }

                if (inst.Contains("impl"))
                    inst = inst.Replace("impl", "");

                Console.WriteLine(address + "  " + opcode + value1 + value2 + "   " + inst);

                i += byteCount;
                System.Threading.Thread.Sleep(250);
            }
            while (i <= 61439);
        }
    }
}
