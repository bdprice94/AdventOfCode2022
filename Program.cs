namespace AdventOfCode2022 {


public class Utils {
    public static string[] GetInput(String s) {
        string CD = AppDomain.CurrentDomain.BaseDirectory;
        string inputFile = System.IO.Path.Combine(CD, @"..\..\..\inputs\" + s);
        return System.IO.File.ReadAllLines(inputFile);
    }
}

public class DayOne {
    public class Elf {
        public Elf(List<int> foods) {
            Food = new List<int>(foods);
            TotalCalories = Food.Sum();
        }
        
        public List<int> Food {get;}

        public int TotalCalories {get;}
        
        public int HighestCaloricFood() {
            return Food.Aggregate((x, y) => x > y ? x : y);
        }
        
        public int FoodCount() {
            return Food.Count;
        }
    }
    public static void Main() {

        // grab our input file contents
        string[] lines = Utils.GetInput("01.txt");
        var elves = new List<Elf>();
        var foods = new List<int>();

        // parse input file into our data structures
        foreach (var line in lines) {
            if (line.CompareTo("") == 0) {
                elves.Add(new Elf(foods));
                foods.Clear();
            }
            else {
                foods.Add(Int32.Parse(line));
            }
        }
        
        Elf largestElf = elves.Aggregate((x, y) => x.TotalCalories > y.TotalCalories ? x : y);

        Console.WriteLine("Top largest elf: " + largestElf.TotalCalories);        
        
        var topElves = new PriorityQueue<Elf, int>(4); // default PQ is a minimum PQ which is what we want
        var iter = elves.GetEnumerator();

        for (int i = 0; i < 3; i++) {
            iter.MoveNext();
            topElves.Enqueue(iter.Current, iter.Current.TotalCalories);
        }
        
        while (iter.MoveNext()) {
            if (iter.Current.TotalCalories > topElves.Peek().TotalCalories) {
                topElves.EnqueueDequeue(iter.Current, iter.Current.TotalCalories);
            }
        } 
        
        int total = 0;
        for (int i = 0; i < 3; i++) {
            total += topElves.Dequeue().TotalCalories;
        }
        Console.WriteLine("Top 3 Largest Elves: " + total);
    }
}

public class DayTwo {
    public const int LOST = 0;
    public const int DRAW = 3;
    public const int WON = 6;
    public const int ROCK = 1;
    public const int PAPER = 2;
    public const int SCISSORS = 3;
    public static readonly Dictionary<char,int> decoder = new Dictionary<char, int> {
        {'A', -1}, // They Rock
        {'B', 0},  // They Paper
        {'C', 1},  // They Scissors
        {'X', 1},  // You Lose
        {'Y', 4},  // You Draw
        {'Z', 7}   // You Win
    };
    
    public static readonly Dictionary<char,int> points = new Dictionary<char,int> {
        {'X', 0},
        {'Y', 3},
        {'Z', 6}
    };
    
    public static readonly int[] results = {DRAW, LOST, WON, WON, DRAW, LOST, LOST, WON, DRAW};
    public static readonly int[] yourPlay = {SCISSORS, ROCK, PAPER, ROCK, PAPER, SCISSORS, PAPER, SCISSORS, ROCK};
    
    public static int CalcPoints(char them, char you) {
        return points[you] + yourPlay[decoder[them] + decoder[you]];
    }
    
    public static void Main() {

        var instructions = new List<Tuple<char, char>>();
        var inputs = Utils.GetInput("02.txt");
        
        foreach (var line in inputs) {
            instructions.Add(Tuple.Create(line[0], line[2]));            
        }

        var score = instructions.Aggregate(0, (int x, Tuple<char,char> y) => x + CalcPoints(y.Item1, y.Item2), (int x) => x);
        
        Console.Write(score);
    }
}

public class DayThree {
    
    public static int CalcPriority(char c) {
        if (Char.IsUpper(c)) {
            return (int)c - (int)'A' + 27;
        }
        else {
            return (int)c - (int)'a' + 1;
        }
    }
    
    public static void Main() {
        var input = Utils.GetInput("03.txt");
        int total = 0;
        for (int i = 0; i < input.Length; i+=3) {
            var elves = new string[3];
            elves[0] = input[i];
            elves[1] = input[i+1];
            elves[2] = input[i+2];
            
            int[] isRepeat = new int[52];
            for (int j = 0; j < 3; j++) {
                bool[] counted = new bool[52];
                for (int k = 0; k < elves[j].Length; k++) {
                    char c = elves[j][k];
                    int type = (Char.IsUpper(c)) ? (int)c - (int)'A'+26 : (int)c - (int)'a';
                    if (!counted[type]) {
                        if (isRepeat[type] == 2) {
                            total += CalcPriority(c);
                            break;
                        }
                        counted[type] = true;
                        isRepeat[type]++;
                    }
                }
            }

        }
        Console.WriteLine(total);
    }
}

public class DayFour{
    
    public static void  Main() {
        var input = Utils.GetInput("04.txt");
        int total = 0;
        foreach (var line in input) {
            var sections = new int[2][];
            var elves = line.Split(",");
            for (int i = 0; i < 2; i++) {
                sections[i] = new int[2];
                var stringSection = elves[i].Split("-");
                for (int j = 0; j < 2; j++) {
                    sections[i][j] = Int32.Parse(stringSection[j]);
                }
            }
            // [0][0] = First elf start
            // [0][1] = First elf end
            // [1][0] = Second elf start
            // [1][1] = Second elf end
            if (!(sections[0][0] > sections[1][1]) && !(sections[0][1] < sections[1][0]))
            {
                total += 1;
            }
        }
    Console.WriteLine(total);
    }
}

public class DayFive {

    public static void Main() {
        var input = Utils.GetInput("05.txt");
        // This input parsing is going to be very hacky. Sorry!
        // parse stack contents
        var stacks = new Stack<char>[9];
        int offset = 0;
        for (int i = 0; i < 9; i++) { // For each stack
            stacks[i] = new Stack<char>(20);
            for (int j = 8; j >= 0; j--) { // for each possible item in stack (max stack items = 8)
                if (input[j][offset] == '[') {
                    stacks[i].Push(input[j][offset+1]);
                }
            } // end foreach item
            offset += 4;
        } // end foreach stack
        
        // parse instructions
        var instructions = new int[input.Length-10][];
        for (int i = 10; i < input.Length; i++) {
            var instruction = new int[3];
            var splitLine = input[i].Split(" ");
            instruction[0] = Int32.Parse(splitLine[1]);
            instruction[1] = Int32.Parse(splitLine[3]);
            instruction[2] = Int32.Parse(splitLine[5]);
            instructions[i-10] = instruction;
        }
        
        // start the crane!
        foreach (var instruction in instructions) {
            for (int i = 0; i < instruction[0]; i++) {
                stacks[instruction[2]-1].Push(stacks[instruction[1]-1].Pop());
            }
        }

        // get our tops!
        string result = "";
        foreach (var stack in stacks) {
            result += stack.Pop();
        }
        Console.Write(result);
    }
}

}