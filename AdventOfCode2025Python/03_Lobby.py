path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day03\input_test.txt'
path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day03\input.txt'

with open(path, 'r') as batteries:
    joltage = 0
    for bank in map(str.strip, batteries.readlines()):
        i, x = max(enumerate(bank[:-1]), key=lambda x: int(x[1]))
        y = max(bank[i + 1:], key=int)
        joltage += int(x + y)
    print(joltage)

with open(path, 'r') as batteries:
    def joltage(bank, d):
        if d == 1:
            return max(bank, key=int)
        else:
            i, x = max(enumerate(bank[:-(d - 1)]), key=lambda x: int(x[1]))
            return x + joltage(bank[i + 1:], d - 1)
    
    tot = sum(int(joltage(bank.strip(), 12)) for bank in batteries.readlines())
    print(tot)