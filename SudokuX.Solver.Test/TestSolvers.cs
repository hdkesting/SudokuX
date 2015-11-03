using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuX.Solver.Core;
using SudokuX.Solver.Grids;
using SudokuX.Solver.SolverStrategies;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Test
{
    [TestClass]
    public class TestSolvers
    {
        readonly Func<IList<int>, IList<int>, bool> _listsAreEqual = (input, check) =>
        {
            if (input.Count != check.Count)
                return false;

            return input.All(check.Contains);
        };

        [TestMethod]
        public void TestPlacingValues()
        {
            const string gridstring = @"
+---+---+
|...|...|
|3..|.56|
+---+---+
|63.|.2.|
|.5.|.3.|
+---+---+
|.15|26.|
|2..|..1|
+---+---+";

            var grid = Grid6X6.LoadFromString(gridstring);

            Assert.AreEqual(Validity.Maybe, grid.CalculateValidity());

            var list = grid.GetCellByRowColumn(0, 0).AvailableValues.ToList();

            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(_listsAreEqual(list, new[] { 1, 4, 5 }));
            list = grid.GetCellByRowColumn(4, 0).AvailableValues.ToList();
            Assert.IsTrue(_listsAreEqual(list, new[] { 4 }));
            list = grid.GetCellByRowColumn(5, 4).AvailableValues.ToList();
            Assert.IsTrue(_listsAreEqual(list, new[] { 4 }));
        }



        [TestMethod]
        public void TestNakedSingles6()
        {
            const string gridstring = @"
+---+---+
|...|...|
|3..|.56|
+---+---+
|63.|.2.|
|.5.|.3.|
+---+---+
|.15|26.|
|2..|..1|
+---+---+";

            var grid = Grid6X6.LoadFromString(gridstring);

            ISolverStrategy solver = new NakedSingle();

            var list = solver.ProcessGrid(grid).ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void TestNakedSingles9()
        {
            const string challenge = @" 
 *-----------*
 |.8.|76.|..2|
 |..7|.2.|...|
 |62.|..8|.7.|
 |---+---+---|
 |..4|...|.91|
 |5..|.8.|..6|
 |39.|...|2..|
 |---+---+---|
 |.6.|8..|.54|
 |...|.4.|1..|
 |4..|.52|.8.|
 *-----------*
";
            var grid = Grid9X9.LoadFromString(challenge);

            var solver = new NakedSingle();

            var list = solver.ProcessGrid(grid).ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);

        }

        [TestMethod]
        public void TestHiddenSingles6()
        {
            const string gridstring = @"
    +---+---+
    |...|...|
    |3..|.56|
    +---+---+
    |63.|.2.|
    |.5.|.3.|
    +---+---+
    |.15|26.|
    |2..|..1|
    +---+---+";

            var grid = Grid6X6.LoadFromString(gridstring);

            ISolverStrategy solver = new HiddenSingle();

            var list = solver.ProcessGrid(grid).ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(9, list.Count);
        }

        [TestMethod]
        public void TestHiddenSingles9()
        {
            const string challenge = @" 
 *-----------*
 |98.|76.|.12|
 |147|.2.|.6.|
 |62.|..8|.7.|
 |---+---+---|
 |874|236|591|
 |512|.8.|736|
 |396|...|248|
 |---+---+---|
 |26.|8..|.54|
 |75.|.4.|12.|
 |43.|.52|.8.|
 *-----------*
";
            var grid = Grid9X9.LoadFromString(challenge);

            ISolverStrategy solver = new HiddenSingle();

            var list = solver.ProcessGrid(grid).ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(5, list.Count);

        }


        [TestMethod]
        public void TestNakedDoubles6()
        {
            const string gridstring = @"
    +---+---+
    |...|...|
    |3..|.56|
    +---+---+
    |63.|.2.|
    |.5.|.3.|
    +---+---+
    |.15|26.|
    |2..|..1|
    +---+---+";

            var grid = Grid6X6.LoadFromString(gridstring);

            ISolverStrategy solver = new NakedDouble();

            var list = solver.ProcessGrid(grid).ToList();

            // first real naked double found is r0c4/r1c3
            // conclusions are r0c3 not 1,4, r0c5 not 4
            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(_listsAreEqual(list[0].ExcludedValues, new[] { 1, 4 }));
            Assert.IsTrue(_listsAreEqual(list[1].ExcludedValues, new[] { 4 }));

        }

        [TestMethod]
        public void TestNakedDoubles9()
        {
            const string gridstring = @"
 *-----------*
 |...|18.|6.3|
 |..3|.5.|...|
 |186|.43|75.|
 |---+---+---|
 |.1.|...|9.4|
 |...|...|..6|
 |6.2|...|.3.|
 |---+---+---|
 |.91|4.5|368|
 |...|.61|495|
 |465|839|...|
 *-----------*
";

            var grid = Grid9X9.LoadFromString(gridstring);

            ISolverStrategy solver = new NakedDouble();

            var list = solver.ProcessGrid(grid).ToList();

            // first real naked double found is r3c2/r7c2
            // conclusions are r0c2 not 7, r4c2 not 7,8 (column)
            // and r5c4 not 7, r4c4 not 2,7
            Assert.AreEqual(4, list.Count);
            Assert.IsTrue(_listsAreEqual(list[0].ExcludedValues, new[] { 7 }));
            Assert.IsTrue(_listsAreEqual(list[1].ExcludedValues, new[] { 7, 8 }));
        }

        [TestMethod]
        public void TestHiddenDoubles9()
        {
            const string gridstring = @"
 *-----------*
 |...|..9|.81|
 |13.|5.8|...|
 |..7|621|3..|
 |---+---+---|
 |...|9.4|72.|
 |...|...|...|
 |.46|1.7|...|
 |---+---+---|
 |..5|.16|4..|
 |...|..3|.76|
 |69.|...|...|
 *-----------*
";

            var grid = Grid9X9.LoadFromString(gridstring);

            ISolverStrategy solver = new HiddenDouble();

            var list = solver.ProcessGrid(grid).ToList();
            Assert.AreEqual(6, list.Count);
        }

        [TestMethod]
        public void TestHiddenTriplet()
        {
            const string challenge = @"
 *-----------*
 |.7.|6..|.3.|
 |..4|3..|9..|
 |32.|..7|56.|
 |---+---+---|
 |.4.|.1.|...|
 |..8|...|7..|
 |...|.3.|.2.|
 |---+---+---|
 |.62|8..|.19|
 |..7|..9|6..|
 |.8.|..3|.7.|
 *-----------*
";

            var grid = Grid9X9.LoadFromString(challenge);

            ISolverStrategy solver = new HiddenTriple();

            var list = solver.ProcessGrid(grid).ToList();
            Assert.AreEqual(2, list.Count);

        }

        [TestMethod]
        public void TestLockedCandidates9()
        {
            const string gridstring = @"

 *-----------*
 |465|3.2|...|
 |...|765|...|
 |.37|8.4|6.5|
 |---+---+---|
 |.78|629|..4|
 |6..|137|..8|
 |3..|548|7.6|
 |---+---+---|
 |7..|951|.63|
 |..6|283|...|
 |..3|476|9.2|
 *-----------*
";

            var grid = Grid9X9.LoadFromString(gridstring);

            ISolverStrategy solver = new LockedCandidates();

            var sw = Stopwatch.StartNew();
            var list = solver.ProcessGrid(grid).ToList();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            // exclude value 8 in r1 c67 (b2) because of b0
            // exclude value 9 in c8 r01 (b2) because of b5
            Assert.AreEqual(5, list.Count);
            Assert.IsTrue(_listsAreEqual(list[0].ExcludedValues, new[] { 8 }));
            Assert.IsTrue(_listsAreEqual(list[1].ExcludedValues, new[] { 8 }));

        }

        [TestMethod]
        public void TestSolveWithColors()
        {
            const string gridstring = @"
 *-----------*
 |.7.|154|32.|
 |.3.|782|5..|
 |5.2|963|.87|
 |---+---+---|
 |...|.79|..8|
 |.9.|541|.7.|
 |7..|.28|9..|
 |---+---+---|
 |42.|8.7|..5|
 |681|435|792|
 |.57|2.6|84.|
 *-----------*
";
            var grid = Grid9X9.LoadFromString(gridstring);
            ISolverStrategy solver = new SolveWithColors();
            var sw = Stopwatch.StartNew();
            var list = solver.ProcessGrid(grid).ToList();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestSolve6X6()
        {
            const string challenge = @"
    +---+---+
    |...|...|
    |3..|.56|
    +---+---+
    |63.|.2.|
    |.5.|.3.|
    +---+---+
    |.15|26.|
    |2..|..1|
    +---+---+";

            var grid = Grid6X6.LoadFromString(challenge);

            var solver = new Core.Solver(grid,
                new ISolverStrategy[] { new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.CalculateValidity());
            var res = solver.ProcessSolvers();
            Trace.WriteLine(res.Score);
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, res.Validity);
        }

        [TestMethod]
        public void TestSolve9X9Easy()
        {
            const string challenge = @"
 *-----------*
 |...|78.|.93|
 |...|...|8..|
 |8..|41.|6..|
 |---+---+---|
 |..3|5..|..1|
 |.6.|2.1|.8.|
 |4..|..8|7..|
 |---+---+---|
 |..2|.47|..6|
 |..6|...|...|
 |37.|.96|...|
 *-----------*
";

            var grid = Grid9X9.LoadFromString(challenge);

            var solver = new Core.Solver(grid,
                new ISolverStrategy[] { new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.CalculateValidity());
            var res = solver.ProcessSolvers();
            Trace.WriteLine(res.Score);
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, res.Validity);
        }

        [TestMethod]
        public void TestSolve9X9Standard()
        {
            const string challenge = @"
 *-----------*
 |..1|..9|..2|
 |.2.|...|6..|
 |..5|34.|.1.|
 |---+---+---|
 |...|6.4|3..|
 |64.|...|.95|
 |..2|9.7|...|
 |---+---+---|
 |.3.|.78|2..|
 |..7|...|.4.|
 |1..|4..|8..|
 *-----------*
";

            var grid = Grid9X9.LoadFromString(challenge);

            var solver = new Core.Solver(grid,
                new ISolverStrategy[] { new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.CalculateValidity());
            var res = solver.ProcessSolvers();
            Trace.WriteLine(res.Score);
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, res.Validity);
        }

        [TestMethod]
        public void TestHard9X9()
        {
            const string challenge = @"
 *-----------*
 |...|...|1.8|
 |.4.|2..|...|
 |...|6..|...|
 |---+---+---|
 |.2.|4..|.3.|
 |7..|.1.|5..|
 |...|...|...|
 |---+---+---|
 |1..|.8.|...|
 |...|...|34.|
 |...|...|.26|
 *-----------*
";

            var grid = Grid9X9.LoadFromString(challenge);

            var solver = new Core.Solver(grid,
                new ISolverStrategy[]
                {
                    new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble(),
                    new NakedTriple(), new LockedCandidates()
                });
            // needs an X(Y)-Wing
            Assert.AreEqual(Validity.Maybe, grid.CalculateValidity());
            var res = solver.ProcessSolvers();
            Trace.WriteLine(res.Score);
            DumpGrid(grid);
            var s = grid.ToStatusString();
            Trace.WriteLine(s);
            //Assert.AreEqual(Validity.Full, grid.IsChallengeDone());

        }

        [TestMethod]
        public void TestSolve16X16()
        {
            const string challenge = @"
.  7  .  .   .  C  .  .   8  .  .  .   5  0  .  D
.  A  C  9   .  .  D  .   .  .  4  .   .  .  .  . 
.  4  .  D   E  7  B  .   2  .  .  F   .  .  .  C 
.  .  B  .   .  .  F  .   .  .  5  0   .  .  E  4 

.  .  5  .   B  .  .  .   A  .  .  .   .  .  .  F 
.  F  E  .   .  .  .  .   .  .  6  .   .  8  C  . 
A  C  .  7   .  8  .  .   .  .  .  .   4  .  .  . 
.  8  D  .   .  .  .  .   F  .  C  .   9  A  .  B 

B  D  6  .   .  .  .  .   .  C  .  .   .  E  3  . 
.  .  .  .   D  .  E  C   5  .  .  .   8  .  .  . 
E  .  1  8   .  3  .  .   .  .  .  A   .  .  F  . 
.  .  .  .   .  9  6  .   3  .  8  7   .  1  .  . 

.  .  A  .   .  .  3  8   .  .  0  .   .  7  .  E 
.  .  4  6   .  .  .  5   .  .  3  1   .  .  2  A 
.  0  .  .   .  .  .  9   .  .  F  C   B  6  .  . 
1  5  .  .   F  .  .  6   E  .  2  .   .  C  D  . 
";

            var grid = Grid16X16.LoadFromString(challenge);

            var solver = new Core.Solver(grid,
                new ISolverStrategy[] { new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.CalculateValidity());
            solver.ProcessSolvers();
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, grid.CalculateValidity());
        }

        private static void DumpGrid(ISudokuGrid grid)
        {
            string s = grid.ToString();
            using (var sr = new StringReader(s))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Debug.WriteLine(line);
                }
            }
        }

    }
}
