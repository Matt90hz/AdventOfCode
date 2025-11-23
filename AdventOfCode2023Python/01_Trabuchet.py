input_path = r'D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day1\Input.txt'

with open(input_path) as file:
    def first_digit(line) : return next(c for c in line if c.isdigit())
    def last_digit(line)  : return first_digit(reversed(line))
    result = sum(int(first_digit(line) + last_digit(line)) for line in file)
    result == 54951