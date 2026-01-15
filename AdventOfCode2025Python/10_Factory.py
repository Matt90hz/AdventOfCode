import sys

path = (
    r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day10\input_test.txt"
    if "t" in sys.argv
    else r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day10\input.txt"
)

with open(path) as file:
    machines = [
        (
            [
                int(c)
                for c in line[line.index("[") : line.index("]")]
                .strip("[]")
                .replace(".", "0")
                .replace("#", "1")
            ],
            [
                [int(c) for c in button.strip("()").split(",")]
                for button in line[line.index("(") : line.rindex(")")].split()
            ],
        )
        for line in file.readlines()
    ]

    def combine(items, times, cache):
        if times in cache:
            return cache[times]
        elif times < 1 or len(items) == 0:
            return []
        elif times == 1:
            return [[item] for item in items]
        elif len(items) == 1:
            return [[items[0]] for _ in range(times)]
        else:
            combos = combine(items, times - 1, cache)
            cache[times] = combos
            return [[item] + combo for item in items for combo in combos]

    tot = 0
    for diagram, buttons in machines:
        presses = 0
        found = False
        while not found:
            presses += 1
            for combo in combine(buttons, presses, {}):
                state = [0 for _ in diagram]

                for button in combo:
                    for switch in button:
                        state[switch] ^= 1

                if state == diagram:
                    tot += presses
                    found = True
                    break

    print(tot)  # 7 419
