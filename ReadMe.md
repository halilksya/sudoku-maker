# Sudoku Maker

**English** | [Türkçe](ReadMe.turkish.md)

Sudoku Maker is a desktop Sudoku app for Windows and macOS, built with Avalonia UI on .NET 10. Each game is generated on the spot rather than pulled from a fixed puzzle bank, so no two games are the same. You can play with the mouse or entirely from the keyboard, jot down pencil marks while you think through a cell, save a puzzle partway through and come back to it later, and see how your solves compare to your own past games on a per-difficulty leaderboard.

## Contents

- [Requirements](#requirements)
- [Running the project](#running-the-project)
- [Features](#features)
- [Puzzle generation and solving](#puzzle-generation-and-solving)
- [Keyboard shortcuts](#keyboard-shortcuts)
- [Project structure](#project-structure)

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- `make` (installed by default on macOS; on Windows, use WSL, Git Bash, or install `make` separately)

## Running the project

The project ships with a `makefile`, so no manual `dotnet` commands are needed:

```bash
make build   # build the project
make start   # run it
make all     # build and run in one step
```

## Features

**Puzzles and difficulty.** Every new game is generated fresh at one of three difficulties — Easy, Medium, or Hard.

**Saving and resuming.** Save a puzzle mid-solve and pick it back up later from the same state, including elapsed time, notes, and hint count.

**Conflict highlighting.** If a row, column, or 3x3 box ends up with a duplicate number, that whole group is highlighted immediately — no need to press Check to find out.

**Pencil marks.** Toggle Note mode to jot down candidate numbers in a small 3x3 grid inside a cell. Notes clear automatically once you commit a final value there.

**Undo and redo.** Every value change is tracked, so you can step backward and forward through your moves.

**Hints.** Reveal one correct cell at a time. Hints are counted and factored into your score, so they're not free.

**Completion.** Finishing a puzzle stops the timer, saves the game automatically, and shows a one-time summary with your score. Reopening an already-finished save won't show that summary again, and the timer stays frozen at the time you finished.

**Scoring and leaderboard.** Your score depends on difficulty, completion time, hints used, and how steadily you entered numbers. The main menu has a leaderboard with separate rankings for Easy, Medium, and Hard, sortable by score or by time.

**PDF export.** Export the current puzzle to a printable PDF.

**Language.** The whole interface is available in English and Turkish, switchable from the main menu without restarting.

## Puzzle generation and solving

Both generating and solving a board come down to the same core routine: a backtracking search that walks the grid cell by cell, tries a number, and recurses; if that leads to a dead end, it undoes the number and tries the next one.

To build a fresh, fully solved board, the generator runs that backtracking search on an empty grid, but shuffles the order it tries `1`–`9` in at each cell. This is what makes every generated solution different, rather than always producing the same filled grid.

To turn a solved board into a puzzle, the generator picks cells in random order and clears them one at a time. After clearing a cell, it re-solves the board while counting how many distinct solutions exist, stopping early as soon as it finds a second one. If the puzzle still has exactly one solution, the cell stays empty; if clearing it made the puzzle ambiguous, the number is put back. This uniqueness check is what guarantees every generated puzzle has exactly one valid solution, no matter how many cells end up empty. The difficulty levels simply set the target number of cells to clear — more empty cells means more of the board you have to work out yourself.

## Keyboard shortcuts

| Key | Action |
|---|---|
| `1`–`9` (number row or numpad) | Enter a number in the selected cell, or toggle a note if Note mode is on |
| Arrow keys | Move the selected cell |
| `Delete` / `Backspace` | Clear the selected cell (value and notes) |
| `N` | Toggle Note mode |
| `Ctrl+Z` | Undo |
| `Ctrl+Y` or `Ctrl+Shift+Z` | Redo |

## Project structure
```
sudoku-maker/
├── Models/ Difficulty, SaveGame, SudokuBoard, ...
├── ViewModels/ SudokuViewModel, SudokuCellViewModel, LeaderboardViewModel, ...
├── Views/ MainWindow, SudokuView, LeaderboardView, dialogs, ...
├── Services/ SudokuGenerator, SudokuSolver, SaveGameService, PdfExportService, LocalizationService
├── Localization/ The {loc:Loc} markup extension used for instant language switching
└── Assets/ Icons and SVG button images
```