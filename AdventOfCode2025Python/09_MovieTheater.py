#path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day09\input_test.txt"
path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day09\input.txt"


def area(coo1, coo2):
    return (abs(coo1[0] - coo2[0]) + 1) * (abs(coo1[1] - coo2[1]) + 1)


with open(path) as file:
    points = [list(map(int, line.split(","))) for line in file]
    areas = (area(coo1, coo2) for i, coo1 in enumerate(points) for coo2 in points[:i])

    print(max(areas))  # 50 4749672288

with open(path) as file:
    points = [list(map(int, line.split(","))) for line in file]

    edges = list(zip(points, points[1:]))
    edges.append((points[-1], points[0]))

    vertEdges = [
        ([x1, y1], [x2, y2])
        for ([x1, y1], [x2, y2]) in edges
        if x1 == x2
    ]

    areas = [
        (points[i], points[j], area(points[i], points[j]))
        for i in range(len(points) - 1)
        for j in range(i + 1, len(points))
    ]
    areas.sort(key=lambda corner: corner[2], reverse=True)

    max_area = 0

    for [x1, y1], [x2, y2], a in areas:
        xmin = min(x1, x2) + 1
        ymin = min(y1, y2) + 1
        xmax = max(x1, x2) - 1
        ymax = max(y1, y2) - 1

        edgeCrossed = 0

        for [ex1, ey1], [_, ey2] in vertEdges:
            if xmin > ex1 and min(ey1, ey2) < ymin and max(ey1, ey2) > ymin:
                edgeCrossed += 1

        isOutside = edgeCrossed % 2 == 0

        if isOutside:
            continue

        isCrossed = False

        for [ex1, ey1], [ex2, ey2] in edges:

            if ex1 == ex2:
                eymin = min(ey1, ey2)
                eymax = max(ey1, ey2)

                is_inside = xmin < ex1 and xmax > ex1
                cross_top = eymax >= ymax and eymin <= ymax
                cross_low = eymin <= ymin and eymax >= ymin

                if is_inside and (cross_top or cross_low):
                    isCrossed = True
                    break

            else:
                exmin = min(ex1, ex2)
                exmax = max(ex1, ex2)

                is_inside = ymin < ey1 and ymax > ey1
                cross_rgt = exmax >= xmax and exmin <= xmax
                cross_lft = exmin <= xmin and exmax >= xmin

                if is_inside and (cross_lft or cross_rgt):
                    isCrossed = True
                    break

        if isCrossed: continue

        max_area = a
        break

    print(max_area) # 24 1479665889