import sys
from functools import cache

path_part_1 = (
    r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input_test1.txt"
    if "t" in sys.argv
    else r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input.txt"
)

path_part_2 = (
    r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input_test2.txt"
    if "t" in sys.argv
    else r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input.txt"
)

# Part 1
with open(path_part_1) as file:
    lines = [line.split(":") for line in file.readlines()]
    connections = {split[0]: split[1].split() for split in lines}

    stack = connections["you"]
    ways_out = 0

    while stack:
        nxt = connections[stack.pop()]
        if "out" in nxt:
            ways_out += 1
        else:
            stack.extend(nxt)

    print(ways_out)  # 5 477

# Part 2
with open(path_part_2) as file:
    lines = [line.split(":") for line in file.readlines()]
    connections = {split[0]: split[1].split() for split in lines}

    @cache
    def find_ways(f, t):
        if f == t:
            return 1
        else:
            return sum(find_ways(c, t) for c in connections.get(f, []))

    svr_dac = find_ways("svr", "dac")
    dac_fft = find_ways("dac", "fft")
    fft_out = find_ways("fft", "out")
    svr_fft = find_ways("svr", "fft")
    fft_dac = find_ways("fft", "dac")
    dac_out = find_ways("dac", "out")

    print(svr_dac * dac_fft * fft_out + svr_fft * fft_dac * dac_out)
    # 2 383307150903216