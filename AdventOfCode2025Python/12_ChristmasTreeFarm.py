import sys

path = (
    r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day12\input_test.txt"
    if "t" in sys.argv
    else r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day12\input.txt"
)

with open(path) as input:
    fit_regions = 0
    for line in input.readlines():
        if not "x" in line:
            continue

        [regions, presents] = line.split(":")
        [width, height] = regions.split("x")
        size_region = int(width) * int(height)
        tot_presents = sum(map(int, presents.split()))

        if size_region >= tot_presents * 9:
            fit_regions += 1

    print(fit_regions)  # 505
