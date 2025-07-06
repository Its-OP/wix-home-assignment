# About the project
A solver for the randomly generated Snakes and Ladders games, which prints out the board and the combination of dice rolls that leads to the fastest path on it.

The project is implemented in the scope of a Home Assignment, given as a part of the interview process for the position of a Server Engineer at Wix.

# Terminology
The game board consists of **tiles** - rectangles marked with numbers. Every **tile** has **position** and **value**.

The **position** of a **tile** refers to the number drawn on it - number 1 on the first **tile**, 2 on the second one, 30 on the last one, and so on.

The **value** of the current **tile** refers to the **position** of a **tile**, where the player would get moved after stepping on the current **tile**.
**Value** and **position** match for tiles with no snakes and no ladders; **value** is greater than **position** for tiles with a ladder; and **value** is smaller than **position** for tiles with a snake.
For instance, on the picture below, the tile with **position** of 3 (the first ladder) has **value** of 22; the tile with **position** of 17 (the first snake) has **value** of 4; and the tile with **position** of 2 has **value** of 2.

![{307F63EC-4F7B-477B-9C69-517EFF669797}](https://github.com/user-attachments/assets/488e44b3-dbba-4e86-a580-b48fd7537702)

# Domain rules
Consider that there are $n$ tiles on the board.
1. All tiles have their **values** and **positions** in the range of $[1, n]$.
2. The first and the last tiles of the board can never have either snakes or ladders.
3. In order to use either a snake or a ladder, the player must land on a **tile** with a dice roll. For example, in case they land on a **tile** with a ladder after rolling down a snake, they won't be able to use this ladder.

# The interface
This is a console project, and therefore, all the interaction is conducted through the CLI. Whenever the user launches the project, the game asks the user to enter the dimensions of the board, the probability of any particular cell to spawn a snake or a ladder,
and the probability of a non-empty cell (i.e. with either a snake or a ladder) to have the ladder. When all of this information is entered, the game generates a board, solves it, and prints it out to the user, along with one of the combinations of dice rolls
that leads to the shortest path.

The board is printed in the same format as in the [Snakes and Ladders problem on LeetCode](https://leetcode.com/problems/snakes-and-ladders/description/). The first tile of the board is always located at the bottom left corner, and the direction of the rows of
the board is alternated according to the Boustrophedon style. Therefore, the last tile of the board is located in the top left corner for boards with an even number of rows, and in the top right corner for boards with an odd number of rows.

When the board is printed, regular **tiles** are represented with $-1$, while snakes and ladders are represented with their **values**. Additionally, to improve the user experience, the board highlights **tiles** visited by dice rolls (when using the suggested
dice combination) in red, and **tiles** visited by climbing up a ladder or rolling down a snake in green.

Take a look at the example below. The player starts in the bottom left corner, and makes a dice roll of 1, landing on a ladder with **value** of 82 (highlighted in red). This ladder moves the player to **tile** 82, highlighted in green.
Then, the player makes a roll of $5$, landing on the ladder with **value** of $97$, which moves them to the **tile**, highlighted in green, at the very top of the board. From there, they roll another $6$ to land at the final **tile**.

![{FD4194AD-855F-4323-BE7F-97876B4A2E72}](https://github.com/user-attachments/assets/cae0afdd-2c5e-4441-9c3b-a5ab9a953032)

# How to run the project
1. Clone the repository
2. From the root, run `docker build . -t wix-home-assignment`
3. Then, run `docker run -it wix-home-assignment`

# Implementation logic
The main idea behind this solution is to simplify the 2D board with weird folding logic to a 1D collection. Recall, that never before in this document the 2D coordinates of a **tile** have been mentioned - only **value** and **position**. I noticed this effect when solving the problem, and a simplification of the board-marix to a 1D list with indices representing **positions** and values - **values**, made the algorithms way more approachable.

The solution can be logically split into 4 components - Board Generator, responsible for generating random S&L boards; Board Solver, responsible for finding an optimal sequence of dice throws for a given board; Board Folder, responsible for folding a 1D list into a board-matrix; and a Console UI, responsible for receiving player's commands, and printing the board.

## Board Generator
Board Generator is represented with a class, which takes the number of tiles on a board, probability of either a snake or a ladder to spawn on a **tile**, and probability of a non-empty **tile** to contain a ladder. The class can work with an arbitrary positive integer number of tiles - even prime numbers, which by their nature cannot be folded into a 2D board. For every **tile**, besides the first and the last ones, it attempts to generate either a snake or a ladder, based on the two probability parameters mentioned before. The generator is hardcoded to not generate more than 5 snakes in a row, since such a pattern can make the board unsolvable.

As a result of the generation, the generator returns a 1D list of **tiles**.

## Board Solver
Board Solve is also represented with a single class. The purpose of the class is to take the 1D list, produced by the generator, and find the optimal sequence of dice throws.

In order to find such a sequence, I modified the standard BFS algorithm, used to solve the [Snakes and Ladders problem on LeetCode](https://leetcode.com/problems/snakes-and-ladders/description/). In my version, every visited **tile** is represented with a linked list node, connected to the node of a **tile** from where the current one was reached first. Therefore, when the final **tile** is reached, it becomes trivial to backtrack to the origin, and retrieve the dice roll values.

The algorithm has both time and space complexities of $O(N)$, where $N$ is the **total number of tiles** of the board.

## Board Folder
Initially, the board folder was a part of the Console UI. However, eventually, I figured out that there might be other presentation modes (i.e. Web UI), which would require the board to be folded, so I decided to move this logic into a separate module.

The module itself works relatively simple. It takes a 1D list of tiles along with dimensions of the desired board, throwing an error if the dimensions are invalid for the number of **tiles**. Then, it folds the list into the board with the given dimensions.

Note, that folding **tiles** into a board, makes it harder to represent both **value** and **position** of each tile at once. To address this problem, at this step, I decided to replace integers with dedicated `Tile` structures, containing both **value** and **position** of the respective tiles.

## Console UI
Console UI is represented by a few classes, responsible for stringifying the board, and interacting with the player. The board stringifier builds a pretty table with colored cues for optimal throws from a board-matrix. The class responsible for interacting with the player is responsible for taking the player's inputs; validating and processing them; and printing the solved board.
