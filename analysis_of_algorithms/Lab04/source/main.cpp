#include <iostream>
#include <vector>
#include <algorithm>

void patience_sort(std::vector<int> &arr, size_t size);

void print(std::vector<int> arr, size_t size);

int main() {
    std::vector<int> a = {3, 14, -35, 2, -1, 4, -94, 78, 9, 4, 2, 2};
    std::vector<int> d = {1, 2, 3};
    // bin_insertion_sort(a);

    patience_sort(a, a.size());
    print(a, a.size());

    // tests();

    return 0;
}

void print(std::vector<int> arr, size_t size) {
    std::cout << "\n" << size << " - ";
    for (size_t i = 0; i < size; i++) {
        std::cout << arr[i] << " ";
    }
}

#include <stack>

struct moreStack {
    bool operator()(std::stack<int> s1, std::stack<int> s2) {
        return s1.top() > s2.top();
    }
};

struct lessStack {
    bool operator()(std::stack<int> s1, std::stack<int> s2) {
        return s1.top() < s2.top();
    }
};

#include <queue>
#include <algorithm>

// http://neerc.ifmo.ru/wiki/index.php?title=Терпеливая_сортировка
// http://rosettacode.org/wiki/Sorting_algorithms/Patience_sort
void patience_sort(std::vector<int> &arr, size_t size) {
    std::vector<std::stack<int>> stacks;  // массив стеков

    typedef std::vector<std::stack<int>>::iterator Iterator;

    for (auto it = arr.begin(); it != arr.end(); it++) {
        std::stack<int> stack;
        stack.push(*it);
        Iterator iter =
            std::lower_bound(stacks.begin(), stacks.end(), stack, lessStack());
        if (iter == stacks.end())
            stacks.push_back(stack);
        else
            iter->push(*it);
    }

    printf("stackes size %d", stacks.size());

    // Превратить массив стеком в контейнер
    std::make_heap(stacks.begin(), stacks.end(), moreStack());
    for (size_t i = 0; i < size; i++) {
        // Первый стек становится последним, остальные перестраиваются под кучу
        std::pop_heap(stacks.begin(), stacks.end(), moreStack());
        // Достаем последний стек, который на самом деле имеет наибольший приоритет
        std::stack<int> &smallest = stacks.back();
        // На верху стека лежит наименьшее число, а до этого был выбран стек с
        // наибольшим приоритетом(наименьшей верхушкой)
        arr[i] = smallest.top();
        smallest.pop();
        if (smallest.empty()) {
            stacks.pop_back();
        } else {
            // Поскольку последний стек обновился, надо пересобрать кучу, имитируя
            // вставку нового элемента. push_heap "вставляет" последний элемент
            std::push_heap(stacks.begin(), stacks.end(), moreStack());
        }
    }
}

/*
 * Модель
 *'/ ','<', '=', '[]', '++', '--', '.'
 * стоимость 1
 */
/*
void bin_insertion_sort(std::vector<int> &arr) {
    size_t size = arr.size(); //1 +
    size_t m = 0; //1 +
    for (size_t i = 1; i < size; i++) { // 2 + size * (
        size_t a = 0; // 1 +
        size_t b = i; // 1 +
        int save = arr[i]; // 2 +

        while (a < b) { // 1 + ...
            m = a + (b - a) / 2; // 4 +
            if (save < arr[m]) { // 2 + [1][2]
                b = m;
            } else {
                a = m + 1;
            }
        } // int size2 = i - a
        for (size_t j = i; (j > a); j--) { // 2 + size2 *
            arr[j] = arr[j - 1]; // 4
        }
        arr[a] = save; // + 1)
    }
}
*/
void bin_insertion_sort(std::vector<int> &arr, size_t size) {
    size_t m = 0;
    for (size_t i = 1; i < size; i++) {
        size_t a = 0;
        size_t b = i;
        int save = arr[i];

        while (a < b) {
            m = a + (b - a) / 2;
            if (save < arr[m]) {
                b = m;
            } else {
                a = m + 1;
            }
        }
        for (size_t j = i; (j > a); j--) {
            arr[j] = arr[j - 1];
        }
        arr[a] = save;
    }
}

// Получение максимального числа
void get_min_max_numbers(std::vector<int> arr, size_t size, int &min,
                         int &max) {
    min = max = arr[0];
    for (size_t i = 1; i < size; i++) {
        if (arr[i] > max) {
            max = arr[i];
        } else if (arr[i] < min) {
            min = arr[i];
        }
    }
}

void counting_sort(std::vector<int> &arr, size_t size, int exp) {
    size_t k = 10;
    std::vector<size_t> c(k, 0);
    for (size_t i = 0; i < size; ++i) c[(arr[i] / exp) % 10]++;
    ;
    size_t sum = 0;
    for (size_t i = 0; i < k; ++i) {
        size_t tmp = c[i];
        c[i] = sum;
        sum += tmp;
    }
    std::vector<int> b(size);
    for (size_t i = 0; i < size; ++i) {
        b[c[(arr[i] / exp) % 10]++] = arr[i];
    }
    arr.clear();
    c.clear();
    arr = b;
    b.clear();
}

void add_to_all(std::vector<int> &arr, int num) {
    size_t n = arr.size();
    for (size_t i = 0; i < n; ++i) {
        arr[i] += num;
    }
}

// http://neerc.ifmo.ru/wiki/index.php?title=Цифровая_сортировка
void radix_sort(std::vector<int> &arr, size_t n) {
    int min = 0, max = 0;
    get_min_max_numbers(arr, n, min, max);

    if (min < 0) {
        add_to_all(arr, -min);
        max -= min;
    }
    for (int exp = 1; max / exp > 0; exp *= 10) {
        counting_sort(arr, n, exp);
    }
    if (min < 0) {
        add_to_all(arr, min);
    }
}

template <typename PileType>
bool pile_less(const PileType &x, const PileType &y) {
    return x.top() < y.top();
}

// reverse less predicate to turn max-heap into min-heap
template <typename PileType>
bool pile_more(const PileType &x, const PileType &y) {
    return pile_less(y, x);
}

unsigned long long tick(void) {
    unsigned long long d;
    __asm__ __volatile__("rdtsc" : "=A"(d));
    return d;
}

template <typename Iterator>
unsigned long long patience_sort1(Iterator begin, Iterator end) {
    typedef typename std::iterator_traits<Iterator>::value_type DataType;
    typedef std::stack<DataType> PileType;
    std::vector<PileType> piles;

    for (Iterator it = begin; it != end; it++) {
        PileType new_pile;
        new_pile.push(*it);
        typename std::vector<PileType>::iterator insert_it = std::lower_bound(
                    piles.begin(), piles.end(), new_pile, pile_less<PileType>);
        if (insert_it == piles.end()) {
            piles.push_back(new_pile);
        } else {
            insert_it->push(*it);
        }
    }
    // sorted array already satisfies heap property for min-heap
    unsigned long long best1 = tick();
    for (Iterator it = begin; it != end; it++) {
        std::pop_heap(piles.begin(), piles.end(), pile_more<PileType>);
        *it = piles.back().top();
        piles.back().pop();
        if (piles.back().empty()) {
            piles.pop_back();
        } else {
            std::push_heap(piles.begin(), piles.end(), pile_more<PileType>);
        }
    }
    unsigned long long best2 = tick();

    return best2 - best1;
}

void tests() {
    unsigned long long best1, best2, worst1, worst2, random1;
    unsigned long long random2, best_sr = 0, worst_sr = 0, random_sr = 0;

    std::vector<int> arr1(10001, 0);
    std::vector<int> arr2(10001, 0);
    std::vector<int> arr3(10001, 0);

    std::vector<int> arr4(10001, 0);
    std::vector<int> arr5(10001, 0);
    std::vector<int> arr6(10001, 0);

    unsigned long long repeat = 100;
    std::cout << "amount        best          random           worst\n";
    for (size_t i = 1000; i <= 10000; i += 1000) {
        for (size_t j = 0; j < i; j++) {
            arr5[j] = rand() % 1000;
            arr4[j] = rand() % 10;
            arr6[j] = static_cast<int>(j - i) * 10000;
        }
        best_sr = 0;
        random_sr = 0;
        worst_sr = 0;

        for (unsigned long long k = 0; k < repeat; k++) {
            arr1 = arr4;
            arr2 = arr5;
            arr3 = arr6;

            best1 = tick();
            // patience_sort1(arr1.begin(), arr1.begin() + i);
            radix_sort(arr1, i);
            best2 = tick();
            best_sr += (best2 - best1);

            random1 = tick();
            // bin_insertion_sort(arr2, i);
            radix_sort(arr2, i);
            // patience_sort1(arr2.begin(), arr2.begin() + i);
            random2 = tick();
            random_sr += (random2 - random1);

            worst1 = tick();
            radix_sort(arr3, i);
            // patience_sort1(arr3.begin(), arr3.begin() + i);
            worst2 = tick();
            worst_sr += (worst2 - worst1);
        }
        best_sr /= repeat;
        random_sr /= repeat;
        worst_sr /= repeat;

        std::cout << "\n";

        std::cout << i << "     " << best_sr << "     " << random_sr << "     "
                  << worst_sr;
    }
}
