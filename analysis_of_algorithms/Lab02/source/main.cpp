#include <QCoreApplication>
#include <iostream>
#include <qDebug>
#include <vector>
#include <stdlib.h>
#include <time.h>

class IntMatrix {
public:
    std::vector<std::vector<int>> m;
    IntMatrix(size_t rows = 0, size_t columns = 0) {
        srand(time(nullptr));

        std::vector<std::vector<int>> matr;
        for (size_t i = 0; i < rows; i++) {
            std::vector<int> row;
            for (size_t j = 0; j < columns; j++) {
                row.push_back(rand() % 20);
            }
            matr.push_back(row);
        }
        m = matr;
    }

    IntMatrix(const IntMatrix& im) {
        copy(im);
    }

    IntMatrix(IntMatrix&& im) {
        copy(im);
    }

    IntMatrix& operator=(const IntMatrix& im) {
        copy(im);
    }

    IntMatrix& operator=(IntMatrix&& im) {
        copy(im);
    }

    size_t rows() {
        return m.size();
    }

    size_t columns() {
        return m[0].size();
    }


#pragma optimize("", off)
    static IntMatrix multiplicate(IntMatrix im1, IntMatrix im2) {

        if (im2.rows() != im1.columns())
            return (IntMatrix());
        IntMatrix return_me(im1.rows(), im2.columns());
        for (size_t i = 0; i < im1.rows(); i++) {// 3 + im1.rows()*(
            std::vector<int> row;
            return_me.m[i].clear(); //4 +
            for (size_t j = 0; j < im2.columns(); j++) {//3 + im2.columns()*(
                return_me.m[i].push_back(0); //4 +
                for (size_t k = 0; k < im2.rows(); k++) //3 + im2.rows() * (
                    return_me.m[i][j] = return_me.m[i][j] + im1.m[i][k] * im2.m[k][j]; // 15)))
            }
        }
        // 3 + im1.rows()*(4 + 3 + im2.columns()*(4 + 3 + im2.rows() * 12))

        // 3 + 7 * im1.rows() + 7 * im1.rows() * im2.columns() +
        // 12 * im1.rows() * im2.columns() * im2.rows()
        return return_me;
    }

#pragma optimize("", off)
    static IntMatrix multiplicate_alg(IntMatrix im1, IntMatrix im2) {

        if (im2.rows() != im1.columns())
            return (IntMatrix());
        IntMatrix return_me(im1.rows(), im2.columns());

        std::vector<int> r(im1.rows());
        std::vector<int> c(im2.columns());

        for (size_t i = 0; i < im1.rows(); i++) { // 3 + im1.rows() * (
            for (size_t j = 0; j < im2.rows() / 2; j++) { //4 + im2.rows() *
                r[i] = r[i] + im1.m[i][2 * j] * im1.m[i][2 * j + 1]; //14)
            }
        }

        for (size_t i = 0; i < im2.columns(); i++) { // 3 + im2.columns() * (
            for (size_t j = 0; j < im2.rows() / 2; j++) {// 4 + im2.rows() *
                c[i] = c[i] + im2.m[2 * j][i] * im2.m[2 * j + 1][i]; //14)
            }
        }

        for (size_t i = 0; i < im1.rows(); i++) { // 3 + im1.rows() * (
            return_me.m[i].clear(); // 1 +
            for (size_t j = 0; j < im2.columns(); j++) { // 3 + im2.columns() * (
                return_me.m[i].push_back(0); // 3 +
                return_me.m[i][j] = -r[i] - c[j]; // 8 +
                for (size_t k = 0; k < im2.rows() / 2; k++) { // 4 + 1/2*im2.rows() *
                    return_me.m[i][j] =
                        return_me.m[i][j] + ((im1.m[i][2 * k] + im2.m[2 * k + 1][j]) *
                                             (im1.m[i][2 * k + 1] + im2.m[2 * k][j]));
                        //29))
                }
            }
        }

        if (im2.rows() % 2) { // 2 +
            for (size_t i = 0; i < im1.rows(); i++) { // 3 + im1.rows() * (
                for (size_t j = 0; j < im2.columns(); j++) { // 3 + im2.columns() *
                    return_me.m[i][j] = return_me.m[i][j] +
                        im1.m[i][im2.rows() - 1] * im2.m[im2.rows() - 1][j];
                    // 19)
                }
            }
        }

        //3 + im1.rows() * (4 + im2.rows() * 14) + 3 + im2.columns() * (4 + im2.rows() * 14) +
        //3 + im1.rows() * (4 + im2.columns() * (15 + im2.rows() * 29)) +
        //2 + 3 + im1.rows() * (3 + im2.columns() * 19)

        //14 + 11 * im1.rows() + 14 * im1.rows() * im2.rows() + 4 * im2.columns() + 14 * im2.columns() * im2.rows() +
        //im1.rows() * im2.columns() * 34 + 15 * im1.rows() * im2.columns() * im2.rows()

        return return_me;
    }

#pragma optimize("", off)
    static IntMatrix multiplicate_best(IntMatrix im1, IntMatrix im2) {

        if (im2.rows() != im1.columns())
            return (IntMatrix());
        IntMatrix return_me(im1.rows(), im2.columns());

        std::vector<int> r(im1.rows());
        std::vector<int> c(im2.columns());

        size_t im1_rows = im1.rows(); // 2 +
        size_t im2_rows_2 = im2.rows() / 2; // 3 +
        size_t im2_columns = im2.columns(); // 2 +

        for (size_t i = 0; i < im1_rows; i++) { // 2 + im1_rows * (
            for (size_t j = 0; j < im2_rows_2; j++) { // 2 + im2_rows_2 *
                r[i] += im1.m[i][j] * im1.m[i][j + 1]; // 10
            }
        }

        for (size_t i = 0; i < im2_columns; i++) { // 2 + im2_columns * (
            for (size_t j = 0; j < im2_rows_2; j++) { // 2 + im2_rows_2 * (
                c[i] += im2.m[j][i] * im2.m[j + 1][i]; // 10
            }
        }

        for (size_t i = 0; i < im1_rows; i++) { // 2 + im1_rows * (
            return_me.m[i].clear(); // 1 +
            for (size_t j = 0; j < im2_columns; j++) { // 2 + im2_columns * (
                return_me.m[i].push_back(0); // 3 +
                return_me.m[i][j] = -r[i] - c[j]; // 7 +
                for (size_t k = 0; k < im2_rows_2; k++) { //2 + im2_rows_2 *
                    return_me.m[i][j] += ((im1.m[i][k] + im2.m[k + 1][j]) *
                                          (im1.m[i][k + 1] + im2.m[k][j]));
                    // 21))
                }
            }
        }

        size_t im2_rows = im2.rows(); // 1 +
        if (im2_rows % 2) { // 1 +
            for (size_t i = 0; i < im1_rows; i++) { // 2 + im1_rows * (
                for (size_t j = 0; j < im2_columns; j++) {// 2 + im2_columns *
                    return_me.m[i][j] += im1.m[i][im2_rows - 1] * im2.m[im2_rows - 1][j];
                    // 13)
                }
            }
        }
        //7 +
        //2 + im1.rows() * (2 + im2.rows() * 10) + 2 + im2.columns() * (2 + im2.rows() * 10) +
        //2 + im1.rows() * (3 + im2.columns() * (12 + im2.rows() * 21)) +
        //4 + im1.rows() * (2 + im2.columns() * 13)

        //17 + 4 * im1.rows() + 10 * im1.rows() * im2.rows() + 2 * im2.columns() + 10 * im2.columns() * im2.rows() +
        //im1.rows() * im2.columns() * 16 + 21/2 * im1.rows() * im2.columns() * im2.rows()

        return return_me;
    }

private:
    void copy(const IntMatrix& from) {
        this->m = from.m;
    }
    void clear() {
        for (size_t i = 0; i < m.size(); i++) {
            m[i].clear();
        }
        m.clear();
    }
};

unsigned long long tick(void) {
    unsigned long long d;
    __asm__ __volatile__("rdtsc" : "=A"(d));
    return d;
}

int main() {

    int start = 101;
    int end = 801;
    int step = 100;
    int rep = 10;

     unsigned long long tb, te, t_mid1 = 0, t_mid2 = 0;

    for (size_t i = start; i <= end; i += step) {
        printf("\ntest with %d elements\n", i);
        IntMatrix a(i, i);
        IntMatrix b(i, i);
        IntMatrix c;
        for (size_t j = 0; j < rep; j++) {
            tb = tick();
            c = IntMatrix::multiplicate(a, b);
            te = tick();
            t_mid1 += (te - tb);
        }
        t_mid1 /= rep;
        printf("usual %lld \n", t_mid1);

        for (size_t j = 0; j < rep; j++) {
            tb = tick();
            c = IntMatrix::multiplicate_alg(a, b);
            te = tick();
            t_mid1 += (te - tb);
        }
        t_mid1 /= rep;
        printf("alg %lld \n", t_mid1);

        for (size_t j = 0; j < rep; j++) {
            tb = tick();
            c = IntMatrix::multiplicate_best(a, b);
            te = tick();
            t_mid1 += (te - tb);
        }
        t_mid1 /= rep;
         printf("best %lld \n", t_mid1);
    }

    return 0;
}
