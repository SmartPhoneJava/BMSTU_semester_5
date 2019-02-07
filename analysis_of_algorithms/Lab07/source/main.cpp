//#include <QCoreApplication>

#include "windows.h"
#include <mutex>
#include <thread>
#include <queue>
#include <iostream>

using Ms = std::chrono::milliseconds;

class Unit{};

class Component{};

unsigned long long tick(void) {
    unsigned long long d;
    __asm__ __volatile__("rdtsc" : "=A"(d));
    return d;
}


class Body : public Component
{
public:
   Body(){built = false;}

   void build() {Body::count++; built = true;}

   bool is_built(){return built;}

   static int count;
protected:
    bool built;

    int max_health;
    int health;
    int rotation_speed;
    int current_rotation_angle;
};

class Engine : public Component
{
public:
    Engine(){built = false;}

    void build() {Engine::count++; built = true;}

    int get_speed() const;
    int get_max_speed() const;
    int get_max_backspeed() const;

    void get_speed(int sp);
    void get_max_speed(int sp);
    void get_max_backspeed(int sp);

    static int count;

    bool is_built(){return built;}
private:
    bool built;
    int speed_;
    int max_speed_;
    int max_backspeed_;
};

class Weapon: public Component
{
public:
    Weapon(){built = false;}
    void build() {Weapon::count++;built = true;}

    bool is_built(){return built;}

    void fire();

    static int count;
private:
    bool built;
    int damage;
    int recharge;
    int amount_bullets;
};

class Tank : public Unit
{
public:
    static int count;
    int id;
    std::mutex tank_mutex;

    Tank(){id = Tank::count++; built = false;}
     ~Tank(){}

    void insert_weapon(Weapon weapon, DWORD time) {
        Sleep(time);
        _weapon = weapon;
    }

    void insert_body(Body body, DWORD time) {
        Sleep(time);
        _body = body;
    }

    void insert_engine(Engine engine, DWORD time) {
        Sleep(time);
        _engine = engine;
    }

    bool has_engine(){return _engine.is_built();}
    bool has_weapon(){return _weapon.is_built();}
    bool has_body(){return _body.is_built();}

    bool is_ready() {return _engine.is_built() && _body.is_built() && _weapon.is_built();}

    bool is_built() {return built;}


    void complete() {
        if (_engine.is_built() && _body.is_built() && _weapon.is_built()) {
            Sleep(50);
            built = true;
        }
    }

    // Конструктор копирования для queue.pop
    Tank(const Tank & other) {
        _engine = other._engine;
        _body = other._body;
        _weapon = other._weapon;
        id = other.id;
    }

    Tank & operator=(const Tank & other) {
        _engine = other._engine;
        _body = other._body;
        _weapon = other._weapon;
        id = other.id;

        return *this;
    }

    void move(int speed);
    void rotate(double angle);
    void fire();

private:
    bool built;
    Engine _engine;
    Body _body;
    Weapon _weapon;
};

// Для очередей, которые ничего не должны строить
Tank noWork(Tank t, DWORD d){return t;};

class TankQueue {
private:
    // Очередь готовых на выполнение
    std::queue<Tank> tanks;
    // Функтор - функция постройки
    std::function<Tank(Tank &tank, DWORD time)> work;
    // Мьютекс для блокировки доступа к очереди выполнения
    std::timed_mutex queue_mutex;
    // Мьютекс для блокировки доступа к флагу finish
    std::mutex finish_mutex;
    // Мьютекс для блокировки доступа к флагу main
    std::mutex main_mutex;

    // флаг первого рабочего конвейера
    bool main;
public:
    // флаг конца работы, можно было сделать без него, но так понятнее
    bool finish;
    // Время ожидания на мьютексе
    DWORD time_for_mutex;
    // Время ожидания появления элементов в очереди
    DWORD downtime;
    // время постройки - занесено сюда для тестирования
    DWORD build_time;
    // идентификатор танка - занесен сюда для отладки
    int id;
    // статическая переменаня нужна для id
    static int count;
    std::vector<clock_t> times_in;
    std::vector<clock_t> times_out;
    clock_t begin;
    clock_t end;

    // Конструкторы копирования и перемещения
    TankQueue(const TankQueue & other);

    TankQueue(const TankQueue && other);

    // Для очереди функций работы с танками
    TankQueue(std::function<Tank(Tank&, DWORD)> f, DWORD t);

    // Для очереди готовых танков
    TankQueue(){} // не использовать больше нигде

    // Для очереди пустых танков
    TankQueue(size_t amount);

    void set_main() {
        main = true;
    }

    void start(TankQueue &TQ);

    Tank pop() {
        Tank tank = tanks.front();
        tanks.pop();
        return tank;
    }

    void push(Tank &tank) {
        tanks.push(tank);
    }

    size_t size() {
        return tanks.size();
    }
};

// Для пустой очереди
TankQueue::TankQueue(size_t amount){
    end = begin = clock();
    id = TankQueue::count++;
    for (size_t i = 0; i < amount; i++) {
        Tank tank;
        tanks.push(tank);
    }
    main = 1;
    finish = 0;
    work = noWork;
    time_for_mutex = 100;
    downtime = 100;
}

// Для очереди функций работы с танками
TankQueue::TankQueue(std::function<Tank(Tank&, DWORD)> f, DWORD t){
    end = begin = clock();
    id = TankQueue::count++;
    main = 0;
    finish = 0;
    work = f;
    build_time = t;
    time_for_mutex = 100;
    downtime = 100;
}

TankQueue::TankQueue(const TankQueue & other) {
    id = other.id;
    tanks = other.tanks;
    work = other.work;
    finish = other.finish;
    main = other.main;
    time_for_mutex = other.time_for_mutex;
    downtime = other.downtime;
}

TankQueue::TankQueue(const TankQueue && other) {

    id = other.id;
    tanks = other.tanks;
    work = other.work;
    finish = other.finish;
    main = other.main;
    time_for_mutex = other.time_for_mutex;
    downtime = other.downtime;
}

void TankQueue::start(TankQueue &TQ){
    finish_mutex.lock();

    while (!finish) {
        finish_mutex.unlock();
        queue_mutex.lock();
        while (tanks.size() > 0) {
            times_in.push_back(clock());
            Tank tank = pop();
            queue_mutex.unlock();
            tank = work(tank, build_time);
            bool connect = 0;
            while (!connect) {
                if (TQ.queue_mutex.try_lock_for(
                            Ms(time_for_mutex))) {
                           TQ.push(tank);
                           times_out.push_back(clock());
                           connect = 1;
                           TQ.queue_mutex.unlock();
                }
                std::this_thread::sleep_for(Ms(50));
            }
            queue_mutex.lock();
        }
        queue_mutex.unlock();
        main_mutex.lock();
        if (main && tanks.size() == 0) {
            main_mutex.unlock();

            finish_mutex.lock();
            finish = 1;
            finish_mutex.unlock();

            TQ.main_mutex.lock();
            TQ.set_main();
            TQ.main_mutex.unlock();


            end = clock();
        } else {
            main_mutex.unlock();
            std::this_thread::sleep_for(Ms(downtime));
        }
        finish_mutex.lock();
    }
    //std::cout << "\nfinish";
    finish_mutex.unlock();
}

Tank add_engine(Tank &tank, DWORD time) {
    tank.tank_mutex.lock();

    if (!tank.has_engine()) {
        Engine engine;
        tank.insert_engine(engine, time);
        //std::cout << "\n add engine to tank " << tank.id;
    }

    tank.tank_mutex.unlock();

    return tank;
}

Tank add_weapon(Tank &tank, DWORD time) {
    tank.tank_mutex.lock();

    if (!tank.has_weapon()) {
        Weapon weapon;
        tank.insert_weapon(weapon, time);
        //std::cout << "\n add weapon to tank " << tank.id;
    }

    tank.tank_mutex.unlock();

    return tank;
}

Tank add_body(Tank &tank, DWORD time) {
    tank.tank_mutex.lock();

    if (!tank.has_body()) {
        Body body;
        tank.insert_body(body, time);
        //std::cout << "\n add body to tank " << tank.id;
    }
    tank.tank_mutex.unlock();
    return tank;
}

int Body::count = 1;
int Tank::count = 1;
int Weapon::count = 1;
int Engine::count = 1;
int TankQueue::count = 1;

void test(size_t amount, int time, DWORD a, DWORD b, DWORD c) {
    unsigned long long tb, te, t_mid = 1;

    TankQueue tanks(amount);

    TankQueue queue_body(add_body, a);
    TankQueue queue_engine(add_engine, b);
    TankQueue queue_weapon(add_weapon, c);

    TankQueue ready_tanks;

    std::thread give_to_body([&tanks, &queue_body](){ tanks.start(queue_body);});

    std::thread threads[100];
    int j = 0;
    for (int i = 0; i < time; i++) {
         threads[j] = std::thread ([&queue_body, &queue_engine](){ queue_body.start(queue_engine);});
         j++;
    }
    for (int i = 0; i < time; i++) {
         threads[j] = std::thread ([&queue_engine, &queue_weapon](){ queue_engine.start(queue_weapon);});
         j++;
    }
    for (int i = 0; i < time; i++) {
         threads[j] = std::thread ([&queue_weapon, &ready_tanks](){ queue_weapon.start(ready_tanks);});
         j++;
    }

    give_to_body.detach();
    for (int i = 0; i < time; i++) {
        j--;
        threads[j].join();
    }

    for (int i = 0; i < time; i++) {
        j--;
        threads[j].join();
    }

    for (int i = 0; i < time; i++) {
        j--;
        threads[j].join();
    }
    //while (queue_weapon.end == queue_weapon.begin){
    //    Sleep(100);
    //}
    clock_t t1_r, t2_r, t3_r;
    t1_r = queue_body.end - queue_body.begin;
    t2_r = queue_engine.end - queue_engine.begin;
    t3_r = queue_weapon.end - queue_weapon.begin;

    //std::cout << "\n" << a << " " << " " << b << " " <<
    //             c << " "<< t1_r <<
    //             " " << t2_r << " " << t3_r;

    //std::cout << "\n" << time << " " << t1_r <<
    //             " " << t2_r << " " << t3_r;

    for (int i = 0; i < amount; i++) {
        std::cout << "\n" << 1 << " " << queue_body.times_in[i] << " " << queue_body.times_out[i];
        std::cout << "\n" << 2 << " " << queue_engine.times_in[i] << " " << queue_engine.times_out[i];
        std::cout << "\n" << 3 << " " << queue_weapon.times_in[i] << " " << queue_weapon.times_out[i];
    }

}

int main()
{
    Tank::count = 1;
    TankQueue::count = 1;

    //for (int i = 1; i < 11; i++)
        test(5, 1, 700, 1400, 900);

    /*
    test(5, 1, 100, 100, 2800);
    test(5, 1, 1000, 1000, 1000);
    test(5, 1, 2800, 100, 100);

    for (int i = 100; i < 1000; i += 200) {
        test(5, 1, 1000 - i, 1000 + i, 1000);
        std::cout << " " << i * 2;
    }
     std::cout << "\n";
    for (int i = 100; i < 1000; i += 200) {
        test(5, 1, 1000 + i, 1000, 1000 - i);
        std::cout << " " << i * 2;
    }
     std::cout << "\n";
    for (int i = 100; i < 1000; i += 200) {
        test(5, 1, 1000, 1000 - i, 1000 + i);
        std::cout << " " << i * 2;
    }
    */

   //test(5, 1, 1000, 500, 1500);

    return 0;
}
