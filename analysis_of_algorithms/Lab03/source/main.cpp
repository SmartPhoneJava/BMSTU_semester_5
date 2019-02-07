#include <vector>
#include <iostream>

int main(int argc, char *argv[]) {
    std::vector<std::vector<int>> matrix;           // 0
    int p = 0;                                      // 1
    int g = 0;                                      // 2
    int rows = 2;                                   // 3
    int columns = 3;                                // 4

    for (int i = 0; i < rows; i++) {                // 5
        std::vector<int> row;                       // 6
        for (int j = 0; j < columns; j++) {         // 7
            p += i + j;                             // 8
            row.push_back(p);                       // 9
        }
        matrix.push_back(row);                      // 10
    }
    for (int i = 0; i < rows - 1; i++) {            // 11
        for (int j = i; j < columns; j++) {         // 12
            while (p % 2 != 0) {                    // 13
                g += matrix[i][j];                  // 14
                matrix[i][j] = matrix[i + 1][j];    // 15
                p += matrix[i][j] + g + i * j + 1;  // 16

            }
        }
    }

    return 0;
}
