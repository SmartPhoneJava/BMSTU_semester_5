#include <QCoreApplication>
#include <iostream>
#include <qdebug>
#include <vector>
#include <stdlib.h>
#include<thread>
#include <time.h>
#include <ctime>
#include<mutex>

using namespace std;

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
        return *this;
    }

    IntMatrix& operator=(IntMatrix&& im) {
        copy(im);
        return *this;
    }

    size_t rows() {
        return m.size();
    }

    size_t columns() {
        return m[0].size();
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

#pragma optimize("", off)
void work_1_3(IntMatrix im1,
              size_t im1_rows_begin,
              size_t im1_rows,
              size_t im2_rows_2,
              std::vector<int> &row){
    size_t i_begin = im1_rows_begin;
    size_t i_size = im1_rows;
    size_t j_size = im2_rows_2;

    for (size_t i = i_begin; i < i_size; i++) {
        for (size_t j = 0; j < j_size; j++) {
            row[i] += im1.m[i][j] * im1.m[i][j + 1];
        }
    }
}

#pragma optimize("", off)
void work_2_3(IntMatrix im2,
              size_t im2_columns_begin,
              size_t im2_columns,
              size_t im2_rows_2,
              std::vector<int> &column){

    size_t i_begin = im2_columns_begin;
    size_t i_size = im2_columns;
    size_t j_size = im2_rows_2;

    for (size_t i = i_begin; i < i_size; i++) { // 2 + im2_columns * (
        for (size_t j = 0; j < j_size; j++) { // 2 + im2_rows_2 * (
            column[i] += im2.m[j][i] * im2.m[j + 1][i]; // 10
        }
    }
}

#pragma optimize("", off)
void work_3_3(IntMatrix im1, IntMatrix im2,
              std::vector<int> c,
              std::vector<int> r,
              size_t im1_rows_begin,
              size_t im1_rows,
              size_t im2_columns,
              size_t im2_rows_2,
              IntMatrix &return_me){
    size_t im2_rows = im2.rows();
    size_t i_begin = im1_rows_begin;
    size_t i_size = im1_rows;
    size_t j_size = im2_columns;
    size_t k_size = im2_rows_2;

    for (size_t i = i_begin; i < i_size; i++) {
        return_me.m[i].clear();

        for (size_t j = 0; j < j_size; j++) {
            return_me.m[i].push_back(0);
            return_me.m[i][j] = -r[i] - c[j];

            for (size_t k = 0; k < k_size; k++) {
                return_me.m[i][j] += (
                            (im1.m[i][k] + im2.m[k + 1][j]) *
                            (im1.m[i][k + 1] + im2.m[k][j])
                        );
            }
            if (im2_rows % 2) {
                return_me.m[i][j] += im1.m[i][im2_rows - 1] *
                        im2.m[im2_rows - 1][j];
            }
        }
    }
}

#pragma optimize("", off)
IntMatrix multi8(IntMatrix im1, IntMatrix im2) {

    if (im2.rows() != im1.columns())
        return (IntMatrix());

    IntMatrix return_me(im1.rows(), im2.columns());

    std::vector<int> r(im1.rows());
    std::vector<int> c(im2.columns());

    size_t im1_rows = im1.rows();
    size_t im2_rows_2 = im2.rows() / 2;
    size_t im2_columns = im2.columns();

    mutex mutex_im1, mutex_im2;
    mutex mutex_im1_rows, mutex_im2_rows_2, mutex_im2_columns;
    mutex mutex_row, mutex_column;
    mutex mutex_return_me, mutex_out;

    //std::cout << "1111";
    thread thread1_1(work_1_3,
                     im1, ref(mutex_im1),
                     0, im1_rows / 4, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread1_2(work_1_3,
                     im1, ref(mutex_im1),
                     im1_rows / 4, im1_rows / 4 * 2, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread1_3(work_1_3,
                     im1, ref(mutex_im1),
                     im1_rows / 4 * 2, im1_rows / 4 * 3, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread1_4(work_1_3,
                     im1, ref(mutex_im1),
                     im1_rows / 4 * 3, im1_rows, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread2_1(work_2_3, im2, ref(mutex_im2),
                     0, im2_columns / 4, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread thread2_2(work_2_3, im2, ref(mutex_im2),
                     im2_columns / 4, im2_columns / 4 * 2, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread thread2_3(work_2_3, im2, ref(mutex_im2),
                     im2_columns / 4 * 2, im2_columns / 4 * 3, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread thread2_4(work_2_3, im2, ref(mutex_im2),
                     im2_columns / 4 * 3, im2_columns / 4 * 4, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread1_1.join();
    thread1_2.join();
    thread1_3.join();
    thread1_4.join();
    thread2_1.join();
    thread2_2.join();
    thread2_3.join();
    thread2_4.join();

    thread thread3_1(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     0, im1_rows / 8, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_2(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 8, im1_rows / 8 * 2, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));
    thread thread3_3(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 8 * 2, im1_rows / 8 * 3, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_4(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 8 * 3, im1_rows / 8 * 4, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_5(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 8 * 4, im1_rows / 8 * 5, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_6(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 8 * 5, im1_rows / 8 * 6, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_7(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 8 * 6, im1_rows / 8 * 7, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_8(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 8 * 7, im1_rows / 8 * 8, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));
    thread3_1.join();
    thread3_2.join();
    thread3_3.join();
    thread3_4.join();
    thread3_5.join();
    thread3_6.join();
    thread3_7.join();
    thread3_8.join();

    return return_me;
}

#pragma optimize("", off)
IntMatrix multi6(IntMatrix im1, IntMatrix im2) {

    if (im2.rows() != im1.columns())
        return (IntMatrix());

    IntMatrix return_me(im1.rows(), im2.columns());

    std::vector<int> r(im1.rows());
    std::vector<int> c(im2.columns());

    size_t im1_rows = im1.rows();
    size_t im2_rows_2 = im2.rows() / 2;
    size_t im2_columns = im2.columns();

    mutex mutex_im1, mutex_im2;
    mutex mutex_im1_rows, mutex_im2_rows_2, mutex_im2_columns;
    mutex mutex_row, mutex_column;
    mutex mutex_return_me, mutex_out;

    //std::cout << "1111";
    thread thread1_1(work_1_3,
                     im1, ref(mutex_im1),
                     0, im1_rows / 3, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread1_2(work_1_3,
                     im1, ref(mutex_im1),
                     im1_rows / 3, im1_rows / 3 * 2, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread1_3(work_1_3,
                     im1, ref(mutex_im1),
                     im1_rows / 3 * 2, im1_rows, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread2_1(work_2_3, im2, ref(mutex_im2),
                     0, im2_columns / 3, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread thread2_2(work_2_3, im2, ref(mutex_im2),
                     im2_columns / 3, im2_columns / 3 * 2, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread thread2_3(work_2_3, im2, ref(mutex_im2),
                     im2_columns / 3 * 2, im2_columns, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread1_1.join();
    thread1_2.join();
    thread1_3.join();
    thread2_1.join();
    thread2_2.join();
    thread2_3.join();

    thread thread3_1(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     0, im1_rows / 6, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_2(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 6, im1_rows / 6 * 2, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));
    thread thread3_3(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 6 * 2, im1_rows / 6 * 3, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_4(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 6 * 3, im1_rows / 6 * 4, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_5(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 6 * 4, im1_rows / 6 * 5, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_6(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 6 * 5, im1_rows, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));
    thread3_1.join();
    thread3_2.join();
    thread3_3.join();
    thread3_4.join();
    thread3_5.join();
    thread3_6.join();

    return return_me;
}

#pragma optimize("", off)
IntMatrix multi4(IntMatrix im1, IntMatrix im2) {

    if (im2.rows() != im1.columns())
        return (IntMatrix());

    IntMatrix return_me(im1.rows(), im2.columns());

    std::vector<int> r(im1.rows());
    std::vector<int> c(im2.columns());

    size_t im1_rows = im1.rows();
    size_t im2_rows_2 = im2.rows() / 2;
    size_t im2_columns = im2.columns();

    mutex mutex_im1, mutex_im2;
    mutex mutex_im1_rows, mutex_im2_rows_2, mutex_im2_columns;
    mutex mutex_row, mutex_column;
    mutex mutex_return_me, mutex_out;

    //std::cout << "1111";
    thread thread1_1(work_1_3,
                     im1, ref(mutex_im1),
                     0, im1_rows / 2, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread1_2(work_1_3,
                     im1, ref(mutex_im1),
                     im1_rows / 2, im1_rows, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread2_1(work_2_3, im2, ref(mutex_im2),
                     0, im2_columns / 2, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread thread2_2(work_2_3, im2, ref(mutex_im2),
                     im2_columns / 2, im2_columns, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));

    thread1_1.join();
    thread1_2.join();
    thread2_1.join();
    thread2_2.join();

    thread thread3_1(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     0, im1_rows / 4, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_2(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 4, im1_rows / 2, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));
    thread thread3_3(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 2, im1_rows / 4 * 3, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_4(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 4 * 3, im1_rows, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));
    thread3_1.join();
    thread3_2.join();
    thread3_3.join();
    thread3_4.join();

    return return_me;
}

#pragma optimize("", off)
IntMatrix multi2(IntMatrix im1, IntMatrix im2) {

    if (im2.rows() != im1.columns())
        return (IntMatrix());

    IntMatrix return_me(im1.rows(), im2.columns());

    std::vector<int> r(im1.rows());
    std::vector<int> c(im2.columns());

    size_t im1_rows = im1.rows();
    size_t im2_rows_2 = im2.rows() / 2;
    size_t im2_columns = im2.columns();

    mutex mutex_im1, mutex_im2;
    mutex mutex_im1_rows, mutex_im2_rows_2, mutex_im2_columns;
    mutex mutex_row, mutex_column;
    mutex mutex_return_me, mutex_out;

    //std::cout << "1111";
    thread thread1_1(work_1_3,
                     im1, ref(mutex_im1),
                     0, im1_rows, ref(mutex_im1_rows),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(r), ref(mutex_row),
                     ref(mutex_out));

    thread thread2_1(work_2_3, im2, ref(mutex_im2),
                     0, im2_columns, ref(mutex_im2_columns),
                     im2_rows_2, ref(mutex_im2_rows_2),
                     ref(c), ref(mutex_column),
                     ref(mutex_out));
    thread1_1.join();
    thread2_1.join();

    thread thread3_1(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     0, im1_rows / 2, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));

    thread thread3_2(work_3_3, im1, im2,
                     ref(mutex_im1), ref(mutex_im2),
                     c, ref(mutex_column), r, ref(mutex_row),
                     im1_rows / 2, im1_rows, im2_columns, im2_rows_2,
                     ref(return_me), ref(mutex_return_me));


    thread3_1.join();
    thread3_2.join();

    return return_me;
}


unsigned long long tick(void) {
    unsigned long long d;
    __asm__ __volatile__("rdtsc" : "=A"(d));
    return d;
}

int test();

int main() {
    test();
    cout<<"work";
    return 0;
}

int test() {
    int start = 101;
    int end = 801;
    int step = 100;
    int rep = 100;

     unsigned long long tb, te, t_mid1 = 0, t_mid2 = 0;

    for (size_t i = start; i <= end; i += step) {
        printf("\ntest with %d elements\n", i);
        IntMatrix a(i, i);
        IntMatrix b(i, i);
        IntMatrix c;
        /*
        for (size_t j = 0; j < rep; j++) {
            tb = tick();
            c = IntMatrix::multiplicate(a, b);
            te = tick();
            t_mid1 += (te - tb);
        }
        t_mid1 /= rep;
        printf("usual %lld \n", t_mid1);
        */
        for (size_t j = 0; j < rep; j++) {
            tb = std::clock();
            c = IntMatrix::multiplicate_best(a, b);
            te = std::clock();
            t_mid1 += (te - tb);
        }
        t_mid1 /= rep;
         printf("usual  %lld \n", t_mid1);
         for (size_t j = 0; j < rep; j++) {
             tb = std::clock();
             c = multi2(a, b);
             te = std::clock();
             t_mid1 += (te - tb);
         }
         t_mid1 /= rep;
         printf("multi2 %lld \n", t_mid1);
         for (size_t j = 0; j < rep; j++) {
             tb = std::clock();
             c = multi4(a, b);
             te = std::clock();
             t_mid1 += (te - tb);
         }
         t_mid1 /= rep;
         printf("multi4 %lld \n", t_mid1);
         for (size_t j = 0; j < rep; j++) {
             tb = std::clock();
             c = multi6(a, b);
             te = std::clock();
             t_mid1 += (te - tb);
         }
         t_mid1 /= rep;
         printf("multi6 %lld \n", t_mid1);
         for (size_t j = 0; j < rep; j++) {
             tb = std::clock();
             c = multi8(a, b);
             te = std::clock();
             t_mid1 += (te - tb);
         }
         t_mid1 /= rep;
         printf("multi8 %lld \n", t_mid1);
    }

    return 0;
}

