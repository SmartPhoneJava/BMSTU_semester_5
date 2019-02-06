#include <iostream>
#include <string>
#include <qstring>
#include <clocale>
#include <cstdlib>
#include <QTextStream>

#include <windows.h>
#include <time.h>
#include <vector>

typedef std::vector<int>::size_type size_type;

int minim(int a, int b, int c);
std::vector<std::vector<int>> createMatrix(int lsize, int usize);

int matrixwork(std::vector<std::vector<int>> &matrix, QString left, QString u);
int matrixwork4(std::vector<std::vector<int>> &matrix, QString left, QString u);

void killMatrix(std::vector<std::vector<int>> matrix);

int recurs(QString l, QString u, int i, int j);
int recurs_tree(QString l, QString u, int i, int j, int oldi, int oldj);

void printM(std::vector<std::vector<int>> matrix);

void usualWork();
void testWork();
#define TEST_AMOUNT 100

unsigned long long tick(void)
{
    unsigned long long d;
    __asm__ __volatile__ ("rdtsc" : "=A" (d));
    return d;
}

int main()
{
    usualWork();

    // Замер времени
    //testWork();

    system("pause");

    return 0;
}

void usualWork() {
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);

    setlocale(LC_ALL, "Rus");

    QString str1 = "автомобиль", str2 = "астронавт";

    std::cout << "Input first word: ";

    QTextStream qtin(stdin);
    str1 = qtin.readLine();

    std::cout << "Input second word: ";

    QTextStream qtin1(stdin);
    str2 = qtin1.readLine();

    std::vector<std::vector<int>> n = createMatrix(str1.size() + 1, str2.size() + 1);
    int ans = matrixwork(n, str1, str2);
    std::cout << "\n Linear algorighm: " << ans;
    printM(n);

    std::vector<std::vector<int>> n1 = createMatrix(str1.size() + 1, str2.size() + 1);
    int ans1 = matrixwork4(n1, str1, str2);
    std::cout << "\n Linear algorighm with 4: " << ans1;
    printM(n1);

    int ans2 = recurs(str1, str2, str1.size() + 1, str2.size() + 1);
    std::cout << "\n Recursive algorighm: " << ans2;
}

size_type getSize(QString str) {
     return static_cast<size_type>(str.size() + 1);
}

std::vector<std::vector<int>> createMatrix(int lsize, int usize)
{
    std::vector<std::vector<int>> matrix;

    std::vector<int> r;
    for (int j = 0; j < usize; j++)
    {
        r.push_back(j);
    }
    matrix.push_back(r);
    for (int i = 1; i < lsize; i++)
    {
        std::vector<int> row;
        for (int j = 0; j < usize; j++)
        {
            row.push_back(0);
        }
        row[0] = i;
        matrix.push_back(row);
    }

    return matrix;
}

void killMatrix(std::vector<std::vector<int>> matrix)
{
    for (std::vector<int> row : matrix)
        row.clear();
    matrix.clear();
}

unsigned __int64 timeCreateMatrix(int l, int u)
{
    unsigned __int64 startTime, endTime, averageTime = 0;
    std::vector<std::vector<int>> n;
    for (int i = 0; i < TEST_AMOUNT; i++)
    {
        startTime = tick();
        n = createMatrix(l, u);
        endTime = tick();
        killMatrix(n);
        averageTime += endTime - startTime;
    }
    return (averageTime / 100);
}

int matrixwork(std::vector<std::vector<int>> &num, QString left, QString u)
{
    if (left.size() == 0)
        return u.size();
    if (u.size() == 0)
        return left.size();

    size_type lsize = getSize(left);
    size_type usize =  getSize(u);
    for (size_type i = 1; i < lsize; i++)
    {
        for (size_type j = 1; j < usize; j++)
        {
            num[i][j] = minim(
                num[i - 1][j - 1] + (!(left[i - 1] == u[j - 1])),
                num[i][j - 1] + 1,
                num[i - 1][j] + 1
            );
        }
    }
    return num[lsize - 1][usize - 1];
}

int compare(int a, std::vector<std::vector<int>> &m, size_type i, size_type j, QString l, QString u)
{
    int c = a;
    if ((i > 1 && j > 1) && (l[i] == u[j - 1])
        &&
        (l[i - 1] == u[j])
        )
    {
        c = m[i - 2][j - 2] + 1;
    }
    if (c < a)
        return c;
    return a;
}

int matrixwork4(std::vector<std::vector<int>> &num, QString left, QString u)
{
    if (left.size() == 0)
        return u.size();
    if (u.size() == 0)
        return left.size();

    size_type lsize = getSize(left);
    size_type usize = getSize(u);
    for (size_type i = 1; i < usize; i++)
    {
        num[1][i] = minim(
            num[0][i - 1] + (!(left[0] == u[i - 1])),
            num[1][i - 1] + 1,
            num[0][i] + 1
        );
    }
    for (size_type i = 2; i < lsize; i++)
    {
        num[i][1] = minim(
            num[i - 1][0] + (!(left[i - 1] == u[0])),
            num[i][0] + 1,
            num[i - 1][1] + 1
        );
        for (size_type j = 2; j < usize; j++)
        {
            num[i][j] = compare(minim(
                                    num[i - 1][j - 1] + (!(left[i - 1] == u[j - 1])),
                                    num[i][j - 1] + 1,
                                    num[i - 1][j] + 1
                                ), num, i, j, left, u);
        }
    }
    return num[lsize - 1][usize - 1];
}

unsigned __int64 timeMatrix4Work(std::vector<std::vector<int>> num, QString l, QString u)
{
    unsigned __int64 startTime = 0, endTime = 0, averageTime = 0;

    for (int i = 0; i < TEST_AMOUNT; i++)
    {
        startTime = tick();
        matrixwork4(num, l, u);
        endTime = tick();
        averageTime += endTime - startTime;
    }
    return (averageTime / 100);
}

unsigned __int64 timeMatrixWork(std::vector<std::vector<int>> num, QString l, QString u)
{
    unsigned __int64 startTime = 0, endTime = 0, averageTime = 0;

    for (int i = 0; i < TEST_AMOUNT; i++)
    {
        startTime = tick();
        matrixwork(num, l, u);
        endTime = tick();
        averageTime += endTime - startTime;
    }
    return (averageTime / 100);
}

void timeMatrix(QString l, QString u)
{
    int lsize = l.size() + 1, usize = u.size() + 1;
    std::vector<std::vector<int>> num = createMatrix(lsize, usize);
    unsigned __int64 timeCreate, timeWork;

    timeCreate = timeCreateMatrix(lsize, usize);
    timeWork = timeMatrixWork(num, l, u);
    int answer = matrixwork(num, l, u);

    //std::cout << "\n" << "время создания матрицы для строк размером " << lsize << " на " << usize << " составляет " << timeCreate << " тиков.";
    //std::cout << "\n" << "время нахождения пути с помощью матрицы: " << timeCreate + timeWork << " тиков. Расстояние: " << answer << "\n";
    std::cout << "\n" << "the time distance with matrix " <<
                 timeCreate + timeWork << " ticks. Distance: " << answer << "\n";

    killMatrix(num);
}

void timeMatrix4(QString l, QString u)
{
    int lsize = l.size() + 1, usize = u.size() + 1;
    std::vector<std::vector<int>> num = createMatrix(lsize, usize);
    unsigned __int64 timeCreate, timeWork;

    timeCreate = timeCreateMatrix(lsize, usize);
    timeWork = timeMatrix4Work(num, l, u);
    int answer = matrixwork(num, l, u);

    //std::cout << "\n" << "время создания матрицы для строк размером " << lsize << " на " << usize << " составляет " << timeCreate << " тиков.";
    //std::cout << "\n" << "время нахождения пути с помощью матрицы: " << timeCreate + timeWork << " тиков. Расстояние: " << answer << "\n";
    std::cout << "\n" << "the time distance with matrix with 4 ways " <<
                 timeCreate + timeWork << " ticks. Distance: " << answer << "\n";

    killMatrix(num);
}

int recurs_tree(QString l, QString u, int i, int j, int oldi, int oldj, int counter)
{
    int returning = 0;
    if (i < 1)
        returning =  j;
    if (j < 1)
        returning = i;
    if ((i > 0) && (j > 0))
        returning = minim(
                recurs_tree(l, u, i - 1, j - 1, i, j, counter + 1) + !(l[i - 1] == u[j - 1]),
                recurs_tree(l, u, i, j - 1, i, j, counter + 1) + 1,
                recurs_tree(l, u, i - 1, j, i, j, counter + 1) + 1);
    std::cout<< "\nmy number is " << returning<< " and my step is " << oldi << " " << oldj;

    return returning;
}

int recurs(QString l, QString u, int i, int j)
{
    if (i < 1)
        return j;
    if (j < 1)
        return i;

    return minim(
        recurs(l, u, i - 1, j - 1) + !(l[i - 1] == u[j - 1]),
        recurs(l, u, i, j - 1) + 1,
        recurs(l, u, i - 1, j) + 1);
}

unsigned __int64 timeRecur(QString l, QString u)
{
    unsigned __int64 startTime = 0, endTime = 0, averageTime = 0;

    for (int i = 0; i < TEST_AMOUNT; i++)
    {
        startTime = tick();
        recurs(l, u, l.size() + 1, u.size() + 1);
        endTime = tick();
        averageTime += endTime - startTime;
    }
    return (averageTime / 100);
}

void timeRec(QString l, QString u)
{
    unsigned __int64 timeWork = timeRecur(l, u);
    int answer = recurs(l, u, l.size() + 1, u.size() + 1);
    std::cout << "\n" << "the time distance with recursion " <<
                 timeWork << " ticks. Distance: " << answer;
}

void timeSend(QString l, QString u)
{

    std::cout<< "\n\n";
    timeMatrix(l, u);
    timeMatrix4(l, u);
    //timeRec(l, u);

}

void test1() {
    timeSend("kol", "gol");
    timeSend("school", "partner");
    timeSend("qwertyqwerty", "qwretyqwreyt");
    timeSend("рентгеноэлектрокардиографического", "превысокомногорассмотрительствующий");
}

void test2() {
    for (int i = 1; i <= 10; i += 1)
    {
       QString l, u;
        for (int j = 0; j < i; j++)
        {
            l.append('a' + rand() % ('z' - 'a'));
            u.append('a' + rand() % ('z' - 'a'));
        }
        timeSend(l, u);
    }
}

void test3() {
    for (int i = 100; i < 1000; i += 100)
    {
        QString l, u;
        for (int j = 0; j < i; j++)
        {
            l.append('a' + rand() % ('z' - 'a'));
            u.append('a' + rand() % ('z' - 'a'));
        }
        timeSend(l, u);
    }
}

void testWork()
{
    test1();
    test2();
    test3();
}

int minim(int a, int b, int c)
{
    if (a < c)
    {
        if (a < b)
            return a;
        return b;
    }
    if (c < b)
        return c;
    return b;
}

bool isMatch(std::string left, std::string up, size_type index1, size_type index2)
{
    return (left[index1] == up[index2]);
}

void swap(int **a, int **b)
{
    int* save = *a;
    *a = *b;
    *b = save;
}

void correctString(std::string &left, std::string &up)
{
    if (left.size() < up.size())
    {
        std::string tmp = left;
        left = up;
        up = tmp;
    }
}

void printM(std::vector<std::vector<int>> matrix)
{
    std::cout << "\n";
    for (int i = 0; i < matrix.size(); i++)
    {
        for (int j = 0; j < matrix[0].size(); j++)
        {
            std::cout << matrix[i][j] << " ";
        }
        std::cout << "\n";
    }
}
