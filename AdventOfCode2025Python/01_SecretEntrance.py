# input_path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day01\input_test.txt'
input_path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day01\input.txt'

with open(input_path, 'r') as moves:

    position = 50
    password = 0

    for move in moves:

        steps = int(move[1:])

        if move[0] == 'R':
            position = (position + steps) % 100
        else:
            position = (position - steps) % 100 
        
        print(position)

        if position == 0:
            password = password + 1

    print(password) # 982

with open(input_path, 'r') as moves:

    position = 50
    password = 0

    for move in moves:

        steps = int(move[1:])

        print(move[:-1])

        if move[0] == 'R':     

            div, mod = divmod(steps, 100)
            password += div

            if position + mod >= 100:
                password += 1

            position = (position + steps) % 100

        else:

            div, mod = divmod(-steps, -100)
            password += div

            if position != 0 and position + mod <= 0:
                password += 1
            
            position = (position - steps) % 100 

        print(f'{position}: {password}')


    print(password) # 6106
