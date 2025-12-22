
path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day06\input_test.txt"
path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day06\input.txt"

with open(path) as worksheet:
    worksheet = [line.strip().split() for line in worksheet.readlines()]
    total = 0
    for i in range(len(worksheet[0])):
        op = worksheet[-1][i]
        partial = 0 if op == "+" else 1
        for num in worksheet[:-1]:
            if op == "+":
                partial += int(num[i])
            else:
                partial *= int(num[i])
        total += partial

    print(total)  # 4277556 6172481852142


def prd(iterable):
    p = 1
    for x in iterable:
        p *= x
    return p


with open(path) as worksheet:
    lines = worksheet.readlines()
    partial = []
    total = 0

    for i in range(len(lines[0]) - 2, -1, -1):
        num = ""

        for line in lines[:-1]:
            num += line[i]

        num = num.strip()

        if not num.isnumeric():
            partial.clear()
            continue

        partial.append(num)

        op = lines[-1][i]

        if op != " ":
            total += eval(op.join(partial))

    print(total)  # 3263827 10188206723429