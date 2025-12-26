path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day08\input_test.txt"
path = r"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day08\input.txt"


def dist(a, b):
    ax, ay, az = a
    bx, by, bz = b
    return (ax - bx) ** 2 + (ay - by) ** 2 + (az - bz) ** 2


with open(path) as file:
    coordinates = [
        (int(x), int(y), int(z))
        for [x, y, z] in map(lambda x: x.strip().split(","), file.readlines())
    ]

    coo_by_dist = [
        (coordinates[i], coordinates[j])
        for i in range(len(coordinates) - 1)
        for j in range(i + 1, len(coordinates))
    ]

    coo_by_dist.sort(key=lambda x: dist(x[0], x[1]))

    circuits = [{coo} for coo in coordinates]

    for a, b in coo_by_dist[:1000]:
        a_cir = next(cir for cir in circuits if a in cir)
        b_cir = next(cir for cir in circuits if b in cir)

        if a_cir != b_cir:
            circuits.remove(a_cir)
            circuits.remove(b_cir)
            circuits.append(a_cir | b_cir)

    circuits.sort(key=len, reverse=True)

    print(len(circuits[0]) * len(circuits[1]) * len(circuits[2]))  # 80446