#path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day02\input_test.txt'
path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day02\input.txt'

with open(path, 'r') as file:
    ranges = file.read().split(',')
    ids = (r.split('-') for r in ranges)
    ids = (str(x) for r in ids for x in range(int(r[0]),int(r[1])+1))
    invalid_ids = (int(x) for x in ids if x[:len(x)//2] == x[len(x)//2:])
    print(sum(invalid_ids))#1227775554 18700015741

with open(path, 'r') as file:
    sum_invalids = 0
    for ranges in file.read().split(','):
        ids = ranges.split('-')
        s_id = int(ids[0])
        e_id = int(ids[1]) + 1
        for x in range(s_id, e_id):
            s = str(x)
            mid = len(s) // 2
            if s[:mid] == s[mid:]:
                sum_invalids += x

    print(sum_invalids) #1227775554 18700015741

with open(path, 'r') as file:
    sum_invalids = 0
    for ranges in file.read().split(','):
        ids = ranges.split('-')
        s_id = int(ids[0])
        e_id = int(ids[1]) + 1
        for x in range(s_id, e_id):
            s = str(x)
            for i in range(2, len(s) + 1):
                if len(s) % i == 0 and s[:len(s) // i] * i == s:
                    sum_invalids += x
                    break
            
    print(sum_invalids) # 4174379265 20077272987