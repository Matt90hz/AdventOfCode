path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day05\input_test.txt'
path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day05\input.txt'

with open(path) as file:
    ranges = []
    ids = []
    for line in map(str.strip, file.readlines()):
        if '-' in line:
            s, e = line.split('-')
            ranges.append((int(s), int(e)))
        elif len(line) > 0:
            ids.append(int(line))

    fresh = 0
    for i in ids:
        for s, e in ranges:
            if i >= s and i <= e:
                fresh += 1
                break

    print(fresh) # 3 513

with open(path) as file:
    ranges = []
    for line in map(str.strip, file.readlines()):
        if '-' not in line: break
        s, e = line.split('-')
        ranges.append((int(s), int(e)))

    fresh = 0
    while(ranges):
        (s, e), rest = ranges[0], ranges[1:]

        for ss, ee in rest:
            if e < ss - 1 or s > ee + 1: continue
            
            ranges.remove((s, e))
            ranges.remove((ss, ee))
            ranges.append((min(s, ss), max(e, ee)))
            break
        else:
            ranges.remove((s, e))
            fresh += e - s + 1

    print(fresh) # 14 339668510830757
