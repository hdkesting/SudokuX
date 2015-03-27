using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuX.Solver.Grids;
using SudokuX.Solver.Strategies;
using SudokuX.Solver.Support;

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

            Assert.AreEqual(Validity.Maybe, grid.IsChallengeDone());

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

            ISolver solver = new NakedSingle();

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

            ISolver solver = new HiddenSingle();

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

            ISolver solver = new HiddenSingle();

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

            ISolver solver = new NakedDouble();

            var list = solver.ProcessGrid(grid).ToList();

            // first real naked double found is r0c4/r1c3
            // conclusions are r0c3 not 1,4, r0c5 not 4
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(_listsAreEqual(list[0].ExcludedValues, new[] { 1, 4 }));
            Assert.IsTrue(_listsAreEqual(list[1].ExcludedValues, new[] { 4 }));

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

            ISolver solver = new LockedCandidates();

            var list = solver.ProcessGrid(grid).ToList();

            // value 2 in c0 r12 (b0)
            // conclusions are c1, b0
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(_listsAreEqual(list[0].ExcludedValues, new[] { 2 }));
            Assert.IsTrue(_listsAreEqual(list[1].ExcludedValues, new[] { 2 }));

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

            var solver = new Strategies.Solver(grid,
                new ISolver[] { new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.IsChallengeDone());
            solver.ProcessSolvers();
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
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

            var solver = new Strategies.Solver(grid,
                new ISolver[] { new BasicRule(), new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.IsChallengeDone());
            solver.ProcessSolvers();
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
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

            var solver = new Strategies.Solver(grid,
                new ISolver[] { new BasicRule(), new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.IsChallengeDone());
            solver.ProcessSolvers();
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
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

            var solver = new Strategies.Solver(grid,
                new ISolver[] { new BasicRule(), new NakedSingle(), new HiddenSingle(), new NakedDouble(), new HiddenDouble() });

            Assert.AreEqual(Validity.Maybe, grid.IsChallengeDone());
            solver.ProcessSolvers();
            DumpGrid(grid);
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
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
