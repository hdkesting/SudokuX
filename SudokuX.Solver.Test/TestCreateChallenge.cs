using System;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Test
{
    [TestClass]
    public class TestCreateChallenge
    {
        [TestMethod]
        public void CreateChallenge4()
        {
            var creator = new ChallengeCreator(BoardSize.Board4);
            creator.CreateChallenge(new Random(9));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
            Debug.WriteLine(((IRegularSudokuGrid)grid).ToChallengeString());

            var testgrid = grid.CloneBoardAsChallenge();
            var solver = new GridSolver(creator.Solvers);
            solver.Solve(testgrid);
            Assert.AreEqual(Validity.Full, solver.Validity);
            Trace.WriteLine("Board score: " + solver.GridScore); // 11
        }

        [TestMethod]
        public void CreateChallenge4_multi()
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= 30; i++)
            {
                Debug.WriteLine("####################### {0} ####################", i);
                var creator = new ChallengeCreator(BoardSize.Board4);
                creator.CreateChallenge(new Random(i));
                var grid = creator.Grid;

                Assert.IsTrue(grid.IsAllKnown());
                Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
                var s = ((IRegularSudokuGrid)grid).ToChallengeString();
                sb.AppendLine(s);

                var testgrid = grid.CloneBoardAsChallenge();
                var solver = new GridSolver(creator.Solvers);
                solver.Solve(testgrid);
                Assert.AreEqual(Validity.Full, solver.Validity);
                sb.AppendLine("Board score: " + solver.GridScore);
            }
            Debug.WriteLine(sb.ToString());
        }


        [TestMethod]
        public void CreateChallenge6()
        {
            var creator = new ChallengeCreator(BoardSize.Board6);
            creator.CreateChallenge(new Random(1));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
            Debug.WriteLine(((IRegularSudokuGrid)grid).ToChallengeString());

            var testgrid = grid.CloneBoardAsChallenge();
            var solver = new GridSolver(creator.Solvers);
            solver.Solve(testgrid);
            Assert.AreEqual(Validity.Full, solver.Validity);
            Trace.WriteLine("Board score: " + solver.GridScore); // 18
        }

        [TestMethod]
        [TestProperty("Time", "Medium")]
        public void CreateChallenge6_multi()
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= 10; i++)
            {
                Debug.WriteLine("####################### {0} ####################", i);
                var creator = new ChallengeCreator(BoardSize.Board6);
                creator.CreateChallenge(new Random(i));
                var grid = creator.Grid;

                Assert.IsTrue(grid.IsAllKnown());
                Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
                var s = ((IRegularSudokuGrid)grid).ToChallengeString();
                sb.AppendLine(s);

                var testgrid = grid.CloneBoardAsChallenge();
                var solver = new GridSolver(creator.Solvers);
                solver.Solve(testgrid);
                Assert.AreEqual(Validity.Full, solver.Validity);
                sb.AppendLine("Board score: " + solver.GridScore);
            }

            Debug.WriteLine(sb.ToString());
        }

        [TestMethod]
        public void CreateChallenge9()
        {
            var creator = new ChallengeCreator(BoardSize.Board9);
            creator.CreateChallenge(new Random(10));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
            Debug.WriteLine(((IRegularSudokuGrid)grid).ToChallengeString());

            var testgrid = grid.CloneBoardAsChallenge();
            var solver = new GridSolver(creator.Solvers);
            solver.Solve(testgrid);
            Assert.AreEqual(Validity.Full, solver.Validity);
            Trace.WriteLine("Board score: " + solver.GridScore); // 49
        }

        [TestMethod]
        [TestProperty("Time", "Long")]
        public void CreateChallenge9_multi()
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= 10; i++)
            {
                var creator = new ChallengeCreator(BoardSize.Board9);
                creator.CreateChallenge(new Random(i));
                var grid = creator.Grid;

                Assert.IsTrue(grid.IsAllKnown());
                Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
                var s = ((IRegularSudokuGrid)grid).ToChallengeString();
                sb.AppendLine(s);

                var testgrid = grid.CloneBoardAsChallenge();
                var solver = new GridSolver(creator.Solvers);
                solver.Solve(testgrid);
                Assert.AreEqual(Validity.Full, solver.Validity);
                sb.AppendLine("Board score: " + solver.GridScore);
            }

            Debug.WriteLine(sb.ToString());
        }

        [TestMethod]
        public void CreateChallenge9WithX()
        {
            var creator = new ChallengeCreator(BoardSize.Board9X);
            creator.CreateChallenge(new Random(2));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
            Debug.WriteLine(((IRegularSudokuGrid)grid).ToChallengeString());

            var testgrid = grid.CloneBoardAsChallenge();
            var solver = new GridSolver(creator.Solvers);
            solver.Solve(testgrid);
            Assert.AreEqual(Validity.Full, solver.Validity);
            Trace.WriteLine("Board score: " + solver.GridScore); // 47
        }

        [TestMethod]
        [TestProperty("Time", "Long")]
        public void CreateChallenge9X_multi()
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= 10; i++)
            {
                var creator = new ChallengeCreator(BoardSize.Board9X);
                creator.CreateChallenge(new Random(i));
                var grid = creator.Grid;

                Assert.IsTrue(grid.IsAllKnown());
                Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
                var s = ((IRegularSudokuGrid)grid).ToChallengeString();
                sb.AppendLine(s);
            }

            Debug.WriteLine(sb.ToString());
        }

        [TestMethod]
        [TestProperty("Time", "Long")]
        public void CreateChallenge12()
        {
            var creator = new ChallengeCreator(BoardSize.Board12);
            creator.CreateChallenge(new Random(2));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
            Debug.WriteLine(((IRegularSudokuGrid)grid).ToChallengeString());

            var testgrid = grid.CloneBoardAsChallenge();
            var solver = new GridSolver(creator.Solvers);
            solver.Solve(testgrid);
            Assert.AreEqual(Validity.Full, solver.Validity);
            Trace.WriteLine("Board score: " + solver.GridScore); // 88
        }


        [TestMethod]
        [TestProperty("Time", "Long")]
        public void CreateChallenge16()
        {
            var creator = new ChallengeCreator(BoardSize.Board16);
            creator.CreateChallenge(new Random(2));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
            Debug.WriteLine(((IRegularSudokuGrid)grid).ToChallengeString());
        }


        [TestMethod]
        public void CreateIrregularChallenge9()
        {
            var creator = new ChallengeCreator(BoardSize.Board9Irregular);
            creator.CreateChallenge(new Random(2));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());

            var testgrid = grid.CloneBoardAsChallenge();
            var solver = new GridSolver(creator.Solvers);
            solver.Solve(testgrid);
            Assert.AreEqual(Validity.Full, solver.Validity);
            Trace.WriteLine("Board score: " + solver.GridScore); // 182
        }

        [TestMethod]
        public void CreateIrregularChallenge6()
        {
            var creator = new ChallengeCreator(BoardSize.Board6Irregular);
            creator.CreateChallenge(new Random(2));
            var grid = creator.Grid;

            Assert.IsTrue(grid.IsAllKnown());
            Assert.AreEqual(Validity.Full, grid.IsChallengeDone());
        }
    }
}
