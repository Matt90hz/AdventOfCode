using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day06;
static class TuningTrouble
{
    public static int GetFirstMarkerPosition(string input) => input
        .TakeWhile((_, i) => input[i..(i + 4)].Distinct().Count() != 4)
        .Count() + 4;

    public static int GetFirstMessageMarkerPosition(string input) => input
        .TakeWhile((_, i) => input[i..(i + 14)].Distinct().Count() != 14)
        .Count() + 14;
}
