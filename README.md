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

Take a look at the example below. The player starts in the bottom left corner, and makes a dice roll of 6, landing on a ladder with **value** of 14 (highlighted in red). This ladder moves the player to **tile** 14, highlighted in green right above.
Then, the player makes a roll of $4$, landing on the ladder with **value** of $94$, which moves them to the **tile**, highlighted in green, at the very top of the board. From there, they roll another $6$ to land at the final **tile**.

![{CF68F2C6-3C7D-4BEA-BA6C-A727BC5A05DA}](https://github.com/user-attachments/assets/6077f54b-3bc8-4817-bf3c-d202710e88c9)

# How to run
1. Clone the repository
2. From the root, run `docker build . -t wix-home-assignment`
3. Then, run `docker run -it wix-home-assignment`

# Implementation
