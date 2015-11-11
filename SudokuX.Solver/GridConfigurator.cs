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
    /// Configures empty grids and sets up solvers and such.
    /// </summary>
    /// <seealso cref="EnumExtensions"/>
    public static class GridConfigurator
    {
        /// <summary>
        /// The list of possible levels for each board size, plus the solvers used.
        /// </summary>
        /// <remarks>Make sure there is at least a "Difficulty.Normal" version.</remarks>
        private static readonly Dictionary<BoardSize, Dictionary<Difficulty, List<SolverType>>> Levels = new Dictionary<BoardSize, Dictionary<Difficulty, List<SolverType>>>
        {
            { BoardSize.Board4, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Easy, new List<SolverType>
                        {
                            SolverType.HiddenSingle
                        }
                    },
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.HiddenSingle,
                            SolverType.NakedSingle
                        }
                    }
                }
            },
            { BoardSize.Board6, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Easy, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle
                        }
                    },
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble
                        }
                    }
                }
            },
            { BoardSize.Irregular6, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Easy, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle
                        }
                    },
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble
                        }
                    }
                }
            },
            { BoardSize.Board9, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Easy, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble
                        }
                    },
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.XWing,
                            SolverType.SolveWithColors,
                            SolverType.XYWing
                        }
                    },
                    { Difficulty.Harder, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.XWing,
                            SolverType.SolveWithColors,
                            SolverType.XWing,
                            SolverType.SolveWithColors,
                            SolverType.XYWing,
                            SolverType.HiddenQuad,
                            SolverType.NakedQuad
                        }
                    }
                }
            },
            { BoardSize.Board9X, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Easy, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble
                        }
                    },
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.HiddenTriple,
                            SolverType.SolveWithColors
                        }
                    },
                    { Difficulty.Harder, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.HiddenTriple,
                            SolverType.SolveWithColors,
                            SolverType.XWing,
                            SolverType.XYWing,
                            SolverType.HiddenQuad,
                            SolverType.NakedQuad
                        }
                    }
                }
            },
            { BoardSize.Irregular9, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Easy, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble
                        }
                    },
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.HiddenTriple,
                            SolverType.XYWing
                        }
                    }
                }
            },
            { BoardSize.Hyper9, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Easy, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble
                        }
                    },
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.HiddenTriple
                        }
                    },
                    { Difficulty.Harder, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.HiddenTriple,
                            SolverType.XWing,
                            SolverType.XYWing,
                            SolverType.NakedQuad,
                            SolverType.HiddenQuad
                        }
                    }
                }
            },
            { BoardSize.Board12, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.SolveWithColors
                        }
                    },
                    { Difficulty.Harder, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.SolveWithColors,
                            SolverType.HiddenTriple,
                            SolverType.XWing,
                            SolverType.HiddenQuad,
                            SolverType.NakedQuad
                        }
                    }
                }
            },
            { BoardSize.Board16, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble
                        }
                    },
                    { Difficulty.Harder, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.SolveWithColors,
                            SolverType.HiddenQuad,
                            SolverType.NakedQuad
                        }
                    }
                }
            },
            { BoardSize.Irregular12, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.HiddenTriple
                        }
                    }
                }
            },
            { BoardSize.Board8Column, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.HiddenTriple
                        }
                    }
                }
            },
            { BoardSize.Board8Row, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.HiddenTriple
                        }
                    }
                }
            },
            { BoardSize.Board8Mix, new Dictionary<Difficulty, List<SolverType>>
                {
                    { Difficulty.Normal, new List<SolverType>
                        {
                            SolverType.NakedSingle,
                            SolverType.HiddenSingle,
                            SolverType.LockedCandidates,
                            SolverType.NakedDouble,
                            SolverType.HiddenDouble,
                            SolverType.NakedTriple,
                            SolverType.HiddenTriple
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

#if DEBUG
                case BoardSize.Irregular12:
                    return new Irregular12(generateBlocks);
#endif

                case BoardSize.Board8Column:
                    return new Grid8X8Column();

                case BoardSize.Board8Row:
                    return new Grid8X8Row();

                case BoardSize.Board8Mix:
                    return new Grid8X8Mix();
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
                case BoardSize.Board8Mix:
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

                case BoardSize.Board8Column:
                    return new VerticalMirroredPattern();
                case BoardSize.Board8Row:
                    return new HorizontalMirrorPattern();
            }

            throw new InvalidEnumArgumentException("size", (int)boardSize, typeof(BoardSize));
        }

        /// <summary>
        /// Gets all the grid solvers for these settings.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        /// <param name="difficulty">The difficulty.</param>
        /// <returns></returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        /// boardSize
        /// or
        /// difficulty
        /// </exception>
        public static IList<ISolverStrategy> GetGridSolvers(BoardSize boardSize, Difficulty difficulty)
        {

            if (!Levels.ContainsKey(boardSize)) throw new InvalidEnumArgumentException("boardSize", (int)boardSize, typeof(BoardSize));

            // get definition for this size
            var sizedef = Levels[boardSize];

            // get solvers for this difficulty (if present)
            if (sizedef.ContainsKey(difficulty))
                return sizedef[difficulty].Select(GetSolver).ToList();

            throw new InvalidEnumArgumentException("difficulty", (int)difficulty, typeof(Difficulty));
        }

        /// <summary>
        /// Gets an instance of the specified solver.
        /// </summary>
        /// <param name="solverType">Type of the solver.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Do not use the BasicRule directly, it's built-in.</exception>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">solverType</exception>
        public static ISolverStrategy GetSolver(SolverType solverType)
        {
            switch(solverType)
            {
                case SolverType.Basic:
                    throw new InvalidOperationException("Do not use the BasicRule directly, it's built-in.");
                case SolverType.NakedSingle:
                    return new NakedSingle();
                case SolverType.HiddenSingle:
                    return new HiddenSingle();
                case SolverType.NakedDouble:
                    return new NakedDouble();
                case SolverType.HiddenDouble:
                    return new HiddenDouble();
                case SolverType.NakedTriple:
                    return new NakedTriple();
                case SolverType.HiddenTriple:
                    return new NakedTriple();
                case SolverType.NakedQuad:
                    return new NakedQuad();
                case SolverType.HiddenQuad:
                    return new HiddenQuad();
                case SolverType.LockedCandidates:
                    return new LockedCandidates();
                case SolverType.SolveWithColors:
                    return new SolveWithColors();
                case SolverType.XWing:
                    return new XWing();
                case SolverType.XYWing:
                    return new XYWing();
            }

            throw new InvalidEnumArgumentException("solverType", (int)solverType, typeof(SolverType));
        }
    }
}
