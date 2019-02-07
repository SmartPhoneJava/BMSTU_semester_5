#include <QCoreApplication>
#include <vector>
#include <iostream>
#include <ctime>


using namespace std;

class Ant {
public:
    Ant(size_t ccn, int Q) {
       current_city_number = ccn;
       length = 0;
       pheromone = Q;
    }
    vector<size_t> tabu_list; // массив посещенных городов
    size_t current_city_number; // номер текущего города
    int length; // пройденное расстояние
    float pheromone; // выделение феромона

    // Поместить текущий номер в массив посещенных городов
    // Установить ноый номер текущего города
    // Обновить пройденное расстояние
    void make_move(size_t new_current_city, int new_length);
};

class Map {
public:
    Map(vector<vector<int>> matrix_d,
        vector<vector<float>> matrix_p,
        float eva, float al, float be, size_t ci) :
    matrix_distance(matrix_d), matrix_pheromone(matrix_p),
    evaporation(eva), alpha(al), beta(be), cities(ci){;}

    // Матрица дистанций и феромонов
    vector<vector<int>> matrix_distance;
    vector<vector<float>> matrix_pheromone;

    // Интенсивность испарения. От 0 до 1.
    float evaporation;
    // Важность веса ребра и уровня феромонов соответственно
    float alpha, beta;
    // Количество городов
    size_t cities;

    // Возвращает ложь, если пройдены все города, иначе истину
    bool can_move(Ant &ant) {
        size_t tabu_size = ant.tabu_list.size();
        if (tabu_size >= this->cities) {
            return false;
        }
        return true;
    }

    // Уменьшение феромона
    void reduce_pheromone();

    // Выделение нового феромона
    void increase_pheromone(Ant &ant);

    // Обновить данные муравья
    void make_move(Ant &ant);

private:
    // Получить массив исходов
    float get_p_chisl(size_t from, size_t to);

    // Получить вес ребра для перехода. Если <0 значит ребра нет
    int get_w(size_t from, size_t to);

    // Получить интенсивность феромона
    float get_t(size_t from, size_t to);

    // Выбор следующего города
    size_t choose_city(Ant &ant);

    // Получить массив вероятностей походов
    void  get_p(Ant &ant, vector<pair<size_t, float>>& procents);
};

int test();

int run(vector<vector<int>> mat_d, size_t time, size_t ants_amount,
        size_t elite_amount,
        size_t cities, float eva, float al, float be,
        size_t city_from, int Q, int EQ);

void test_run();

vector<vector<int>> generate_matrix(int size);

void test_today();

int main()
{
    srand(0);
    test_today();
    return 0;
}

void cout_matrix(vector<vector<int>> matrix) {
    for (size_t i = 0; i < matrix.size(); i++) {
        std::cout << "\n";
        for (size_t j = 0; j < matrix[i].size(); j++) {
            std::cout << matrix[i][j] << " ";
        }
    }
}

vector<vector<int>> generate_matrix(size_t size) {
    srand(0);
    //size_t size = rand() % 20 + 3;
    vector<int> row(size);
    vector<vector<int>> matrix(size, row);
    for (size_t i = 0; i < size; i++) {
        for (size_t j = i + 1; j < size; j++) {
            matrix[i][j] = rand() % (10 * (j * (size + 1 - i)));
            matrix[j][i] = matrix[i][j];
        }
    }

    return matrix;
}

unsigned long long tick(void) {
    unsigned long long d;
    __asm__ __volatile__("rdtsc" : "=A"(d));
    return d;
}

void test_today() {
    float alpha = -0.05f;
    float beta = 1.05f;
    //float beta_max = 4.f;
    float eva = 0.1f;
    int Q = 1, EQ = 1;
    size_t ants = 50;

    int right_answer = 28;
    int current_answer = 0;
    float difference = 0.0f;

    std::vector<std::vector<int>> distance_matrix( {{0, 16, 15, 17,  1, 16},
                                                   {6, 0, 8, 10, 6, 25},
                                                   {3, 8, 0, 6,  7, 34},
                                                   {7, 10,6, 0,  5, 4},
                                                   {50, 26, 27, 25,  0, 31},
                                                   {16,25,34,4, 31, 0}});

    std::vector<std::vector<int>> distance_matrix_big(
        {{0, 4, 5, 7, 8, 9, 2, 3, 4, 10},
         {4, 0, 3, 2, 6, 7, 2, 5, 9, 3},
         {5, 3, 0, 9, 8, 7, 6, 7, 8, 9},
         {7, 2, 9, 0, 4, 1, 8, 5, 6, 6},
         {8, 6, 8, 4, 0, 7, 4, 2, 1, 8},
         {9, 7, 7, 1, 7, 0, 8, 3, 2, 1},
         {2, 2, 6, 8, 4, 8, 0, 9, 9, 9},
         {3, 5, 7, 5, 2, 3, 9, 0, 9, 9},
         {4, 9, 8, 6, 1, 2, 9, 9, 0, 9},
         {10,3, 9, 6, 8, 1, 9, 9, 9, 0}});

    for (int i = 0; i < 10; i++) {
        for (int j = 0; j < 9; j++) {
            std::cout << distance_matrix_big[i][j] << " & ";
        }
        std::cout << distance_matrix_big[i][9] << " \\\\\n";
    }

    size_t cities = distance_matrix_big.size();

    unsigned long long tb, te, t_mid = 1;

    size_t replyes = 50;
    size_t time = 200;
    // Изменяем коэффициенты

    /*
    for (int a = 0; a < 21; a++) {
        alpha += 0.05f;
        beta -= 0.05f;
        t_mid = 0;
        for (size_t reply = 0; reply < replyes; reply++) {
            tb = tick();
            current_answer = run(distance_matrix_big, time, ants, ants / 10,
            cities, eva, alpha, beta,
              0, Q, EQ);
            te = tick();
            t_mid += te - tb;
        }
        t_mid /= replyes;
        difference = float(right_answer)/float(current_answer);
        //std::cout << "\n A:" << float(right_answer) << " " <<
        //             float(current_answer);
        //printf("correct:%1.2f, ants:%4d, cities:%4d, alpha:%1.4f, beta:%1.4f ,generation:%4d, result - %llu \n",
        //    difference, ants, cities, alpha, beta, time, t_mid);
        printf("%1.2f & %1.2f & %1.2f & %10llu \n",
            difference, alpha, beta, t_mid);

     }
    // Изменяем количество поколений и муравьев
    alpha = 5;
    beta = 12;

    int max = 0;

    int time_add = 5;
    for (time = 20; time <= 800; time += time_add) {
        int ants_add = 5;
        for (ants = 10; ants <= 100; ants += ants_add) {
            t_mid = 0;
            for (size_t reply = 0; reply < replyes; reply++) {
                tb = tick();
                current_answer = run(distance_matrix_big, time, ants, ants / 10,
                cities, eva, alpha, beta,
                  0, Q, EQ);
                te = tick();
                t_mid += te - tb;
                //if (max > current_answer) {
                //    current_answer = max;
                //} else {
                //    max = current_answer;
                //}
            }

            t_mid /= replyes;
            difference = float(right_answer)/float(current_answer);
            //std::cout << "\n A:" << float(right_answer) << " " <<
            //             float(current_answer);
            //printf("correct:%1.2f, ants:%4d, cities:%4d, alpha:%1.4f, beta:%1.4f ,generation:%4d, result - %llu \n",
            //    difference, ants, cities, alpha, beta, time, t_mid);
            printf("%1.2f & %3d & %3d & %10llu \\\\ \n",
                difference, ants, time, t_mid);
            ants_add *= 2;
        }
        time_add *= 2;
    }
    */

    alpha = 5;
    beta = 12;

    int max = 0;

    ants = 50;
    int elite_ant = ants;

    int del_add = 1;

    EQ = 10;

    /*
    for (int del = 1; del <= ants; del += 5) {

        elite_ant = ants/del;
        if (elite_ant < 1) {
            elite_ant = 1;
        }
        del_add = del_add * 2;

        for (Q = 1; Q <= 5; Q += 2) {
            t_mid = 0;
            for (size_t reply = 0; reply < replyes; reply++) {
                tb = tick();
                current_answer = run(distance_matrix_big, time,
                                     ants, elite_ant,
                        cities, eva, alpha, beta,
                        5, Q, EQ);
                te = tick();
                t_mid += te - tb;
                //if (max > current_answer) {
                //    current_answer = max;
                //} else {
                //    max = current_answer;
                //}
            }

            t_mid /= replyes;
            difference = float(right_answer)/float(current_answer);
            //std::cout << "\n A:" << float(right_answer) << " " <<
            //             float(current_answer);
            //printf("correct:%1.2f, ants:%4d, cities:%4d, alpha:%1.4f, beta:%1.4f ,generation:%4d, result - %llu \n",
            //    difference, ants, cities, alpha, beta, time, t_mid);
            printf("%1.2f & %3d & %3d & %10llu \\\\ \n",
                difference, Q, ants/elite_ant, t_mid);
        }
    }
    */

    time = 20;
    eva = 0.0f;
    beta = 5;
    alpha = 1;
    ants = 30;
    elite_ant = 5;
    EQ = Q;
    for (int ii = 1; ii <= 10; ii += 1) {
        for (beta = 2; beta <= 10; beta += 2){
            for (size_t reply = 0; reply < replyes; reply++) {
                tb = tick();
                current_answer = run(distance_matrix_big, time,
                                     ants, elite_ant,
                        cities, eva, alpha, beta,
                        5, Q, EQ);
                te = tick();
                t_mid += te - tb;
                //if (max > current_answer) {
                //    current_answer = max;
                //} else {
                //    max = current_answer;
                //}
            }

            t_mid /= replyes;
            difference = float(right_answer)/float(current_answer);
            //std::cout << "\n A:" << float(right_answer) << " " <<
            //             float(current_answer);
            //printf("correct:%1.2f, ants:%4d, cities:%4d, alpha:%1.4f, beta:%1.4f ,generation:%4d, result - %llu \n",
            //    difference, ants, cities, alpha, beta, time, t_mid);
            printf("%1.2f & %2.0f & %1.2f & %10llu \\\\ \n",
                difference, beta, eva, t_mid);
        }
        eva += 0.1f;
    }
}

#define NOERROR 0
#define ERROR -1

int run_check(vector<vector<int>> mat_d,
              size_t time,
              size_t ants_amount,
              size_t elite_amount,
              size_t cities, float eva,
              size_t city_from) {

    // Если городов нет
    if (cities < 1) {
        std::cout << "\n run_check() - cities < 1";
        return ERROR;
    }

    // Если элитных муравьев больше чем муравьев в общем
    if (ants_amount < elite_amount) {
        std::cout << "\n run_check() - elite_amount error";
        return ERROR;
    }

    // Если поколений муравьев нет
    if (time < 1) {
        std::cout << "\n run_check() - time < 1";
        return ERROR;
    }

    // Если c матрией расстояний что то не так
    if (mat_d.size() == 0 ||
            mat_d[0].size() == 0 ||
            mat_d.size() != mat_d[0].size()) {
        std::cout << "\n run_check() - mat_d error";
        return ERROR;
    }

    // Если муравьев нет
    if (ants_amount < 1) {
        std::cout << "\n run_check() - ants_amount < 1";
        return ERROR;
    }

    // Если распыляется больше 100%
    if (eva > 1.0f) {
        std::cout << "\n run_check() - eva > 1.0";
        return ERROR;
    }

    // Если указанного города не существует
    if (city_from >= cities) {
        std::cout << "\n run_check() - city_from >= cities";
        return ERROR;
    }
    return NOERROR;
}

// time - количество поколений муравьев
// ants_amount - количество муравьев
// elite_one - каждый elite_one-ый муравей будет элитным
// cities - количество городов
// eva(evaporation) - процент распыления феромона
// al(alpha) - коэффицент важности величины пути
// be(beta) - коэффициент важности мощности феромона
// city_from - город появления муравьев
// Q, EQ - величина феромона обычного и элитного муравьев
int run(vector<vector<int>> mat_d, size_t time, size_t ants_amount,
        size_t elite_one,
        size_t cities, float eva, float al, float be,
        size_t city_from, int Q, int EQ) {

    // Проверка на корректность входных данных
    if (run_check(mat_d,time, ants_amount, elite_one, cities,
                  eva, city_from) == ERROR) {
        return ERROR;
    }

    // Создаем матрицу дистанций и феромонов
    vector<vector<int>> matrix_d = mat_d;
    vector<float> row(matrix_d[0].size());
    vector<vector<float>> matrix_f(row.size(), row);

    Map map(matrix_d, matrix_f, eva, al, be, cities);

    vector<Ant> ants_save;

    for (size_t i = 0; i < ants_amount ; i++) {
        if (i % elite_one == 0) {
            ants_save.push_back(Ant(city_from, EQ));
        } else {
            ants_save.push_back(Ant(city_from, Q));
        }
    }

    vector<size_t> tabu_list;

    vector<Ant> ants;
    int best_way = -1;

    size_t ants_size = ants_save.size();

    for (size_t t = 0; t < time; t++) {
        ants = ants_save;
        for (size_t a = 0; a < ants_size; a++) {
            ants[a].tabu_list.push_back(city_from);
            while (map.can_move(ants[a])) {
                map.make_move(ants[a]);
            }

            // Выполняем дополнительный шаг, возврающий
            // муравья в начало
            if (ants[a].tabu_list.back() < map.matrix_distance.size()) {
                ants[a].length += map.matrix_distance[ants[a].tabu_list.back()][city_from];
                ants[a].tabu_list.push_back(city_from);
            } else {
                std::cout << "\n strange error";
            }

            map.increase_pheromone(ants[a]);

            if (best_way < 0 || best_way > ants[a].length) {
                best_way = ants[a].length;
                tabu_list = ants[a].tabu_list;
            }
            map.reduce_pheromone();
        }

    }

    return best_way;
}

int Map::get_w(size_t from, size_t to) {
    return matrix_distance[from][to];
}

float Map::get_t(size_t from, size_t to) {
    return matrix_pheromone[from][to];
}

float Map::get_p_chisl(size_t from, size_t to) {
    float a = 1/powf(get_w(from, to), alpha);
    float b = powf(get_t(from, to), beta);
    float chisl = a + b;
    return chisl;
}

void Map::get_p(Ant &ant, vector<pair<size_t, float>>& procents) {
    procents.clear();
    float summ = 0;
    bool tabu = false;

    float eps = 0.00000001f;
    size_t tabu_size = ant.tabu_list.size();

    // Цикл по всем городам
    for(size_t i = 0; i < cities; i++){
        // Проверяем, что выбранный нами город не был уже посещен
        tabu = false;
        for (size_t j = 0; j < tabu_size && !tabu; j++) {
            if (i == ant.tabu_list[j]) {
                // Если был, то пометим это
                tabu = true;
            }
        }
        // Если муравей еще не был в этом городе
        if (!tabu) {
            size_t current = ant.current_city_number;

            // Проверяем, что доступ в город есть(вес ребра <0 если
            // доступа нет) и что это два разных города(вес 0)
            // На самом деле проверка про разные города происходит в табу,
            // поскольку на текущем шаге мы в табу заносим следующий город
            // но на всякий случай, я оставлю все как есть
            if (get_w(current, i) > 0) {
                float chisl = get_p_chisl(current, i);
                pair<size_t, float> p(i,chisl);
                procents.push_back(p);
                summ += chisl;
            }
        }
    }
    // В массиве chislitel - количество исходов, что муравей пойдет
    // в i-ым путем, summ - общее кол-во исходов. Чтобы получить
    // вероятности поделим каждый элемент chislitel на summ
    // std::cout << "\nsumm is " << summ << " cause i have " <<
    //             ant.tabu_list.size() << " and " << cities;
    // Вдруг каким то неведомым чудом summ ноль
    if (summ < eps) {
        procents.clear();
        std::cout << "\nzero division";
        return;
    }

    size_t procents_size = procents.size();

    if (procents_size == 0) {
        std::cout << "\nError with adding elements";
        return;
    }
    for (size_t i = 0; i < procents_size; i++) {
        procents[i].second = procents[i].second / summ * 100;
    }
}

size_t Map::choose_city(Ant &ant) {
    vector<pair<size_t, float>> procents;
    get_p(ant, procents);
    int procent = rand() % 100;
    int compare_procent = static_cast<int>(procents[0].second);

    size_t procents_size = procents.size();
    size_t i = 0;
    for (;i < procents_size - 1 && compare_procent < procent;i++){
       compare_procent += procents[i + 1].second;
    };
    if (i == procents_size) {
       std::cout << "\n choose_city out of borders " <<
                    i << "/" << procents_size;
       return 0;
    }

    if (compare_procent > 100) {
        std::cout << "\n choose_city() error procenting " <<
                     compare_procent;
        return 0;
    }

    return procents[i].first;
}

void Ant::make_move(size_t new_current_city, int new_length) {
    tabu_list.push_back(new_current_city);
    current_city_number = new_current_city;
    length += new_length;
}

void Map::make_move(Ant &ant) {
    size_t new_city = choose_city(ant);
    int length = get_w(ant.current_city_number, new_city);
    ant.make_move(new_city, length);
}

// Уменьшение феромана
void Map::reduce_pheromone() {
    // Матрица квадратная поэтому достаточно одной величины
    size_t size = matrix_pheromone.size();
    float k = 1 - evaporation;
    float eps = 0.00001f;

    for (size_t i = 0; i < size; i++) {
        for (size_t j = 0; j < size; j++) {
            if (i != j && matrix_pheromone[i][j] > eps) {
                matrix_pheromone[i][j] *= k;
            }
        }
    }
}

void Map::increase_pheromone(Ant &ant) {
    size_t prev_city;
    size_t curr_city;
    size_t tabu_size = ant.tabu_list.size();

    for (size_t city = 1; city < tabu_size; city++) {
        prev_city = ant.tabu_list[city-1];
        curr_city =  ant.tabu_list[city];
        matrix_pheromone[prev_city][curr_city] += ant.pheromone / ant.length;
        matrix_pheromone[curr_city][prev_city] += ant.pheromone / ant.length;
    }
}
