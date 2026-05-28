# Solve Christmas Tree Farm in a legit way

## The problem
I have a set of lists of numbers. I want to know if exists at least one way to rearrange the items in the lists so that for every list, the items in a given position are unique among the all items in the same position in the other lists. At the same time all items in all lists must not exceed the given constraint. The lists can transform only by a give set of transformers.

## Example
Given lists `[1, 2, 3]`, `[1, 2, 4]`, `[1, 3, 4]` with constraint 5:
- Position 0: all have 1 (not unique) ❌
- Position 1: 2, 2, 3 (not unique) ❌
- Position 2: 3, 4, 4 (not unique) ❌

After rearranging to `[2, 1, 3]`, `[1, 3, 2]`, `[3, 4, 1]`:
- Position 0: 2, 1, 3 (unique) ✓
- Position 1: 1, 3, 4 (unique) ✓
- Position 2: 3, 2, 1 (unique) ✓
- All values ≤ 5 ✓

## Constraints
- Each list can only be rearranged using the provided set of transformers
- All values must respect the given numeric constraint
- Must find at least one valid arrangement or determine none exists

