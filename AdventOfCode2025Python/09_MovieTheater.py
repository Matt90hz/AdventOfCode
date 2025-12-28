path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day09\input.txt"

with open(path) as file:
    points = [list(map(int, line.split(","))) for line in file]
    areas = (
        (abs(x1 - x2) + 1) * (abs(y1 - y2) + 1)
        for i, (x1, y1) in enumerate(points)
        for x2, y2 in points[:i]
    )

    print(max(areas))  # 50 4749672288