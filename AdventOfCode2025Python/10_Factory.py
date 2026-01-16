import sys, z3

path = (
    r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day10\input_test.txt"
    if "t" in sys.argv
    else r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day10\input.txt"
)

# part 1
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
            cache[times] = [[item] + combo for item in items for combo in combos]
            return cache[times]

    tot = 0
    for diagram, buttons in machines:
        presses = 0
        found = False
        cache = {}
        while not found:
            presses += 1

            for combo in combine(buttons, presses, cache):
                state = [0 for _ in diagram]

                for button in combo:
                    for switch in button:
                        state[switch] ^= 1

                if state == diagram:
                    tot += presses
                    found = True
                    break

    print(tot)  # 7 419

# part 2
with open(path) as file:
    machines = [
        (
            [int(j) for j in ln[ln.index("{") : ln.index("}")].strip("{}").split(",")],
            [
                [int(c) for c in button.strip("()").split(",")]
                for button in ln[ln.index("(") : ln.rindex(")")].split()
            ],
        )
        for ln in file.readlines()
    ]

    tot = 0

    for joltage, buttons in machines:
        ctx = z3.Optimize()
        buttons_symbols = z3.Ints(map(str, buttons))

        for bs in buttons_symbols:
            ctx.add(bs >= 0)

        for i, j in enumerate(joltage):
            buttons_to_press = filter(lambda j: i in buttons[j], range(len(buttons)))
            symbols_to_press = map(lambda j: buttons_symbols[j], buttons_to_press)
            ctx.add(sum(symbols_to_press) == j)

        ctx.minimize(sum(buttons_symbols))
        ctx.check()
        tot += ctx.model().eval(sum(buttons_symbols)).as_long()

    print(tot)  # 33 18369