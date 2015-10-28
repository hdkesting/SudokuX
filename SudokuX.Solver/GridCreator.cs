using System;
using System.ComponentModel;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Grids;
using SudokuX.Solver.Support.Enums;
using SudokuX.Solver.GridPatterns;
using System.Collections.Generic;
using SudokuX.Solver.SolverStrategies;

namespace SudokuX.Solver
{
    /// <summary>
    /// Creates empty grids and sets up solvers and such.
    /// </summary>
    public static class GridCreator
    {
        /// <summary>
        /// The list of possible levels for each board size, plus the solvers used.
        /// </summary>
        /// <remarks>Make sure there is at least a "Difficulty.Normal" version.</remarks>
        private static readonly Dictionary<BoardSize, Dictionary<Difficulty, List<ISolverStrategy>>> Levels = new Dictionary<BoardSize, Dictionary<Difficulty, List<ISolverStrategy>>>
        {
            { BoardSize.Board4, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Easy, new List<ISolverStrategy>
                        {
                            new HiddenSingle()
                        }
                    },
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle()
                        }
                    }
                }
            },
            { BoardSize.Board6, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Easy, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                        }
                    },
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new NakedDouble(),
                            new HiddenDouble()
                        }
                    }
                }
            },
            { BoardSize.Irregular6, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Easy, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle()
                        }
                    },
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new NakedDouble(),
                            new HiddenDouble()
                        }
                    }
                }
            },
            { BoardSize.Board9, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Easy, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble()
                        }
                    },
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new NakedTriple(),
                            new XWing(),
                            new SolveWithColors()
                        }
                    }
                }
            },
            { BoardSize.Board9X, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Easy, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                        }
                    },
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple(),
                            new SolveWithColors()
                        }
                    },
                    { Difficulty.Harder, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple(),
                            new SolveWithColors(),
                            new XWing()
                        }
                    }
                }
            },
            { BoardSize.Irregular9, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Easy, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble()
                        }
                    },
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple()
                        }
                    }
                }
            },
            { BoardSize.Hyper9, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Easy, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble()
                        }
                    },
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple()
                        }
                    },
                    { Difficulty.Harder, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple(),
                            new NakedTriple(),
                        }
                    }
                }
            },
            { BoardSize.Board12, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            //new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new NakedTriple(),
                            new SolveWithColors()
                        }
                    },
                    { Difficulty.Harder, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new NakedTriple(),
                            new SolveWithColors(),
                            new HiddenTriple(),
                            new XWing()
                        }
                    }
                }
            },
            { BoardSize.Board16, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new SolveWithColors()
                        }
                    },
                    { Difficulty.Harder, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new NakedTriple(),
                            new SolveWithColors()
                        }
                    }
                }
            },
            { BoardSize.Irregular12, new Dictionary<Difficulty, List<ISolverStrategy>>
                {
                    { Difficulty.Normal, new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new NakedTriple(),
                            new HiddenTriple()
                        }
                    }
                }
            }
        };

        /// <summary>
        /// Creates a grid of the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="generateBlocks">if set to <c>true</c>, generate blocks for irregular grids.</param>
        /// <returns></returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">size</exception>
        public static ISudokuGrid Create(BoardSize size, bool generateBlocks = true)
        {
            switch (size)
            {
                case BoardSize.Board4:
                    return new Grid4X4();

                case BoardSize.Board6:
                    return new Grid6X6();

                case BoardSize.Irregular6:
                    return new Irregular6(generateBlocks);

                case BoardSize.Board9:
                    return new Grid9X9();

                case BoardSize.Board9X:
                    return new Grid9X9WithX();

                case BoardSize.Irregular9:
                    return new Irregular9(generateBlocks);

                case BoardSize.Hyper9:
                    return new Hyper9();

                case BoardSize.Board12:
                    return new Grid12X12();

                case BoardSize.Board16:
                    return new Grid16X16();

                case BoardSize.Irregular12:
                    return new Irregular12(generateBlocks);
            }

            throw new InvalidEnumArgumentException("size", (int)size, typeof(BoardSize));
        }

        /// <summary>
        /// Gets the range of supported difficulty levels.
        /// </summary>
        /// <remarks>
        /// To be called by the UI to fill the "difficulty" dropdown.
        /// </remarks>
        /// <param name="boardSize">The board's size.</param>
        /// <returns></returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">size</exception>
        public static Tuple<Difficulty, Difficulty> GetLevelRange(BoardSize boardSize)
        {
            if (!Levels.ContainsKey(boardSize)) throw new InvalidEnumArgumentException("size", (int)boardSize, typeof(BoardSize));

            // get the difficulty levels avaulable for this board size
            var set = Levels[boardSize].Select(x => x.Key).OrderBy(diff => (int)diff).ToList();

            return Tuple.Create(set.First(), set.Last());
        }

        internal static IGridPattern GetGridPattern(BoardSize boardSize, Difficulty difficulty)
        {
            switch (boardSize)
            {
                case BoardSize.Board4:
                case BoardSize.Board6:
                case BoardSize.Irregular6:
                case BoardSize.Irregular9:
                case BoardSize.Irregular12:
                    return new RandomPattern();

                case BoardSize.Board9:
                    return new Rotational2Pattern();

                case BoardSize.Board9X:
                    return new DoubleMirroredPattern();

                case BoardSize.Hyper9:
                    return difficulty == Difficulty.Harder ? (IGridPattern)new RandomPattern() : (IGridPattern)new VerticalMirroredPattern();

                case BoardSize.Board12:
                case BoardSize.Board16:
                    return new Rotational4Pattern();
            }

            throw new InvalidEnumArgumentException("size", (int)boardSize, typeof(BoardSize));
        }

        public static IList<ISolverStrategy> GetGridSolvers(BoardSize boardSize, Difficulty difficulty)
        {

            if (!Levels.ContainsKey(boardSize)) throw new InvalidEnumArgumentException("boardSize", (int)boardSize, typeof(BoardSize));

            // get definition for this size
            var sizedef = Levels[boardSize];

            // get solvers for this difficulty (if present)
            if (sizedef.ContainsKey(difficulty))
                return sizedef[difficulty];

            throw new InvalidEnumArgumentException("difficulty", (int)difficulty, typeof(Difficulty));
        }
    }
}
