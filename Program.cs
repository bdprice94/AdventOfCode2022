using Trace = System.Diagnostics.Trace;

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
        for (int i = 0, offset = 0; i < 9; i++, offset += 4) { // For each stack
            stacks[i] = new Stack<char>(20);
            for (int j = 8; j >= 0; j--) { // for each possible item in stack (max stack items = 8)
                if (input[j][offset] == '[') {
                    stacks[i].Push(input[j][offset+1]);
                }
            } // end foreach item
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
            var tempStack = new Stack<char>(instruction[0]);
            for (int i = 0; i < instruction[0]; i++) {
                tempStack.Push(stacks[instruction[1]-1].Pop());
            }
            for (int i = 0; i < instruction[0]; i++) {
                stacks[instruction[2]-1].Push(tempStack.Pop());
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

public class DaySix {
    public static readonly int LENGTH = 14;
    public static int recurseFindMessage(string s) {
        if (s.Length == 1) {
            return 0;
        }
        for (int i = s.Length-2; i >= 0; i--) {
            if (s[i] == s.Last()) {
                return i+1; // don't know if this results in an off-by-one error somewhere butttt we got the right answer
            }
        }
        return recurseFindMessage(s.Substring(0,s.Length-1));
    }
    public static void Main() {
        var input = Utils.GetInput("06.txt")[0]; // just getting one line
        for (int i = LENGTH; i < input.Length; i++) {
            var substr = input.Substring(i-LENGTH, LENGTH);
            var idx = recurseFindMessage(substr);
            if (idx == 0) {
                Console.WriteLine(i);
                return;
            }
            i += idx-1;
        }
        
        Console.WriteLine("Not supposed to get here!");
    }

}

public class DaySeven {

    public interface File {
        public string Name();
        public int Size();
    }

    public class PlainFile : File {
        private readonly int _size;
        private readonly string _name;

        public PlainFile(string name, int size) {
            _size = size;
            _name = name;
        }
        
        public string Name() {return _name;}
        public int Size() {return _size;}
    }

    public class Directory : File {
        public readonly List<File> Files = new List<File>();
        private readonly string _name;
        public Directory(string name, Directory? parent) {
            _name = name;
            Parent = parent ?? this;
        }
        
        public void AddFile(File f) {
            Files.Add(f);
        }
        public string Name() {return _name;}
        
        public int Size() {
            return Files.Aggregate(0, (total, file) => total + file.Size());
        }
        
        public Directory Parent {get;}
    }

    public static void Main() {
        var input = Utils.GetInput("07.txt");
        
        // First, we build our system directory
        var root = new Directory("/", null);
        var directories = new List<Directory>();
        directories.Add(root);
        var cd = root;
        for (int i = 1; i < input.Length; i++) {
            var line = input[i];
            var command = line.Split(" ");
            switch (command[1]) {
                case "cd":
                    if (command[2] == "..") {
                        cd = cd.Parent;
                    }
                    else {
                        foreach (var file in cd.Files) {
                            if (file.Name() == command[2]) {
                                cd = (Directory) file; // could break but ehhhh garbage in...
                                break;
                            }
                        }
                    }
                    break;
                case "ls":
                    // ugh brain melted this so ugly sry
                    i++;
                    while (i < input.Length && input[i][0] != '$') {
                        line = input[i];
                        var details = line.Split(" ");
                        if (details[0] == "dir") {
                            var dir = new Directory(details[1], cd);
                            cd.AddFile(dir);
                            directories.Add(dir);
                        }
                        else {
                            var fil = new PlainFile(details[1], Int32.Parse(details[0]));
                            cd.AddFile(fil);
                        }
                        i++;
                    }
                    i--;
                    break;
            }
        }
        
        // Now we find which to delete
        int totalSpace = 70000000;
        int spaceNeeded = 30000000;
        int smallest = root.Size();
        foreach (var dir in directories) {
            var size = dir.Size();
            if (size < smallest && (totalSpace - root.Size()) + size >= spaceNeeded) {
                smallest = size;
            }
        }
        
        Console.WriteLine(smallest);
    }
}

public class DayEight {
    public const int DIM = 99;
    public static void Main() {
        var input = Utils.GetInput("08.txt");
        var forest = new int[DIM][];
        for (int i = 0; i < DIM; i++) {
            forest[i] = new int[DIM];
            for (int j = 0; j < DIM; j++) {
                forest[i][j] = (int)input[i][j] - (int)'0';
            }
        }
        
        var scores = new int[DIM][][];
        for (int i = 0; i < DIM; i++) {
            scores[i] = new int[DIM][];
            for (int j = 0; j < DIM; j++) {
                scores[i][j] = new int[4];
            }
        }
        const int WEST  = 0;
        const int EAST  = 1;
        const int NORTH = 2;
        const int SOUTH = 3;
        for (int i = 0; i < DIM; i++) {
            for (int j = 0; j < DIM; j++) {
                bool northBlocked = false;
                bool southBlocked = false;
                bool eastBlocked = false;
                bool westBlocked = false;
                for (int k = j-1
                        ;k >= 0 && !(northBlocked && southBlocked && eastBlocked && westBlocked )
                        ;k--) {
                    if (!eastBlocked && forest[i][k] >= forest[i][j]) {
                        eastBlocked = true;
                        scores[i][j][EAST]++;
                    }
                    else {
                        scores[i][j][EAST]++;
                    }
                    if (!westBlocked && forest[i][DIM-1-k] >= forest[i][DIM-1-j]) {
                        westBlocked = true;
                        scores[i][DIM-1-j][WEST]++;
                    }
                    else {
                        scores[i][DIM-1-j][WEST]++;
                    }
                    if (!northBlocked && forest[k][i] >= forest[j][i]) {
                        northBlocked = true;
                        scores[j][i][NORTH]++;
                    }
                    else {
                        scores[j][i][NORTH]++;
                    }
                    if (!southBlocked && forest[DIM-1-k][i] >= forest[DIM-1-j][i]) {
                        southBlocked = true;
                        scores[DIM-1-j][i][SOUTH]++;
                    }
                    else {
                        scores[DIM-1-j][i][SOUTH]++;
                    }
                }
            }
        }
        
        int highest = 0;
        foreach (var row in scores) {
            foreach (var cell in row) {
                int score = cell[NORTH] * cell[SOUTH] * cell[EAST] * cell[WEST];
                if (highest < score) {
                    highest = score;
                }
            }
        }
        
        Console.WriteLine(highest);

    }
}

public class DayNine {
    public  const int UP = 0;
    public  const int DOWN = 1;
    public  const int LEFT = 2;
    public  const int RIGHT = 3;
    public  const int DIR = 0;
    public  const int AMN = 1;
    public  const int NEWHEAD = 0;
    public  const int NEWTAIL = 1;
    public  const int x = 0;
    public  const int y = 1;
    public static Point upIter = new Point(0, 1);
    public static Point downIter = new Point(0, -1);
    public static Point leftIter = new Point(-1, 0);
    public static Point rightIter = new Point(1, 0);
    public class Point {
        public int x {get;}
        public int y{get;}
        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }
        public static Point operator +(Point a) => a;
        public static Point operator -(Point a) => new Point(-a.x, -a.y);
        public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y-b.y);
        public static bool operator ==(Point a, Point b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Point a, Point b) => a.x != b.x || a.y != b.y;
        public override bool Equals(Object? o) {
            if (o == null || !this.GetType().Equals(o.GetType())) {
                return false;
            }
            return this == (Point)o;
        }
        public override int GetHashCode() {
            return HashCode.Combine(x, y);
        }
        public int largestDifference() => Math.Abs(x) > Math.Abs(y) ? x : y;
    }
    public static void Main() {
        var input = Utils.GetInput("09.txt");
        
        int maxLat = 0;
        int minLat = 0;
        int maxLong = 0;
        int minLong = 0;
        int currentLat = 0;
        int currentLong = 0;
        var instructions = new List<int[]>();
        foreach (var line in input) {
            var instruction = new int[2];
            var split = line.Split(" ");
            var direction = split[0];
            var amount = Int32.Parse(split[1]);
            switch(line[0]) {
                case 'U':
                    instruction[DIR] = UP;
                    instruction[AMN] = amount;
                    currentLat += amount;
                    if (currentLat > maxLat) { maxLat = currentLat; }
                    break;
                case 'D':
                    instruction[DIR] = DOWN;
                    instruction[AMN] = amount;
                    currentLat -= amount;
                    if (currentLat < minLat) { minLat = currentLat; }
                    break;
                case 'L':
                    instruction[DIR] = LEFT;
                    instruction[AMN] = amount;
                    currentLong -= amount;
                    if (currentLong < minLong) { minLong = currentLong; }
                    break;
                case 'R':
                    instruction[DIR] = RIGHT;
                    instruction[AMN] = amount;
                    currentLong += amount;
                    if (currentLong > maxLong) { maxLong = currentLong; }
                    break;
                default:
                    Console.WriteLine("Uhoh, we shoudln't get here! Input: " + line[0]);
                    break;
            }
            
            instructions.Add(instruction);
        }
        
        int height = maxLat - minLat + 1;
        int width  = maxLong - minLong + 1;
        
        var traveled = new bool[height][];
        for (int i = 0; i < height; i++) {
            traveled[i] = new bool[width];
        }
        
        var startX = Math.Abs(minLong);
        var startY = Math.Abs(minLat);
        var head = new Point(startX, startY);
        var tail = new Point(startX, startY);
        traveled[startY][startX] = true;
        int totalNew = 1;
        foreach (var instr in instructions) {
            Point destPoint;
            Point iter;
            switch (instr[DIR]) {
                case UP:
                    destPoint = new Point(head.x, head.y + instr[AMN]);
                    iter = upIter;
                    break;
                case DOWN:
                    destPoint = new Point(head.x, head.y - instr[AMN]);
                    iter = downIter;
                    break;
                case LEFT:
                    destPoint = new Point(head.x - instr[AMN], head.y);
                    iter = leftIter;
                    break;
                case RIGHT:
                    destPoint = new Point(head.x + instr[AMN], head.y);
                    iter = rightIter;
                    break;
                default:
                    iter = new Point(0, 0);
                    destPoint = new Point(0, 0);
                    Console.WriteLine("Error: Shouldn't be here. Instruction: " + instr[DIR]);
                    break;
            }
            
            while (head != destPoint) {
                head += iter;                
                var diff = head - tail;
                if (Math.Abs(diff.y) > 1 || Math.Abs(diff.x) > 1) {
                    tail = head - iter;
                    if (!traveled[tail.y][tail.x]) { totalNew++; }
                    traveled[tail.y][tail.x] = true;
                }
            }
        }
        Console.WriteLine(totalNew);
    }
}

}