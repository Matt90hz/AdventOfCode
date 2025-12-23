path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day07\input_test.txt"
path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day07\input.txt"

with open(path) as file:
    diagram = list(map(lambda line: list(line.strip()), file.readlines()))
    split = 0
    for i in range(len(diagram) - 1):
        for j, x in enumerate(diagram[i]):
            if x == "|" and diagram[i + 1][j] == "^":
                diagram[i + 1][j - 1] = "|"
                diagram[i + 1][j + 1] = "|"
                split += 1
            elif x in ["S", "|"]:
                diagram[i + 1][j] = "|"

    #list(map(print, diagram))
    print(split)  # 21 1590

with open(path) as file:
    diagram = [
        list(map(lambda c: 0 if c == "." else c, line.strip()))
        for line in file.readlines()
    ]

    for i in range(len(diagram) - 1):
        for j, x in enumerate(diagram[i]):
            if type(x) == int and diagram[i + 1][j] == "^":
                diagram[i + 1][j - 1] += x
                diagram[i + 1][j + 1] += x
            elif type(x) == int:
                diagram[i + 1][j] += x
            elif x == 'S':
                diagram[i + 1][j] += 1

    #list(map(print, diagram))
    print(sum(filter(lambda x: type(x) == int, diagram[-1])))
    # 40 20571740188555