using System.ComponentModel;
using SudokuX.Solver.Grids;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver
{
    public static class GridCreator
    {
        public static ISudokuGrid Create(BoardSize size)
        {
            switch (size)
            {
                case BoardSize.Board4:
                    return new Grid4X4();

                case BoardSize.Board6:
                    return new Grid6X6();

                    case BoardSize.Board6Irregular:
                    return new Irregular6();

                case BoardSize.Board9:
                    return new Grid9X9();

                case BoardSize.Board9X:
                    return new Grid9X9WithX();

                case BoardSize.Board9Irregular:
                    return new Irregular9();

                case BoardSize.Board12:
                    return new Grid12X12();

                case BoardSize.Board16:
                    return new Grid16X16();
            }

            throw new InvalidEnumArgumentException("size", (int)size, typeof(BoardSize));
        }
    }
}
