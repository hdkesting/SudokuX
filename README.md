# SudokuX

My first WPF program: a Sudoku game.

The development is continued in another repo as UWP app. That app is published on the Microsoft Store: https://www.microsoft.com/store/apps/9NBLGGH697QJ

It can create boards of different sizes:

* 4x4
* 6x6
* 9x9
* 12x12
* 16x16

There are regular (rectangular) grids, but also:

* regular plus diagonals (implemented for 9x9)
* irregular grids (implemented for 6x6 and 9x9)

The challenges can have various symmetries, like:

* none (mainly for 4x4 and irregular)
* vertical (mirror) symmetry
* both vertical and horizontal symmetry
* rotational symmetry

There are several levels of difficulty possible.
