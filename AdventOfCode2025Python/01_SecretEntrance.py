# input_path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day01\input_test.txt'
input_path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day01\input.txt'

def calculate_password():
    with open(input_path, 'r') as moves:
    
        position = 50
        password = 0
    
        for move in moves:
    
            steps = int(move[1:])
    
            if move[0] == 'R':
                position = (position + steps) % 100
            else:
                position = (position - steps) % 100 
    
            if position == 0:
                password = password + 1
    
        return password # 982

def calculate_password_new_protocol():
    with open(input_path, 'r') as moves:
    
        position = 50
        password = 0
    
        for move in moves:
    
            steps = int(move[1:])
    
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
    
        return password # 6106