# Setup
From the root of the repository, run:
- `docker build -t wix-fifteen-puzzle -f ./src/Solution/Dockerfile ./src/Solution`
- `docker run -it --rm wix-fifteen-puzzle`

# Game Logic
The player starts with 15 tiles numbered 1-15 shuffled randomly across a 4x4 board. A tile can be moved to the **neighbouring** empty place. The goal of the game is to order the tiles from 1 to 15, so 1 is in the top left corner, the empty tile is in the bottom right corner, and 15 is just to the left of the empty tile.

# Gameplay
When the game starts, a random tile configuration is generated and displayed on the board. At the beginning of each turn, the board is printed as a 4x4 matrix in the terminal. The player then types the number of the tile they want to move. Since each tile has **at most one** empty neighboring position, the direction of the move does not need to be specified. After the move, the board updates, and the game checks if the current tile configuration matches the winning configuration (i.e., tiles are ordered). If it does, a congratulatory message is displayed, and the game ends. Otherwise, the next turn begins.

Each player input is validated. If the input is invalid for the current tile configuration (e.g., the selected tile is not adjacent to the empty space or does not exist on the board), an appropriate error message is displayed in the terminal, and the board remains unchanged.

# Implementation

### TDD
This solution was implemented using the **Test-Driven Development (TDD)** methodology, supported by a CI pipeline that runs all tests on each commit. Based on it, I could start implementing the assignment with the entities encapsulating the business logic, and not ones required for the presentation. The simulation of different states of the game board, as well as the verification of various requirements, was done with tests, and I was confident that a couple refactorings of some components did not break the business logic.

### Clean Code and Expressive Design
The game logic is cleanly separated from the presentation layer. This means new presentation methods can be introduced without changing the core behavior. Special attention was paid to avoiding magic values, ensuring each piece of logic resides in its own dedicated class, and maintaining a single source of truth. Finally, the classes were designed to be easily extended or to accommodate potential ruleset changes in the future.

### Random Initial Board Generation
For an N×N board, the random initial tile configuration is generated as follows:
1. A set of numbers from 1 to N²-1 is created.
2. A placeholder for the empty tile is added to the set.
3. The set is shuffled randomly.
4. The set of N² numbers is ‘folded’ into an N×N array.

### Verification of the Winning Tile Configuration
As mentioned in the **Gameplay** section, each turn requires checking whether the tiles are in the correct order. A naive approach here would be to iterate through all tiles (O(N²) complexity), which becomes problematic for very large boards (e.g., 10,000×10,000).

The approach can be optimized if we consider that only two tiles move each turn: the empty tile and one neighboring tile. Thus, we can track a measure of how far the current board is from the winning configuration, updating it incrementally after each move. This measure is called *d*, the “deviation from the winning configuration.”

In the solved state, the deviation is 0. For any cell *(i, j)*, if the correct tile value is *x* and the current tile value is *y*, its local deviation is *|y - x|*. Summing these deviations across all cells yields *d*.

Let *d1* be the deviation on turn 1. After a single move, only two cells change: the empty cell and the cell whose tile was moved. We remove their old deviations and add the new ones:

```latex
d₂ = d₁ - d(i₁, j₁) - d(i₂, j₂) + d′(i₁, j₁) + d′(i₂, j₂)```

Hence, after the initial O(N²) computation during board generation, verifying the winning configuration is O(1) per turn.