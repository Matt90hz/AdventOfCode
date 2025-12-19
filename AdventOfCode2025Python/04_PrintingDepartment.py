path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day04\input_test.txt"
path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day04\input.txt"

with open(path) as text:
    department = [[char for char in line.strip()] for line in text.readlines()]
    movable = 0
    for x in range(len(department)):
        for y in range(len(department[x])):
            if department[x][y] != '@': continue
            ac = 0
            for dx in [-1, 0, 1]:
                ax = x + dx
                if ax < 0 or ax >= len(department): continue
                for dy in [-1, 0, 1]:
                    ay = y + dy
                    if ay < 0 or ay >= len(department): continue
                    if department[ax][ay] == '@' : ac += 1
            if ac <= 4:
                movable += 1
    print(movable) #13 1437

with open(path) as text:
    department = [[char for char in line.strip()] for line in text.readlines()]
    movable = 0
    keep_going = True
    while keep_going:
        keep_going = False
        for x in range(len(department)):
            for y in range(len(department[x])):
                if department[x][y] != '@': continue
                ac = 0
                for dx in [-1, 0, 1]:
                    ax = x + dx
                    if ax < 0 or ax >= len(department): continue
                    for dy in [-1, 0, 1]:
                        ay = y + dy
                        if ay < 0 or ay >= len(department): continue
                        if department[ax][ay] == '@' : ac += 1
                if ac <= 4:
                    department[x][y] = 'x'
                    movable += 1
                    keep_going = True
    print(movable) #43 8765